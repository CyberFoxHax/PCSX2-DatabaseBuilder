namespace WebPageParser.Models.Enum {
	[System.Flags]
	public enum Language {
		Unspecified = 0x0,
		English		= 0x1,
		Japanese	= 0x2,
		German		= 0x4,
		French		= 0x8,
		Spanish		= 0x10,
		Dutch		= 0x20,
		Italian		= 0x40,
		Swedish		= 0x80,
		Chinese		= 0x100,
		Korean		= 0x200,

		Danish		= 0x400,
		Finnish		= 0x800,
		Norwegian	= 0x1000,
		Portuguese	= 0x2000,
		Greek		= 0x4000,
		Russian		= 0x8000,
		Polish		= 0x10000,
		Czech		= 0x20000,
		Hungarian	= 0x40000,
		Turkish		= 0x80000,
		Hindi		= 0x100000,
		Afrikaans	= 0x200000,
		Croatian	= 0x400000,
		Arabic		= 0x800000,
		Catalan		= 0x1000000,
		Gaelic		= 0x2000000
	}
}
