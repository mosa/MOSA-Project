// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal.BootMemory
{
	public static class BootMemoryMap
	{
		private static BootMemoryMapTable Map;

		public static void Initialize()
		{
			var entry = Platform.GetMemoryMapAddress();

			Map = new BootMemoryMapTable(entry)
			{
				Count = 0
			};
		}

		public static BootMemoryMapEntry SetMemoryMap(IntPtr address, ulong size, BootMemoryMapType type)
		{
			var entry = Map.GetBootMemoryMapEntry(Map.Count);

			entry.Address = address;
			entry.Size = size;
			entry.Type = type;

			Map.Count += 1;

			return entry;
		}
	}
}
