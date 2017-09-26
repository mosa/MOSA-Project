// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	/// Smbios Manager
	/// </summary>
	public static class SmbiosManager
	{
		/// <summary>
		/// Holds the smbios entry point
		/// </summary>
		private static uint entryPoint = 0u;

		/// <summary>
		/// The table address
		/// </summary>
		private static uint tableAddress = 0u;

		/// <summary>
		/// The table length
		/// </summary>
		private static uint tableLength = 0u;

		/// <summary>
		/// The number of structures
		/// </summary>
		private static uint numberOfStructures = 0u;

		/// <summary>
		/// The major version
		/// </summary>
		private static uint majorVersion = 0u;

		/// <summary>
		/// The minor version
		/// </summary>
		private static uint minorVersion = 0u;

		/// <summary>
		/// Checks if SMBIOS is available
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
		/// </value>
		public static bool IsAvailable
		{
			get { return EntryPoint != 0x100000u; }
		}

		/// <summary>
		/// Gets the table's entry point
		/// </summary>
		/// <value>
		/// The entry point.
		/// </value>
		public static uint EntryPoint
		{
			get { return entryPoint; }
		}

		/// <summary>
		/// Gets the major version.
		/// </summary>
		/// <value>
		/// The major version.
		/// </value>
		public static uint MajorVersion
		{
			get { return majorVersion; }
		}

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		/// <value>
		/// The minor version.
		/// </value>
		public static uint MinorVersion
		{
			get { return minorVersion; }
		}

		/// <summary>
		/// Gets the table's length
		/// </summary>
		/// <value>
		/// The length of the table.
		/// </value>
		public static uint TableLength
		{
			get { return tableLength; }
		}

		/// <summary>
		/// Gets the entry address for smbios structures
		/// </summary>
		/// <value>
		/// The table address.
		/// </value>
		public static uint TableAddress
		{
			get { return tableAddress; }
		}

		/// <summary>
		/// Gets the total number of available structures
		/// </summary>
		/// <value>
		/// The number of structures.
		/// </value>
		public static uint NumberOfStructures
		{
			get { return numberOfStructures; }
		}

		/// <summary>
		/// Setups this instance.
		/// </summary>
		public static void Setup()
		{
			LocateEntryPoint();

			if (!IsAvailable)
				return;

			GetTableAddress();
			GetTableLength();
			GetNumberOfStructures();
			GetMajorVersion();
			GetMinorVersion();
		}

		/// <summary>
		/// Gets the type of the structure of.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static uint GetStructureOfType(byte type)
		{
			uint address = TableAddress;
			while (GetType(address) != 127u)
			{
				if (GetType(address) == type)
					return address;
				address = GetAddressOfNextStructure(address);
			}

			return 0xFFFFu;
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static byte GetType(uint address)
		{
			return Intrinsic.Load8(address);
		}

		/// <summary>
		/// Gets the address of next structure.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static uint GetAddressOfNextStructure(uint address)
		{
			byte length = Intrinsic.Load8(address, 0x01u);
			uint endOfFormattedArea = address + length;

			while (Intrinsic.Load16(endOfFormattedArea) != 0x0000u)
				++endOfFormattedArea;
			endOfFormattedArea += 0x02u;
			return endOfFormattedArea;
		}

		/// <summary>
		/// Locates the entry point.
		/// </summary>
		private static void LocateEntryPoint()
		{
			uint memory = 0xF0000u;

			while (memory < 0x100000u)
			{
				char a = (char)Intrinsic.Load8(memory);
				char s = (char)Intrinsic.Load8(memory, 1u);
				char m = (char)Intrinsic.Load8(memory, 2u);
				char b = (char)Intrinsic.Load8(memory, 3u);

				if (a == '_' && s == 'S' && m == 'M' && b == '_')
				{
					entryPoint = memory;
					return;
				}

				memory += 0x10u;
			}

			entryPoint = memory;
		}

		/// <summary>
		/// Gets the major version.
		/// </summary>
		private static void GetMajorVersion()
		{
			majorVersion = Intrinsic.Load8(EntryPoint, 0x06u);
		}

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		private static void GetMinorVersion()
		{
			minorVersion = Intrinsic.Load8(EntryPoint, 0x07u);
		}

		/// <summary>
		/// Gets the table address.
		/// </summary>
		private static void GetTableAddress()
		{
			tableAddress = Intrinsic.Load32(EntryPoint, 0x18u);
		}

		/// <summary>
		/// Gets the length of the table.
		/// </summary>
		private static void GetTableLength()
		{
			tableLength = Intrinsic.Load16(EntryPoint, 0x16u);
		}

		/// <summary>
		/// Gets the number of structures.
		/// </summary>
		private static void GetNumberOfStructures()
		{
			numberOfStructures = Intrinsic.Load16(EntryPoint, 0x1Cu);
		}
	}
}
