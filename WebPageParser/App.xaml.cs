using System;

namespace WebPageParser {
	public partial class App{
		public App(){
			System.IO.File.Delete(Environment.CurrentDirectory + "\\" + Context.PcsxContext.DatabaseName);
			var ctx = new Context.PcsxContext();
			ctx.CreateTables();
		}
	}
}
