/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class FibonacciFixture : X86TestFixture
	{
		[Theory]
		[PropertyData("I4Small")]
		//[InlineData((int)3)]
		public void Fibonacci(int value)
		{
			Assert.Equal(FibonacciTests.Fibonacci(value), Run<int>("Mosa.Test.Collection.FibonacciTests.Fibonacci", value));
		}
	}
}