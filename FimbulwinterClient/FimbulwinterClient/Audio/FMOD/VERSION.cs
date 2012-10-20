namespace FimbulwinterClient.Audio.FMOD {
	public class VERSION
	{
		public const int    number = 0x00044000;
#if WIN64
        public const string dll    = "fmodex64";
#else
		public const string dll    = "fmodex";
#endif
	}
}