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
	public class _ExceptionHandlingFixture : X86TestFixture
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

		[Fact]
		public void TryFinally5()
		{
			Assert.Equal(ExceptionHandlingTests.TryFinally5(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.TryFinally5"));
		}

		[Fact]
		public void TryFinally6()
		{
			Assert.Equal(ExceptionHandlingTests.TryFinally6(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.TryFinally6"));
		}

		[Fact]
		public void TryFinally7()
		{
			Assert.Equal(ExceptionHandlingTests.TryFinally7(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.TryFinally7"));
		}

		[Fact]
		public void ExceptionTest1()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest1(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest1"));
		}

		[Fact]
		public void ExceptionTest2()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest2(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest2"));
		}

		[Fact]
		public void ExceptionTest3()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest3(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest3"));
		}

		[Fact]
		public void ExceptionTest4()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest4(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest4"));
		}

		[Fact]
		public void ExceptionTest5()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest5(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest5"));
		}

		[Fact]
		public void ExceptionTest6()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest6(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest6"));
		}

		[Fact]
		public void ExceptionTest7()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest7(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest7"));
		}

		[Fact]
		public void ExceptionTest8()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest8(), Run<int>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest8"));
		}

		[Fact]
		public void ExceptionTest9()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest9(), Run<ulong>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest9"));
		}

		[Fact]
		public void ExceptionTest10()
		{
			Assert.Equal(ExceptionHandlingTests.ExceptionTest10(), Run<ulong>("Mosa.Test.Collection.ExceptionHandlingTests.ExceptionTest10"));
		}
	}
}