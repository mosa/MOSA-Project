/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers.ISA
{
	/// <summary>
	/// 
	/// </summary>
	[ISADeviceDriver(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
	public class PCIController : HardwareDevice, IDevice, IHardwareDevice, IPCIController
	{

		#region Definitions

		private static readonly uint BaseValue = 0x80000000;

		#endregion

		/// <summary>
		/// 
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort configAddress;

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort configData;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCIController"/> class.
		/// </summary>
		public PCIController() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "PCI_0x" + base.hardwareResources.GetIOPort(0, 0).Address.ToString("X");

			configAddress = base.hardwareResources.GetIOPort(0, 0);
			configData = base.hardwareResources.GetIOPort(0, 4);

			return true;
		}

		/// <summary>
		/// Probes for this device.
		/// </summary>
		/// <returns></returns>
		public bool Probe()
		{
			configAddress.Write32(BaseValue);

			if (configAddress.Read32() != BaseValue)
				return false;

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			if (Probe())
			{
				base.deviceStatus = DeviceStatus.Online;
				return DeviceDriverStartStatus.Started;
			}
			else
			{
				base.deviceStatus = DeviceStatus.NotFound;
				return DeviceDriverStartStatus.NotFound;
			}
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return false; }

		/// <summary>
		/// Gets the index.
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		protected uint GetIndex(byte bus, byte slot, byte function, byte register)
		{
			return (uint)(BaseValue
					   | (uint)((bus & 0xFF) << 16)
					   | (uint)((slot & 0x0F) << 11)
					   | (uint)((function & 0x07) << 8)
					   | (uint)(register));
		}

		/// <summary>
		/// Reads from configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		public uint ReadConfig32(byte bus, byte slot, byte function, byte register)
		{
			configAddress.Write32(GetIndex(bus, slot, function, register));
			return configData.Read32();
		}

		/// <summary>
		/// Reads from configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		public ushort ReadConfig16(byte bus, byte slot, byte function, byte register)
		{
			configAddress.Write32(GetIndex(bus, slot, function, register));
			return configData.Read16();
		}

		/// <summary>
		/// Reads from configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		public byte ReadConfig8(byte bus, byte slot, byte function, byte register)
		{
			configAddress.Write32(GetIndex(bus, slot, function, register));
			return configData.Read8();
		}

		/// <summary>
		/// Writes to configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <param name="value">The value.</param>
		public void WriteConfig32(byte bus, byte slot, byte function, byte register, uint value)
		{
			configAddress.Write32(GetIndex(bus, slot, function, register));
			configData.Write32(value);
		}

		/// <summary>
		/// Writes to configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <param name="value">The value.</param>
		public void WriteConfig16(byte bus, byte slot, byte function, byte register, ushort value)
		{
			configAddress.Write32(GetIndex(bus, slot, function, register));
			configData.Write16(value);
		}

		/// <summary>
		/// Writes to configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <param name="value">The value.</param>
		public void WriteConfig8(byte bus, byte slot, byte function, byte register, byte value)
		{
			configAddress.Write32(GetIndex(bus, slot, function, register));
			configData.Write8(value);
		}

	}
}
