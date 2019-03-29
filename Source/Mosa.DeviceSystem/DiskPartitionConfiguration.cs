// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Disk Device Configuration
	/// </summary>
	public class DiskPartitionConfiguration : BaseDeviceConfiguration
	{
		public uint StartLBA { get; set; }

		public uint TotalBlocks { get; set; }

		public bool ReadOnly { get; set; }

		public uint Index { get; set; }
	}
}
