// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Standard;

public static class FibonacciTests
{
	[MosaUnitTest(Series = "I4Small")]
	public static int Fibonacci(int n)
	{
		if (n == 1 || n == 0)
			return n;

		return Fibonacci(n - 1) + Fibonacci(n - 2);
	}
}
