// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class FibonacciFixture : X86TestFixture
	{
		[Theory]
		[MemberData("I4Small")]
		public void Fibonacci(int value)
		{
			Assert.Equal(FibonacciTests.Fibonacci(value), Run<int>("Mosa.Test.Collection.FibonacciTests.Fibonacci", value));
		}
	}
}
