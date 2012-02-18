/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

namespace Mosa.Utility.IsoImage
{
	static class ConvertTo
	{
		/// <summary>
		/// converts a short integer value to an LSB byte array
		/// </summary>
		/// <param Name="i">the short integer value to convert</param>
		/// <returns>the LSB byte array</returns>
		public static byte[] Short2LSB(short i)
		{
			byte[] b = new byte[2];
			b[0] = (byte)(i & 0xff);
			i >>= 8;
			b[1] = (byte)(i & 0xff);
			return b;
		}

		/// <summary>
		/// converts a short integer value to an MSB byte array
		/// </summary>
		/// <param Name="i">the short integer value to convert</param>
		/// <returns>the MSB byte array</returns>
		public static byte[] Short2MSB(short i)
		{
			byte[] b = new byte[2];
			b[1] = (byte)(i & 0xff);
			i >>= 8;
			b[0] = (byte)(i & 0xff);
			return b;
		}

		/// <summary>
		/// converts a 32-bit integer value to an LSB byte array
		/// </summary>
		/// <param Name="i">the 32-bit integer value to convert</param>
		/// <returns>the LSB byte array</returns>
		public static byte[] Int2LSB(int i)
		{
			byte[] b = new byte[4];
			b[0] = (byte)(i & 0xff);
			i >>= 8;
			b[1] = (byte)(i & 0xff);
			i >>= 8;
			b[2] = (byte)(i & 0xff);
			i >>= 8;
			b[3] = (byte)(i & 0xff);
			return b;
		}

		/// <summary>
		/// converts a 32-bit integer value to an MSB byte array
		/// </summary>
		/// <param Name="i">the 32-bit integer value to convert</param>
		/// <returns>the MSB byte array</returns>
		public static byte[] Int2MSB(int i)
		{
			byte[] b = new byte[4];
			b[3] = (byte)(i & 0xff);
			i >>= 8;
			b[2] = (byte)(i & 0xff);
			i >>= 8;
			b[1] = (byte)(i & 0xff);
			i >>= 8;
			b[0] = (byte)(i & 0xff);
			return b;
		}
	}
}
