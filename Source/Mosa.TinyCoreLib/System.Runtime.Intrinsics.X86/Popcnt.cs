namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Popcnt : Sse42
{
	public new abstract class X64 : Sse42.X64
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

		public static ulong PopCount(ulong value)
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

	internal Popcnt()
	{
	}

	public static uint PopCount(uint value)
	{
		throw null;
	}
}
