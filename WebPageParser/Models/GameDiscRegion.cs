using System.ComponentModel.DataAnnotations;

namespace WebPageParser.Models{
	public class GameDiscRegion {
		[Key]
		public long Id { get; set; }
		
		public System.DateTime ReleaseDate { get; set; }

		public Enum.Region Region { get; set; }
		public Enum.Playable PlayableWindows { get; set; }
		public Enum.Playable PlayableLinux { get; set; }
		public Enum.Playable PlayableMac { get; set; }

		public virtual GameBasicInfo GameBasicInfo { get; set; }

		public virtual System.Collections.Generic.IEnumerable<GameDiscSerial> GameDiscSerialNumbers { get; set; }
		public virtual System.Collections.Generic.IEnumerable<GameDiscCrc> GameDiscCrcs { get; set; }
	}
}