using System.Runtime.Versioning;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class X86Base
{
	public abstract class X64
	{
		public static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal X64()
		{
		}

		[RequiresPreviewFeatures("DivRem is in preview.")]
		public static (ulong Quotient, ulong Remainder) DivRem(ulong lower, ulong upper, ulong divisor)
		{
			throw null;
		}

		[RequiresPreviewFeatures("DivRem is in preview.")]
		public static (long Quotient, long Remainder) DivRem(ulong lower, long upper, long divisor)
		{
			throw null;
		}
	}

	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	internal X86Base()
	{
	}

	public static (int Eax, int Ebx, int Ecx, int Edx) CpuId(int functionId, int subFunctionId)
	{
		throw null;
	}

	[RequiresPreviewFeatures("DivRem is in preview.")]
	public static (uint Quotient, uint Remainder) DivRem(uint lower, uint upper, uint divisor)
	{
		throw null;
	}

	[RequiresPreviewFeatures("DivRem is in preview.")]
	public static (int Quotient, int Remainder) DivRem(uint lower, int upper, int divisor)
	{
		throw null;
	}

	[RequiresPreviewFeatures("DivRem is in preview.")]
	public static (UIntPtr Quotient, UIntPtr Remainder) DivRem(UIntPtr lower, UIntPtr upper, UIntPtr divisor)
	{
		throw null;
	}

	[RequiresPreviewFeatures("DivRem is in preview.")]
	public static (IntPtr Quotient, IntPtr Remainder) DivRem(UIntPtr lower, IntPtr upper, IntPtr divisor)
	{
		throw null;
	}

	public static void Pause()
	{
		throw null;
	}
}
