// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public class PCIDeviceDriverRegistryEntry : DeviceDriverRegistryEntry
{
	public override DeviceBusType BusType => DeviceBusType.PCI;

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
