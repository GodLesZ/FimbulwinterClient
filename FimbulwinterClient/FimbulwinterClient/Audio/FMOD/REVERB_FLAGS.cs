using System.Runtime.InteropServices;

namespace FimbulwinterClient.Audio.FMOD {
	[StructLayout(LayoutKind.Sequential)]
	public struct REVERB_FLAGS
	{
		public const uint HIGHQUALITYREVERB     = 0x00000400; /* Wii. Use high quality reverb */
		public const uint HIGHQUALITYDPL2REVERB = 0x00000800; /* Wii. Use high quality DPL2 reverb */
		public const uint DEFAULT               = 0x00000000;
	}
}