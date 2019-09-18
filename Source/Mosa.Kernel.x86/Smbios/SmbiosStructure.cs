// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	/// Smbios Structure
	/// </summary>
	public abstract class SmbiosStructure
	{
		protected Pointer address;
		protected uint length;
		protected uint handle;

		protected SmbiosStructure(Pointer address)
		{
			this.address = address;
			length = Intrinsic.Load8(address, 0x01u);
			handle = Intrinsic.Load16(address, 0x02u);
		}

		protected unsafe string GetStringFromIndex(byte index)
		{
			if (index == 0)
				return string.Empty;

			var first = address + (int)length;
			int offset = 0;

			for (byte count = 1; count != index;)
			{
				if (Intrinsic.Load8(first, offset++) == 0x00)
					count++;
			}

			var start = first + offset;
			var end = start;
			int len = 0;

			while (Intrinsic.Load8(end, len++) != 0x00)
			{
			}

			return new string((sbyte*)start.ToPointer(), 0, len);
		}
	}
}
