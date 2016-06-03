namespace DataScraperRaw{
	public class Request {
		private readonly string _url;
		public RequestState State { get; set; }
		public string GameName { get; set; }
		internal RequestHelper.RequestHandler Handler;

		internal string Response;

		public Request(string url, System.Windows.Threading.Dispatcher dispatcher){
			_url = url;
			State = RequestState.Waiting;
			GameName = url.Replace("_", " ");
			CreateHandler();
		}

		public void CreateHandler(){
			Handler = new RequestHelper.RequestHandler(GamesUrlsList.UrlPrefix + _url) {
				Timeout = 10000
			};
			Handler.OnSuccess += HandlerOnOnSuccess;
			Handler.OnError += HandlerOnOnError;
		}

		public void Send(){
			Handler.SendCallback();
			State = RequestState.Running;
		}

		private void HandlerOnOnError(RequestHelper.RequestHandler requestHandler){
			Handler.SendCallback();
		}

		private void HandlerOnOnSuccess(RequestHelper.RequestHandler requestHandler){
			try{
				Response = requestHandler.ResponseAs<string>();
			}
			catch{
				HandlerOnOnError(requestHandler);
			}
			State = RequestState.Finished;

			if (Change != null) Change(this);
		}

		public event System.Action<Request> Change;
	}
}