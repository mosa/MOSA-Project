using Internal;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Bmi1 : X86Base
{
	public new abstract class X64 : X86Base.X64
	{
		public new static bool IsSupported => throw new NotImplementedException();

		internal X64()
		{
		}

		public static ulong AndNot(ulong left, ulong right) => throw new NotImplementedException();

		public static ulong BitFieldExtract(ulong value, byte start, byte length) => throw new NotImplementedException();

		public static ulong BitFieldExtract(ulong value, ushort control) => throw new NotImplementedException();

		public static ulong ExtractLowestSetBit(ulong value) => throw new NotImplementedException();

		public static ulong GetMaskUpToLowestSetBit(ulong value) => throw new NotImplementedException();

		public static ulong ResetLowestSetBit(ulong value) => throw new NotImplementedException();

		public static ulong TrailingZeroCount(ulong value) => throw new NotImplementedException();
	}

	public new static bool IsSupported => throw new NotImplementedException();

	internal Bmi1()
	{
	}

	public static uint AndNot(uint left, uint right) => throw new NotImplementedException();

	public static uint BitFieldExtract(uint value, byte start, byte length) => throw new NotImplementedException();

	public static uint BitFieldExtract(uint value, ushort control) => throw new NotImplementedException();

	public static uint ExtractLowestSetBit(uint value) => throw new NotImplementedException();

	public static uint GetMaskUpToLowestSetBit(uint value) => throw new NotImplementedException();

	public static uint ResetLowestSetBit(uint value) => Impl.Bmi1.ResetLowestSetBit(value);

	public static uint TrailingZeroCount(uint value) => Impl.Bmi1.TrailingZeroCount(value);
}
