// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class ExceptionHandlingFixture : X86TestFixture
	{
		[Fact]
		public void TryFinally1()
		{
			Assert.Equal(_ExceptionHandlingTests.TryFinally1(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.TryFinally1"));
		}

		[Fact]
		public void TryFinally2()
		{
			Assert.Equal(_ExceptionHandlingTests.TryFinally2(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.TryFinally2"));
		}

		[Fact]
		public void TryFinally3()
		{
			Assert.Equal(_ExceptionHandlingTests.TryFinally3(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.TryFinally3"));
		}

		[Fact]
		public void TryFinally4()
		{
			Assert.Equal(_ExceptionHandlingTests.TryFinally4(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.TryFinally4"));
		}

		[Fact]
		public void TryFinally5()
		{
			Assert.Equal(_ExceptionHandlingTests.TryFinally5(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.TryFinally5"));
		}

		[Fact]
		public void TryFinally6()
		{
			Assert.Equal(_ExceptionHandlingTests.TryFinally6(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.TryFinally6"));
		}

		[Fact]
		public void TryFinally7()
		{
			Assert.Equal(_ExceptionHandlingTests.TryFinally7(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.TryFinally7"));
		}

		[Fact]
		public void ExceptionTest1()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest1(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest1"));
		}

		[Fact]
		public void ExceptionTest2()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest2(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest2"));
		}

		[Fact]
		public void ExceptionTest3()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest3(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest3"));
		}

		[Fact]
		public void ExceptionTest4()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest4(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest4"));
		}

		[Fact]
		public void ExceptionTest5()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest5(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest5"));
		}

		[Fact]
		public void ExceptionTest6()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest6(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest6"));
		}

		[Fact]
		public void ExceptionTest7()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest7(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest7"));
		}

		[Fact]
		public void ExceptionTest8()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest8(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest8"));
		}

		[Fact]
		public void ExceptionTest9()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest9(), Run<ulong>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest9"));
		}

		[Fact]
		public void ExceptionTest10()
		{
			Assert.Equal(_ExceptionHandlingTests.ExceptionTest10(), Run<ulong>("Mosa.Test.Collection._ExceptionHandlingTests.ExceptionTest10"));
		}

		//[Fact]
		//public void ExceptionTest20()
		//{
		//	Assert.Equal(_ExceptionHandlingTests._ExceptionTest20(), Run<int>("Mosa.Test.Collection._ExceptionHandlingTests._ExceptionTest20"));
		//}
	}
}
