using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebPageParser.Models{
	public class GameDisk {
		public long GameDiskId { get; set; }
		public Enum.Region Region { get; set; }
		public Enum.Playable PlayableWindows { get; set; }
		public Enum.Playable PlayableLinux { get; set; }
		public Enum.Playable PlayableMac { get; set; }
		public virtual PcsxProperties PcsxProperties { get; set; }
		public virtual GameBasicInfo GameBasicInfo { get; set; }

		public virtual ICollection<GameDiskInfo> GameDiskInfos { get; set; }
	}
}