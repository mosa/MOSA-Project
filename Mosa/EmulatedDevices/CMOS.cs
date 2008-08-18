/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using Mosa.ClassLib;
using Mosa.EmulatedDevices.Utils;

namespace Mosa.EmulatedDevices
{
	/// <summary>
	/// Represents an emulated CMOS chip
	/// </summary>
	public class CMOS : IDisposable
	{
		public const ushort StandardIOBase = 0x70;

		protected IOPort<byte> commandPort;
		protected IOPort<byte> dataPort;

		public CMOS(ushort ioBase)
		{
			commandPort = new IOPort<byte>(ioBase, 0, null, CommandWrite);
			dataPort = new IOPort<byte>(ioBase + 1, 0);

			IOPortDispatch.RegisterPort(commandPort);
			IOPortDispatch.RegisterPort(dataPort);
		}

		public void Dispose()
		{
			IOPortDispatch.UnregisterPort(commandPort);
			IOPortDispatch.UnregisterPort(dataPort);
		}

		protected byte CommandWrite(byte data)
		{
			byte result = 0;

			switch (data & 0x1F) {  // mask out last three bits
				case 0x00: result = (byte)DateTime.Now.Second; break;
				case 0x01: result = (byte)DateTime.Now.Second; break;
				case 0x02: result = (byte)DateTime.Now.Minute; break;
				case 0x03: result = (byte)DateTime.Now.Minute; break;
				case 0x04: result = (byte)DateTime.Now.Hour; break;
				case 0x05: result = (byte)DateTime.Now.Hour; break;
				case 0x06: result = (byte)DateTime.Now.DayOfWeek; break;
				case 0x07: result = (byte)DateTime.Now.Day; break;
				case 0x08: result = (byte)DateTime.Now.Month; break;
				case 0x09: result = (byte)DateTime.Now.Year; break;
				case 0x0A: result = 0; break; // Status Register A
				case 0x0B: result = 0x04; break; // Status Register B (data mode - binary)
				case 0x0C: result = 0; break; // Status Register C
				case 0x0D: result = 0x80; break; // Status Register D (battery power good)
				case 0x0E: result = 0; break; // Diagnostic Status
				case 0x0F: result = 0; break; // Shutdown Status
				case 0x10: result = 0x40; break; // Drive Type (one 1.44 Mb 3 1/2 Drive)
				case 0x11: result = 0; break; // System Configuration Settings (only BIOS cares about this)
				case 0x12: result = 0; break; // Hard Disk Types
				case 0x13: result = 0; break; // Typematic Parameters
				case 0x14: result = 0x29; break; // Equipment List (80 Column Display + Keyboard + Math Co + 1 Diskette Drive)
				// TODO: And many more
				default: break;
			}

			dataPort.Value = result;
			return data;
		}
	}
}
