// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.ClassLib
{
	/// <summary>
	/// Simple Checksum
	/// </summary>
	public static class CRC
	{
		public const uint InitialCRC = 0xFFFFFFFF;

		public static uint Update(uint crc, byte value)
		{
			// note: CRC must starts with 0xFFFFFFFF

			crc = crc ^ value;
			for (int i = 0; i < 8; i++)
			{
				int mask = -((int)crc & 1);
				crc = (uint)((crc >> 1) ^ (0xEDB88320 & mask));
			}

			return crc;
		}
	}
}
