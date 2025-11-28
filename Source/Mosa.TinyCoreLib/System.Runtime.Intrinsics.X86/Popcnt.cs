using Internal;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Popcnt : Sse42
{
	public new abstract class X64 : Sse42.X64
	{
		public new static bool IsSupported => throw new NotImplementedException();

		internal X64()
		{
		}

		public static ulong PopCount(ulong value) => throw new NotImplementedException();
	}

	public new static bool IsSupported => throw new NotImplementedException();

	internal Popcnt()
	{
	}

	public static uint PopCount(uint value) => Impl.Popcnt.PopCount(value);
}
