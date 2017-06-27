// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger
{
	public class WatchEntry
	{
		public string Name { get; private set; }

		public ulong Address { get; private set; }

		public int Size { get; private set; }

		public bool Signed { get; private set; }

		public WatchEntry(string name, ulong address, int size, bool signed = false)
		{
			Name = name;
			Address = address;
			Size = size;
			Signed = signed;
		}
	}
}
