/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

namespace Mosa.Test.Collection
{

	static class Switch
	{
		static sbyte SwitchI1(sbyte expect, sbyte a) { return Switch_Target(a); }
		static sbyte Switch_Target(sbyte a)
		{
			switch (a)
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case -1:
					return -1;
				case 2:
					return 2;
				case -2:
					return -2;
				case 23:
					return 23;
				case sbyte.MinValue:
					return sbyte.MinValue;
				case sbyte.MaxValue:
					return sbyte.MaxValue;
				default:
					return 42;
			}
		}

		static bool SwitchU1(byte expect, byte a) { return expect == Switch_Target(a); }
		static byte Switch_Target(byte a)
		{
			switch (a)
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case 2:
					return 2;
				case 23:
					return 23;
				case byte.MaxValue:
					return byte.MaxValue;
				default:
					return 42;
			}
		}

		static bool SwitchI2(short expect, short a) { return expect == Switch_Target(a); }
		static short Switch_Target(short a)
		{
			switch (a)
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case -1:
					return -1;
				case 2:
					return 2;
				case -2:
					return -2;
				case 23:
					return 23;
				case short.MinValue:
					return short.MinValue;
				case short.MaxValue:
					return short.MaxValue;
				default:
					return 42;
			}
		}

		static bool SwitchU2(ushort expect, ushort a) { return expect == Switch_Target(a); }
		static ushort Switch_Target(ushort a)
		{
			switch (a)
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case 2:
					return 2;
				case 23:
					return 23;
				case ushort.MaxValue:
					return ushort.MaxValue;
				default:
					return 42;
			}
		}

		static bool SwitchI4(int expect, int a) { return expect == Switch_Target(a); }
		static int Switch_Target(int a)
		{
			switch (a)
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case -1:
					return -1;
				case 2:
					return 2;
				case -2:
					return -2;
				case 23:
					return 23;
				case int.MinValue:
					return int.MinValue;
				case int.MaxValue:
					return int.MaxValue;
				default:
					return 42;
			}
		}

		static bool SwitchU4(uint expect, uint a) { return expect == Switch_Target(a); }
		static uint Switch_Target(uint a)
		{
			switch (a)
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case 2:
					return 2;
				case 23:
					return 23;
				case uint.MaxValue:
					return uint.MaxValue;
				default:
					return 42;
			}
		}

		static long SwitchI8(long expect, long a) { return Switch_Target(a); }
		static long Switch_Target(long a)
		{
			switch (a)
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case -1:
					return -1;
				case 2:
					return 2;
				case -2:
					return -2;
				case 23:
					return 23;
				case long.MinValue:
					return long.MinValue;
				case long.MaxValue:
					return long.MaxValue;
				default:
					return 42;
			}
		}

		static bool SwitchU8(ulong expect, ulong a) { return expect == Switch_Target(a); }
		static ulong Switch_Target(ulong a)
		{
			switch (a)
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case 2:
					return 2;
				case 23:
					return 23;
				case ulong.MaxValue:
					return ulong.MaxValue;
				default:
					return 42;
			}
		}

	}
}
