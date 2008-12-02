/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Kernel.Memory.X86
{
	/// <summary>
	/// Static class of helpful memory functions
	/// </summary>
	public static class Multiboot
	{
		/// <summary>
		/// Set by the kernel immediately upon startup. 
		/// </summary>
		public static uint MultibootAddress = 0x0;

		/// <summary>
		/// Sets the multiboot location.
		/// </summary>
		/// <param name="address">The address.</param>
		public static void SetMultibootLocation(uint address)
		{
			MultibootAddress = address;
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public static byte Flags
		{
			get
			{
				if (MultibootAddress == 0) return 0;

				return Util.Get8(MultibootAddress);
			}
		}

		/// <summary>
		/// Gets the memory lower.
		/// </summary>
		/// <value>The lower memory.</value>
		public static uint MemoryLower
		{
			get
			{
				return Util.Get32(MultibootAddress + 4);
			}
		}

		/// <summary>
		/// Gets the memory upper.
		/// </summary>
		/// <value>The memory upper.</value>
		public static uint MemoryUpper
		{
			get
			{
				return Util.Get32(MultibootAddress + 8);
			}
		}

		/// <summary>
		/// Gets the boot device.
		/// </summary>
		/// <value>The boot device.</value>
		public static uint BootDevice
		{
			get
			{
				return Util.Get32(MultibootAddress + 12);
			}
		}

		/// <summary>
		/// Gets the CMD line address.
		/// </summary>
		/// <value>The CMD line address.</value>
		public static uint CmdLineAddress
		{
			get
			{
				return Util.Get32(MultibootAddress + 16);
			}
		}

		/// <summary>
		/// Gets the modules start.
		/// </summary>
		/// <value>The modules start.</value>
		public static uint ModulesStart
		{
			get
			{
				return Util.Get32(MultibootAddress + 20);
			}
		}

		/// <summary>
		/// Gets the modules count.
		/// </summary>
		/// <value>The modules count.</value>
		public static uint ModulesCount
		{
			get
			{
				return Util.Get32(MultibootAddress + 24);
			}
		}

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <value>The length of the memory map.</value>
		public static uint MemoryMapLength
		{
			get
			{
				return Util.Get32(MultibootAddress + 44);
			}
		}

		/// <summary>
		/// Gets the memory map start.
		/// </summary>
		/// <value>The memory map start.</value>
		public static uint MemoryMapStart
		{
			get
			{
				return Util.Get32(MultibootAddress + 48);
			}
		}

		/// <summary>
		/// Gets the memory map index location.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private static uint GetMemoryMapIndexLocation(uint index)
		{
			uint location = MemoryMapStart;

			for (uint i = 0; i < index; i++)
				location = location + Util.Get32(location - 4);

			return location;
		}

		/// <summary>
		/// Gets the memory map base.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static ulong GetMemoryMapBase(uint index)
		{
			return Util.Get64(GetMemoryMapIndexLocation(index) + 0);
		}

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static ulong GetMemoryMapLength(uint index)
		{
			return Util.Get64(GetMemoryMapIndexLocation(index) + 8);
		}

		/// <summary>
		/// Gets the type of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static ulong GetMemoryMapType(uint index)
		{
			return Util.Get64(GetMemoryMapIndexLocation(index) + 16);
		}

		/// <summary>
		/// Gets the length of the drive.
		/// </summary>
		/// <value>The length of the drive.</value>
		public static uint DriveLength
		{
			get
			{
				return Util.Get32(MultibootAddress + 52);
			}
		}

		/// <summary>
		/// Gets the drive start.
		/// </summary>
		/// <value>The drive start.</value>
		public static uint DriveStart
		{
			get
			{
				return Util.Get32(MultibootAddress + 56);
			}
		}

		/// <summary>
		/// Gets the configuration table.
		/// </summary>
		/// <value>The configuration table.</value>
		public static uint ConfigurationTable
		{
			get
			{
				return Util.Get32(MultibootAddress + 60);
			}
		}

		/// <summary>
		/// Gets the name of the boot loader.
		/// </summary>
		/// <value>The name of the boot loader.</value>
		public static uint BootLoaderName
		{
			get
			{
				return Util.Get32(MultibootAddress + 64);
			}
		}

		/// <summary>
		/// Gets the APM table.
		/// </summary>
		/// <value>The APM table.</value>
		public static uint APMTable
		{
			get
			{
				return Util.Get32(MultibootAddress + 68);
			}
		}

		/// <summary>
		/// Gets the VBE control information.
		/// </summary>
		/// <value>The VBE control information.</value>
		public static uint VBEControlInformation
		{
			get
			{
				return Util.Get32(MultibootAddress + 72);
			}
		}


		/// <summary>
		/// Gets the VBE mode info.
		/// </summary>
		/// <value>The VBE mode info.</value>
		public static uint VBEModeInfo
		{
			get
			{
				return Util.Get32(MultibootAddress + 72);
			}
		}

		/// <summary>
		/// Gets the VBE mode.
		/// </summary>
		/// <value>The VBE mode.</value>
		public static uint VBEMode
		{
			get
			{
				return Util.Get32(MultibootAddress + 76);
			}
		}

		/// <summary>
		/// Gets the VBE interface seg.
		/// </summary>
		/// <value>The VBE interface seg.</value>
		public static uint VBEInterfaceSeg
		{
			get
			{
				return Util.Get32(MultibootAddress + 80);
			}
		}

		/// <summary>
		/// Gets the VBE interface off.
		/// </summary>
		/// <value>The VBE interface off.</value>
		public static uint VBEInterfaceOff
		{
			get
			{
				return Util.Get32(MultibootAddress + 84);
			}
		}

		/// <summary>
		/// Gets the VBE interface len.
		/// </summary>
		/// <value>The VBE interface len.</value>
		public static uint VBEInterfaceLen
		{
			get
			{
				return Util.Get32(MultibootAddress + 86);
			}
		}
	}
}
