/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// 
	/// </summary>
	public static class SubClassCodeTable
	{
		/// <summary>
		/// Looks up the specified sub class code.
		/// </summary>
		/// <param name="classCode">The class code.</param>
		/// <param name="subClassCode">The sub class code.</param>
		/// <param name="progIF">The prog IF.</param>
		/// <returns></returns>
		public static string Lookup(byte classCode, byte subClassCode, byte progIF)
		{
			switch ((classCode << 16) | (subClassCode << 8) | (progIF))
			{
				case 0x000000: return "Non-VGA Device";
				case 0x000001: return "VGA Device";

				case 0x010000: return "SCSI Controller";
				case 0x010100: return "IDE controller";
				case 0x010101: return "IDE controller";
				case 0x010102: return "IDE controller";
				case 0x010103: return "IDE controller";
				case 0x010104: return "IDE controller [reserved]";
				case 0x010105: return "IDE controller [reserved]";
				case 0x010106: return "IDE controller [reserved]";
				case 0x010107: return "IDE controller [Master IDE Device]";
				case 0x010200: return "Floppy Disk";
				case 0x010300: return "IPI controller";
				case 0x010400: return "RAID controller";
				case 0x010520: return "ATA controller [Single DMA]";
				case 0x010530: return "ATA controller [Chained DMA]";
				case 0x010600: return "Serial ATA controller";
				case 0x010601: return "Serial ATA controller [ACHI]";
				case 0x010700: return "Serial Attached SCSI (SAS) controller";
				case 0x018000: return "Mass storage controller";

				case 0x020000: return "Ethernet controller";
				case 0x020100: return "Token ring";
				case 0x020200: return "FDDI controller";
				case 0x020300: return "ATM controller";
				case 0x020500: return "WorldFlip controller";
				case 0x020600: return "PCIMG 2.14 controller"; // Prog. I/F xxh per spec
				case 0x028000: return "Network controller";

				case 0x030000: return "VGA compatable controller";
				case 0x030001: return "8514 compatable";
				case 0x030100: return "XGA controller";
				case 0x030200: return "3D controller";
				case 0x038080: return "Display controller";

				case 0x040000: return "Video device";
				case 0x040100: return "Audio device";
				case 0x040200: return "Computer telephony device";
				case 0x048000: return "Multimedia device";

				case 0x050000: return "RAM controller";
				case 0x050100: return "Flash memory controller";
				case 0x058000: return "Memory controller";

				case 0x060000: return "Host/PCI bridge";
				case 0x060100: return "ISA bridge";
				case 0x060200: return "EISA bridge";
				case 0x060300: return "Micro Channel bridge";
				case 0x060400: return "PCI-to-PCI bridge";
				case 0x060401: return "Subtractive PCI-to-PCI bridge";
				case 0x060500: return "PCMCIA bridge";
				case 0x060600: return "NuBus bridge";
				case 0x060700: return "CardBus bridge";
				case 0x060800: return "RACEWay bridge"; // Prog. I/F xxh per spec
				case 0x060940: return "Semi-transparent PCI-to-PCI bridge [Primary]";
				case 0x060980: return "Semi-transparent PCI-to-PCI bridge [Secondary]";
				case 0x060A00: return "InfiniBand bridge";
				case 0x068000: return "Bridge device";

				case 0x070001: return "Generic XT compatable serial controller";
				case 0x070002: return "16450-compatable serial controller";
				case 0x070003: return "16550-compatable serial controller";
				case 0x070004: return "16750-compatable serial controller";
				case 0x070005: return "16850-compatable serial controller";
				case 0x070006: return "16950-compatable serial controller";
				case 0x070100: return "Parallel port";
				case 0x070101: return "Bi-directional parallel port";
				case 0x070102: return "ECP 1.X parallel port";
				case 0x070103: return "IEEE1284 controller";
				case 0x0701FE: return "IEEE1284 target device";
				case 0x070200: return "Multiport serial controller";
				case 0x070300: return "Generic modem";
				case 0x070301: return "Hayes compatible modem [16450]";
				case 0x070302: return "Hayes compatible modem [16550]";
				case 0x070303: return "Hayes compatible modem [16650]";
				case 0x070304: return "Hayes compatible modem [16750]";
				case 0x070400: return "GPIB device";
				case 0x070500: return "Smart Card";
				case 0x078000: return "Communications device";

				case 0x080000: return "Generic 8259 programmable interrupt controller (PIC)";
				case 0x080001: return "ISA PIC";
				case 0x080002: return "EISA PIC";
				case 0x080010: return "I/O APIC interrupt controller";
				case 0x080020: return "I/O APIC interrupt controller";
				case 0x080100: return "Generic 8237 DMA controller";
				case 0x080101: return "ISA DMA controller";
				case 0x080102: return "EISA DMA controller";
				case 0x080200: return "Generic 8254 timer";
				case 0x080201: return "ISA system timer";
				case 0x080202: return "EISA system timer";
				case 0x080300: return "Generic RTC controller";
				case 0x080301: return "ISA RTC controller";
				case 0x080400: return "Generic PCI Hot-Plug controller";
				case 0x080500: return "SD Host controller";
				case 0x088000: return "System peripheral";

				case 0x090000: return "Keyboard controller";
				case 0x090100: return "Digitizer (pen)";
				case 0x090200: return "Mouse controller";
				case 0x090300: return "Scanner controller";
				case 0x090400: return "Gameport controller";
				case 0x090410: return "Gameport controller";
				case 0x098000: return "Input controller";

				case 0x0A0000: return "Generic docking station";
				case 0x0A8000: return "Docking station";

				case 0x0B0000: return "386";
				case 0x0B0100: return "486";
				case 0x0B0200: return "Pentium";
				case 0x0B1000: return "Alpha";
				case 0x0B2000: return "PowerPC";
				case 0x0B4000: return "Co-Processor";

				case 0x0C0000: return "IEEE 1394 (FireWire)";
				case 0x0C0110: return "IEEE 1394 (FireWire) [OpenHCI]";
				case 0x0C0100: return "ACCESS bus";
				case 0x0C0200: return "SSA (Serial Storage Architecture)";
				case 0x0C0300: return "USB (Universal Serial Bus)";
				case 0x0C0310: return "USB (Universal Serial Bus)";
				case 0x0C0320: return "USB (Universal Serial Bus)";
				case 0x0C0380: return "USB (Universal Serial Bus)";
				case 0x0C03FE: return "USB (Universal Serial Bus)";
				case 0x0C0400: return "Fibre Channel";
				case 0x0C0500: return "SMBus (System Management Bus)";
				case 0x0C0600: return "InfiniBand";
				case 0x0C0700: return "IPMI SMIC interface";
				case 0x0C0701: return "IPMI Kybd controller interface";
				case 0x0C0702: return "IPMI Block Transfer interface";
				case 0x0C0800: return "SERCOS interface (IEC 61491)";
				case 0x0C0900: return "CANbus";

				case 0x0D0000: return "iRDA compatible controller";
				case 0x0D0100: return "Consumer IR controller";
				case 0x0D1000: return "RF controller";
				case 0x0D1100: return "Bluetooth";
				case 0x0D1200: return "Broadband";
				case 0x0D2000: return "Ethernet (802.11a – 5 GHz)";
				case 0x0D2100: return "Ethernet (802.11b – 2.4 GHz)";
				case 0x0D8000: return "Wireless controller";

				case 0x0E0000: return "Intelligent I/O (I2O)";

				case 0x0F0100: return "TV";
				case 0x0F0200: return "Audio";
				case 0x0F0300: return "Voice";
				case 0x0F0400: return "Data";

				case 0x100000: return "Network and computing encryption";
				case 0x101000: return "Entertainment encryption";
				case 0x108000: return "Other encryption";

				case 0x110000: return "DPIO modules";
				case 0x110100: return "Performance counters";
				case 0x111000: return "Communications synchronization";
				case 0x112000: return "Management card";
				case 0x118000: return "Data acquisition/signal processing controllers";

				default: return string.Empty;
			}
		}
	}
}
