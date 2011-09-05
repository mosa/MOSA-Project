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

	public static class FibonacciTests
	{
	
		public static int Fibonacci(int n)
		{
			if (n == 1 || n == 0)
				return n;

			return Fibonacci(n - 1) + Fibonacci(n - 2);
		}

	}
}