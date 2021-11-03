﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.Basic
{

	public static class CallTests 
	{

		[MosaUnitTest(Series = "U1")]
		public static bool CallU1(byte value)
		{ 
			return value == CallTargetU1(value); 
		} 

		private static byte CallTargetU1(byte value)
		{ 
			return value; 
		}					
	
		[MosaUnitTest(Series = "U2")]
		public static bool CallU2(ushort value)
		{ 
			return value == CallTargetU2(value); 
		} 

		private static ushort CallTargetU2(ushort value)
		{ 
			return value; 
		}					
	
		[MosaUnitTest(Series = "U4")]
		public static bool CallU4(uint value)
		{ 
			return value == CallTargetU4(value); 
		} 

		private static uint CallTargetU4(uint value)
		{ 
			return value; 
		}					
	
		[MosaUnitTest(Series = "U8")]
		public static bool CallU8(ulong value)
		{ 
			return value == CallTargetU8(value); 
		} 

		private static ulong CallTargetU8(ulong value)
		{ 
			return value; 
		}					
	
		[MosaUnitTest(Series = "I1")]
		public static bool CallI1(sbyte value)
		{ 
			return value == CallTargetI1(value); 
		} 

		private static sbyte CallTargetI1(sbyte value)
		{ 
			return value; 
		}					
	
		[MosaUnitTest(Series = "I2")]
		public static bool CallI2(short value)
		{ 
			return value == CallTargetI2(value); 
		} 

		private static short CallTargetI2(short value)
		{ 
			return value; 
		}					
	
		[MosaUnitTest(Series = "I4")]
		public static bool CallI4(int value)
		{ 
			return value == CallTargetI4(value); 
		} 

		private static int CallTargetI4(int value)
		{ 
			return value; 
		}					
	
		[MosaUnitTest(Series = "I8")]
		public static bool CallI8(long value)
		{ 
			return value == CallTargetI8(value); 
		} 

		private static long CallTargetI8(long value)
		{ 
			return value; 
		}					
	
		[MosaUnitTest(Series = "C")]
		public static bool CallC(char value)
		{ 
			return value == CallTargetC(value); 
		} 

		private static char CallTargetC(char value)
		{ 
			return value; 
		}					
		}
}
