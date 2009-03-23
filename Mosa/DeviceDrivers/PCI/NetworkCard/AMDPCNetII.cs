/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// Port from Cosmos (http://www.codeplex.com/Cosmos) under New BSD License.

// References:
// http://www.amd.com/files/connectivitysolutions/networking/archivednetworking/19436.pdf 

using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceDrivers.PCI.NetworkCard
{
	/// <summary>
	/// AMDPCNet Network Chip
	/// </summary>
	[PCIDeviceDriver(VendorID = 0x1022, DeviceID = 0x2000, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	[DeviceDriverPhysicalMemory(MemorySize = 7 * 4, MemoryAlignment = 4, RestrictUnder4G = true)]
	[DeviceDriverPhysicalMemory(MemorySize = 80 * 4, MemoryAlignment = 16, RestrictUnder4G = true)]
	[DeviceDriverPhysicalMemory(MemorySize = 80 * 4, MemoryAlignment = 16, RestrictUnder4G = true)]
	[DeviceDriverPhysicalMemory(MemorySize = 2048 * 32, MemoryAlignment = 1, RestrictUnder4G = true)]
	public class AMDPCNet : HardwareDevice, INetworkDevice
	{
		#region Memory and Ports

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort ioProm1;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort ioProm4;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort rdp;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort rap;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort bdp;
		/// <summary>
		/// 
		/// </summary>
		protected IMemory initBlock;
		/// <summary>
		/// 
		/// </summary>
		protected IMemory buffers;
		/// <summary>
		/// 
		/// </summary>
		protected MACAddress macAddress;
		/// <summary>
		/// 
		/// </summary>
		protected IMemory txDescriptor;
		/// <summary>
		/// 
		/// </summary>
		protected IMemory rxDescriptor;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="RTL8139"/> class.
		/// </summary>
		public AMDPCNet() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "AMDPCNet_0x" + hardwareResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			ioProm1 = hardwareResources.GetIOPort(0, 0x0);
			ioProm4 = hardwareResources.GetIOPort(0, 0x4);
			rdp = hardwareResources.GetIOPort(0, 0x10);
			rap = hardwareResources.GetIOPort(0, 0x14);
			bdp = hardwareResources.GetIOPort(0, 0x1C);

			initBlock = hardwareResources.GetMemory(1);
			txDescriptor = hardwareResources.GetMemory(2);
			rxDescriptor = hardwareResources.GetMemory(3);
			buffers = hardwareResources.GetMemory(4);

			ushort bufferLen = 2048;
			uint len = (ushort)(~bufferLen);
			len = (len + 1) & 0x0FFF | 0x8000F000;

			for (uint index = 0; index < 16; index++) {
				uint offset = index * 4;
				rxDescriptor.Write32((offset + 1) * 4, len);
				rxDescriptor.Write32((offset + 2) * 4, buffers.Address + (bufferLen * index));
				txDescriptor.Write32((offset + 2) * 4, buffers.Address + (bufferLen * (index + 16)));
			}

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			// Enable the card
			hardwareResources.PCIDeviceResource.EnableDevice();

			// Do a 32-bit write to set 32-bit mode
			rdp.Write32(0);

			// Get the EEPROM MAC Address
			byte[] eepromMac = new byte[6];
			uint data = ioProm1.Read32();
			eepromMac[0] = (byte)(data & 0xFF);
			eepromMac[1] = (byte)((data >> 8) & 0xFF);
			eepromMac[2] = (byte)((data >> 16) & 0xFF);
			eepromMac[3] = (byte)((data >> 24) & 0xFF);
			data = ioProm4.Read32();
			eepromMac[4] = (byte)(data & 0xFF);
			eepromMac[5] = (byte)((data >> 8) & 0xFF);

			macAddress = new MACAddress(eepromMac);

			// Fill in the Initialization block
			initBlock.Write32(0, (0x4 << 28) | (0x4 << 30));
			initBlock.Write32(4, (uint)(eepromMac[0] | (eepromMac[1] << 8) | (eepromMac[2] << 16) | (eepromMac[3] << 24)));
			initBlock.Write32(8, (uint)(eepromMac[4] | (eepromMac[5] << 8))); // Fill in the hardware MAC address
			initBlock.Write32(16, 0x0);
			initBlock.Write32(24, 0x0);
			initBlock.Write32(28, rxDescriptor.Address);
			initBlock.Write32(32, txDescriptor.Address);

			// Write the Initialization blocks address to the registers on the card
			InitializationBlockAddress = initBlock.Address;

			// Set the device to PCNet-PCI II Controller mode (full 32-bit mode)
			SoftwareStyleRegister = 0x03;

			// Initialize the RX and TX buffers
			// TODO

			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return true; }

		/// <summary>
		/// Gets the MAC address.
		/// </summary>
		/// <value>The MAC address.</value>
		public MACAddress MACAddress { get { return macAddress; } }

		/// <summary>
		/// Gets or sets the status register.
		/// </summary>
		/// <value>The status register.</value>
		protected uint StatusRegister
		{
			get
			{
				rap.Write32(0);
				return rdp.Read32();
			}
			set
			{
				rap.Write32(0);
				rdp.Write32(value);
			}
		}

		/// <summary>
		/// Gets or sets the mode register.
		/// </summary>
		/// <value>The mode register.</value>
		protected uint ModeRegister
		{
			get
			{
				rap.Write32(15);
				return rdp.Read32();
			}
			set
			{
				rap.Write32(15);
				rdp.Write32(value);
			}
		}

		/// <summary>
		/// Gets or sets the burst bus control register.
		/// </summary>
		/// <value>The burst bus control register.</value>
		protected uint BurstBusControlRegister
		{
			get
			{
				rap.Write32(18);
				return bdp.Read32();
			}
			set
			{
				rap.Write32(18);
				bdp.Write32(value);
			}
		}

		/// <summary>
		/// Gets or sets the software style register.
		/// </summary>
		/// <value>The software style register.</value>
		protected uint SoftwareStyleRegister
		{
			get
			{
				rap.Write32(20);
				return bdp.Read32();
			}
			set
			{
				rap.Write32(20);
				bdp.Write32(value);
			}
		}

		/// <summary>
		/// Gets or sets the initialization block address.
		/// </summary>
		/// <value>The initialization block address.</value>
		protected uint InitializationBlockAddress
		{
			get
			{
				uint result;
				rap.Write32(1);
				result = rdp.Read32();
				rap.Write32(2);
				result |= (rdp.Read32() << 16);
				return result;
			}
			set
			{
				rap.Write32(1);
				rdp.Write32(value & 0xFFFF);
				rap.Write32(2);
				rdp.Write32(value >> 16);
			}
		}
	}
}
