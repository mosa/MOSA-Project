// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Extension;
using System;

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
		public static IntPtr EntryPoint { get; private set; }

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
		public static IntPtr TableAddress { get; private set; }

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
		public static bool IsAvailable { get { return EntryPoint != new IntPtr(0x100000); } }

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
		public static IntPtr GetStructureOfType(byte type)
		{
			var address = TableAddress;

			while (GetType(address) != 127u)
			{
				if (GetType(address) == type)
					return address;

				address = GetAddressOfNextStructure(address);
			}

			return new IntPtr(0xFFFF);
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static byte GetType(IntPtr address)
		{
			return Intrinsic.Load8(address);
		}

		/// <summary>
		/// Gets the address of next structure.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static IntPtr GetAddressOfNextStructure(IntPtr address)
		{
			byte length = Intrinsic.Load8(address, 0x01);
			var endOfFormattedArea = address + length;

			while (Intrinsic.Load16(endOfFormattedArea) != 0x0000)
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
			var memory = new IntPtr(0xF0000);

			while (memory.LessThan(new IntPtr(0x100000)))
			{
				char a = (char)Intrinsic.Load8(memory);
				char s = (char)Intrinsic.Load8(memory, 1u);
				char m = (char)Intrinsic.Load8(memory, 2u);
				char b = (char)Intrinsic.Load8(memory, 3u);

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
			MajorVersion = Intrinsic.Load8(EntryPoint, 0x06u);
		}

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		private static void GetMinorVersion()
		{
			MinorVersion = Intrinsic.Load8(EntryPoint, 0x07u);
		}

		/// <summary>
		/// Gets the table address.
		/// </summary>
		private static void GetTableAddress()
		{
			TableAddress = Intrinsic.LoadPointer(EntryPoint, 0x18u);
		}

		/// <summary>
		/// Gets the length of the table.
		/// </summary>
		private static void GetTableLength()
		{
			TableLength = Intrinsic.Load16(EntryPoint, 0x16u);
		}

		/// <summary>
		/// Gets the number of structures.
		/// </summary>
		private static void GetNumberOfStructures()
		{
			NumberOfStructures = Intrinsic.Load16(EntryPoint, 0x1Cu);
		}
	}
}
