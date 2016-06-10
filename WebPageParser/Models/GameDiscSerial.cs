using System.ComponentModel.DataAnnotations;

namespace WebPageParser.Models{
	public class GameDiscSerial{
		public long Id { get; set; }
		[StringLength(14)] public string SerialNumber { get; set; }
		[StringLength(80)] public string Tag { get; set; }
		public Enum.Language AvailableLanguages { get; set; }

		public virtual GameDiscRegion GameDiscRegion { get; set; }

		public override string ToString(){
			return SerialNumber + (string.IsNullOrEmpty(Tag)?"":" ("+Tag+")");
		}
	}
}