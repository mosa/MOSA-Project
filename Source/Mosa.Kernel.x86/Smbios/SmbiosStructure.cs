// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	/// Smbios Structure
	/// </summary>
	public abstract class SmbiosStructure
	{
		protected uint address = 0u;
		protected uint length = 0u;
		protected uint handle = 0u;

		protected SmbiosStructure(uint address)
		{
			this.address = address;
			length = Intrinsic.Load8(address + 0x01u);
			handle = Intrinsic.Load16(address + 0x02u);
		}

		protected string GetStringFromIndex(byte index)
		{
			if (index == 0)
				return string.Empty;

			uint stringStart = address + length;
			int count = 1;

			while (count++ != index)
				while (Intrinsic.Load8(stringStart++) != 0x00u)
					;

			uint stringEnd = stringStart;
			while (Intrinsic.Load8(++stringEnd) != 0x00u)
				;

			int stringLength = (int)(stringEnd - stringStart);
			string result = string.Empty;

			for (uint i = 0; i < stringLength; ++i)
				result = string.Concat(result, new string((char)Intrinsic.Load8(stringStart + i), 1));

			return result;
		}
	}
}
