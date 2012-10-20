using System.Runtime.InteropServices;

namespace FimbulwinterClient.Audio.FMOD {
	[StructLayout(LayoutKind.Sequential)]
	public struct REVERB_PROPERTIES
	{                                   /*          MIN     MAX    DEFAULT   DESCRIPTION */
		public int   Instance;          /* [in]     0     , 3     , 0      , EAX4 only. Environment Instance. 3 seperate reverbs simultaneously are possible. This specifies which one to set. (win32 only) */
		public int   Environment;       /* [in/out] -1    , 25    , -1     , sets all listener properties (win32/ps2) */
		public float EnvDiffusion;      /* [in/out] 0.0   , 1.0   , 1.0    , environment diffusion (win32/xbox) */
		public int   Room;              /* [in/out] -10000, 0     , -1000  , room effect level (at mid frequencies) (win32/xbox) */
		public int   RoomHF;            /* [in/out] -10000, 0     , -100   , relative room effect level at high frequencies (win32/xbox) */
		public int   RoomLF;            /* [in/out] -10000, 0     , 0      , relative room effect level at low frequencies (win32 only) */
		public float DecayTime;         /* [in/out] 0.1   , 20.0  , 1.49   , reverberation decay time at mid frequencies (win32/xbox) */
		public float DecayHFRatio;      /* [in/out] 0.1   , 2.0   , 0.83   , high-frequency to mid-frequency decay time ratio (win32/xbox) */
		public float DecayLFRatio;      /* [in/out] 0.1   , 2.0   , 1.0    , low-frequency to mid-frequency decay time ratio (win32 only) */
		public int   Reflections;       /* [in/out] -10000, 1000  , -2602  , early reflections level relative to room effect (win32/xbox) */
		public float ReflectionsDelay;  /* [in/out] 0.0   , 0.3   , 0.007  , initial reflection delay time (win32/xbox) */
		public int   Reverb;            /* [in/out] -10000, 2000  , 200    , late reverberation level relative to room effect (win32/xbox) */
		public float ReverbDelay;       /* [in/out] 0.0   , 0.1   , 0.011  , late reverberation delay time relative to initial reflection (win32/xbox) */
		public float ModulationTime;    /* [in/out] 0.04  , 4.0   , 0.25   , modulation time (win32 only) */
		public float ModulationDepth;   /* [in/out] 0.0   , 1.0   , 0.0    , modulation depth (win32 only) */
		public float HFReference;       /* [in/out] 1000.0, 20000 , 5000.0 , reference high frequency (hz) (win32/xbox) */
		public float LFReference;       /* [in/out] 20.0  , 1000.0, 250.0  , reference low frequency (hz) (win32 only) */
		public float Diffusion;         /* [in/out] 0.0   , 100.0 , 100.0  , Value that controls the echo density in the late reverberation decay. (xbox only) */
		public float Density;           /* [in/out] 0.0   , 100.0 , 100.0  , Value that controls the modal density in the late reverberation decay (xbox only) */
		public uint  Flags;             /* [in/out] REVERB_FLAGS - modifies the behavior of above properties (win32/ps2) */

		#region wrapperinternal
		public REVERB_PROPERTIES(int instance, int environment, float envDiffusion, int room, int roomHF, int roomLF,
		                         float decayTime, float decayHFRatio, float decayLFRatio, int reflections, float reflectionsDelay,
		                         int reverb, float reverbDelay, float modulationTime, float modulationDepth, float hfReference,
		                         float lfReference, float diffusion, float density, uint flags)
		{
			Instance            = instance;
			Environment         = environment;
			EnvDiffusion        = envDiffusion;
			Room                = room;
			RoomHF              = roomHF;
			RoomLF              = roomLF;
			DecayTime           = decayTime;
			DecayHFRatio        = decayHFRatio;
			DecayLFRatio        = decayLFRatio;
			Reflections         = reflections;
			ReflectionsDelay    = reflectionsDelay;
			Reverb              = reverb;
			ReverbDelay          = reverbDelay;
			ModulationTime      = modulationTime;
			ModulationDepth     = modulationDepth;
			HFReference         = hfReference;
			LFReference         = lfReference;
			Diffusion           = diffusion;
			Density             = density;
			Flags               = flags;
		}
		#endregion
	}
}