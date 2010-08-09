using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86.Smbios
{
	/// <summary>
	///
	/// </summary>
	public static class SmbiosManager
	{
		/// <summary>
		///
		/// </summary>
		public struct StructureHeader
		{
			/// <summary>
			///		Specifies the type of structure.
			/// </summary>
			public byte Type;
			/// <summary>
			///		Specifies the length of the formatted area of the structure,
			///		starting at the Type field. The length of the structure's
			///		string-set is not included.
			/// </summary>
			public byte Length;
			/// <summary>
			///		Specifies the structure's handle.
			/// </summary>
			public ushort Handle;
		}
		
		/// <summary>
		///
		/// </summary>
		private static uint entryPoint = 0u;
		
		/// <summary>
		///
		/// </summary>
		private static uint entryPointLength = 0u;
		
		/// <summary>
		///
		/// </summary>
		private static uint tableAddress = 0u;
		
		/// <summary>
		///
		/// </summary>
		private static uint tableLength = 0u;
		
		/// <summary>
		///
		/// </summary>
		private static uint numberOfStructures = 0u;
		
		/// <summary>
		///		Checks if SMBIOS is available
		/// </summary>
		public static bool IsAvailable
		{
			get { return EntryPoint != 0x100000u; }
		}
		
		/// <summary>
		///
		/// </summary>
		public static uint EntryPoint
		{
			get { return entryPoint; }
		}
		
		/// <summary>
		///
		/// </summary>
		public static uint EntryPointLength
		{
			get { return entryPointLength; }
		}
		
		/// <summary>
		///
		/// </summary>
		public static uint TableLength
		{
			get { return tableLength; }
		}
		
		/// <summary>
		///
		/// </summary>
		public static uint TableAddress
		{
			get { return tableAddress; }
		}
		
		/// <summary>
		///
		/// </summary>
		public static uint NumberOfStructures
		{
			get { return numberOfStructures; }
		}
		
		/// <summary>
		///
		/// </summary>
		public static void Setup ()
		{
			entryPoint = LocateEntryPoint ();
			
			if (!IsAvailable)
				return;
				
			entryPointLength = GetEntryPointLength ();
			tableAddress = GetTableAddress ();
			tableLength = GetTableLength ();
			numberOfStructures = GetNumberOfStructures ();
		}
		
		/// <summary>
		///
		/// </summary>
		public static uint GetStructureOfType (byte type)
		{
			uint address = tableAddress;
			for (int i = 0; i < NumberOfStructures; ++i)
			{
				if (GetType (address) == type)
					return address;
				address = GetAddressOfNextStructure (address);
			}
			
			return 0;
		}
		
		/// <summary>
		///
		/// </summary>
		private static byte GetType (uint address)
		{
			return Native.Get8 (address);
		}
		
		/// <summary>
		///
		/// </summary>
		private static uint GetAddressOfNextStructure (uint address)
		{
			uint endOfFormattedArea = Native.Get8 (address + 1) + address;
			
			while (Native.Get8 (endOfFormattedArea) != 0x00 && Native.Get8 (endOfFormattedArea) != 0x00)
				++endOfFormattedArea;
			return endOfFormattedArea + 2;
		}
		
		/// <summary>
		///
		/// </summary>
		/// <returns>
		///
		/// </returns>
		private static uint LocateEntryPoint ()
		{
			uint memory = 0xF0000u;
			byte checksum = 0;
			
			while (memory < 0x100000u)
			{
				byte a = Native.Get8 (memory);
				byte b = Native.Get8 (memory + 1u);
				byte c = Native.Get8 (memory + 2u);
				byte d = Native.Get8 (memory + 3u);
				
				if (a == 0x5F && b == 0x53 && c == 0x4D && d == 0x5F)
				{
					byte length = Native.Get8 (memory + 5);
					
					for (uint i = 0; i < length; ++i)
						checksum += Native.Get8 (memory + i);
					if (checksum == 0)
						return memory;
				}
				memory += 16;
			}
			
			return memory;
		}
		
		/// <summary>
		///
		/// </summary>
		private static byte GetEntryPointLength ()
		{
			return Native.Get8 (EntryPoint + 0x5);
		}
		
		/// <summary>
		///
		/// </summary>
		private static uint GetTableAddress ()
		{
			return Native.Get32 (EntryPoint + 0x18);
		}
		
		/// <summary>
		///
		/// </summary>
		private static uint GetTableLength ()
		{
			return Native.Get16 (EntryPoint + 0x16);
		}
		
		/// <summary>
		///
		/// </summary>
		private static uint GetNumberOfStructures ()
		{
			return Native.Get16 (EntryPoint + 0x1C);
		}
	}
}