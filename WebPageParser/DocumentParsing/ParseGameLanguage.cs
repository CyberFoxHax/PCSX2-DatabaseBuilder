using System.Collections.Generic;
using System.Linq;

namespace WebPageParser.DocumentParsing{
	public class ParseGameLanguage{

		public ParseGameLanguage(CsQuery.CQ doc){
			Doc = doc;
		}

		private CsQuery.CQ Doc { get; set; }

		public List<LangaugeGroup> Parse(){
			var elms = Doc["table[style=\"border: 0px; text-align:center; width:35px; height:32px; float:left; vertical-align: middle;\"] a > img"];

			var groups = elms
				.Select(p => p.GetAttribute("alt").Split(':'))
				.ToDictionary(
					p => (Models.Enum.Language)System.Enum.Parse(typeof(Models.Enum.Language), p[0]),
					p => p[1].Split('&').Select(pp => pp.Trim())
				).SelectMany(
					p=>p.Value.Select(pp => new{
						Language = p.Key,
						DiskId = pp
					})
				)
				.GroupBy(
					p=>p.DiskId,
					p=>p.Language
				)
				.Select(p => new LangaugeGroup {
					Languages = p.Aggregate(Models.Enum.Language.Unspecified, (a,b) => a | b),
					GameDiskId = p.Key
				})
				.ToList();

			return groups;
		}

		public class LangaugeGroup{
			public Models.Enum.Language Languages { get; set; }
			public string GameDiskId { get; set; }
		}
	}
}