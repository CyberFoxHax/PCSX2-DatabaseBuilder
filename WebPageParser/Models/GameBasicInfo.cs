using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace WebPageParser.Models{
	public class GameBasicInfo {
		[Column(IsPrimaryKey = true)]
		public long Id { get; set; }

		[StringLength(100)]	public string Title { get; set; }
		[StringLength(40)]	public string Developer { get; set; }
		[StringLength(40)]	public string Genre { get; set; }
		[StringLength(100)]	public string UrlPcsx2Wiki { get; set; }
		[StringLength(2000)]public string Description { get; set; }

		public byte? ReviewScore { get; set; }

		public virtual IEnumerable<GameDisc> GameDiscs { get; set; }
	}
}