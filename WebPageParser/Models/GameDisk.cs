namespace WebPageParser.Models{
	public class GameDisk {
		public Region Region { get; set; }
		public Playable PlayableWindows { get; set; }
		public Playable PlayableLinux { get; set; }
		public Playable PlayableMac { get; set; }
		public PcsxProperties PcsxProperties { get; set; }

		public GameBasicInfo GameBasicInfo { get; set; }
	}
}