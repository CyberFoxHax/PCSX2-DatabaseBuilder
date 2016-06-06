namespace WebPageParser.Models{
	public class PcsxProperties {
		public int Id { get; set; }
		public string Bios { get; set; }
		public EmulationSettings EmulationSettings { get; set; }
		
		public Plugin SoundPlugin { get; set; }
		public Plugin InputPlugin { get; set; }
		public GraphicsPlugin GraphicsPlugin { get; set; }
	}
}