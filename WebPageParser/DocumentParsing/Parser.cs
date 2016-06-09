using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageParser.DocumentParsing {
	public class Parser {
		public Models.GameBasicInfo GameBasicInfo { get; set; }

		public void Parse(string htmlText, string url) {
			var doc = CsQuery.CQ.CreateDocument(htmlText);
			GameBasicInfo = new ParseBasicInfo(doc).Parse();
			var gameDisks = new ParseGameDisk(doc).Parse();
			var languages = new ParseGameLanguage(doc).Parse();
			if(languages.Any())
				foreach (var gameDiscId in gameDisks.NotNullMany(p => p.GameDiscIds)) {
					var langaugeGroup = languages.FirstOrDefault(p => p.GameDiskId == gameDiscId.SerialNumber);
					if (langaugeGroup != null)
						gameDiscId.AvailableLanguages = langaugeGroup.Languages;
				}

			GameBasicInfo.UrlPcsx2Wiki = url;
			GameBasicInfo.GameDiscs = gameDisks;
			gameDisks.ForEach(p=>p.GameBasicInfo = GameBasicInfo);
		}
	}
}
