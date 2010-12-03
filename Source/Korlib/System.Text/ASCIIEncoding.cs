/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Text
{
	/// <summary>
	/// Implementation of the "ASCIIEncoding" class.
	/// </summary>
	public class ASCIIEncoding : Encoding
	{
		// Decode a buffer of bytes into a string.
		public unsafe override String GetString(byte[] bytes, int byteIndex, int byteCount)
		{
			if (byteCount == 0)
				return String.Empty;

			string result = String.InternalAllocateString(byteCount);

			char* chars = result.first_char;

			for (int index = byteIndex; index < byteIndex + byteCount; index++)
			{
				byte b = bytes[index];
				*chars = (b <= 0x7F) ? (char)b : (char)'?';
				chars++;
			}

			return result;
		}

	}
}
