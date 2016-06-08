namespace WebPageParser.Models{
	public class GameDiscId{
		public long Id { get; set; }
		public string DiskId { get; set; }
		public string Tag { get; set; }

		//public long GameDiskId { get; set; }
		public virtual GameDisc GameDisc { get; set; }

		public override string ToString(){
			return DiskId + (string.IsNullOrEmpty(Tag)?"":" ("+Tag+")");
		}
	}
}