namespace System.Runtime.Intrinsics.Arm;

[CLSCompliant(false)]
public abstract class Crc32 : ArmBase
{
	public new abstract class Arm64 : ArmBase.Arm64
	{
		public new static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal Arm64()
		{
		}

		public static uint ComputeCrc32(uint crc, ulong data)
		{
			throw null;
		}

		public static uint ComputeCrc32C(uint crc, ulong data)
		{
			throw null;
		}
	}

	public new static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	internal Crc32()
	{
	}

	public static uint ComputeCrc32(uint crc, byte data)
	{
		throw null;
	}

	public static uint ComputeCrc32(uint crc, ushort data)
	{
		throw null;
	}

	public static uint ComputeCrc32(uint crc, uint data)
	{
		throw null;
	}

	public static uint ComputeCrc32C(uint crc, byte data)
	{
		throw null;
	}

	public static uint ComputeCrc32C(uint crc, ushort data)
	{
		throw null;
	}

	public static uint ComputeCrc32C(uint crc, uint data)
	{
		throw null;
	}
}
