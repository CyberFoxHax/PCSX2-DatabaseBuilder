namespace WebPageParser.Models{
	public class GameDiskInfo{
		public string Id { get; set; }
		public string Tag { get; set; }
		public System.DateTime ReleaseDate { get; set; }
		public Language[] AvailableLanguages { get; set; }
		public GameDisk GameDisk { get; set; }
	}
}