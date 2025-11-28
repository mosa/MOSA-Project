namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class X86Serialize : X86Base
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
	}

	public new static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	internal X86Serialize()
	{
	}

	public static void Serialize()
	{
		throw null;
	}
}
