/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.ISA
{
	/// <summary>
	/// Direct Memory Access (DMA) Device Driver
	/// </summary>
	//[DeviceSignature(AutoLoad = false, BasePort = 0x0000, PortRange = 32, AltBasePort = 0x0080, AltPortRange = 8, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class DMA8Bit : HardwareDevice, IDevice, IHardwareDevice
	{
		#region Definitions

		internal struct DMAModeValue
		{
			internal const byte ReadFromMemory = 0x08;	// TRN=10
			internal const byte WriteToMemory = 0x04;   // TRN=01
		}

		internal struct DMATransferTypeValue
		{
			internal const byte OnDemand = 0x00;	// MOD=00
			internal const byte Single = 0x40;		// MOD=01
			internal const byte Block = 0x80;		// MOD=10
			internal const byte CascadeMode = 0xC0;	// MOD=11
		}

		internal struct DMAAutoValue
		{
			internal const byte Auto = 0x10;
			internal const byte NoAuto = 0x00;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		protected IReadOnlyIOPort statusRegister;
		/// <summary>
		/// 
		/// </summary>
		protected IWriteOnlyIOPort commandRegister;
		/// <summary>
		/// 
		/// </summary>
		protected IWriteOnlyIOPort requestRegister;
		/// <summary>
		/// 
		/// </summary>
		protected IWriteOnlyIOPort channelMaskRegister;
		/// <summary>
		/// 
		/// </summary>
		protected IWriteOnlyIOPort modeRegister;
		/// <summary>
		/// 
		/// </summary>
		protected IWriteOnlyIOPort byteWordRegister;
		/// <summary>
		/// 
		/// </summary>
		protected IReadOnlyIOPort intermediateRegister;
		/// <summary>
		/// 
		/// </summary>
		protected IWriteOnlyIOPort maskRegister;

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel0Address;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel0Count;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel0Page;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel1Address;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel1Count;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel1Page;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel2Address;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel2Count;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel2Page;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel3Address;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel3Count;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort channel3Page;

		/// <summary>
		/// 
		/// </summary>
		protected IMemory memory0;
		/// <summary>
		/// 
		/// </summary>
		protected IMemory memory1;
		/// <summary>
		/// 
		/// </summary>
		protected IMemory memory2;
		/// <summary>
		/// 
		/// </summary>
		protected IMemory memory3;

		/// <summary>
		/// Initializes a new instance of the <see cref="DMA8Bit"/> class.
		/// </summary>
		public DMA8Bit() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "DMA_0x" + base.hardwareResources.GetIOPort(0, 0).Address.ToString("X");

			statusRegister = base.hardwareResources.GetIOPort(0, 0x08);
			commandRegister = base.hardwareResources.GetIOPort(0, 0x08);
			requestRegister = base.hardwareResources.GetIOPort(0, 0x09);
			channelMaskRegister = base.hardwareResources.GetIOPort(0, 0x0A);
			modeRegister = base.hardwareResources.GetIOPort(0, 0x0B);
			byteWordRegister = base.hardwareResources.GetIOPort(0, 0x0C);
			intermediateRegister = base.hardwareResources.GetIOPort(0, 0x0D);
			maskRegister = base.hardwareResources.GetIOPort(0, 0x0F);

			channel0Address = base.hardwareResources.GetIOPort(0, 0x00);
			channel0Count = base.hardwareResources.GetIOPort(0, 0x01);
			channel0Page = base.hardwareResources.GetIOPort(0, 0x87);

			channel1Address = base.hardwareResources.GetIOPort(0, 0x02);
			channel1Count = base.hardwareResources.GetIOPort(0, 0x03);
			channel1Page = base.hardwareResources.GetIOPort(0, 0x83);

			channel2Address = base.hardwareResources.GetIOPort(0, 0x04);
			channel2Count = base.hardwareResources.GetIOPort(0, 0x05);
			channel2Page = base.hardwareResources.GetIOPort(0, 0x81);

			channel3Address = base.hardwareResources.GetIOPort(0, 0x06);
			channel3Count = base.hardwareResources.GetIOPort(0, 0x07);
			channel3Page = base.hardwareResources.GetIOPort(0, 0x82);

			memory0 = base.hardwareResources.GetMemory(0);
			memory1 = base.hardwareResources.GetMemory(1);
			memory2 = base.hardwareResources.GetMemory(2);
			memory3 = base.hardwareResources.GetMemory(3);

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>
		public override LinkedList<IDevice> CreateSubDevices() { return null; }

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return false; }

		/// <summary>
		/// Setups the channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="count">The count.</param>
		/// <param name="mode">The mode.</param>
		/// <param name="type">The type.</param>
		/// <param name="auto">if set to <c>true</c> [auto].</param>
		/// <returns></returns>
		public bool SetupChannel(byte channel, uint count, DMAMode mode, DMATransferType type, bool auto)
		{
			IWriteOnlyIOPort dmaAddress;
			IWriteOnlyIOPort dmaCount;
			IWriteOnlyIOPort dmaPage;
			IMemory memory;

			switch (channel) {
				case 0: dmaAddress = channel0Address; dmaCount = channel0Count; dmaPage = channel0Page; memory = memory0; break;
				case 1: dmaAddress = channel1Address; dmaCount = channel1Count; dmaPage = channel1Page; memory = memory1; break;
				case 2: dmaAddress = channel2Address; dmaCount = channel2Count; dmaPage = channel2Page; memory = memory2; break;
				case 3: dmaAddress = channel3Address; dmaCount = channel3Count; dmaPage = channel3Page; memory = memory3; break;
				default: return false;
			}

			uint address = memory.Address;

			// Disable DMA Controller
			channelMaskRegister.Write8((byte)((byte)channel | 4));

			// Clear any current transfers
			byteWordRegister.Write8((byte)0xFF);	// reset flip-flop			

			// Set Address	
			dmaAddress.Write8((byte)(address & 0xFF)); // low byte
			dmaAddress.Write8((byte)((address >> 8) & 0xFF)); // high byte
			dmaPage.Write8((byte)((address >> 16) & 0xFF)); // page

			// Clear any current transfers
			byteWordRegister.Write8((byte)0xFF);	// reset flip-flop

			// Set Count
			dmaCount.Write8((byte)((count - 1) & 0xFF)); // low
			dmaCount.Write8((byte)(((count - 1) >> 8) & 0xFF)); // high

			byte value = channel;

			if (auto)
				value = (byte)(value | DMAAutoValue.Auto);
			else
				value = (byte)(value | DMAAutoValue.NoAuto);

			if (mode == DMAMode.ReadFromMemory)
				value = (byte)(value | DMAModeValue.ReadFromMemory);
			else
				value = (byte)(value | DMAModeValue.WriteToMemory);

			switch (type) {
				case DMATransferType.Block: value = (byte)(value | DMATransferTypeValue.Block); break;
				case DMATransferType.CascadeMode: value = (byte)(value | DMATransferTypeValue.CascadeMode); break;
				case DMATransferType.OnDemand: value = (byte)(value | DMATransferTypeValue.OnDemand); break;
				case DMATransferType.Single: value = (byte)(value | DMATransferTypeValue.Single); break;
				default: break;
			}

			// Set DMA Channel to write
			modeRegister.Write8(value);

			// Enable DMA Controller
			channelMaskRegister.Write8((byte)(channel));

			return true;
		}

		/// <summary>
		/// Gets the DMA transer address.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <returns></returns>
		protected IMemory GetTranserAddress(byte channel)
		{
			switch (channel) {
				case 0: return memory0;
				case 1: return memory1;
				case 2: return memory2;
				case 3: return memory3;
				default: return null;
			}
		}

		/// <summary>
		/// Transfers the out.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="count">The count.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public bool TransferOut(byte channel, uint count, byte[] destination, uint offset)
		{
			if (count > (1024 * 64))
				return false;

			if (destination.Length + offset > count)
				return false;

			IMemory address = GetTranserAddress(channel);

			if (address.Address == 0x00)
				return false;

			for (uint i = 0; i < count; i++)
				destination[offset + i] = address[i];

			return true;
		}

		/// <summary>
		/// Transfers the in.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="count">The count.</param>
		/// <param name="source">The source.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public bool TransferIn(byte channel, uint count, byte[] source, uint offset)
		{
			if (count > (1024 * 64))
				return false;

			if (source.Length + offset > count)
				return false;

			IMemory address = GetTranserAddress(channel);

			if (address.Address == 0x00)
				return false;

			for (uint i = 0; i < count; i++)
				address[i] = source[offset + i];

			return true;
		}
	}
}

