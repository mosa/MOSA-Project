/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System.Runtime.InteropServices;

namespace Mosa.Test.Collection
{

	public static class CallOrderTests
	{
		public static void CallEmpty()
		{
			CallEmpty_Target();
		}

		private static void CallEmpty_Target()
		{
		}

		public static bool CallOrderI4(int a)
		{
			return (a == 1);
		}

		public static bool CallOrderI4I4(int a, int b)
		{
			return (a == 1 && b == 2);
		}

		public static bool CallOrderI4I4I4(int a, int b, int c)
		{
			return (a == 1 && b == 2 && c == 3);
		}

		public static bool CallOrderI4I4I4I4(int a, int b, int c, int d)
		{
			return (a == 1 && b == 2 && c == 3 && d == 4);
		}

		public static bool CallOrderU8(ulong a, ulong b, ulong c, ulong d)
		{
			return (a == 1 && b == 2 && c == 3 && d == 4);
		}

		public static bool CallOrderU4_U8_U8_U8(uint a, ulong b, ulong c, ulong d)
		{
			return (a == 1 && b == 2 && c == 3 && d == 4);
		}
	}
}