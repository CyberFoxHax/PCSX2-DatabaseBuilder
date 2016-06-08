using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace WebPageParser.DocumentParsing{
	public class ParseGameDisk{
		private readonly CsQuery.CQ _doc;

		public ParseGameDisk(CsQuery.CQ doc){
			_doc = doc;
		}

		public List<Models.GameDisc> Parse(){

			var type = typeof (Models.Enum.Region);
			var allowedRegions = type
				.GetMembers()
				.Where(p => p.DeclaringType == type && p.Name != "value__")
				.Select(p =>{
					var attrib = p.GetCustomAttribute<Models.LabelAttribute>();
					return attrib != null ? attrib.Label : p.ToString();
				})
				.ToList();

			var headers = _doc["th[style=background-color:#B1EDFF; color:#008EB9][colspan=2]"];

			var regionRegex = new Regex("Region ([A-Z]{3,4}[-]{0,1}[A-Z]{0,1}):");

			var regionheaders = headers
				.Where(p =>{
					var regexMatch = regionRegex.Match(p.InnerText);
					if (regexMatch.Groups.Count <= 1) return false;
					var match = regexMatch.Groups[1].Value;
					return allowedRegions.Contains(match);
				})
				.ToList();
			var gameDiscs = new List<Models.GameDisc>();
			foreach (var th in regionheaders) {
				var gameDiskInfo = new Models.GameDisc();
				foreach (var tr in th.ParentNode.ParentNode.ChildElements.Skip(1))
					ParseDiskInfo(tr, gameDiskInfo);
				gameDiscs.Add(gameDiskInfo);
			}

			return gameDiscs;
		}


		private static void ParseDiskInfo(CsQuery.IDomElement tr, Models.GameDisc gameDisc){
			var fieldType = tr.FirstElementChild.FirstElementChild;
			if (fieldType.NodeName == "FONT")
				fieldType = fieldType.FirstElementChild;
			switch (fieldType.InnerText){
				case "Serial numbers:": ParseDiskId(tr, gameDisc); break;
				case "Release date:": ParseReleaseDate(tr, gameDisc); break;
				case "CRCs:": ParseCrcs(tr, gameDisc); break;
				case "Windows Status:": gameDisc.PlayableWindows = ParseStatus(tr); break;
				case "Linux Status:": gameDisc.PlayableLinux = ParseStatus(tr); break;
				case "Mac Status:": gameDisc.PlayableMac = ParseStatus(tr); break;
				default: break;
			}
		}

		private static Models.Enum.Playable ParseStatus(CsQuery.IDomElement tr){
			var elm = tr
				.ChildElements
				.ElementAt(1);
			string outVal;
			if (elm.FirstElementChild != null && elm.FirstElementChild.FirstElementChild != null)
				outVal = elm.FirstElementChild.FirstElementChild.InnerText;
			else if (elm.FirstElementChild != null)
				outVal = elm.FirstElementChild.InnerText;
			else
				outVal = elm.InnerText;
			switch (outVal){
				case "Playable"	: return Models.Enum.Playable.Playable;
				case "Ingame"	: return Models.Enum.Playable.Ingame;
				case "Broken"	: return Models.Enum.Playable.Broken;
				case "?"		: return Models.Enum.Playable.Unspecified;
			}
			return default(Models.Enum.Playable);
		}

		private static void ParseCrcs(CsQuery.IDomElement tr, Models.GameDisc gameDisc){
			var result = new List<Models.GameDiscCrc>();
			var newDiskId = new Models.GameDiscCrc { GameDisc = gameDisc };
			Action newDisk = () => {
				newDiskId = new Models.GameDiscCrc { GameDisc = gameDisc };
				result.Add(newDiskId);
			};
			newDisk();
			foreach (var childElement in tr
				.ChildElements
				.ElementAt(1)
				.ChildNodes
			) {
				if (childElement.NodeName == "BR") {
					newDisk();
					continue;
				}
				switch (childElement.NodeName) {
					case "#text":
						var str = childElement.ToString();
						if (str.Contains(",")) {
						}
						var regex = Regex.Matches(str, "([0-9A-F]{6,8})", RegexOptions.IgnoreCase);
						if(regex.Count > 1)
							foreach (var match in regex.Cast<Group>().Select(p=>p.Value)) {
								newDiskId.Crc = int.Parse(match, System.Globalization.NumberStyles.HexNumber);
								newDisk();
							}
						else if (regex.Count == 1)
							newDiskId.Crc = int.Parse(regex[0].Value, System.Globalization.NumberStyles.HexNumber);
						else{
							int val;
							if(int.TryParse(str, out val))
								newDiskId.Crc = val;
							else
								//if (str != "12/09/04")
									throw new Exception("Invalid CRC: " + str);
								Console.WriteLine("Invalid CRC: " + str);
						}
						break;
					case "SMALL":
						newDiskId.Tag = childElement.InnerText.Replace("(", "").Replace(")", "");
						break;
				}
			}
			gameDisc.GameDiscCrcs = result;
		}

		private static void ParseReleaseDate(CsQuery.IDomElement tr, Models.GameDisc gameDisc){
			foreach (var childElement in tr
				.ChildElements
				.ElementAt(1)
				.ChildNodes
			) {
				if (childElement.NodeName == "BR")
					continue;
				if (childElement.NodeName == "#text"){
					var outVal = childElement.ToString();

					if (outVal == "?"){
						gameDisc.ReleaseDate = default(DateTime);
						return;
					}

					DateTime time;
					if (DateTime.TryParse(outVal, out time)) {
						gameDisc.ReleaseDate = time;
						return;
					}

					var regex = Regex.Match(outVal, "Q([1234]) ([0-9]{4})");
					if (regex.Success){
						var q = int.Parse(regex.Groups[1].Value);
						var year = int.Parse(regex.Groups[2].Value);
						gameDisc.ReleaseDate = new DateTime(year, 3*q, 1);
						return;
					}

					regex = Regex.Match(outVal, "([0-9]{4})");
					if (regex.Success && regex.Groups.Count > 1) {
						gameDisc.ReleaseDate = new DateTime(int.Parse(regex.Groups[1].Value), 1, 1);
						return;
					}

					throw new Exception("Invalid date: " + outVal);
				}
			}
		}

		private static void ParseDiskId(CsQuery.IDomElement tr, Models.GameDisc gameDisc){
			var result = new List<Models.GameDiscId>();
			var newDiskId = new Models.GameDiscId{GameDisc = gameDisc};
			Action newDisk = () =>{
				newDiskId = new Models.GameDiscId { GameDisc = gameDisc };
				result.Add(newDiskId);
			};
			newDisk();
			foreach (var childElement in tr
				.ChildElements
				.ElementAt(1)
				.ChildNodes
			){
				if (childElement.NodeName == "BR"){
					newDisk();
					continue;
				}
				switch (childElement.NodeName){
					case "#text":
						newDiskId.DiskId = childElement.ToString();
						break;
					case "SMALL":
						newDiskId.Tag = childElement.InnerText.Replace("(", "").Replace(")", "");
						break;
				}
			}
			gameDisc.GameDiscIds = result;
		}
	}
}