// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Port from Cosmos (http://www.codeplex.com/Cosmos) under New BSD License.

// References:
// http://www.amd.com/files/connectivitysolutions/networking/archivednetworking/19436.pdf

using Mosa.DeviceSystem;
using Mosa.HardwareSystem;

namespace Mosa.DeviceDriver.PCI.AMD
{
	/// <summary>
	/// AMDPCNet Network Chip
	/// </summary>
	[PCIDeviceDriver(VendorID = 0x1022, DeviceID = 0x2000, Platforms = PlatformArchitecture.X86AndX64)]
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

		/// <summary>
		///
		/// </summary>
		protected byte nextTXDesc;

		/// <summary>
		///
		/// </summary>
		protected ushort bufferSize;

		/// <summary>
		///
		/// </summary>
		protected NetworkDevicePacketBuffer packetBuffer;

		/// <summary>
		///
		/// </summary>
		protected uint physicalBufferAddress;

		#endregion Memory and Ports

		/// <summary>
		/// Initializes a new instance of the <see cref="AMDPCNet"/> class.
		/// </summary>
		public AMDPCNet()
		{
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(HardwareResources hardwareResources)
		{
			this.HardwareResources = hardwareResources;
			base.Name = "AMDPCNet_0x" + hardwareResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			ioProm1 = hardwareResources.GetIOPort(0, 0x0);
			ioProm4 = hardwareResources.GetIOPort(0, 0x4);
			rdp = hardwareResources.GetIOPort(0, 0x10);
			rap = hardwareResources.GetIOPort(0, 0x14);
			bdp = hardwareResources.GetIOPort(0, 0x1C);

			initBlock = hardwareResources.GetMemory(0);
			txDescriptor = hardwareResources.GetMemory(1);
			rxDescriptor = hardwareResources.GetMemory(2);
			buffers = hardwareResources.GetMemory(3);

			bufferSize = 2048;
			uint len = (ushort)(~bufferSize);
			len = (len + 1) & 0x0FFF | 0x8000F000;

			physicalBufferAddress = HAL.GetPhysicalAddress(buffers);

			for (uint index = 0; index < 16; index++)
			{
				uint offset = index * 4;
				rxDescriptor.Write32((offset + 1) * 4, len);
				rxDescriptor.Write32((offset + 2) * 4, physicalBufferAddress + (bufferSize * index));
				txDescriptor.Write32((offset + 2) * 4, physicalBufferAddress + (bufferSize * (index + 16)));
			}

			nextTXDesc = 0;

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			// Enable the card
			HardwareResources.DeviceResource.EnableDevice();

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

			// Fill in the initialization block
			initBlock.Write32(0, (0x4 << 28) | (0x4 << 30));
			initBlock.Write32(4, (uint)(eepromMac[0] | (eepromMac[1] << 8) | (eepromMac[2] << 16) | (eepromMac[3] << 24)));
			initBlock.Write32(8, (uint)(eepromMac[4] | (eepromMac[5] << 8))); // Fill in the hardware MAC address
			initBlock.Write32(16, 0x0);
			initBlock.Write32(24, 0x0);
			initBlock.Write32(28, rxDescriptor.Address);
			initBlock.Write32(32, txDescriptor.Address);

			// Write the initialization blocks address to the registers on the card
			InitializationBlockAddress = HAL.GetPhysicalAddress(initBlock);

			// Set the device to PCNet-PCI II Controller mode (full 32-bit mode)
			SoftwareStyleRegister = 0x03;

			nextTXDesc = 0;

			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			uint status = StatusRegister;

			if ((status & 0x200) != 0)
				if (packetBuffer != null)
					packetBuffer.Pulse();

			if ((status & 0x400) != 0)
				RetrievePackets();

			return true;
		}

		/// <summary>
		/// Assigns the packet buffer to the device
		/// </summary>
		/// <param name="packetBuffer">The packet buffer.</param>
		public void AssignPacketBuffer(NetworkDevicePacketBuffer packetBuffer)
		{
			this.packetBuffer = packetBuffer;
		}

		/// <summary>
		/// Sends the packet.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public bool SendPacket(byte[] data)
		{
			uint txd = nextTXDesc++;

			if (nextTXDesc >= 16)
				nextTXDesc = 0;

			uint offset = txd * 4;

			// check if (oldest) descriptor is available (Bit 31/OWN = 0 available)
			if ((txDescriptor.Read32(offset + 1) & 0x80000000) == 0)
			{
				for (uint i = 0; i < data.Length; i++)
					buffers.Write8((txd * bufferSize) + i, data[i]);

				ushort length = (ushort)(~data.Length);
				length++;

				// Set bits 31/OWN, 25/STP (start of packet), 24/ENP (end of packet) and two's compliment of the buffer length
				txDescriptor.Write32(offset + 1, length & (uint)(0x0FFF) | (uint)(0x8300F000));

				return true;
			}

			return false;
		}

		/// <summary>
		/// Retrieves the packets.
		/// </summary>
		protected void RetrievePackets()
		{
			// Check all descriptors
			for (uint rxd = 0; rxd < 16; rxd++)
			{
				uint offset = rxd * 4;

				uint status = rxDescriptor.Read32(offset + 1);

				// Check is 31/OWN bit is not set
				if ((status & 0x80000000) == 0)
				{
					ushort length = (ushort)(rxDescriptor.Read16(offset + 0) & 0xFFF);
					byte[] data = new byte[length];

					for (uint i = 0; i < data.Length; i++)
						data[i] = buffers.Read8((rxd * bufferSize) + i);

					// if queue fails because it is already full, the packet is discarded
					packetBuffer.QueuePacketForStack(data);

					// Clear 31/OWN bit
					rxDescriptor.Write32(offset + 1, status | 0x80000000);
				}
			}
		}

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
