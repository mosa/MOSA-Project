// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.HardwareSystem
{
	public interface IISADeviceDriver : IDeviceDriver
	{
		ushort BasePort { get; }

		ushort PortRange { get; }

		ushort AltBasePort { get; }

		ushort AltPortRange { get; }

		bool AutoLoad { get; }

		string ForceOption { get; }

		byte IRQ { get; }

		uint BaseAddress { get; }

		uint AddressRange { get; }

		LinkedList<DeviceDriverPhysicalMemory> PhysicalMemory { get; }
	}
}
