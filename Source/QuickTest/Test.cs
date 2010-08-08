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
	public static class App
	{
		public static int Sum(int a, int b)
		{
			return a + b;
		}

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			int c = Sum(10, 20);
		}


	}
}
