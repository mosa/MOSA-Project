using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	///
	/// </summary>
	public static class SmbiosManager
	{	
		/// <summary>
		///		Holds the smbios entry point
		/// </summary>
		private static uint entryPoint = 0u;
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
		///
		/// </summary>
		private static uint majorVersion = 0u;
		/// <summary>
		///
		/// </summary>
		private static uint minorVersion = 0u;
		
		/// <summary>
		///		Checks if SMBIOS is available
		/// </summary>
		public static bool IsAvailable
		{
			get { return EntryPoint != 0x100000u; }
		}
		
		/// <summary>
		///		Gets the table's entry point
		/// </summary>
		public static uint EntryPoint 
		{
			get { return entryPoint; }
		}
		
		/// <summary>
		///
		/// </summary>
		public static uint MajorVersion
		{
			get { return majorVersion; }
		}

		/// <summary>
		///
		/// </summary>
		public static uint MinorVersion
		{
			get { return minorVersion; }
		}
		
		/// <summary>
		///		Gets the table's length
		/// </summary>
		public static uint TableLength
		{
			get { return tableLength; }
		}
		
		/// <summary>
		///		Gets the entry address for smbios structures
		/// </summary>
		public static uint TableAddress
		{
			get { return tableAddress; }
		}
		
		/// <summary>
		///		Gets the total number of available structures
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
			LocateEntryPoint ();
			
			if (!IsAvailable)
				return;
			
			GetTableAddress ();
			GetTableLength ();
			GetNumberOfStructures ();
			GetMajorVersion ();
			GetMinorVersion ();
		}
				
		/// <summary>
		///		
		/// </summary>
		public static uint GetStructureOfType (byte type)
		{
			uint address = TableAddress;
			while (GetType (address) != 127u)
			{
				if (GetType (address) == type)
					return address;
				address = GetAddressOfNextStructure (address);
			}
			
			return 0xFFFFu;
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
			byte length = Native.Get8 (address + 0x01u);
			uint endOfFormattedArea = address + length;
			
			while (Native.Get16 (endOfFormattedArea) != 0x0000u)
				++endOfFormattedArea;
			endOfFormattedArea += 0x02u;
			return endOfFormattedArea;
		}
		
		/// <summary>
		///
		/// </summary>
		/// <returns>
		///
		/// </returns>
		private static void LocateEntryPoint ()
		{
			uint memory = 0xF0000u;
			
			while (memory < 0x100000u)
			{
				char a = (char)Native.Get8 (memory);
				char s = (char)Native.Get8 (memory + 1u);
				char m = (char)Native.Get8 (memory + 2u);
				char b = (char)Native.Get8 (memory + 3u);
				
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
		///
		/// </summary>
		private static void GetMajorVersion ()
		{
			majorVersion = Native.Get8 (EntryPoint + 0x06u);
		}
		
		/// <summary>
		///
		/// </summary>
		private static void GetMinorVersion ()
		{
			minorVersion = Native.Get8 (EntryPoint + 0x07u);
		}
		
		/// <summary>
		///
		/// </summary>
		private static void GetTableAddress ()
		{
			tableAddress = Native.Get32 (EntryPoint + 0x18u);
		}
		
		/// <summary>
		///
		/// </summary>
		private static void GetTableLength ()
		{
			tableLength = Native.Get16 (EntryPoint + 0x16u);
		}
		
		/// <summary>
		///
		/// </summary>
		private static void GetNumberOfStructures ()
		{
			numberOfStructures = Native.Get16 (EntryPoint + 0x1Cu);
		}
	}
}