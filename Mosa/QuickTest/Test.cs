/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.QuickTest
{
	/// <summary>
	/// 
	/// </summary>
	//public struct Struct
	//{
	//    public int x;
	//    public int y;
	//    public int z;
	//}

	/// <summary>
	/// 
	/// </summary>
	public static class App
	{
		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			bool rest = CallOrderU4_U8_U8_U8(1, 2, 3, 4);
		}

		static bool CallOrderU4_U8_U8_U8(uint a, ulong b, ulong c, ulong d)
		{
			return (a == 1 && b == 2 && c == 3 && d == 4);
		}

		static bool CallOrderU8(ulong a, ulong b, ulong c, ulong d)
		{
			return (a == 1 && b == 2 && c == 3 && d == 4);
		}

	}
}
