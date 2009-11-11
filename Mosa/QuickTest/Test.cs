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
		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			int x = 3;
			int y = 4 + x;
			int z = x * y;
			int a = z - 5;
			int b = a;
			if (a > z)
				b = a * 100;
			else
				b = a * 1000;
			int q = a / 10;
		}

		/// <summary>
		/// Tests the specified expect.
		/// </summary>
		/// <param name="expect">The expect.</param>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
        static long AddPtr(long a, long b)
        {
            if (a > 2)
                b = a * 100;
            else
                b = a * 1000;

            if (b > a)
                return (a / b + a);

            return (a + b);
        }
	}
}
