/*
 * (c) 2008 The Ensemble OS Project
 * http://www.ensemble-os.org
 * All Rights Reserved
 *
 * This code is covered by the New BSD License, found in license.txt
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 * PCIBaseAddress.cs: 
*/

using Mosa.DeviceDrivers;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.PCI
{
    public enum AddressRegion : byte
    {
        IO,
        Memory,
        Unimplemented
    }

    public class PCIBaseAddress
    {
        protected uint address;
        protected uint size;
        protected AddressRegion region;
        protected bool prefetchable;

        public uint Address { get { return address; } }
        public uint Size { get { return size; } }
        public AddressRegion Region { get { return region; } }
        public bool Prefetchable { get { return prefetchable; } }

        public PCIBaseAddress()
        {
            region = AddressRegion.Unimplemented;
        }

        public PCIBaseAddress(AddressRegion region, uint address, uint size, bool prefetchable)
        {
            this.region = region;
            this.address = address;
            this.size = size;
            this.prefetchable = prefetchable;
        }

        public override string ToString()
        {
            if (region == AddressRegion.Unimplemented)
                return string.Empty;

            if (region == AddressRegion.IO)
                return "I/O Port at 0x" + address.ToString("X") + " [size=" + size.ToString() + "]";

            if (prefetchable)
                return "Memory at 0x" + address.ToString("X") + " [size=" + size.ToString() + "] (prefetchable)";

            return "Memory at 0x" + address.ToString("X") + " [size=" + size.ToString() + "] (non-prefetchable)";
        }

    }
}