/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using Xunit;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class ExceptionHandlingFixture : X86TestFixture
	{
		[Fact]
		public void TryFinally1()
		{
			Assert.Equal(ExceptionHandlingTests.TryFinally1(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.TryFinally1"));
		}

		[Fact]
		public void TryFinally2()
		{
			Assert.Equal(ExceptionHandlingTests.TryFinally2(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.TryFinally2"));
		}

		[Fact]
		public void TryFinally3()
		{
			Assert.Equal(ExceptionHandlingTests.TryFinally3(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.TryFinally3"));
		}

		[Fact]
		public void TryFinally4()
		{
			Assert.Equal(ExceptionHandlingTests.TryFinally4(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.TryFinally4"));
		}
	}
}