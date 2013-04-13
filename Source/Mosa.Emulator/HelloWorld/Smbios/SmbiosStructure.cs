using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	///
	/// </summary>
	public abstract class SmbiosStructure
	{
		protected uint address = 0u;
		protected uint length = 0u;
		protected uint handle = 0u;

		protected SmbiosStructure(uint address)
		{
			this.address = address;
			this.length = Native.Get8(address + 0x01u);
			this.handle = Native.Get16(address + 0x02u);
		}

		protected string GetStringFromIndex(byte index)
		{
			if (index == 0)
				return string.Empty;

			uint stringStart = this.address + this.length;
			int count = 1;

			while (count++ != index)
				while (Native.Get8(stringStart++) != 0x00u)
					;

			uint stringEnd = stringStart;
			while (Native.Get8(++stringEnd) != 0x00u)
				;

			int stringLength = (int)(stringEnd - stringStart);
			string result = string.Empty;

			for (uint i = 0; i < stringLength; ++i)
				result = string.Concat(result, new string((char)Native.Get8(stringStart + i), 1));

			return result;
		}
	}
}