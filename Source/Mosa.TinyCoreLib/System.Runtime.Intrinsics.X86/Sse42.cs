namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Sse42 : Sse41
{
	public new abstract class X64 : Sse41.X64
	{
		public new static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal X64()
		{
		}

		public static ulong Crc32(ulong crc, ulong data)
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

	internal Sse42()
	{
	}

	public static Vector128<long> CompareGreaterThan(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static uint Crc32(uint crc, byte data)
	{
		throw null;
	}

	public static uint Crc32(uint crc, ushort data)
	{
		throw null;
	}

	public static uint Crc32(uint crc, uint data)
	{
		throw null;
	}
}
