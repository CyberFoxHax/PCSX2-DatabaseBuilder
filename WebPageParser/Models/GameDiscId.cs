namespace WebPageParser.Models{
	public class GameDiscId{
		public long Id { get; set; }
		public string DiskId { get; set; }
		[System.ComponentModel.DataAnnotations.StringLength(80)]
		public string Tag { get; set; }
		public Enum.Language AvailableLanguages { get; set; }

		public virtual GameDisc GameDisc { get; set; }

		public override string ToString(){
			return DiskId + (string.IsNullOrEmpty(Tag)?"":" ("+Tag+")");
		}
	}
}