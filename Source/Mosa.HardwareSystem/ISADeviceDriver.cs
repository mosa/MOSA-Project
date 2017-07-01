// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.HardwareSystem
{
	public class ISADeviceDriver : IDeviceDriver
	{
		public PlatformArchitecture Platforms { get; set; }

		public DeviceBusType BusType { get { return DeviceBusType.ISA; } }

		public string Name { get; set; }

		public InstantiateDevice Factory { get; set; }

		public ushort BasePort { get; set; }

		public ushort PortRange { get; set; }

		public ushort AltBasePort { get; set; }

		public ushort AltPortRange { get; set; }

		public bool AutoLoad { get; set; }

		public string ForceOption { get; set; }

		public byte IRQ { get; set; }

		public uint BaseAddress { get; set; }

		public uint AddressRange { get; set; }
	}
}
