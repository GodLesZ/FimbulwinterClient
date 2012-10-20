using System;
using System.Runtime.InteropServices;

namespace FimbulwinterClient.Audio.FMOD {
	[StructLayout(LayoutKind.Sequential)]
	public struct REVERB_CHANNELPROPERTIES  
	{                                          /*          MIN     MAX    DEFAULT  DESCRIPTION */
		public int       Direct;               /* [in/out] -10000, 1000,  0,       direct path level (at low and mid frequencies) (win32/xbox) */
		public int       Room;                 /* [in/out] -10000, 1000,  0,       room effect level (at low and mid frequencies) (win32/xbox) */
		public uint      Flags;                /* [in/out] REVERB_CHANNELFLAGS - modifies the behavior of properties (win32) */
		public IntPtr    ConnectionPoint;      /* [in/out] See remarks.            DSP network location to connect reverb for this channel.    (SUPPORTED:SFX only).*/
	}
}