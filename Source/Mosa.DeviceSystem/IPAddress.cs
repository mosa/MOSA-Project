// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

/// <summary>
/// IP Address
/// </summary>
public struct IPAddress
{
	public static readonly IPAddress Default = new IPAddress(0, 0, 0, 0);
	public static readonly IPAddress Local = new IPAddress(127, 0, 0, 1);

	private readonly uint address;

	public readonly byte A => (byte)address;

	public readonly byte B => (byte)(address >> 8);

	public readonly byte C => (byte)(address >> 16);

	public readonly byte D => (byte)(address >> 24);

	public IPAddress(IPAddress ipAddress) => address = ipAddress.address;

	public IPAddress(byte a, byte b, byte c, byte d) => address = a | ((uint)b << 8) | ((uint)c << 16) | ((uint)d << 24);

	private IPAddress(uint address)
	{
		this.address = address;
	}

	public static bool TryParse(string src, out IPAddress ipAddress)
	{
		int octetCount = 0;
		int startIndex = 0;
		int endIndex = 0;
		uint parsedOctets = 0;

		for (int i = 0; i < src.Length; i++)
		{
			if (src[i] == '.')
			{
				if (octetCount > 3)
				{
					ipAddress = default;
					return false;
				}

				if (startIndex == endIndex)
				{
					ipAddress = default;
					return false;
				}

				if (uint.TryParse(src.Substring(startIndex, endIndex - startIndex), out uint octetValue))
				{
					if (octetValue <= 255)
					{
						parsedOctets |= octetValue << (8 * (3 - octetCount));
						octetCount++;
					}
					else
					{
						ipAddress = default;

						return false;
					}
				}
				else
				{
					ipAddress = default;
					return false;
				}

				startIndex = endIndex + 1;
			}

			endIndex++;
		}

		if (octetCount != 3)
		{
			ipAddress = default;

			return false;
		}

		if (int.TryParse(src.Substring(startIndex), out int lastOctet))
		{
			if (lastOctet >= 0 && lastOctet <= 255)
			{
				ipAddress = new IPAddress(parsedOctets);
				return true;
			}
		}

		ipAddress = default;
		return false;
	}

	public static IPAddress Parse(string src)
	{
		return TryParse(src, out var address) ? address : default;
	}

	public override readonly string ToString() => $"{A}.{B}.{C}.{D}";
}
