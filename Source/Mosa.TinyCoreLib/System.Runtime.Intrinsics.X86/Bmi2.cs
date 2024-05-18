namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Bmi2 : X86Base
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

		public static ulong MultiplyNoFlags(ulong left, ulong right)
		{
			throw null;
		}

		public unsafe static ulong MultiplyNoFlags(ulong left, ulong right, ulong* low)
		{
			throw null;
		}

		public static ulong ParallelBitDeposit(ulong value, ulong mask)
		{
			throw null;
		}

		public static ulong ParallelBitExtract(ulong value, ulong mask)
		{
			throw null;
		}

		public static ulong ZeroHighBits(ulong value, ulong index)
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

	internal Bmi2()
	{
	}

	public static uint MultiplyNoFlags(uint left, uint right)
	{
		throw null;
	}

	public unsafe static uint MultiplyNoFlags(uint left, uint right, uint* low)
	{
		throw null;
	}

	public static uint ParallelBitDeposit(uint value, uint mask)
	{
		throw null;
	}

	public static uint ParallelBitExtract(uint value, uint mask)
	{
		throw null;
	}

	public static uint ZeroHighBits(uint value, uint index)
	{
		throw null;
	}
}
