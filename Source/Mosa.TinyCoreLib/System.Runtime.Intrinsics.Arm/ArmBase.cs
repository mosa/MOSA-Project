namespace System.Runtime.Intrinsics.Arm;

[CLSCompliant(false)]
public abstract class ArmBase
{
	public abstract class Arm64
	{
		public static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal Arm64()
		{
		}

		public static int LeadingSignCount(int value)
		{
			throw null;
		}

		public static int LeadingSignCount(long value)
		{
			throw null;
		}

		public static int LeadingZeroCount(long value)
		{
			throw null;
		}

		public static int LeadingZeroCount(ulong value)
		{
			throw null;
		}

		public static long MultiplyHigh(long left, long right)
		{
			throw null;
		}

		public static ulong MultiplyHigh(ulong left, ulong right)
		{
			throw null;
		}

		public static long ReverseElementBits(long value)
		{
			throw null;
		}

		public static ulong ReverseElementBits(ulong value)
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

	internal ArmBase()
	{
	}

	public static int LeadingZeroCount(int value)
	{
		throw null;
	}

	public static int LeadingZeroCount(uint value)
	{
		throw null;
	}

	public static int ReverseElementBits(int value)
	{
		throw null;
	}

	public static uint ReverseElementBits(uint value)
	{
		throw null;
	}

	public static void Yield()
	{
		throw null;
	}
}
