// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	/// Smbios Manager
	/// </summary>
	public static class SmbiosManager
	{
		/// <summary>
		/// Gets the table's entry point
		/// </summary>
		/// <value>
		/// The entry point.
		/// </value>
		public static Pointer EntryPoint { get; private set; }

		/// <summary>
		/// Gets the major version.
		/// </summary>
		/// <value>
		/// The major version.
		/// </value>
		public static uint MajorVersion { get; private set; }

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		/// <value>
		/// The minor version.
		/// </value>
		public static uint MinorVersion { get; private set; }

		/// <summary>
		/// Gets the table's length
		/// </summary>
		/// <value>
		/// The length of the table.
		/// </value>
		public static uint TableLength { get; private set; }

		/// <summary>
		/// Gets the entry address for smbios structures
		/// </summary>
		/// <value>
		/// The table address.
		/// </value>
		public static Pointer TableAddress { get; private set; }

		/// <summary>
		/// Gets the total number of available structures
		/// </summary>
		/// <value>
		/// The number of structures.
		/// </value>
		public static uint NumberOfStructures { get; private set; }

		/// <summary>
		/// Checks if SMBIOS is available
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
		/// </value>
		public static bool IsAvailable { get { return EntryPoint != new Pointer(0x100000); } }

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
		public static Pointer GetStructureOfType(byte type)
		{
			var address = TableAddress;

			while (GetType(address) != 127u)
			{
				if (GetType(address) == type)
					return address;

				address = GetAddressOfNextStructure(address);
			}

			return new Pointer(0xFFFF);
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static byte GetType(Pointer address)
		{
			return address.Load8();
		}

		/// <summary>
		/// Gets the address of next structure.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static Pointer GetAddressOfNextStructure(Pointer address)
		{
			byte length = address.Load8(0x01);
			var endOfFormattedArea = address + length;

			while (endOfFormattedArea.Load16() != 0x0000)
			{
				endOfFormattedArea += 1;
			}

			endOfFormattedArea += 0x02;

			return endOfFormattedArea;
		}

		/// <summary>
		/// Locates the entry point.
		/// </summary>
		private static void LocateEntryPoint()
		{
			var memory = new Pointer(0xF0000);

			while (memory < new Pointer(0x100000))
			{
				char a = (char)memory.Load8();
				char s = (char)memory.Load8(1u);
				char m = (char)memory.Load8(2u);
				char b = (char)memory.Load8(3u);

				if (a == '_' && s == 'S' && m == 'M' && b == '_')
				{
					EntryPoint = memory;
					return;
				}

				memory += 0x10;
			}

			EntryPoint = memory;
		}

		/// <summary>
		/// Gets the major version.
		/// </summary>
		private static void GetMajorVersion()
		{
			MajorVersion = EntryPoint.Load8(0x06u);
		}

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		private static void GetMinorVersion()
		{
			MinorVersion = EntryPoint.Load8(0x07u);
		}

		/// <summary>
		/// Gets the table address.
		/// </summary>
		private static void GetTableAddress()
		{
			TableAddress = EntryPoint.LoadPointer(0x18u);
		}

		/// <summary>
		/// Gets the length of the table.
		/// </summary>
		private static void GetTableLength()
		{
			TableLength = EntryPoint.Load16(0x16u);
		}

		/// <summary>
		/// Gets the number of structures.
		/// </summary>
		private static void GetNumberOfStructures()
		{
			NumberOfStructures = EntryPoint.Load16(0x1Cu);
		}
	}
}
