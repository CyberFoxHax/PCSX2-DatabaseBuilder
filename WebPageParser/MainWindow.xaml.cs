using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebPageParser {
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();

			System.IO.File.Delete(Environment.CurrentDirectory + "\\" + Context.PcsxContext.DatabaseName);
			var ctx = new Context.PcsxContext();
			ctx.CreateTables();

			var disc = ctx.GameDisks.Add(new Models.GameDisk{
				PlayableLinux = Models.Enum.Playable.Ingame,
				PlayableWindows = Models.Enum.Playable.Broken,
			});
			
			ctx.SaveChanges();

			var discInfo = ctx.GameDiskInfoes.Add(new Models.GameDiskInfo {
				GameDiskId = disc.GameDiskId,
			});

			ctx.SaveChanges();

			ctx = new Context.PcsxContext();
			discInfo = ctx.GameDiskInfoes.First();

		}
	}
}
