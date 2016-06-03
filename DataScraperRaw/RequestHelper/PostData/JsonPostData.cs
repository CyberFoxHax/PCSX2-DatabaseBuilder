using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DataScraperRaw.RequestHelper.PostData{
	public class JsonPostData : IPostData{
		public JsonPostData(){}
		public JsonPostData(object serializeObject): this() {
			SerializeObject = serializeObject;
		}

		public object SerializeObject { get; set; }

		public Stream GetStream() {
			if (SerializeObject == null) return null;
			var stringData = JsonConvert.SerializeObject(SerializeObject, new MyDateTimeConverter());
			var buffer = Encoding.UTF8.GetBytes(stringData);

			var postData = new MemoryStream();
			postData.Write(buffer, 0, buffer.Length);

			return postData;
		}


		public string ContentType {
			get { return "application/json;charset=UTF-8"; }
		}
	}
}