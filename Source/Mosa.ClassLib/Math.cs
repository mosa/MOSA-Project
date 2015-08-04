// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.ClassLib
{
	/// <summary>
	///
	/// </summary>
	public static class Math
	{
		/// <summary>
		/// Returns the lowest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static uint Min(uint a, uint b)
		{
			if (a < b) return a; else return b;
		}

		/// <summary>
		/// Returns the highest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static uint Max(uint a, uint b)
		{
			if (a >= b) return a; else return b;
		}

		/// <summary>
		/// Returns the lowest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static int Min(int a, int b)
		{
			if (a < b) return a; else return b;
		}

		/// <summary>
		/// Returns the highest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static int Max(int a, int b)
		{
			if (a >= b) return a; else return b;
		}

		/// <summary>
		/// Returns the lowest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static byte Min(byte a, byte b)
		{
			if (a < b) return a; else return b;
		}

		/// <summary>
		/// Returns the highest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static byte Max(byte a, byte b)
		{
			if (a >= b) return a; else return b;
		}
	}
}
