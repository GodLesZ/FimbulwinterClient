namespace FimbulwinterClient.Audio.FMOD {
	public enum SPEAKERMODE :int
	{
		RAW,              /* There is no specific speakermode.  Sound channels are mapped in order of input to output.  See remarks for more information. */
		MONO,             /* The speakers are monaural. */
		STEREO,           /* The speakers are stereo (DEFAULT). */
		QUAD,             /* 4 speaker setup.  This includes front left, front right, rear left, rear right.  */
		SURROUND,         /* 4 speaker setup.  This includes front left, front right, center, rear center (rear left/rear right are averaged). */
		_5POINT1,         /* 5.1 speaker setup.  This includes front left, front right, center, rear left, rear right and a subwoofer. */
		_7POINT1,         /* 7.1 speaker setup.  This includes front left, front right, center, rear left, rear right, side left, side right and a subwoofer. */

		SRS5_1_MATRIX,    /* Stereo compatible output, embedded with surround information. SRS 5.1/Prologic/Prologic2 decoders will split the signal into a 5.1 speaker set-up or SRS virtual surround will decode into a 2-speaker/headphone setup.  See remarks about limitations. */
		MYEARS,           /* Stereo output, but data is encoded using personalized HRTF algorithms.  See myears.net.au */

		MAX,              /* Maximum number of speaker modes supported. */
	}
}