using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageParser.Models {
	public enum Language {
		Unspecified,
		English, Japanese, German, French, Spanish, Dutch, Italian, Swedish, Chinese, Korean
	}

	public enum Playable {
		Unspecified,
		Playable,
		Ingame,
		Broken
	}

	public enum RendererBackend{
		DirectX9,
		DirectX10,
		DirectX11,
		OpenGL3
	}

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

	public class GameBasicInfo {
		public string Title { get; set; }
		public string Developer { get; set; }
		public string Genre { get; set; }
		public string UrlPcsx2Wiki { get; set; }
		public byte ReviewScore { get; set; }
		public string Description { get; set; }
	}

	public class GameDiskInfo{
		public string Id { get; set; }
		public string Tag { get; set; }
		public DateTime ReleaseDate { get; set; }
		public Language[] AvailableLanguages { get; set; }
		public GameDisk GameDisk { get; set; }
	}

	public class GameDisk {
		public Region Region { get; set; }
		public Playable PlayableWindows { get; set; }
		public Playable PlayableLinux { get; set; }
		public Playable PlayableMac { get; set; }
		public PcsxProperties PcsxProperties { get; set; }

		public GameBasicInfo GameBasicInfo { get; set; }
	}

	public enum Compile {
		Interpreter,
		Recompiler,

		MicroVURecompiler,
		SuperVURecompiler
	}

	public enum RoundMode {
		Nearest,
		Negative,
		Positive,
		ChopZero
	}

	public enum ClampMode {
		None,
		Normal,
		ExtraPreserveSign,
		Full
	}

	public class EmulationSettings{
		public Compile EECompile { get; set; }
		public bool EECache { get; set; }
		public Compile IopCompile { get; set; }
		public RoundMode EEFPURoundMode { get; set; }
		public ClampMode EEFPUClampMode { get; set; }
		public Compile V0Compile { get; set; }
		public Compile V1Compile { get; set; }
		public RoundMode V0V1RoundMode { get; set; }
		public ClampMode V0V1ClampMode { get; set; }
	}

	public class Plugin{
		public PluginKeys PluginKeyKey { get; set; }
		public string Version { get; set; }
	}

	public class GraphicsPlugin{
		public RendererBackend RenderMode { get; set; }
		public bool UseHardwareRenderer { get; set; }
	}

	public class PcsxProperties{
		public string Bios { get; set; } // Europe v1.60
		public EmulationSettings EmulationSettings { get; set; }

		public Plugin SoundPlugin { get; set; }
		public Plugin InputPlugin { get; set; }
		public GraphicsPlugin GraphicsPlugin { get; set; }
	}

	public enum PluginKeys{
		[Label("SPU-2")] Spu2,
		[Label("GSDX")] Gsdx,
		Lilypad
	}
}
