using System.ComponentModel.DataAnnotations;

namespace WebPageParser.Models{
	public class PcsxProperties {
		public int Id { get; set; }

		[StringLength(14)] public string DiscSerialNumber { get; set; }
		[StringLength(100)] public string GameName { get; set; }
		public int GameDiscCrc { get; set; }
		public Enum.Region Region { get; set; }

		public string Bios { get; set; }
		public virtual EmulationSettings EmulationSettings { get; set; }
		public virtual Plugin SoundPlugin { get; set; }
		public virtual Plugin InputPlugin { get; set; }
		public virtual GraphicsPlugin GraphicsPlugin { get; set; }
	}
}