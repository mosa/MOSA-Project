using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Specialized;

public struct BitVector32 : IEquatable<BitVector32>
{
	public readonly struct Section : IEquatable<Section>
	{
		private readonly int _dummyPrimitive;

		public short Mask
		{
			get
			{
				throw null;
			}
		}

		public short Offset
		{
			get
			{
				throw null;
			}
		}

		public bool Equals(Section obj)
		{
			throw null;
		}

		public override bool Equals([NotNullWhen(true)] object? o)
		{
			throw null;
		}

		public override int GetHashCode()
		{
			throw null;
		}

		public static bool operator ==(Section a, Section b)
		{
			throw null;
		}

		public static bool operator !=(Section a, Section b)
		{
			throw null;
		}

		public override string ToString()
		{
			throw null;
		}

		public static string ToString(Section value)
		{
			throw null;
		}
	}

	private int _dummyPrimitive;

	public int Data
	{
		get
		{
			throw null;
		}
	}

	public int this[Section section]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool this[int bit]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public BitVector32(BitVector32 value)
	{
		throw null;
	}

	public BitVector32(int data)
	{
		throw null;
	}

	public static int CreateMask()
	{
		throw null;
	}

	public static int CreateMask(int previous)
	{
		throw null;
	}

	public static Section CreateSection(short maxValue)
	{
		throw null;
	}

	public static Section CreateSection(short maxValue, Section previous)
	{
		throw null;
	}

	public bool Equals(BitVector32 other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static string ToString(BitVector32 value)
	{
		throw null;
	}
}
