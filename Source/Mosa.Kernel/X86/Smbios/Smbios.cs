using Mosa.Platform.x86;
using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.X86.Smbios
{
	/// <summary>
	///
	/// </summary>
	public static class Smbios
	{
		/// <summary>
		///
		/// </summary>
		private struct StructureHeader
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
		public static void Setup ()
		{
			entryPoint = LocateEntryPoint ();
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
	}
}