/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 *
 */
 
using System;

namespace Mosa.Test.Collection
{

	public class StructTests
	{
	
		static bool Set1U1(byte one)
		{
			Struct1U1 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3U1(byte one, byte two, byte three)
		{
			Struct3U1 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1U2(ushort one)
		{
			Struct1U2 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3U2(ushort one, ushort two, ushort three)
		{
			Struct3U2 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1U4(uint one)
		{
			Struct1U4 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3U4(uint one, uint two, uint three)
		{
			Struct3U4 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1U8(ulong one)
		{
			Struct1U8 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3U8(ulong one, ulong two, ulong three)
		{
			Struct3U8 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1I1(sbyte one)
		{
			Struct1I1 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3I1(sbyte one, sbyte two, sbyte three)
		{
			Struct3I1 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1I2(short one)
		{
			Struct1I2 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3I2(short one, short two, short three)
		{
			Struct3I2 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1I4(int one)
		{
			Struct1I4 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3I4(int one, int two, int three)
		{
			Struct3I4 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1I8(long one)
		{
			Struct1I8 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3I8(long one, long two, long three)
		{
			Struct3I8 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1R4(float one)
		{
			Struct1R4 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3R4(float one, float two, float three)
		{
			Struct3R4 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1R8(double one)
		{
			Struct1R8 structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3R8(double one, double two, double three)
		{
			Struct3R8 structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
		static bool Set1C(char one)
		{
			Struct1C structure;
			
			structure.One = one;
	
			return (structure.One == one);
		}
	
		static bool Set3C(char one, char two, char three)
		{
			Struct3C structure;
			
			structure.One = one;
			structure.Two = two;
			structure.Three = three;
			
			return (structure.One == one && structure.Two == two && structure.Three == three);
		}
		
	}

	
	public struct Struct1U1
	{
		public byte One;
	}
	
	public struct Struct3U1
	{
		public byte One;
		public byte Two;
		public byte Three;
	}
	
	public struct Struct1U2
	{
		public ushort One;
	}
	
	public struct Struct3U2
	{
		public ushort One;
		public ushort Two;
		public ushort Three;
	}
	
	public struct Struct1U4
	{
		public uint One;
	}
	
	public struct Struct3U4
	{
		public uint One;
		public uint Two;
		public uint Three;
	}
	
	public struct Struct1U8
	{
		public ulong One;
	}
	
	public struct Struct3U8
	{
		public ulong One;
		public ulong Two;
		public ulong Three;
	}
	
	public struct Struct1I1
	{
		public sbyte One;
	}
	
	public struct Struct3I1
	{
		public sbyte One;
		public sbyte Two;
		public sbyte Three;
	}
	
	public struct Struct1I2
	{
		public short One;
	}
	
	public struct Struct3I2
	{
		public short One;
		public short Two;
		public short Three;
	}
	
	public struct Struct1I4
	{
		public int One;
	}
	
	public struct Struct3I4
	{
		public int One;
		public int Two;
		public int Three;
	}
	
	public struct Struct1I8
	{
		public long One;
	}
	
	public struct Struct3I8
	{
		public long One;
		public long Two;
		public long Three;
	}
	
	public struct Struct1R4
	{
		public float One;
	}
	
	public struct Struct3R4
	{
		public float One;
		public float Two;
		public float Three;
	}
	
	public struct Struct1R8
	{
		public double One;
	}
	
	public struct Struct3R8
	{
		public double One;
		public double Two;
		public double Three;
	}
	
	public struct Struct1C
	{
		public char One;
	}
	
	public struct Struct3C
	{
		public char One;
		public char Two;
		public char Three;
	}
		
}