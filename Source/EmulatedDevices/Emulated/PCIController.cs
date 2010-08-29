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
	/// Represents an emulated PCI Controller
	/// </summary>
	public class PCIController : PCIDevice, IHardwareDevice, IIOPortDevice
	{
		/// <summary>
		/// 
		/// </summary>
		public const ushort StandardIOBase = 0xCF8;

		/// <summary>
		/// 
		/// </summary>
		protected ushort ioBase;

		/// <summary>
		/// 
		/// </summary>
		protected uint address;

		/// <summary>
		/// 
		/// </summary>
		protected PCIBus pciBus;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCIController"/> class.
		/// </summary>
		/// <param name="ioBase">The io base.</param>
		/// <param name="pciBus">The pci bus.</param>
		public PCIController(ushort ioBase, PCIBus pciBus)
		{
			this.ioBase = ioBase;
			this.pciBus = pciBus;

			base.VendorID = 0x8086;
			base.DeviceID = 0x1237;
			base.Revision = 2;
			base.ClassCode = 6;
			base.SubClassCode = 0;
			base.HeaderType = 0;

			Initialize();
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public bool Initialize()
		{
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
				case 0: return 0xFF;
				case 4: return ReadConfig8();
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
			switch (port - ioBase)
			{
				case 0: return 0xFF;
				case 4: return ReadConfig16();
				default: return 0xFFFF;
			}
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public uint Read32(ushort port)
		{
			switch (port - ioBase)
			{
				case 0: return address;
				case 4: return ReadConfig32();
				default: return 0xFFFFFFFF;
			}
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="data"></param>
		public void Write8(ushort port, byte data)
		{
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write16(ushort port, ushort data)
		{
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write32(ushort port, uint data)
		{
			switch (port - ioBase)
			{
				case 0: address = data; return;
				case 4: WriteConfig32(data); return;
				default: return;
			}
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
		/// Gets the index.
		/// </summary>
		/// <returns></returns>
		protected uint GetIndex()
		{
			byte bus = (byte)((address >> 16) & 0xFF);
			byte slot = (byte)((address >> 11) & 0x0F);
			byte fun = (byte)((address >> 8) & 0x07);

			return (uint)(bus | (slot << 8) | (fun << 16));
		}

		/// <summary>
		/// Gets the PCI device.
		/// </summary>
		/// <returns></returns>
		protected PCIDevice GetPCIDevice()
		{
			return pciBus.Get(GetIndex());
		}

		/// <summary>
		/// Reads the configuration.
		/// </summary>
		/// <returns></returns>
		protected uint ReadConfig32()
		{
			PCIDevice pciDevice = GetPCIDevice();

			if (pciDevice == null)
				return 0xFFFFFFFF;

			return pciDevice.ReadConfig32((byte)(address & 0xFF));
		}

		/// <summary>
		/// Reads the configuration.
		/// </summary>
		/// <returns></returns>
		protected ushort ReadConfig16()
		{
			PCIDevice pciDevice = GetPCIDevice();

			if (pciDevice == null)
				return 0xFFFF;

			return pciDevice.ReadConfig16((byte)(address & 0xFF));
		}

		/// <summary>
		/// Reads the configuration.
		/// </summary>
		/// <returns></returns>
		protected byte ReadConfig8()
		{
			PCIDevice pciDevice = GetPCIDevice();

			if (pciDevice == null)
				return 0xFF;

			return pciDevice.ReadConfig8((byte)(address & 0xFF));
		}
		/// <summary>
		/// Writes the configuration.
		/// </summary>
		/// <param name="data">The data.</param>
		protected void WriteConfig32(uint data)
		{
			PCIDevice pciDevice = GetPCIDevice();

			if (pciDevice == null)
				return;

			byte registry = (byte)(address & 0xFF);
			uint index = GetIndex();

			uint barMask = 0xFFFFFFFF;

			if ((registry >= 0x10) && (registry < 0x28))
				barMask = GetBarMask((byte)((index - 0x10) >> 2));

			pciDevice.WriteConfig32(registry, data & barMask & GetWriteMask32(registry));
		}

	}
}
