/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.EmulatedKernel;
using System;

namespace Mosa.EmulatedDevices.Emulated
{
	/// <summary>
	/// Represents an emulated CMOS chip
	/// </summary>
	public class CMOS : IHardwareDevice, IIOPortDevice
	{
		/// <summary>
		///
		/// </summary>
		public const ushort StandardIOBase = 0x70;

		/// <summary>
		///
		/// </summary>
		protected ushort ioBase;

		/// <summary>
		///
		/// </summary>
		protected byte index = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="CMOS"/> class.
		/// </summary>
		/// <param name="ioBase">The io base.</param>
		public CMOS(ushort ioBase)
		{
			this.ioBase = ioBase;

			Initialize();
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public bool Initialize()
		{
			index = 0;
			return true;
		}

		/// <summary>
		/// Resets this instance.
		/// </summary>
		public void Reset()
		{
			Initialize();
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public byte Read8(ushort port)
		{
			switch (port - ioBase)
			{
				case 1: return ReadCMOS(index);
				default: return 0xFF;
			}
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public ushort Read16(ushort port)
		{
			return (ushort)(Read8(port) | (Read8(port) << 8));
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public uint Read32(ushort port)
		{
			return (ushort)(Read8(port) | (Read8(port) << 8) | (Read8(port) << 16) | (Read8(port) << 24));
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="data"></param>
		public void Write8(ushort port, byte data)
		{
			switch (port - ioBase)
			{
				case 0: index = data; return;
				default: return;
			}
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write16(ushort port, ushort data)
		{
			Write8(port, (byte)(data & 0xFF));
			Write8(port, (byte)((data >> 8) & 0xFF));
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write32(ushort port, uint data)
		{
			Write8(port, (byte)(data & 0xFF));
			Write8(port, (byte)((data >> 8) & 0xFF));
			Write8(port, (byte)((data >> 16) & 0xFF));
			Write8(port, (byte)((data >> 24) & 0xFF));
		}

		/// <summary>
		/// Gets the ports requested.
		/// </summary>
		/// <returns></returns>
		public ushort[] GetPortsRequested()
		{
			return PortRange.GetPortList(ioBase, 2);
		}

		/// <summary>
		/// Reads the CMOS.
		/// </summary>
		/// <returns></returns>
		protected byte ReadCMOS(byte index)
		{
			switch (index & 0x1F)
			{  // mask out last three bits
				case 0x00: return (byte)DateTime.Now.Second;
				case 0x01: return 0;
				case 0x02: return (byte)DateTime.Now.Minute;
				case 0x03: return 0;
				case 0x04: return (byte)DateTime.Now.Hour;
				case 0x05: return 0;
				case 0x06: return (byte)DateTime.Now.DayOfWeek;
				case 0x07: return (byte)DateTime.Now.Day;
				case 0x08: return (byte)DateTime.Now.Month;
				case 0x09: return (byte)(DateTime.Now.Year % 100);
				case 0x0A: return 0;  // Status Register A
				case 0x0B: return 0x00;  // Status Register B (data mode - binary)
				case 0x0C: return 0;  // Status Register C
				case 0x0D: return 0x80;  // Status Register D (battery power good)
				case 0x0E: return 0;  // Diagnostic Status
				case 0x0F: return 0;  // Shutdown Status
				case 0x10: return 0x40;  // Drive Type (one 1.44 Mb 3 1/2 Drive)
				case 0x11: return 0;  // System Configuration Settings (only BIOS cares about this)
				case 0x12: return 0;  // Hard Disk Types
				case 0x13: return 0;  // Typematic Parameters
				case 0x14: return 0x29;  // Equipment List (80 Column Display + Keyboard + Math Co + 1 Diskette Drive)

				// TODO: And many more
				default: return 0xFF;
			}
		}
	}
}