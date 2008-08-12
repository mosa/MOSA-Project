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
	public delegate byte Read8();
	public delegate ushort Read16();
	public delegate uint Read32();
	public delegate void Write8(byte data);
	public delegate void Write16(ushort data);
	public delegate void Write32(uint data);

	public static class EmulatedIOPorts
	{
		private static Read8[] read8 = new Read8[ushort.MaxValue];
		private static Read16[] read16 = new Read16[ushort.MaxValue];
		private static Read32[] read32 = new Read32[ushort.MaxValue];

		private static Write8[] write8 = new Write8[ushort.MaxValue];
		private static Write16[] write16 = new Write16[ushort.MaxValue];
		private static Write32[] write32 = new Write32[ushort.MaxValue];

		public static byte Read8(ushort port) { if (read8[port] == null) return 0; else return read8[port](); }
		public static ushort Read16(ushort port) { if (read16[port] == null) return 0; else return read16[port](); }
		public static uint Read32(ushort port) { if (read32[port] == null) return 0; else return read32[port](); }

		public static void Write8(ushort port, byte data) { if (write8[port] != null) write8[port](data); }
		public static void Write16(ushort port, ushort data) { if (write16[port] != null) write16[port](data); }
		public static void Write32(ushort port, uint data) { if (write32[port] != null) write32[port](data); }

		public static void SetRead8(ushort port, Read8 read) { read8[port] = read; }
		public static void SetRead16(ushort port, Read16 read) { read16[port] = read; }
		public static void SetRead32(ushort port, Read32 read) { read32[port] = read; }

		public static void SetWrite8(ushort port, Write8 write) { write8[port] = write; }
		public static void SetWrite16(ushort port, Write16 write) { write16[port] = write; }
		public static void SetWrite32(ushort port, Write32 write) { write32[port] = write; }

		public static IReadWriteIOPort RegisterPort(ushort port)
		{
			return new EmulatedIOPort(port);
		}
	}

}
