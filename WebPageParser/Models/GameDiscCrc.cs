namespace WebPageParser.Models{
	public class GameDiscCrc{
		[System.ComponentModel.DataAnnotations.Key]
		public long Id { get; set; }
		public int Crc { get; set; }
		public string Tag { get; set; }

		//public long GameDiskId { get; set; }
		public virtual GameDisc GameDisc { get; set; }

		public override string ToString() {
			return Crc.ToString("X") + (string.IsNullOrEmpty(Tag) ? "" : " (" + Tag + ")");
		}
	}
}