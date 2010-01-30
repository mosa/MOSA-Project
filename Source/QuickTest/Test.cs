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
			bool result = boolAddConstantR4Left(18.2f, 1f);
		}

		static bool boolAddConstantR4Left(float expect, float x)
		{
			return expect == (17.2f + x);
		}

	}
}
