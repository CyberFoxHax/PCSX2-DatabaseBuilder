namespace WebPageParser.Models{
	[System.Data.Linq.Mapping.Table]
	public class GameBasicInfo {
		[System.Data.Linq.Mapping.Column(IsPrimaryKey = true)]
		public int Id { get; set; }

		public string Title { get; set; }
		public string Developer { get; set; }
		public string Genre { get; set; }
		public string UrlPcsx2Wiki { get; set; }
		public byte? ReviewScore { get; set; }
		public string Description { get; set; }
	}
}