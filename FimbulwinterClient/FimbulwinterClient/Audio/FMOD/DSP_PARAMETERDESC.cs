using System.Runtime.InteropServices;

namespace FimbulwinterClient.Audio.FMOD {
	public struct DSP_PARAMETERDESC
	{
		public float         min;             /* [in] Minimum value of the parameter (ie 100.0). */
		public float         max;             /* [in] Maximum value of the parameter (ie 22050.0). */
		public float         defaultval;      /* [in] Default value of parameter. */
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public char[]        name;            /* [in] Name of the parameter to be displayed (ie "Cutoff frequency"). */
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public char[]        label;           /* [in] Short string to be put next to value to denote the unit type (ie "hz"). */
		public string        description;     /* [in] Description of the parameter to be displayed as a help item / tooltip for this parameter. */
	}
}