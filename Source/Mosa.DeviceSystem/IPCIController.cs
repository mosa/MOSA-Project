// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem;

/// <summary>
/// Interface to a PCI Controller
/// </summary>
public interface IPCIController
{
	/// <summary>
	/// Reads from configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <returns></returns>
	uint ReadConfig32(PCIDevice pciDevice, byte register);

	/// <summary>
	/// Reads from configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <returns></returns>
	ushort ReadConfig16(PCIDevice pciDevice, byte register);

	/// <summary>
	/// Reads from configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <returns></returns>
	byte ReadConfig8(PCIDevice pciDevice, byte register);

	/// <summary>
	/// Writes to configuration space
	/// </summary>
	/// <param name="bus">The bus.</param>
	/// <param name="slot">The slot.</param>
	/// <param name="function">The function.</param>
	/// <param name="register">The register.</param>
	/// <param name="value">The value.</param>
	void WriteConfig32(PCIDevice pciDevice, byte register, uint value);

	/// <summary>
	/// Writes to configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <param name="value">The value.</param>
	void WriteConfig16(PCIDevice pciDevice, byte register, ushort value);

	/// <summary>
	/// Writes to configuration space
	/// </summary>
	/// <param name="pciDevice">The PCI Device.</param>
	/// <param name="register">The register.</param>
	/// <param name="value">The value.</param>
	void WriteConfig8(PCIDevice pciDevice, byte register, byte value);
}
