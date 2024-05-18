namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Bmi1 : X86Base
{
	public new abstract class X64 : X86Base.X64
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

		public static ulong AndNot(ulong left, ulong right)
		{
			throw null;
		}

		public static ulong BitFieldExtract(ulong value, byte start, byte length)
		{
			throw null;
		}

		public static ulong BitFieldExtract(ulong value, ushort control)
		{
			throw null;
		}

		public static ulong ExtractLowestSetBit(ulong value)
		{
			throw null;
		}

		public static ulong GetMaskUpToLowestSetBit(ulong value)
		{
			throw null;
		}

		public static ulong ResetLowestSetBit(ulong value)
		{
			throw null;
		}

		public static ulong TrailingZeroCount(ulong value)
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

	internal Bmi1()
	{
	}

	public static uint AndNot(uint left, uint right)
	{
		throw null;
	}

	public static uint BitFieldExtract(uint value, byte start, byte length)
	{
		throw null;
	}

	public static uint BitFieldExtract(uint value, ushort control)
	{
		throw null;
	}

	public static uint ExtractLowestSetBit(uint value)
	{
		throw null;
	}

	public static uint GetMaskUpToLowestSetBit(uint value)
	{
		throw null;
	}

	public static uint ResetLowestSetBit(uint value)
	{
		throw null;
	}

	public static uint TrailingZeroCount(uint value)
	{
		throw null;
	}
}
