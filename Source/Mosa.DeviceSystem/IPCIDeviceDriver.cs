// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public interface IPCIDeviceDriver : IDeviceDriver
	{
		ushort DeviceID { get; }

		ushort VendorID { get; }

		ushort SubSystemVendorID { get; }

		ushort SubSystemID { get; }

		byte RevisionID { get; }

		byte ProgIF { get; }

		ushort ClassCode { get; }

		byte SubClassCode { get; }

		PCIFields PCIFields { get; }
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
