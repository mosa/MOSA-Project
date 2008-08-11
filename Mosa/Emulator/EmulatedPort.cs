/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.Emulator
{
    public class EmulatedPort : IReadWriteIOPort
    {
        protected ushort port;

        public EmulatedPort(ushort port) { this.port = port; }

        public ushort Address { get { return port; } }

        public byte Read8() { return EmulatedPorts.Read8(port); }
        public ushort Read16() { return EmulatedPorts.Read16(port); }
        public uint Read32() { return EmulatedPorts.Read32(port); }

        public void Write8(byte data) { EmulatedPorts.Write8(port, data); }
        public void Write16(ushort data) { EmulatedPorts.Write16(port, data); }
        public void Write32(uint data) { EmulatedPorts.Write32(port, data); }
    }
}
