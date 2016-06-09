using System.Data.Linq.Mapping;

namespace WebPageParser.Models{
	[Table]
	public class GraphicsPlugin : Plugin{
		[Column(IsPrimaryKey = true)]
		public int Id { get; set; }
		public Enum.RendererBackend RenderMode { get; set; }
		public bool UseHardwareRenderer { get; set; }
	}
}