// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class PCIDeviceDriver : IDeviceDriver
	{
		public ushort DeviceID { get; set; }

		public ushort VendorID { get; set; }

		public ushort SubSystemVendorID { get; set; }

		public ushort SubSystemID { get; set; }

		public byte RevisionID { get; set; }

		public byte ProgIF { get; set; }

		public ushort ClassCode { get; set; }

		public byte SubClassCode { get; set; }

		public PCIFields PCIFields { get; set; }

		public PlatformArchitecture Platforms { get; set; }

		public DeviceBusType BusType { get { return DeviceBusType.PCI; } }

		public string Name { get; set; }

		public InstantiateDevice Factory { get; set; }
	}

	[System.Flags]
	public enum PCIFields : byte
	{
		DeviceID = 1,
		VendorID = 2,
		SubSystemVendorID = 4,
		SubSystemID = 8,
		RevisionID = 16,
		ProgIF = 32,
		ClassCode = 64,
		SubClassCode = 128
	};
}
