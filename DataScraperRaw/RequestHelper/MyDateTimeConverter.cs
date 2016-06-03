using System;
using Newtonsoft.Json;

namespace DataScraperRaw.RequestHelper {
	public class MyDateTimeConverter : Newtonsoft.Json.Converters.IsoDateTimeConverter {
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (typeof(DateTime) == objectType)
				return UnixToDateTime((double)reader.Value);
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}


		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer){
			if (value is DateTime)
				base.WriteJson(writer, DateTimeToUnix((DateTime)value), serializer);
			base.WriteJson(writer, value, serializer);
		}

		private static DateTime UnixToDateTime(double milliSeconds) { //时间戳
			var dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			dateTime = dateTime.AddMilliseconds(milliSeconds);
			return dateTime;
		}

		private static double DateTimeToUnix(DateTime dateTime) {
			var unixTime = (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
			if (dateTime.IsDaylightSavingTime())
				unixTime -= 3600;
			return unixTime;
		}
	}
}
