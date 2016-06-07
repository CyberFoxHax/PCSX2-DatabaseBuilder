namespace WebPageParser.Models.Enum {
	[System.Flags]
	public enum Language {
		Unspecified = 0x0,
		English		= 0x2,
		Japanese	= 0x4,
		German		= 0x8,
		French		= 0x10,
		Spanish		= 0x20,
		Dutch		= 0x40,
		Italian		= 0x80,
		Swedish		= 0x100,
		Chinese		= 0x200,
		Korean		= 0x400
	}
}
