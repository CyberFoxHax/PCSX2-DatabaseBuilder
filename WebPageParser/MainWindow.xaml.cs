using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsQuery.ExtensionMethods;

namespace WebPageParser {
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
		 }

		protected override async void OnInitialized(EventArgs e){
			base.OnInitialized(e);
			OutputText.Text = "Loading Files...";
			var files = await LoadAllFiles();
			OutputText.Text = "Processing...";
			await Task.Run(() => Parse(files));
		}

		private async Task<string[]> LoadAllFiles(){
			var regex = new System.Text.RegularExpressions.Regex("#([0-9]+).html");
			return await Task.Run(() => System.IO.Directory
				.GetFiles(Environment.CurrentDirectory + "\\pages\\")
				.OrderBy(p => int.Parse(regex.Match(p).Groups[1].Value))
				//.Skip(1074).Take(1)
				//.Take(100)
				.Select(System.IO.File.ReadAllText)
				.ToArray()
			);
		}

		private async void Parse(string[] files){
			var stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			var result = new Models.GameBasicInfo[files.Length];
			var completed = 0;
			var grouped = new List<List<string>>();
			var groupSize = (int) Math.Ceiling((double) files.Length/Environment.ProcessorCount);
			for (var i = 0; i < files.Length; i += groupSize)
				grouped.Add(files.Skip(i).Take(groupSize).ToList());

			Parallel.ForEach(grouped, (items, state, i) =>{
				var tempResult = new List<Models.GameBasicInfo>(items.Count);
				var parser = new DocumentParsing.Parser();
				for (var ii = 0; ii < items.Count; ii++){
					parser.Parse(items[ii], GamesUrlsList.Urls[i*groupSize + ii]);
					tempResult.Add(parser.GameBasicInfo);
					result[i*groupSize + ii] = parser.GameBasicInfo;
					completed++;
					if ((completed & 0x10) == 0)
						Dispatcher.Invoke(() =>{
							ProgressBar.Value = (float) completed/files.Length*100;
							OutputText.Text = "Parsing: " + completed + "/" + files.Length;
						});
				}
			});
			stopwatch.Stop();
			Dispatcher.Invoke(new Action(() => OutputText.Text = stopwatch.Elapsed + ""));
			{
				var context = new Context.PcsxContext();

				var basicInfo = result.NotNull().ToArray();
				var gameDiscs		= basicInfo.NotNullMany(p => p.GameDiscs	).ToArray();
				var gameDiscCrcs	= gameDiscs.NotNullMany(p => p.GameDiscCrcs	).ToArray();
				var gameDiscIds		= gameDiscs.NotNullMany(p => p.GameDiscIds	).ToArray();

				context.GameBasicInfoes	.AddRange(basicInfo		);
				context.GameDisks		.AddRange(gameDiscs		);
				context.GameDiskCrcs	.AddRange(gameDiscCrcs	);
				context.GameDiskIds		.AddRange(gameDiscIds	);

				await context.SaveChangesAsync();
			}
			Dispatcher.Invoke(new Action(() => OutputText.Text += "... Saved to DB"));
		}
	}
}
