using System.Collections.Generic;
using System.Data.Linq.Mapping;

namespace WebPageParser.Models{
	public class GameBasicInfo {
		[Column(IsPrimaryKey = true)]
		public long Id { get; set; }

		public string Title { get; set; }
		public string Developer { get; set; }
		public string Genre { get; set; }
		public string UrlPcsx2Wiki { get; set; }
		public byte? ReviewScore { get; set; }
		public string Description { get; set; }

		public virtual IEnumerable<GameDisc> GameDiscs { get; set; }
	}
}