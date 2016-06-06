namespace WebPageParser.Models{
	[System.Data.Linq.Mapping.Table]
	public class Plugin {
		[System.Data.Linq.Mapping.Column(IsPrimaryKey = true)]
		public int Id { get; set; }
		public Enum.PluginKeys PluginKeyKey { get; set; }
		public string Version { get; set; }
	}
}