// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class PCIDeviceDriverRegistryEntry : DeviceDriverRegistryEntry
	{
		public override DeviceBusType BusType { get { return DeviceBusType.PCI; } }

		public ushort DeviceID { get; set; }

		public ushort VendorID { get; set; }

		public ushort SubSystemVendorID { get; set; }

		public ushort SubSystemID { get; set; }

		public byte RevisionID { get; set; }

		public byte ProgIF { get; set; }

		public ushort ClassCode { get; set; }

		public byte SubClassCode { get; set; }

		public PCIField PCIFields { get; set; }
	}

	[System.Flags]
	public enum PCIField : byte
	{
		DeviceID = 1,
		VendorID = 2,
		SubSystemVendorID = 4,
		SubSystemID = 8,
		RevisionID = 16,
		ProgIF = 32,
		ClassCode = 64,
		SubClassCode = 128
	}
}
