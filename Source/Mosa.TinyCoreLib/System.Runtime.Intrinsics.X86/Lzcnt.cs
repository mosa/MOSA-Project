namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Lzcnt : X86Base
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

		public static ulong LeadingZeroCount(ulong value)
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

	internal Lzcnt()
	{
	}

	public static uint LeadingZeroCount(uint value)
	{
		throw null;
	}
}
