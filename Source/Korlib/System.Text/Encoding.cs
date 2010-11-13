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
	/// Implementation of the "Encoder" class.
	/// </summary>
	public abstract class Encoding
	{

		public abstract String GetString(byte[] bytes, int index, int count);

		public virtual String GetString(byte[] bytes)
		{
			return GetString(bytes, 0, bytes.Length);
		}

	}
}
