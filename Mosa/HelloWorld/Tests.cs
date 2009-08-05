/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Tests
	{

		/// <summary>
		/// Runs the tests.
		/// </summary>
		/// <returns></returns>
		static void RunTests()
		{
			if (AddU2(0, 0, 0))
				Screen.Write('.');
			else
				Screen.Write('E');
		}

		/// <summary>
		/// Adds the u2.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		/// <param name="expect">The expect.</param>
		/// <returns></returns>
		static bool AddU2(long a, long b, long expect)
		{
			return expect == (a + b);
		}
	}
}
