using System.ComponentModel.DataAnnotations;

namespace WebPageParser.Models{
	public class GameDiscCrc{
		[Key]
		public long Id { get; set; }
		public int Crc { get; set; }

		[StringLength(50)]
		public string Tag { get; set; }

		public virtual GameDiscRegion GameDiscRegion { get; set; }

		public override string ToString() {
			return Crc.ToString("X") + (string.IsNullOrEmpty(Tag) ? "" : " (" + Tag + ")");
		}
	}
}