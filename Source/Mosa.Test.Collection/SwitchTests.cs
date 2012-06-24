// Remove conditional statement after Mono 2.11 is released
#if !__MonoCS__


namespace Mosa.Test.Collection
{

	public static class SwitchTests 
	{
		
		public static sbyte SwitchI1(sbyte a)
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
		
		public static short SwitchI2(short a)
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
		
		public static int SwitchI4(int a)
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
		
		public static long SwitchI8(long a)
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
			
		public static byte SwitchU1(byte a)
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
		
		public static ushort SwitchU2(ushort a)
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
		
		public static uint SwitchU4(uint a)
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
		
		public static ulong SwitchU8(ulong a)
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

#endif