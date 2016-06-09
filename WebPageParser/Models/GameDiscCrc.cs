namespace WebPageParser.Models{
	public class GameDiscCrc{
		[System.ComponentModel.DataAnnotations.Key]
		public long Id { get; set; }
		public int Crc { get; set; }
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public string Tag { get; set; }

		public virtual GameDisc GameDisc { get; set; }

		public override string ToString() {
			return Crc.ToString("X") + (string.IsNullOrEmpty(Tag) ? "" : " (" + Tag + ")");
		}
	}
}