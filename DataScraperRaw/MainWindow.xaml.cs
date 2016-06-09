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

namespace DataScraperRaw {
	public partial class MainWindow{
		private List<Request> _allRequests;
		private readonly List<Request> _remaining = new List<Request>();
		private int _index;
		private int _changed;
		private readonly System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
		private readonly List<Request> _completed = new List<Request>();

		public MainWindow() {
			InitializeComponent();
		}

		private int _changes;

		private void TimerOnElapsed(){
			_changes += _changed;
			if (_stopwatch.ElapsedMilliseconds > 5000){
				_stopwatch.Restart();
				if (_changes == 0)
					EtaLabel.Content = "∞";
				else{
					var perSec = (decimal)_changes/5;
					AvgSecLabel.Content = decimal.Round(perSec, 2) + "/s";
					EtaLabel.Content = decimal.Round((decimal)_remaining.Count/perSec/60/60, 1) + "hours";
				}
				_changes = 0;
			}
			if (_changed == 0) {
				return;
			}
			ProgressBar.Value = (double)_completed.Count / _allRequests.Count * 100;
			_changed = 0;
			RequestGrid.Items.Refresh();
			HandledLabel.Content = _completed.Count;
			RemainingLabel.Content = _remaining.Count;
			foreach (var source in _remaining.Where(p => p != null && p.State == RequestState.Running).ToList()) {
				RequestGrid.SelectedItems.Add(source);
			}
		}

		protected override void OnInitialized(EventArgs e){
			base.OnInitialized(e);

			var timer = new System.Timers.Timer();
			timer.Interval = 100;
			timer.AutoReset = true;
			timer.Elapsed += (sender, args) => Dispatcher.BeginInvoke((Action)TimerOnElapsed);
			timer.Start();

			_allRequests = GamesUrlsList.Urls.Select(p => {
				var req = new Request(p, Dispatcher);
				req.Change += ReqOnChange;
				return req;
			}).ToList();

			_remaining.AddRange(_allRequests);
			RequestGrid.ItemsSource = _remaining;

			EtaLabel.Content = "∞";
			TotalLabel.Content = _allRequests.Count;

			var exists = _allRequests.Where((t, i) => System.IO.File.Exists(Environment.CurrentDirectory + "\\pages\\#" + (i + 1) + ".html")).ToList();
			foreach (var request in exists){
				_remaining.Remove(request);
				_completed.Add(request);
			}

			_index = 10;
			_stopwatch.Start();
			foreach (var request in _remaining.Take(_index)){
				request.Send();
			}
			foreach (var source in _remaining.Where(p => p.State == RequestState.Running)){
				RequestGrid.SelectedItems.Add(source);
			}

			HandledLabel.Content = _completed.Count;
			RemainingLabel.Content = _remaining.Count;
			ProgressBar.Value = (double)_completed.Count / _allRequests.Count * 100;
		}

		private void ReqOnChange(Request request){
			_changed++;
			var next = _remaining.FirstOrDefault(p=>p.State == RequestState.Waiting);

			_remaining.Remove(request);
			_completed.Add(request);
			SaveFile(request);

			if (next == null) return;
			next.Send();
		}

		private static readonly List<string> UrlsAsList = GamesUrlsList.Urls.ToList();

		private void SaveFile(Request request){
			try{
				System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\pages\\");
				var file = System.IO.File.CreateText(Environment.CurrentDirectory + "\\pages\\#" + (UrlsAsList.IndexOf(request.Url) + 1) + ".html");
				file.Write(request.Response);
				file.Close();
			}
			catch (Exception){
			}
			
		} 

		private void BrowserOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs){
			var doc = (mshtml.IHTMLDocument2)((WebBrowser)sender).Document;
		}
	}
}

