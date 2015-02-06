using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.ClassLib
{
	/// <summary>
	/// Fast checksum algorithm.
	/// </summary>
	/// <remarks>Usefull to track memory changes for debugging purposes</remarks>
	public static class FlechterChecksum
	{
		unsafe public static ushort Fletcher16(uint startAddress, uint bytes)
		{
			return Fletcher16((byte*)startAddress, bytes);
		}

		unsafe public static ushort Fletcher16(byte* data, uint bytes)
		{
			ushort sum1 = 0xff, sum2 = 0xff;

			while (bytes > 0)
			{
				uint tlen = bytes > 20 ? 20 : bytes;
				bytes -= tlen;
				do
				{
					sum2 += sum1 += *data++;
				} while (--tlen > 0);
				sum1 = (ushort)((sum1 & 0xff) + (sum1 >> 8));
				sum2 = (ushort)((sum2 & 0xff) + (sum2 >> 8));
			}
			/* Second reduction step to reduce sums to 8 bits */
			sum1 = (ushort)((sum1 & 0xff) + (sum1 >> 8));
			sum2 = (ushort)((sum2 & 0xff) + (sum2 >> 8));
			return (ushort)(sum2 << 8 | sum1);
		}
	}
}