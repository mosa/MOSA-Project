using Xunit;
using Xunit.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
namespace Mosa.Test.Collection.x86.xUnit
{
	public class ForeachFixture : X86TestFixture
	{
		
		[Fact]
		public void ForeachU1()
		{
			Assert.Equal(Mosa.Test.Collection.ForeachTests.ForeachU1(), Run<byte>("Mosa.Test.Collection.ForeachTests.ForeachU1"));
		}
		
		[Fact]
		public void ForeachU2()
		{
			Assert.Equal(Mosa.Test.Collection.ForeachTests.ForeachU2(), Run<ushort>("Mosa.Test.Collection.ForeachTests.ForeachU2"));
		}
		
		[Fact]
		public void ForeachU4()
		{
			Assert.Equal(Mosa.Test.Collection.ForeachTests.ForeachU4(), Run<uint>("Mosa.Test.Collection.ForeachTests.ForeachU4"));
		}
		
		[Fact]
		public void ForeachU8()
		{
			Assert.Equal(Mosa.Test.Collection.ForeachTests.ForeachU8(), Run<ulong>("Mosa.Test.Collection.ForeachTests.ForeachU8"));
		}
		
		[Fact]
		public void ForeachI1()
		{
			Assert.Equal(Mosa.Test.Collection.ForeachTests.ForeachI1(), Run<sbyte>("Mosa.Test.Collection.ForeachTests.ForeachI1"));
		}
		
		[Fact]
		public void ForeachI2()
		{
			Assert.Equal(Mosa.Test.Collection.ForeachTests.ForeachI2(), Run<short>("Mosa.Test.Collection.ForeachTests.ForeachI2"));
		}
		
		[Fact]
		public void ForeachI4()
		{
			Assert.Equal(Mosa.Test.Collection.ForeachTests.ForeachI4(), Run<int>("Mosa.Test.Collection.ForeachTests.ForeachI4"));
		}
		
		[Fact]
		public void ForeachI8()
		{
			Assert.Equal(Mosa.Test.Collection.ForeachTests.ForeachI8(), Run<long>("Mosa.Test.Collection.ForeachTests.ForeachI8"));
		}
		
	}
}
