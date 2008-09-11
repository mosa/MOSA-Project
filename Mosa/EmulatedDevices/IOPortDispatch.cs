/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;
using Mosa.EmulatedDevices.Kernel;
using Mosa.EmulatedDevices.Utils;

namespace Mosa.EmulatedDevices
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	public delegate byte IOPortRead8();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	public delegate ushort IOPortRead16();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	public delegate uint IOPortRead32();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
	public delegate void IOPortWrite8(byte data);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
	public delegate void IOPortWrite16(ushort data);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
	public delegate void IOPortWrite32(uint data);

    /// <summary>
    /// 
    /// </summary>
	public static class IOPortDispatch
	{
        /// <summary>
        /// 
        /// </summary>
		private static IOPortRead8[] read8 = new IOPortRead8[ushort.MaxValue];

        /// <summary>
        /// 
        /// </summary>
		private static IOPortRead16[] read16 = new IOPortRead16[ushort.MaxValue];

        /// <summary>
        /// 
        /// </summary>
		private static IOPortRead32[] read32 = new IOPortRead32[ushort.MaxValue];

        /// <summary>
        /// 
        /// </summary>
		private static IOPortWrite8[] write8 = new IOPortWrite8[ushort.MaxValue];

        /// <summary>
        /// 
        /// </summary>
		private static IOPortWrite16[] write16 = new IOPortWrite16[ushort.MaxValue];

        /// <summary>
        /// 
        /// </summary>
		private static IOPortWrite32[] write32 = new IOPortWrite32[ushort.MaxValue];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
		public static byte Read8(ushort port) { if (read8[port] == null) return 0; else return read8[port](); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
		public static ushort Read16(ushort port) { if (read16[port] == null) return 0; else return read16[port](); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
		public static uint Read32(ushort port) { if (read32[port] == null) return 0; else return read32[port](); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
		public static void Write8(ushort port, byte data) { if (write8[port] != null) write8[port](data); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
		public static void Write16(ushort port, ushort data) { if (write16[port] != null) write16[port](data); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
		public static void Write32(ushort port, uint data) { if (write32[port] != null) write32[port](data); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="read"></param>
		public static void SetRead8(ushort port, IOPortRead8 read) { read8[port] = read; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="read"></param>
		public static void SetRead16(ushort port, IOPortRead16 read) { read16[port] = read; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="read"></param>
		public static void SetRead32(ushort port, IOPortRead32 read) { read32[port] = read; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="write"></param>
		public static void SetWrite8(ushort port, IOPortWrite8 write) { write8[port] = write; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="write"></param>
		public static void SetWrite16(ushort port, IOPortWrite16 write) { write16[port] = write; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="write"></param>
		public static void SetWrite32(ushort port, IOPortWrite32 write) { write32[port] = write; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
		public static IReadWriteIOPort RegisterIOPort(ushort port)
		{
			return new IOPort(port);
		}

		/// <summary>
		/// Registers the port.
		/// </summary>
		/// <param name="port">The port.</param>
		public static void RegisterPort(IOPort<byte> port)
		{
			SetRead8(port.Port, port.ReadValue);
			SetWrite8(port.Port, port.SetValue);
		}

		/// <summary>
		/// Registers the port.
		/// </summary>
		/// <param name="port">The port.</param>
		public static void RegisterPort(IOPort<ushort> port)
		{
			SetRead16(port.Port, port.ReadValue);
			SetWrite16(port.Port, port.SetValue);
		}

		/// <summary>
		/// Registers the port.
		/// </summary>
		/// <param name="port">The port.</param>
		public static void RegisterPort(IOPort<uint> port)
		{
			SetRead32(port.Port, port.ReadValue);
			SetWrite32(port.Port, port.SetValue);
		}

		/// <summary>
		/// Unregisters the port.
		/// </summary>
		/// <param name="port">The port.</param>
		public static void UnregisterPort(IOPort<byte> port)
		{
			SetRead8(port.Port, null);
			SetWrite8(port.Port, null);
		}

		/// <summary>
		/// Unregisters the port.
		/// </summary>
		/// <param name="port">The port.</param>
		public static void UnregisterPort(IOPort<ushort> port)
		{
			SetRead16(port.Port, null);
			SetWrite16(port.Port, null);
		}

		/// <summary>
		/// Unregisters the port.
		/// </summary>
		/// <param name="port">The port.</param>
		public static void UnregisterPort(IOPort<uint> port)
		{
			SetRead32(port.Port, null);
			SetWrite32(port.Port, null);
		}
	}

}
