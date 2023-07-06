// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceDriver.ISA;

/// <summary>
/// PCI Controller
/// </summary>
//[ISADeviceDriver(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
public sealed class PCIController : BaseDeviceDriver, IPCIControllerLegacy, IPCIController
{
	#region Definitions

	private const uint BaseValue = 0x80000000;

	#endregion Definitions

	/// <summary>
	/// The configuration address
	/// </summary>
	private IOPortReadWrite configAddress;

	/// <summary>
	/// The configuration data
	/// </summary>
	private IOPortReadWrite configData;

	public override void Initialize()
	{
		Device.Name = "PCI_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

		configAddress = Device.Resources.GetIOPortReadWrite(0, 0);
		configData = Device.Resources.GetIOPortReadWrite(0, 4);
	}

	/// <summary>
	/// Probes for this device.
	/// </summary>
	/// <returns></returns>
	public override void Probe()
	{
		configAddress.Write32(BaseValue);

		var found = configAddress.Read32() == BaseValue;

		Device.Status = found ? DeviceStatus.Available : DeviceStatus.NotFound;
	}

	public override void Start()
	{
		if (Device.Status == DeviceStatus.Available)
		{
			Device.Status = DeviceStatus.Online;
		}
	}

	/// <summary>
	/// Called when an interrupt is received.
	/// </summary>
	/// <returns></returns>
	public override bool OnInterrupt()
	{
		// TODO
		return false;
	}

	/// <summary>
	/// Gets the index.
	/// </summary>
	/// <param name="bus">The bus.</param>
	/// <param name="slot">The slot.</param>
	/// <param name="function">The function.</param>
	/// <param name="register">The register.</param>
	/// <returns></returns>
	private static uint GetIndex(byte bus, byte slot, byte function, byte register)
	{
		return BaseValue
			   | (uint)((bus & 0xFF) << 16)
			   | (uint)((slot & 0x0F) << 11)
			   | (uint)((function & 0x07) << 8)
			   | (uint)(register & 0xFC);
	}

	#region IPCIController

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
		return (ushort)((configData.Read32() >> (register % 4 * 8)) & 0xFFFF);
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
		return (byte)((configData.Read32() >> (register % 4 * 8)) & 0xFF);
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

	#endregion IPCIController

	#region IPCIController v2

	/// <summary>
	/// Reads from configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <returns></returns>
	public uint ReadConfig32(PCIDevice pciDevice, byte register)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		return configData.Read32();
	}

	/// <summary>
	/// Reads from configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <returns></returns>
	public ushort ReadConfig16(PCIDevice pciDevice, byte register)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		return (ushort)((configData.Read32() >> (register % 4 * 8)) & 0xFFFF);
	}

	/// <summary>
	/// Reads from configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <returns></returns>
	public byte ReadConfig8(PCIDevice pciDevice, byte register)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		return (byte)((configData.Read32() >> (register % 4 * 8)) & 0xFF);
	}

	/// <summary>
	/// Writes to configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <param name="value">The value.</param>
	public void WriteConfig32(PCIDevice pciDevice, byte register, uint value)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		configData.Write32(value);
	}

	/// <summary>
	/// Writes to configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <param name="value">The value.</param>
	public void WriteConfig16(PCIDevice pciDevice, byte register, ushort value)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		configData.Write16(value);
	}

	/// <summary>
	/// Writes to configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <param name="value">The value.</param>
	public void WriteConfig8(PCIDevice pciDevice, byte register, byte value)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		configData.Write8(value);
	}

	#endregion IPCIController v2
}
