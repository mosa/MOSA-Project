 
using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class ForeachFixture : TestCompilerAdapter
	{
		public ForeachFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
		
		[Test]
		public void ForeachU1()
		{
			Assert.AreEqual(ForeachTests.ForeachU1(), Run<byte>("Mosa.Test.Collection", "ForeachTests", "ForeachU1"));
		}
		
		[Test]
		public void ForeachU2()
		{
			Assert.AreEqual(ForeachTests.ForeachU2(), Run<ushort>("Mosa.Test.Collection", "ForeachTests", "ForeachU2"));
		}
		
		[Test]
		public void ForeachU4()
		{
			Assert.AreEqual(ForeachTests.ForeachU4(), Run<uint>("Mosa.Test.Collection", "ForeachTests", "ForeachU4"));
		}
		
		[Test]
		public void ForeachU8()
		{
			Assert.AreEqual(ForeachTests.ForeachU8(), Run<ulong>("Mosa.Test.Collection", "ForeachTests", "ForeachU8"));
		}
		
		[Test]
		public void ForeachI1()
		{
			Assert.AreEqual(ForeachTests.ForeachI1(), Run<sbyte>("Mosa.Test.Collection", "ForeachTests", "ForeachI1"));
		}
		
		[Test]
		public void ForeachI2()
		{
			Assert.AreEqual(ForeachTests.ForeachI2(), Run<short>("Mosa.Test.Collection", "ForeachTests", "ForeachI2"));
		}
		
		[Test]
		public void ForeachI4()
		{
			Assert.AreEqual(ForeachTests.ForeachI4(), Run<int>("Mosa.Test.Collection", "ForeachTests", "ForeachI4"));
		}
		
		[Test]
		public void ForeachI8()
		{
			Assert.AreEqual(ForeachTests.ForeachI8(), Run<long>("Mosa.Test.Collection", "ForeachTests", "ForeachI8"));
		}
		
	}
}
