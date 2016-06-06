namespace WebPageParser.Models{
	[System.Data.Linq.Mapping.Table]
	public class GraphicsPlugin {
		[System.Data.Linq.Mapping.Column(IsPrimaryKey = true)]
		public int Id { get; set; }
		public Enum.RendererBackend RenderMode { get; set; }
		public bool UseHardwareRenderer { get; set; }
	}
}