using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace WebPageParser.DocumentParsing{
	public class ParseGameDisk{
		public ParseGameDisk(CsQuery.CQ doc){
			_doc = doc;
		}

		private readonly CsQuery.CQ _doc;

		public List<Models.GameDiscRegion> Parse(){
			var gameDiscs = new List<Models.GameDiscRegion>();
			foreach (var th in GetRegionsHeaders()) {
				var gameDiskInfo = new Models.GameDiscRegion();
				foreach (var tr in th.ParentNode.ParentNode.ChildElements.Skip(1))
					ParseDiskInfo(tr, gameDiskInfo);
				gameDiscs.Add(gameDiskInfo);
			}

			return gameDiscs;
		}

		private IEnumerable<CsQuery.IDomObject> GetRegionsHeaders(){
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

			var regionheaders = (
				from header in headers
				let regex = regionRegex.Match(header.InnerText)
				where regex.Groups.Count > 0
				let value = regex.Groups[1].Value
				where allowedRegions.Contains(value)
				select header
			).ToList();
			return regionheaders;
		}


		private static void ParseDiskInfo(CsQuery.IDomElement tr, Models.GameDiscRegion gameDiscRegion){
			var fieldType = tr.FirstElementChild.FirstElementChild;
			if (fieldType.NodeName == "FONT")
				fieldType = fieldType.FirstElementChild;
			switch (fieldType.InnerText){
				case "Serial numbers:": ParseDiskId(tr, gameDiscRegion); break;
				case "Release date:": ParseReleaseDate(tr, gameDiscRegion); break;
				case "CRCs:": ParseCrcs(tr, gameDiscRegion); break;
				case "Windows Status:": gameDiscRegion.PlayableWindows = ParseStatus(tr); break;
				case "Linux Status:": gameDiscRegion.PlayableLinux = ParseStatus(tr); break;
				case "Mac Status:": gameDiscRegion.PlayableMac = ParseStatus(tr); break;
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

		private static void ParseCrcs(CsQuery.IDomElement tr, Models.GameDiscRegion gameDiscRegion){
			var result = new List<Models.GameDiscCrc>();
			var newDiskId = new Models.GameDiscCrc { GameDiscRegion = gameDiscRegion };
			Action newDisk = () => {
				newDiskId = new Models.GameDiscCrc { GameDiscRegion = gameDiscRegion };
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
						if(regex.Count > 1){
							var regexMatches = regex.Cast<Group>().Select(p => p.Value).ToArray();
							for (var i = 0; i < regexMatches.Length; i++){
								newDiskId.Crc = int.Parse(regexMatches[i], System.Globalization.NumberStyles.HexNumber);
								if (i < regexMatches.Length-1)
									newDisk();
							}
						}
						else if (regex.Count == 1)
							newDiskId.Crc = int.Parse(regex[0].Value, System.Globalization.NumberStyles.HexNumber);
						else{
							if (str != "12/09/04" && str != "2")
								throw new Exception("Invalid CRC: " + str);
						}
						break;
					case "SMALL":
						newDiskId.Tag = childElement.InnerText.Replace("(", "").Replace(")", "");
						break;
				}
			}
			gameDiscRegion.GameDiscCrcs = result;
		}

		private static void ParseReleaseDate(CsQuery.IDomElement tr, Models.GameDiscRegion gameDiscRegion){
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
						gameDiscRegion.ReleaseDate = default(DateTime);
						return;
					}

					var regex = Regex.Match(outVal, "Q([1234]) ([0-9]{4})");
					if (regex.Success){
						var q = int.Parse(regex.Groups[1].Value);
						var year = int.Parse(regex.Groups[2].Value);
						gameDiscRegion.ReleaseDate = new DateTime(year, 3*q, 1);
						return;
					}

					regex = Regex.Match(outVal, "([0-9]{4})");
					if (regex.Success && regex.Groups.Count > 1) {
						gameDiscRegion.ReleaseDate = new DateTime(int.Parse(regex.Groups[1].Value), 1, 1);
						return;
					}

					gameDiscRegion.ReleaseDate = DateTime.Parse(outVal);
				}
			}
		}

		private static void ParseDiskId(CsQuery.IDomElement tr, Models.GameDiscRegion gameDiscRegion){
			var result = new List<Models.GameDiscSerial>();
			var newDiskId = new Models.GameDiscSerial{GameDiscRegion = gameDiscRegion};
			Action newDisk = () =>{
				newDiskId = new Models.GameDiscSerial { GameDiscRegion = gameDiscRegion };
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
						newDiskId.SerialNumber = childElement.ToString();
						break;
					case "SMALL":
						newDiskId.Tag = childElement.InnerText.Replace("(", "").Replace(")", "");
						break;
				}
			}
			gameDiscRegion.GameDiscSerialNumbers = result;
		}
	}
}