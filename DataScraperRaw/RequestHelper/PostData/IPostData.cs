﻿namespace DataScraperRaw.RequestHelper.PostData{
	public interface IPostData{
		System.IO.Stream GetStream();
		string ContentType { get; }
	}
}