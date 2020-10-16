// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System.Threading
{
	public static class Interlocked
	{
		public static int Increment(ref int location)
		{
			return Add(ref location, 1);
		}

		public static long Increment(ref long location)
		{
			return Add(ref location, 1);
		}

		public static int Decrement(ref int location)
		{
			return Add(ref location, -1);
		}

		public static long Decrement(ref long location)
		{
			return Add(ref location, -1);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern int Exchange(ref int location1, int value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern long Exchange(ref long location1, long value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern float Exchange(ref float location1, float value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern double Exchange(ref double location1, double value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern Object Exchange(ref Object location1, Object value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern IntPtr Exchange(ref IntPtr location1, IntPtr value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern int CompareExchange(ref int location1, int value, int comparand);

		//[MethodImplAttribute(MethodImplOptions.InternalCall)]
		//public static extern long CompareExchange(ref long location1, long value, long comparand);
		public static long CompareExchange(ref long location1, long value, long comparand)
		{
			return 0; //temp
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern float CompareExchange(ref float location1, float value, float comparand);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern double CompareExchange(ref double location1, double value, double comparand);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern Object CompareExchange(ref Object location1, Object value, Object comparand);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		internal static extern int CompareExchange(ref int location1, int value, int comparand, ref bool succeeded);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		internal static extern int ExchangeAdd(ref int location1, int value);

		//[MethodImplAttribute(MethodImplOptions.InternalCall)]
		//internal static extern long ExchangeAdd(ref long location1, long value);
		internal static long ExchangeAdd(ref long location1, long value)
		{
			return 0; //temp
		}

		public static int Add(ref int location1, int value)
		{
			return ExchangeAdd(ref location1, value) + value;
		}

		public static long Add(ref long location1, long value)
		{
			return ExchangeAdd(ref location1, value) + value;
		}

		public static long Read(ref long location)
		{
			return Interlocked.CompareExchange(ref location, 0, 0);
		}
	}
}
