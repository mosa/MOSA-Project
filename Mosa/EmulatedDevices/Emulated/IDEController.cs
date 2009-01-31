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
using Mosa.EmulatedKernel;

namespace Mosa.EmulatedDevices.Emulated
{
	/// <summary>
	/// Represents an emulated a simple IDE controller
	/// </summary>
	public class IDEController : IHardwareDevice, IIOPortDevice
	{
		/// <summary>
		/// 
		/// </summary>
		protected enum DeviceStatus
		{
			/// <summary>
			/// IDE Device is ready and waiting for instructions
			/// </summary>
			Ready,
			/// <summary>
			/// IDE Device is busy reading
			/// </summary>
			ReadingSector,
			/// <summary>
			/// IDE Device is busy writing
			/// </summary>
			WritingSector,
			/// <summary>
			/// IDE Device shall report itself
			/// </summary>
			IdentifyDrive
		};

		/// <summary>
		/// 
		/// </summary>
		public const ushort PrimaryIOBase = 0x1F0;

		/// <summary>
		/// 
		/// </summary>
		protected ushort ioBase;

		/// <summary>
		/// 
		/// </summary>
		protected byte commandStatus;

		/// <summary>
		/// 
		/// </summary>
		protected byte featureAndError;

		/// <summary>
		/// 
		/// </summary>
		protected byte lbaLow;

		/// <summary>
		/// 
		/// </summary>
		protected byte lbaHigh;

		/// <summary>
		/// 
		/// </summary>
		protected byte lbaMid;

		/// <summary>
		/// 
		/// </summary>
		protected byte deviceHead;

		/// <summary>
		/// 
		/// </summary>
		protected byte sectorCount;

		/// <summary>
		/// 
		/// </summary>
		protected byte numDrives;

		/// <summary>
		/// 
		/// </summary>
		protected FileStream[] driveFiles;

		/// <summary>
		/// 
		/// </summary>
		protected byte[] bufferData;

		/// <summary>
		/// 
		/// </summary>
		protected ushort bufferIndex;

		/// <summary>
		/// 
		/// </summary>
		protected DeviceStatus status;

		/// <summary>
		/// Initializes a new instance of the <see cref="IDEController"/> class.
		/// </summary>
		/// <param name="ioBase">The io base.</param>
		/// <param name="filenames">The filenames.</param>
		public IDEController(ushort ioBase, string[] filenames)
		{
			this.ioBase = ioBase;

			numDrives = (byte)filenames.Length;
			driveFiles = new FileStream[numDrives];
			bufferData = new byte[512];
			bufferIndex = 0;
			status = DeviceStatus.Ready;

			for (int i = 0; i < numDrives; i++) {
				driveFiles[i] = new FileStream(filenames[i], FileMode.Open);
			}

			Initialize();
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public bool Initialize()
		{
			lbaHigh = 0;
			lbaMid = 0;
			lbaLow = 1;
			sectorCount = 1;
			featureAndError = 1;
			commandStatus = 0;
			status = DeviceStatus.Ready;
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
			switch (port - ioBase) {
				case 0: return ReadData();
				case 1: return featureAndError;
				case 2: return sectorCount;
				case 3: return lbaLow;
				case 4: return lbaMid;
				case 5: return lbaHigh;
				case 6: return deviceHead;
				case 7: return commandStatus;
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
			switch (port - ioBase) {
				case 0: return; // TODO
				case 1: featureAndError = data; return;
				case 2: sectorCount = data; return;
				case 3: lbaLow = data; return;
				case 4: lbaMid = data; return;
				case 5: lbaHigh = data; return;
				case 6: WriteDeviceHead(data); return;
				case 7: WriteCommand(data); return;
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
			return PortRange.GetPortList(ioBase, 8);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
			foreach (FileStream driveFile in driveFiles) { driveFile.Close(); }
		}

		/// <summary>
		/// Writes the device head.
		/// </summary>
		/// <param name="data">The data.</param>
		protected void WriteDeviceHead(byte data)
		{
			deviceHead = (byte)(data | 0xA0 & ~0x40);    // force bits to 101xxxxx

			if ((deviceHead & 0x10) == 0x10) // check for selecting 2nd drive
            {
				if (numDrives == 1) // only one drive in system
                {
					commandStatus = (byte)(commandStatus & ~0x40);  // so clear drive ready status bit
					return;
				}
			}

			commandStatus = (byte)(commandStatus | 0x40);  // set drive ready status bit			
		}

		/// <summary>
		/// Reads the LBA28 into buffer.
		/// </summary>
		protected void ReadLBA28IntoBuffer()
		{
			byte drive = (byte)(((deviceHead & 0x10) != 0x10) ? 0 : 1);
			uint lba28 = (uint)((lbaHigh << 16) | (lbaMid << 8) | (lbaLow));
			driveFiles[drive].Seek(lba28 * 512, SeekOrigin.Begin);
			driveFiles[drive].Read(bufferData, 0, 512);
			bufferIndex = 0;
			commandStatus = (byte)((commandStatus | 0x08) & ~0x80); // Set DRQ (bit 3), clear BUSY (bit 7)
			status = DeviceStatus.ReadingSector;
		}

		/// <summary>
		/// Gets the LBA28.
		/// </summary>
		/// <returns></returns>
		protected uint GetLBA28()
		{
			return (uint)(
				((deviceHead >> 24) & (0x0F)) |
				(lbaHigh << 16) |
				(lbaMid << 8) |
				(lbaLow)
				);
		}

		/// <summary>
		/// Sets the LBA28.
		/// </summary>
		/// <param name="lba28">The lba28.</param>
		protected void SetLBA28(uint lba28)
		{
			deviceHead = (byte)((deviceHead & 0x0F) | (byte)lba28 >> 24);
			lbaHigh = (byte)((lba28 & 0xFF0000) >> 16);
			lbaMid = (byte)((lba28 & 0xFF00) >> 8);
			lbaLow = (byte)(lba28 & 0xFF);
		}

		/// <summary>
		/// Reads the data port.
		/// </summary>
		/// <returns></returns>
		protected byte ReadData()
		{
			if ((status == DeviceStatus.Ready) || (status == DeviceStatus.WritingSector))
				return 0xFF;

			byte data = bufferData[bufferIndex++];

			if (bufferIndex == 512) {
				if (status == DeviceStatus.IdentifyDrive)
					bufferIndex = 0;
				else {
					sectorCount = (byte)(sectorCount - 1);
					if (sectorCount != 0) {
						ReadLBA28IntoBuffer();
						SetLBA28(GetLBA28() + 1);
					}
					else
						status = DeviceStatus.Ready;
				}
			}

			return data;
		}

		/// <summary>
		/// Identifies the drive.
		/// </summary>
		protected void IdentifyDrive()
		{
			byte drive = (byte)(((deviceHead & 0x10) != 0x10) ? 0 : 1);
			byte d = (byte)('0' + drive + 1);
			BinaryFormat data = new BinaryFormat(bufferData);

			data.Fill(0, 0, 512);

			// fixed drive and over 5Mb/sec     
			data.SetUShort(0x00, 0x140);
			// Serial Number
			data.Fill(0x0A * 2, d, 20);
			data.SetByte(0x0A * 2, d);
			// Firmware version
			data.Fill(0x17 * 2, d, 8);
			data.SetChar(0x17 * 2 + 0, '1');
			data.SetChar(0x17 * 2 + 1, '.');
			data.SetChar(0x17 * 2 + 2, '0');
			// Model Number
			data.Fill(0x1B * 2, d, 40);
			data.SetChar(0x17 * 2 + 0, 'D');
			data.SetChar(0x17 * 2 + 1, 'R');
			data.SetChar(0x17 * 2 + 2, 'I');
			data.SetChar(0x17 * 2 + 3, 'V');
			data.SetChar(0x17 * 2 + 4, 'E');
			data.SetByte(0x1B * 2 + 5, d);
			// lba28
			data.SetUInt(0x3C * 2, (uint)(driveFiles[drive].Length / 512));

			commandStatus = (byte)((commandStatus | 0x08) & ~0x80); // Set DRQ (bit 3), clear BUSY (bit 7)

			status = DeviceStatus.IdentifyDrive;
		}

		/// <summary>
		/// Writes the command port.
		/// </summary>
		/// <param name="data">The data.</param>
		protected void WriteCommand(byte data)
		{
			switch (data) {
				case 0x20: // lba28 read w/ retry                         
					ReadLBA28IntoBuffer();
					break;
				case 0x21: // lba28 read
					ReadLBA28IntoBuffer();
					break;
				case 0xEC: // identify drive
					IdentifyDrive();
					break;
				//case 0x30: // lba28 write 
				//        status = DeviceStatus.WritingSector;
				//        break;
				default: break;
			}
		}
	}
}
