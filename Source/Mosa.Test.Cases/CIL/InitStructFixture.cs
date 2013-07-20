 

using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class InitStruct : TestCompilerAdapter
	{
		public InitStruct()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void InitStructU1()
		{
			Assert.AreEqual(InitStructTests.InitStructU1(), Run<byte>("Mosa.Test.Collection", "InitStructTests", "InitStructU1"));
		}

		[Test]
		public void InitStructU2()
		{
			Assert.AreEqual(InitStructTests.InitStructU2(), Run<ushort>("Mosa.Test.Collection", "InitStructTests", "InitStructU2"));
		}

		[Test]
		public void InitStructU4()
		{
			Assert.AreEqual(InitStructTests.InitStructU4(), Run<uint>("Mosa.Test.Collection", "InitStructTests", "InitStructU4"));
		}

		[Test]
		public void InitStructU8()
		{
			Assert.AreEqual(InitStructTests.InitStructU8(), Run<ulong>("Mosa.Test.Collection", "InitStructTests", "InitStructU8"));
		}

		[Test]
		public void InitStructI1()
		{
			Assert.AreEqual(InitStructTests.InitStructI1(), Run<sbyte>("Mosa.Test.Collection", "InitStructTests", "InitStructI1"));
		}

		[Test]
		public void InitStructI2()
		{
			Assert.AreEqual(InitStructTests.InitStructI2(), Run<short>("Mosa.Test.Collection", "InitStructTests", "InitStructI2"));
		}

		[Test]
		public void InitStructI4()
		{
			Assert.AreEqual(InitStructTests.InitStructI4(), Run<int>("Mosa.Test.Collection", "InitStructTests", "InitStructI4"));
		}

		[Test]
		public void InitStructI8()
		{
			Assert.AreEqual(InitStructTests.InitStructI8(), Run<long>("Mosa.Test.Collection", "InitStructTests", "InitStructI8"));
		}

		[Test]
		public void InitStructR4()
		{
			Assert.AreEqual(InitStructTests.InitStructR4(), Run<float>("Mosa.Test.Collection", "InitStructTests", "InitStructR4"));
		}

		[Test]
		public void InitStructR8()
		{
			Assert.AreEqual(InitStructTests.InitStructR8(), Run<double>("Mosa.Test.Collection", "InitStructTests", "InitStructR8"));
		}

		[Test]
		public void InitStructB()
		{
			Assert.AreEqual(InitStructTests.InitStructB(), Run<bool>("Mosa.Test.Collection", "InitStructTests", "InitStructB"));
		}

		[Test]
		public void InitStructC()
		{
			Assert.AreEqual(InitStructTests.InitStructC(), Run<char>("Mosa.Test.Collection", "InitStructTests", "InitStructC"));
		}

		[Test]
		public void InitStructO()
		{
			Assert.AreEqual(InitStructTests.InitStructO(), Run<object>("Mosa.Test.Collection", "InitStructTests", "InitStructO"));
		}

	}
}
