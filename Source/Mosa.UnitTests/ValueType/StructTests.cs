// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.ValueType;

public class StructTests
{

	[MosaUnitTest(Series = "U1")]
	public static bool StructTestSet1U1(byte one)
	{
		Struct1U1 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "U1MiniU1Mini")]
	public static bool StructTestSet2U1(byte one, byte two)
	{
		Struct3U1 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "U1MiniU1MiniU1Mini")]
	public static bool StructTestSet3U1(byte one, byte two, byte three)
	{
		Struct3U1 structure;

		structure.One = one;
		structure.Two = two;
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "U2")]
	public static bool StructTestSet1U2(ushort one)
	{
		Struct1U2 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "U2MiniU2Mini")]
	public static bool StructTestSet2U2(ushort one, ushort two)
	{
		Struct3U2 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "U2MiniU2MiniU2Mini")]
	public static bool StructTestSet3U2(ushort one, ushort two, ushort three)
	{
		Struct3U2 structure;

		structure.One = one;
		structure.Two = two;
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "U4")]
	public static bool StructTestSet1U4(uint one)
	{
		Struct1U4 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "U4MiniU4Mini")]
	public static bool StructTestSet2U4(uint one, uint two)
	{
		Struct3U4 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "U4MiniU4MiniU4Mini")]
	public static bool StructTestSet3U4(uint one, uint two, uint three)
	{
		Struct3U4 structure;

		structure.One = one;
		structure.Two = two;
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "U8")]
	public static bool StructTestSet1U8(ulong one)
	{
		Struct1U8 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "U8MiniU8Mini")]
	public static bool StructTestSet2U8(ulong one, ulong two)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3U8 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		After:
					Struct3U8 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		*/
		Struct3U8 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "U8MiniU8MiniU8Mini")]
	public static bool StructTestSet3U8(ulong one, ulong two, ulong three)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3U8 structure;

					structure.One = one;
		After:
					Struct3U8 structure;

					structure.One = one;
		*/
		Struct3U8 structure;

		structure.One = one;
		structure.Two = two;

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "I1")]
		After:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "I1")]
		*/
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "I1")]
	public static bool StructTestSet1I1(sbyte one)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct1I1 structure;

					structure.One = one;
		After:
					Struct1I1 structure;

					structure.One = one;
		*/
		Struct1I1 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "I1MiniI1Mini")]
	public static bool StructTestSet2I1(sbyte one, sbyte two)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3I1 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		After:
					Struct3I1 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		*/
		Struct3I1 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "I1MiniI1MiniI1Mini")]
	public static bool StructTestSet3I1(sbyte one, sbyte two, sbyte three)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3I1 structure;

					structure.One = one;
		After:
					Struct3I1 structure;

					structure.One = one;
		*/
		Struct3I1 structure;

		structure.One = one;
		structure.Two = two;

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "I2")]
		After:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "I2")]
		*/
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "I2")]
	public static bool StructTestSet1I2(short one)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct1I2 structure;

					structure.One = one;
		After:
					Struct1I2 structure;

					structure.One = one;
		*/
		Struct1I2 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "I2MiniI2Mini")]
	public static bool StructTestSet2I2(short one, short two)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3I2 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		After:
					Struct3I2 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		*/
		Struct3I2 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "I2MiniI2MiniI2Mini")]
	public static bool StructTestSet3I2(short one, short two, short three)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3I2 structure;

					structure.One = one;
		After:
					Struct3I2 structure;

					structure.One = one;
		*/
		Struct3I2 structure;

		structure.One = one;
		structure.Two = two;

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "I4")]
		After:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "I4")]
		*/
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "I4")]
	public static bool StructTestSet1I4(int one)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct1I4 structure;

					structure.One = one;
		After:
					Struct1I4 structure;

					structure.One = one;
		*/
		Struct1I4 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "I4MiniI4Mini")]
	public static bool StructTestSet2I4(int one, int two)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3I4 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		After:
					Struct3I4 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		*/
		Struct3I4 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "I4MiniI4MiniI4Mini")]
	public static bool StructTestSet3I4(int one, int two, int three)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3I4 structure;

					structure.One = one;
		After:
					Struct3I4 structure;

					structure.One = one;
		*/
		Struct3I4 structure;

		structure.One = one;
		structure.Two = two;

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
		After:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
		*/
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "I8")]
	public static bool StructTestSet1I8(long one)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct1I8 structure;

					structure.One = one;
		After:
					Struct1I8 structure;

					structure.One = one;
		*/
		Struct1I8 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "I8MiniI8Mini")]
	public static bool StructTestSet2I8(long one, long two)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3I8 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		After:
					Struct3I8 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		*/
		Struct3I8 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "I8MiniI8MiniI8Mini")]
	public static bool StructTestSet3I8(long one, long two, long three)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3I8 structure;

					structure.One = one;
		After:
					Struct3I8 structure;

					structure.One = one;
		*/
		Struct3I8 structure;

		structure.One = one;
		structure.Two = two;

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "R4")]
		After:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "R4")]
		*/
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "R4")]
	public static bool StructTestSet1R4(float one)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct1R4 structure;

					structure.One = one;
		After:
					Struct1R4 structure;

					structure.One = one;
		*/
		Struct1R4 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "R4MiniR4Mini")]
	public static bool StructTestSet2R4(float one, float two)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3R4 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		After:
					Struct3R4 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		*/
		Struct3R4 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "R4MiniR4MiniR4Mini")]
	public static bool StructTestSet3R4(float one, float two, float three)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3R4 structure;

					structure.One = one;
		After:
					Struct3R4 structure;

					structure.One = one;
		*/
		Struct3R4 structure;

		structure.One = one;
		structure.Two = two;

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "R8")]
		After:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "R8")]
		*/
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "R8")]
	public static bool StructTestSet1R8(double one)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct1R8 structure;

					structure.One = one;
		After:
					Struct1R8 structure;

					structure.One = one;
		*/
		Struct1R8 structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "R8MiniR8Mini")]
	public static bool StructTestSet2R8(double one, double two)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3R8 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		After:
					Struct3R8 structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		*/
		Struct3R8 structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "R8MiniR8MiniR8Mini")]
	public static bool StructTestSet3R8(double one, double two, double three)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3R8 structure;

					structure.One = one;
		After:
					Struct3R8 structure;

					structure.One = one;
		*/
		Struct3R8 structure;

		structure.One = one;
		structure.Two = two;

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "C")]
		After:
					structure.Three = three;

					return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
				}

				[MosaUnitTest(Series = "C")]
		*/
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

	[MosaUnitTest(Series = "C")]
	public static bool StructTestSet1C(char one)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct1C structure;

					structure.One = one;
		After:
					Struct1C structure;

					structure.One = one;
		*/
		Struct1C structure;

		structure.One = one;

		return (structure.One.Equals(one));
	}

	//[MosaUnitTest(Series = "CMiniCMini")]
	public static bool StructTestSet2C(char one, char two)
	{

		/* Unmerged change from project 'Mosa.UnitTests'
		Before:
					Struct3C structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		After:
					Struct3C structure;

					structure.One = one;
					structure.Two = two;

					return (structure.One.Equals(one) && structure.Two.Equals(two));
		*/
		Struct3C structure;

		structure.One = one;
		structure.Two = two;

		return (structure.One.Equals(one) && structure.Two.Equals(two));
	}

	//[MosaUnitTest(Series = "CMiniCMiniCMini")]
	public static bool StructTestSet3C(char one, char two, char three)
	{
		Struct3C structure;

		structure.One = one;
		structure.Two = two;
		structure.Three = three;

		return (structure.One.Equals(one) && structure.Two.Equals(two) && structure.Three.Equals(three));
	}

}

internal struct Struct1U1
{
	public byte One;
}

internal struct Struct2U1
{
	public byte One;
	public byte Two;
}

internal struct Struct3U1
{
	public byte One;
	public byte Two;
	public byte Three;
}
internal struct Struct1U2
{
	public ushort One;
}

internal struct Struct2U2
{
	public ushort One;
	public ushort Two;
}

internal struct Struct3U2
{
	public ushort One;
	public ushort Two;
	public ushort Three;
}
internal struct Struct1U4
{
	public uint One;
}

internal struct Struct2U4
{
	public uint One;
	public uint Two;
}

internal struct Struct3U4
{
	public uint One;
	public uint Two;
	public uint Three;
}
internal struct Struct1U8
{
	public ulong One;
}

internal struct Struct2U8
{
	public ulong One;
	public ulong Two;
}

internal struct Struct3U8
{
	public ulong One;
	public ulong Two;
	public ulong Three;
}
internal struct Struct1I1
{
	public sbyte One;
}

internal struct Struct2I1
{
	public sbyte One;
	public sbyte Two;
}

internal struct Struct3I1
{
	public sbyte One;
	public sbyte Two;
	public sbyte Three;
}
internal struct Struct1I2
{
	public short One;
}

internal struct Struct2I2
{
	public short One;
	public short Two;
}

internal struct Struct3I2
{
	public short One;
	public short Two;
	public short Three;
}
internal struct Struct1I4
{
	public int One;
}

internal struct Struct2I4
{
	public int One;
	public int Two;
}

internal struct Struct3I4
{
	public int One;
	public int Two;
	public int Three;
}
internal struct Struct1I8
{
	public long One;
}

internal struct Struct2I8
{
	public long One;
	public long Two;
}

internal struct Struct3I8
{
	public long One;
	public long Two;
	public long Three;
}
internal struct Struct1R4
{
	public float One;
}

internal struct Struct2R4
{
	public float One;
	public float Two;
}

internal struct Struct3R4
{
	public float One;
	public float Two;
	public float Three;
}
internal struct Struct1R8
{
	public double One;
}

internal struct Struct2R8
{
	public double One;
	public double Two;
}

internal struct Struct3R8
{
	public double One;
	public double Two;
	public double Three;
}
internal struct Struct1C
{
	public char One;
}

internal struct Struct2C
{
	public char One;
	public char Two;
}

internal struct Struct3C
{
	public char One;
	public char Two;
	public char Three;
}
