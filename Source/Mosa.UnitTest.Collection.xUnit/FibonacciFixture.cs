// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class FibonacciFixture : TestFixture
	{
		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Fibonacci(int value)
		{
			Assert.Equal(FibonacciTests.Fibonacci(value), Run<int>("Mosa.UnitTest.Collection.FibonacciTests.Fibonacci", value));
		}
	}
}
