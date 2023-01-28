// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP;

internal static class Checksum
{
	public static byte Calculate(byte[] data)
	{
		uint sum = 0;

		foreach (var b in data)
		{
			sum += b;
		}

		return (byte)(sum % 256);
	}
}
