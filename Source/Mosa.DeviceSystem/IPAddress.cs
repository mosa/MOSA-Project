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

	public override readonly string ToString() => $"{A}.{B}.{C}.{D}";
}
