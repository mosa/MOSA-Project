// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
namespace Mosa.UnitTest.Collection.xUnit
{
	public class ForeachFixture : TestFixture
	{
		
		[Fact]
		public void ForeachU1()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ForeachTests.ForeachU1(), Run<byte>("Mosa.UnitTest.Collection.ForeachTests.ForeachU1"));
		}
		
		[Fact]
		public void ForeachU2()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ForeachTests.ForeachU2(), Run<ushort>("Mosa.UnitTest.Collection.ForeachTests.ForeachU2"));
		}
		
		[Fact]
		public void ForeachU4()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ForeachTests.ForeachU4(), Run<uint>("Mosa.UnitTest.Collection.ForeachTests.ForeachU4"));
		}
		
		[Fact]
		public void ForeachU8()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ForeachTests.ForeachU8(), Run<ulong>("Mosa.UnitTest.Collection.ForeachTests.ForeachU8"));
		}
		
		[Fact]
		public void ForeachI1()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ForeachTests.ForeachI1(), Run<sbyte>("Mosa.UnitTest.Collection.ForeachTests.ForeachI1"));
		}
		
		[Fact]
		public void ForeachI2()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ForeachTests.ForeachI2(), Run<short>("Mosa.UnitTest.Collection.ForeachTests.ForeachI2"));
		}
		
		[Fact]
		public void ForeachI4()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ForeachTests.ForeachI4(), Run<int>("Mosa.UnitTest.Collection.ForeachTests.ForeachI4"));
		}
		
		[Fact]
		public void ForeachI8()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ForeachTests.ForeachI8(), Run<long>("Mosa.UnitTest.Collection.ForeachTests.ForeachI8"));
		}
		
	}
}
