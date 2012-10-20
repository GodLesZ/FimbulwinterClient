﻿using System.Text;
using System.IO;

namespace FimbulwinterClient.Extensions {
	public static class BinaryReaderExtensions
	{
		public static string ReadCString(this BinaryReader br)
		{
			StringBuilder str = new StringBuilder();

			do
			{
				byte b = br.ReadByte();

				if (b == 0)
					break;

				str.Append((char)b);
			}
			while (true);

			return str.ToString();
		}

		public static string ReadCString(this BinaryReader br, int size)
		{
			int i;
			StringBuilder str = new StringBuilder(size);

			for (i = 0; i < size; i++)
			{
				byte b = br.ReadByte();

				if (b == 0)
					break;

				str.Append((char)b);
			}

			if (i < size)
				br.ReadBytes(size - i - 1);

			return str.ToString();
		}
	}
}

