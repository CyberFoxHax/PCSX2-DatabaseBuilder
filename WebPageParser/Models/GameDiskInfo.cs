using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebPageParser.Models{
	public class GameDiskInfo {
		[Key]
		public long GameDiskInfoId { get; set; }
		public string DiskId { get; set; }
		public string Tag { get; set; }
		public int Crc { get; set; }
		public System.DateTime ReleaseDate { get; set; }
		public Enum.Language AvailableLanguages { get; set; }

		public long GameDiskId { get; set; }
		[ForeignKey("GameDiskId")]
		public virtual GameDisk GameDisk { get; set; }
	}
}