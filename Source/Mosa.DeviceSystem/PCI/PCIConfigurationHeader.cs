// Copyright (c) MOSA Project. Licensed under the New BSD License.
namespace Mosa.DeviceSystem.PCI;

/// <summary>
/// Provides the most common fields used in the PCI configuration header.
/// </summary>
public struct PCIConfigurationHeader
{
	public const int VendorID = 0x00;
	public const int DeviceID = 0x02;
	public const int CommandRegister = 0x04;
	public const int StatusRegister = 0x06;
	public const int RevisionID = 0x08;
	public const int ProgrammingInterface = 0x09;
	public const int SubClassCode = 0x0A;
	public const int ClassCode = 0x0B;
	public const int CacheLineSize = 0xC;
	public const int LatencyTimer = 0xD;
	public const int HeaderType = 0xE;
	public const int BIST = 0xF;
	public const int BaseAddressRegisterBase = 0x10;
	public const int BaseAddressRegister1 = 0x10;
	public const int BaseAddressRegister2 = 0x14;
	public const int BaseAddressRegister3 = 0x18;
	public const int BaseAddressRegister4 = 0x1C;
	public const int BaseAddressRegister5 = 0x20;
	public const int BaseAddressRegister6 = 0x24;
	public const int CardbusCISPointer = 0x28;
	public const int SubSystemVendorID = 0x2C;
	public const int SubSystemID = 0x2E;
	public const int ExpansionROMBaseAddress = 0x30;
	public const int CapabilitiesPointer = 0x34;
	public const int InterruptLineRegister = 0x3C;
	public const int InterruptPinRegister = 0x3D;
	public const int MIN_GNT = 0x3E;
	public const int MAX_LAT = 0x3F;

	//public const int CapabilityID = 0x80;
	//public const int NextCapabilityPointer = 0x81;
	//public const int PowerManagementCapabilities = 0x82;
	//public const int PowerManagementControlStatusRegister = 0x84;
	//public const int BridgeSupportExtension = 0x86;
	//public const int PowerManagementDataRegister = 0x87;
	//public const int CapabilityID = 0xA0;
	//public const int NextCapabilityPointer = 0xA1;
	//public const int MessageControl = 0xA2;
	//public const int MessageAddress = 0xA4;
	//public const int MessageData = 0xA8;
	//public const int MaskBitsforMSI = 0xAC;
	//public const int PendingBitsforMSI = 0xB0;
}
