// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.TinySimulator
{
	public class Watch
	{
		public ulong Address;

		public int Size;

		public Watch(ulong address, int size)
		{
			Address = address;
			Size = size;
		}
	}
}
