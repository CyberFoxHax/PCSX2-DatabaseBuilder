using Enumerable = System.Linq.Enumerable;

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

			var dict = new System.Collections.Generic.Dictionary<string, System.Action<string>>{
				{"Developer(s): ", p=>basicInfo.Developer = p},
				{"Game description: ", p=>basicInfo.Description = p},
				{"Genre: ", p=>basicInfo.Genre = p},
			//	{"Publisher(s): ", p=>basicInfo.Publisher = p},
			//	{"Wikipedia: ", p=>basicInfo.Wikipedia = p},
			//	{"Game review links: ", p=>basicInfo.ReviewScore = p},
			};
			var basicInfoElements = _doc["#mw-content-text > p"].Children();
			foreach (var keyPair in dict) {
				var keyElm = Enumerable.FirstOrDefault(basicInfoElements, p => p.InnerText == keyPair.Key);
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

			float high;
			if (textSplit.Length > 1)
				high = float.Parse(textSplit[1]);
			else
				high = (float) System.Math.Floor(System.Math.Log(low));

			return (byte) (255 * high/low);
		}
	}
}