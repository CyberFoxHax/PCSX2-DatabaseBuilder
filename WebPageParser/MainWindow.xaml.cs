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
using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;

namespace WebPageParser {
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
		 }

		protected override async void OnInitialized(EventArgs e){
			var regex = new System.Text.RegularExpressions.Regex("#([0-9]+).html");
			var files = System.IO.Directory
				.GetFiles(Environment.CurrentDirectory + "\\pages\\")
				.OrderBy(p => int.Parse(regex.Match(p).Groups[1].Value))
				//.Skip(1074).Take(1)
				//.Skip(1003).Take(1)
				.Select(System.IO.File.ReadAllText)
				.ToList();

			var stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			var result = new Models.GameBasicInfo[files.Count];
			{
				var grouped = new List<List<string>>();
				var groupSize = files.Count / Environment.ProcessorCount;
				for (var i = 0; i < files.Count; i += groupSize)
					grouped.Add(files.Skip(i).Take(groupSize).ToList());

				var diu = new object();

				Parallel.ForEach(grouped, (items, state, i) =>{
					var tempResult = new List<Models.GameBasicInfo>(items.Count);
					var parser = new DocumentParsing.Parser();
					for (var ii = 0; ii < items.Count; ii++) {
						parser.Parse(items[ii]);
						tempResult.Add(parser.GameBasicInfo);
						result[i * groupSize + ii] = parser.GameBasicInfo;
					}
				});
			}
			stopwatch.Stop();

			var resultCount = result.Count(p => p != null);

			for (var i = 0; i < result.Length; i++){
				result[i].UrlPcsx2Wiki = GamesUrlsList.Urls[i];
			}

			Content = new TextBlock { Text = stopwatch.Elapsed.ToString() };

			var context = new Context.PcsxContext();
			context.GameBasicInfoes.AddRange(result.Where(p=>p != null));
			await context.SaveChangesAsync();

			Content = new TextBlock { Text = "Saved" };
		}
	}
}
