using System.ComponentModel.DataAnnotations;

namespace WebPageParser.Models{
	public class Plugin {
		[Key]
		public int Id { get; set; }
		public Enum.PluginKeys PluginKeyKey { get; set; }
		public string Version { get; set; }
	}
}