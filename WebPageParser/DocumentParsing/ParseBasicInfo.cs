using System.Linq;
using System;

namespace WebPageParser.DocumentParsing{
	public class ParseBasicInfo{
		private readonly CsQuery.CQ _doc;

		public ParseBasicInfo(CsQuery.CQ doc){
			_doc = doc;
		}

		public Models.GameBasicInfo Parse() {
			var basicInfo = new Models.GameBasicInfo();
			basicInfo.Title = _doc["#firstHeading > span[dir=auto]"].Text();
			basicInfo.ReviewScore = GetReviewScore();

			var dict = new System.Collections.Generic.Dictionary<string, Action<string>>{
				{"Developer(s): ", p=>basicInfo.Developer = p},
				{"Game description: ", p=>basicInfo.Description = p},
				{"Genre: ", p=>basicInfo.Genre = p},
			//	{"Publisher(s): ", p=>basicInfo.Publisher = p},
			//	{"Wikipedia: ", p=>basicInfo.Wikipedia = p},
			//	{"Game review links: ", p=>basicInfo.ReviewScore = p},
			};
			var basicInfoElements = _doc["#mw-content-text > p"].Children();
			foreach (var keyPair in dict) {
				var keyElm = basicInfoElements.FirstOrDefault(p => p.InnerText == keyPair.Key);
				if (keyElm == null) continue;

				var valueElm = keyElm.NextSibling;
				var valueOut = new System.Collections.Generic.List<string>();

				while (valueElm != null && valueElm.NodeName != "BR") {
					if (valueElm.NodeName == "A")
						valueOut.Add(valueElm.InnerText);
					else if (valueElm.NodeName == "#text")
						valueOut.Add(valueElm.ToString());
					valueElm = valueElm.NextElementSibling;
				}

				keyPair.Value(string.Join(", ", valueOut));
			}
			return basicInfo;
		}

		private byte? GetReviewScore(){
			var reviewText = _doc["#mw-content-text > p"]["span[style=color:#008F00;]"].Text();
			if (string.IsNullOrEmpty(reviewText)) return null;
			var textSplit = reviewText.Split('/');
			var low = float.Parse(textSplit[0]);
			var high = (float) Math.Pow(10, Math.Ceiling(Math.Log10(low)));

			return (byte)(100 * low/high);
		}
	}
}