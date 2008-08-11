/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.EmulatedDevices
{
    public class EmulatedIOPort : IReadWriteIOPort
    {
        protected ushort port;

        public EmulatedIOPort(ushort port) { this.port = port; }

        public ushort Address { get { return port; } }

        public byte Read8() { return EmulatedIOPorts.Read8(port); }
        public ushort Read16() { return EmulatedIOPorts.Read16(port); }
        public uint Read32() { return EmulatedIOPorts.Read32(port); }

        public void Write8(byte data) { EmulatedIOPorts.Write8(port, data); }
        public void Write16(ushort data) { EmulatedIOPorts.Write16(port, data); }
        public void Write32(uint data) { EmulatedIOPorts.Write32(port, data); }
    }
}
