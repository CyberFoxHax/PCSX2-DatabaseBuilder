namespace WebPageParser.Models{
	public class LabelAttribute : System.Attribute{
		public LabelAttribute(string label){
			Label = label;
		}

		public string Label { get; set; }
	}
}