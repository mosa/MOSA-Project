/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.PCI
{
	public class PCIBaseAddress
	{
		protected uint address;
		protected uint size;
		protected PCIAddressRegion region;
		protected bool prefetchable;

		public uint Address { get { return address; } }
		public uint Size { get { return size; } }
		public PCIAddressRegion Region { get { return region; } }
		public bool Prefetchable { get { return prefetchable; } }

		public PCIBaseAddress(PCIAddressRegion region, uint address, uint size, bool prefetchable)
		{
			this.region = region;
			this.address = address;
			this.size = size;
			this.prefetchable = prefetchable;
		}

		public override string ToString()
		{
			if (region == PCIAddressRegion.Undefined)
				return string.Empty;

			if (region == PCIAddressRegion.IO)
				return "I/O Port at 0x" + address.ToString("X") + " [size=" + size.ToString() + "]";

			if (prefetchable)
				return "Memory at 0x" + address.ToString("X") + " [size=" + size.ToString() + "] (prefetchable)";

			return "Memory at 0x" + address.ToString("X") + " [size=" + size.ToString() + "] (non-prefetchable)";
		}

	}
}