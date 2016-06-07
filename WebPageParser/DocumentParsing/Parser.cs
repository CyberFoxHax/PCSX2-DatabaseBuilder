using System.Text;
using System.Threading.Tasks;

namespace WebPageParser.DocumentParsing {
	public class Parser {
		public Models.GameBasicInfo GameBasicInfo { get; set; }

		public void Parse(string htmlText) {
			var doc = CsQuery.CQ.CreateDocument(htmlText);
			GameBasicInfo = new ParseBasicInfo(doc).Parse();
		}
	}
}
