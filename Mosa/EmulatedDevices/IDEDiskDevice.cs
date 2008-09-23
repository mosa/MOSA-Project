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
using Mosa.DeviceDrivers;
using Mosa.EmulatedKernel;

namespace Mosa.EmulatedDevices
{
	/// <summary>
	/// Represents an emulated a simple IDE controller
	/// </summary>
	public class IDEDiskDevice
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
		protected IOPort<byte> dataPort;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<ushort> dataPort2;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> featureAndErrorPort;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> sectorCountPort;
        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> lbaLowPort;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> lbaHighPort;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> lbaMidPort;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> deviceHead;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> commandAndStatusPort;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> altCommandAndStatusPort;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> driveAddress;

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
		protected DeviceStatus status = DeviceStatus.Ready;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioBase"></param>
        /// <param name="filenames"></param>
		public IDEDiskDevice(ushort ioBase, string[] filenames)
		{
			this.ioBase = ioBase;

			numDrives = (byte)filenames.Length;
			driveFiles = new FileStream[numDrives];
			bufferData = new byte[512];
			bufferIndex = 0;

			for (int i = 0; i < numDrives; i++) {
				driveFiles[i] = new FileStream(filenames[i], FileMode.OpenOrCreate);
			}

			dataPort = new IOPort<byte>(ioBase + 0, 0, DataPortRead, null);
			dataPort2 = new IOPort<ushort>(ioBase + 0, 0, DataPortRead2, null);
			featureAndErrorPort = new IOPort<byte>(ioBase + 1, 1);
			sectorCountPort = new IOPort<byte>(ioBase + 2, 1);
			lbaLowPort = new IOPort<byte>(ioBase + 3, 1);
			lbaMidPort = new IOPort<byte>(ioBase + 4, 0);
			lbaHighPort = new IOPort<byte>(ioBase + 5, 0);
			deviceHead = new IOPort<byte>(ioBase + 6, 0, null, DeviceHeadPortWrite);
			commandAndStatusPort = new IOPort<byte>(ioBase + 7, 0, null, CommandPortWrite);
			altCommandAndStatusPort = new IOPort<byte>(ioBase + 6 + 0x200, 0);
			driveAddress = new IOPort<byte>(ioBase + 7 + 0x200, 0);

			IOPortDispatch.RegisterPort(dataPort);
			IOPortDispatch.RegisterPort(dataPort2);
			IOPortDispatch.RegisterPort(featureAndErrorPort);
			IOPortDispatch.RegisterPort(sectorCountPort);
			IOPortDispatch.RegisterPort(lbaLowPort);
			IOPortDispatch.RegisterPort(lbaMidPort);
			IOPortDispatch.RegisterPort(lbaHighPort);
			IOPortDispatch.RegisterPort(deviceHead);
			IOPortDispatch.RegisterPort(commandAndStatusPort);
			IOPortDispatch.RegisterPort(altCommandAndStatusPort);
			IOPortDispatch.RegisterPort(driveAddress);
		}

        /// <summary>
        /// 
        /// </summary>
		public void Dispose()
		{
			foreach (FileStream driveFile in driveFiles) { driveFile.Close(); }

			IOPortDispatch.UnregisterPort(dataPort);
			IOPortDispatch.UnregisterPort(featureAndErrorPort);
			IOPortDispatch.UnregisterPort(sectorCountPort);
			IOPortDispatch.UnregisterPort(lbaLowPort);
			IOPortDispatch.UnregisterPort(lbaMidPort);
			IOPortDispatch.UnregisterPort(lbaHighPort);
			IOPortDispatch.UnregisterPort(deviceHead);
			IOPortDispatch.UnregisterPort(commandAndStatusPort);
			IOPortDispatch.UnregisterPort(altCommandAndStatusPort);
			IOPortDispatch.UnregisterPort(driveAddress);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public byte DeviceHeadPortWrite(byte data)
		{
			byte head = (byte)(data | 0xA0 & ~0x40);    // force bits to 101xxxxx

			if ((head & 0x10) == 0x10) // check for selecting 2nd drive
            {
				if (numDrives == 1) // only one drive in system
                {
					commandAndStatusPort.Value = (byte)(commandAndStatusPort.Value & ~0x40);  // so clear drive ready status bit
					return head;
				}
			}

			commandAndStatusPort.Value = (byte)(commandAndStatusPort.Value | 0x40);  // set drive ready status bit

			return head;
		}

        /// <summary>
        /// 
        /// </summary>
		public void ReadLBA28IntoBuffer()
		{
			byte drive = (byte)(((deviceHead.Value & 0x10) != 0x10) ? 0 : 1);
			uint lba28 = (uint)((lbaHighPort.Value << 16) | (lbaMidPort.Value << 8) | (lbaLowPort.Value));
			driveFiles[drive].Seek(lba28 * 512, SeekOrigin.Begin);
			driveFiles[drive].Read(bufferData, 0, 512);
			bufferIndex = 0;
			commandAndStatusPort.Value = (byte)((commandAndStatusPort.Value | 0x08) & ~0x80); // Set DRQ (bit 3), clear BUSY (bit 7)
			status = DeviceStatus.ReadingSector;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected uint GetLBA28()
		{
			return (uint)(
				((deviceHead.Value >> 24) & (0x0F)) |
				(lbaHighPort.Value << 16) |
				(lbaMidPort.Value << 8) |
				(lbaLowPort.Value)
				);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lba28"></param>
		protected void SetLBA28(uint lba28)
		{
			deviceHead.Value = (byte)((deviceHead.Value & 0x0F) | (byte)lba28 >> 24);
			lbaHighPort.Value = (byte)((lba28 & 0xFF0000) >> 16);
			lbaMidPort.Value = (byte)((lba28 & 0xFF00) >> 8);
			lbaLowPort.Value = (byte)(lba28 & 0xFF);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public byte DataPortRead(byte data)
		{
			if (status != DeviceStatus.ReadingSector) { return 0; }

			data = bufferData[bufferIndex++];

			if (bufferIndex == 512) {
				sectorCountPort.Value = (byte)(sectorCountPort.Value - 1);
				if (sectorCountPort.Value != 0) {
					ReadLBA28IntoBuffer();
					SetLBA28(GetLBA28() + 1);
				}
				else { status = DeviceStatus.Ready; }
			}

			return data;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public ushort DataPortRead2(ushort data) { return (ushort)(DataPortRead(0) | (DataPortRead(0) << 8)); }

        /// <summary>
        /// 
        /// </summary>
		public void IdentifyDrive()
		{
			byte drive = (byte)(((deviceHead.Value & 0x10) != 0x10) ? 0 : 1);
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

			commandAndStatusPort.Value = (byte)((commandAndStatusPort.Value | 0x08) & ~0x80); // Set DRQ (bit 3), clear BUSY (bit 7)

			status = DeviceStatus.IdentifyDrive;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public byte CommandPortWrite(byte data)
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
			return commandAndStatusPort.Value;  // keep it the same as before
		}
	}
}
