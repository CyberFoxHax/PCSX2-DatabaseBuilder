namespace DataScraperRaw{
	public class Request {
		public RequestState State { get; set; }
		public string GameName { get; set; }
		internal RequestHelper.RequestHandler Handler;

		internal string Response;

		public Request(string url, System.Windows.Threading.Dispatcher dispatcher){
			State = RequestState.Waiting;
			GameName = url.Replace("_", " ");
			Handler = new RequestHelper.RequestHandler(GamesUrlsList.UrlPrefix + url){
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
			Response = requestHandler.ResponseAs<string>();
			State = RequestState.Finished;

			if (Change != null) Change(this);
		}

		public event System.Action<Request> Change;
	}
}