// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class ISADeviceDriverRegistryEntry : DeviceDriverRegistryEntry
	{
		public override DeviceBusType BusType { get { return DeviceBusType.ISA; } }

		public ushort BasePort { get; set; }

		public ushort PortRange { get; set; }

		public ushort AltBasePort { get; set; }

		public ushort AltPortRange { get; set; }

		public bool AutoLoad { get; set; }

		public string ForceOption { get; set; }

		public uint BaseAddress { get; set; }

		public uint AddressRange { get; set; }
	}
}
