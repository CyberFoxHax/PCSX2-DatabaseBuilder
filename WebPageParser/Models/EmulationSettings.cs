namespace WebPageParser.Models{
	[System.Data.Linq.Mapping.Table]
	public class EmulationSettings{
		[System.Data.Linq.Mapping.Column(IsPrimaryKey = true)]
		public int Id { get; set; }
		// EE/IOP
		public Enum.Compile EECompile { get; set; }
		public bool EECache { get; set; }
		public Enum.Compile IopCompile { get; set; }
		public Enum.RoundMode EEFPURoundMode { get; set; }
		public Enum.ClampMode EEFPUClampMode { get; set; }

		// VUs
		public Enum.VUCompile V0Compile { get; set; }
		public Enum.VUCompile V1Compile { get; set; }
		public Enum.RoundMode V0V1RoundMode { get; set; }
		public Enum.ClampMode V0V1ClampMode { get; set; }

		// GS
		public bool DisableFrameLimit { get; set; }
		public float BaseFramerateAdjust { get; set; }
		public float SlowMotionAdjust { get; set; }
		public float TurboAdjust { get; set; }
		public Enum.FrameSkipMode FrameSkipMode { get; set; }
		public int FramesToDraw { get; set; }
		public int FramesToSkip { get; set; }

		// GSWindow
		public Enum.GSAspectRatio GsAspectRatio { get; set; }
		public int CustomWindowSizeX { get; set; }
		public int CustomWindowSizeY { get; set; }
		public float GSZoom { get; set; }
		public bool DisableResizeBorder { get; set; }
		public bool HideCursor { get; set; }
		public bool DefaultFullscreen { get; set; }
		public bool DoubleFullscreen { get; set; }
		public bool WaitVsync { get; set; }
		public bool DynamicVsync { get; set; }

		// SpeedHacks
		public byte EECycleRate { get; set; }
		public byte VUCycleSteal { get; set; }
		public bool INTCSpinDetect { get; set; }
		public bool WaitLoopDetection { get; set; }
		public bool FastCDVD { get; set; }
		public bool mVUFlagHack { get; set; }
		public bool MTVU { get; set; }

		// GameFixes
		public bool VUAddHack { get; set; }
		public bool VUClipFlagHack{ get; set; }
		public bool FPUCompareHack { get; set; }
		public bool FPUMultiplyHack { get; set; }
		public bool FPUNegativeDivHack { get; set; }
		public bool VUXGkickHack { get; set; }
		public bool FFXVideoFix { get; set; }
		public bool EETimingHack { get; set; }
		public bool SkipMpegHack { get; set; }
		public bool OPHFlagHack { get; set; }
		public bool IgnoreDMAC { get; set; }
		public bool SimulateVIF1FIFO { get; set; }
		public bool DelayVIF1Stalls { get; set; }
		public bool BusDirPath3Trans { get; set; }
		public bool SwitchGSDxSoftware { get; set; } 
	}
}