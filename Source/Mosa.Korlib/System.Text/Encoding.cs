// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
