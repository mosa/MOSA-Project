// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.xUnit
{
	public class FibonacciFixture : TestFixture
	{
		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Fibonacci(int value)
		{
			Assert.Equal(FibonacciTests.Fibonacci(value), Run<int>("Mosa.Test.Collection.FibonacciTests.Fibonacci", value));
		}
	}
}
