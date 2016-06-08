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
			if (elm.FirstElementChild != null)
				outVal = elm.FirstElementChild.FirstElementChild.InnerText;
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
						newDiskId.Crc = int.Parse(childElement.ToString(), System.Globalization.NumberStyles.HexNumber);
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
					if (outVal.Length == 4)
						gameDisc.ReleaseDate = new DateTime(int.Parse(outVal), 1, 1);
					else
						gameDisc.ReleaseDate = DateTime.Parse(outVal);
					return;
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