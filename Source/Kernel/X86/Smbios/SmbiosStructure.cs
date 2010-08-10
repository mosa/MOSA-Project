using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86.Smbios
{
	/// <summary>
	///
	/// </summary>
	public class SmbiosStructure
	{
		/// <summary>
		///
		/// </summary>
		protected uint type = 0;
		/// <summary>
		///
		/// </summary>
		protected uint length = 0;
		/// <summary>
		///
		/// </summary>
		protected uint handle = 0;
		/// <summary>
		///
		/// </summary>
		protected uint address = 0u;
		
		/// <summary>
		///
		/// </summary>
		/// <param name="address">
		///
		/// </param>
		public SmbiosStructure (uint address)
		{
		}
		
		/// <summary>
		///
		/// </summary>
		/// <param name="address">
		///
		/// </param>
		protected void ReadHeader (uint address)
		{
			this.address = address;
			type = Native.Get8 (address);
			length = Native.Get8 (address + 0x01u);
			handle = Native.Get16 (address + 0x02u);
		}
		
		/// <summary>
		///
		/// </summary>
		/// <param name="index">
		///
		/// </param>
		/// <returns>
		///
		/// </returns>
		protected string GetStringFromIndex (byte index)
		{
			char content = ' ';
			uint stringStart = address + length;
			int count = 1;
			
			while (count != index)
			{
				while (Native.Get8 (stringStart++) != 0x00u)
					;
				++count;
			}
			
			uint stringEnd = stringStart;
			while (Native.Get8 (++stringEnd) != 0x00u)
				;

			string result = new string (' ', (int)(stringEnd - stringStart));
			for (uint i = 0; i < stringEnd - stringStart; ++i)
			{
				content = (char)Native.Get8 (stringStart + i);
				result = string.Concat (result, new string (content, 1));
			}

			return result;
		}
	}
}