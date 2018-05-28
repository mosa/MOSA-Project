﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class ArrayLayoutFixture : TestFixture
	{
		[Fact]
		public void ArrayB()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.B(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.B"));
		}

		[Fact]
		public void ArrayC()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.C(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.C"));
		}

		[Fact]
		public void ArrayC1()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.C1(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.C1"));
		}

		[Fact]
		public void ArrayU1()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.U1(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.U1"));
		}

		[Fact]
		public void ArrayU2()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.U2(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.U2"));
		}

		[Fact]
		public void ArrayU4()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.U4(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.U4"));
		}

		[Fact]
		public void ArrayU8()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.U8(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.U8"));
		}

		[Fact]
		public void ArrayU8a()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.U8a(), Run<ulong>("Mosa.UnitTest.Collection.ArrayLayoutTests.U8a"));
		}

		[Fact]
		public void ArrayU8b()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.U8b(), Run<ulong>("Mosa.UnitTest.Collection.ArrayLayoutTests.U8b"));
		}

		[Fact]
		public void ArrayU8c()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.U8c(), Run<ulong>("Mosa.UnitTest.Collection.ArrayLayoutTests.U8c"));
		}

		[Fact]
		public void ArrayU8d()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.U8d(), Run<ulong>("Mosa.UnitTest.Collection.ArrayLayoutTests.U8d"));
		}

		[Fact]
		public void ArrayI8()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.I8(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.I8"));
		}

		[Fact]
		public void ArrayR4()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.R4(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.R4"));
		}

		[Fact]
		public void ArrayR8()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayLayoutTests.R8(), Run<bool>("Mosa.UnitTest.Collection.ArrayLayoutTests.R8"));
		}
	}
}
