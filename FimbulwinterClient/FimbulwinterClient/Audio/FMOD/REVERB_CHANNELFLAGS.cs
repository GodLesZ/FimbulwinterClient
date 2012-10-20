using System.Runtime.InteropServices;

namespace FimbulwinterClient.Audio.FMOD {
	[StructLayout(LayoutKind.Sequential)]
	public struct REVERB_CHANNELFLAGS
	{
		public const uint INSTANCE0     = 0x00000010; /* SFX/Wii. Specify channel to target reverb instance 0.  Default target. */
		public const uint INSTANCE1     = 0x00000020; /* SFX/Wii. Specify channel to target reverb instance 1. */
		public const uint INSTANCE2     = 0x00000040; /* SFX/Wii. Specify channel to target reverb instance 2. */
		public const uint INSTANCE3     = 0x00000080; /* SFX. Specify channel to target reverb instance 3. */
		public const uint DEFAULT       = INSTANCE0;
	}
}