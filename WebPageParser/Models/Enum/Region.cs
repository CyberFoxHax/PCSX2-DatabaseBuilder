namespace WebPageParser.Models.Enum{
	public enum Region{
		Unspecified,
		[Label("NTSC")]   Ntsc,
		[Label("NTSC-U")] NtscU,
		[Label("NTSC-J")] NtscJ,
		[Label("NTSC-K")] NtscK,
		[Label("PAL")]    Pal ,
		[Label("PAL-A")]  PalA,
		[Label("PAL-B")]  PalB,
	}
}