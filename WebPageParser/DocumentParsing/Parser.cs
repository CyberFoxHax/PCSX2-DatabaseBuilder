using System.Text;
using System.Threading.Tasks;

namespace WebPageParser.DocumentParsing {
	public class Parser {
		public Models.GameBasicInfo GameBasicInfo { get; set; }

		public void Parse(string htmlText, string url) {
			var doc = CsQuery.CQ.CreateDocument(htmlText);
			GameBasicInfo = new ParseBasicInfo(doc).Parse();
			var gameDisks = new ParseGameDisk(doc).Parse();

			GameBasicInfo.GameDiscs = gameDisks;
			gameDisks.ForEach(p=>p.GameBasicInfo = GameBasicInfo);
		}
	}
}
