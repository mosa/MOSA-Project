using Internal;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Lzcnt : X86Base
{
	public new abstract class X64 : X86Base.X64
	{
		public new static bool IsSupported => throw new NotImplementedException();

		internal X64()
		{
		}

		public static ulong LeadingZeroCount(ulong value) => throw new NotImplementedException();
	}

	public new static bool IsSupported => throw new NotImplementedException();

	internal Lzcnt()
	{
	}

	public static uint LeadingZeroCount(uint value) => Impl.Lzcnt.LeadingZeroCount(value);
}
