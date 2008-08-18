/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.EmulatedDevices.Kernel
{
	public class IOPort : IReadWriteIOPort
	{
		protected ushort port;

		public IOPort(ushort port) { this.port = port; }

		public ushort Address { get { return port; } }

		public byte Read8() { return IOPortDispatch.Read8(port); }
		public ushort Read16() { return IOPortDispatch.Read16(port); }
		public uint Read32() { return IOPortDispatch.Read32(port); }

		public void Write8(byte data) { IOPortDispatch.Write8(port, data); }
		public void Write16(ushort data) { IOPortDispatch.Write16(port, data); }
		public void Write32(uint data) { IOPortDispatch.Write32(port, data); }
	}
}
