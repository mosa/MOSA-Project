// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.HardwareSystem
{
	public class DeviceDriverPhysicalMemory
	{
		public uint MemorySize { get; } = 0;
		public uint MemoryAlignment { get; } = 1;
		public bool RestrictUnder16M { get; } = false;
		public bool RestrictUnder4G { get; } = false;
	}
}
