namespace WebPageParser.Models{
	public class PcsxProperties{
		public string Bios { get; set; }
		public EmulationSettings EmulationSettings { get; set; }

		public Plugin SoundPlugin { get; set; }
		public Plugin InputPlugin { get; set; }
		public GraphicsPlugin GraphicsPlugin { get; set; }
	}
}