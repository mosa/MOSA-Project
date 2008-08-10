/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

#if ENSEMBLE

namespace Mosa.Devices.Kernel
{
    public class IOPortKernelAdapter : IReadWriteIOPort
    {
        protected Ensemble.Kernel.Platforms.IIOPort ioPort;

        public IOPortKernelAdapter(Ensemble.Kernel.Platforms.IIOPort ioPort) { this.ioPort = ioPort; }

        public ushort Address { get { return ioPort.Address; } }

        public byte Read8Bits() { return ioPort.Read8Bits(); }
        public void Write8Bits(byte data) { ioPort.Write8Bits(data); }
        public ushort Read16Bits() { return ioPort.Read16Bits(); }
        public void Write16Bits(ushort data) { ioPort.Write16Bits(data); }
        public uint Read32Bits() { return ioPort.Read32Bits(); }
        public void Write32Bits(uint data) { ioPort.Write32Bits(data); }
    }
}

#endif