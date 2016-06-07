using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WebPageParser {
	public partial class App{
		public App(){
			System.IO.File.Delete(Environment.CurrentDirectory + "\\" + Context.PcsxContext.DatabaseName);
			var ctx = new Context.PcsxContext();
			ctx.CreateTables();
		}
	}
}
