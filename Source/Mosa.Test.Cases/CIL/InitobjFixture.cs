 

using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class Initobj : TestCompilerAdapter
	{
		public Initobj()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void InitobjU1()
		{
			Assert.AreEqual(InitobjTests.InitobjTestU1(), Run<byte>("Mosa.Test.Collection", "InitobjTests", "InitobjTestU1"));
		}

		[Test]
		public void InitobjU2()
		{
			Assert.AreEqual(InitobjTests.InitobjTestU2(), Run<ushort>("Mosa.Test.Collection", "InitobjTests", "InitobjTestU2"));
		}

		[Test]
		public void InitobjU4()
		{
			Assert.AreEqual(InitobjTests.InitobjTestU4(), Run<uint>("Mosa.Test.Collection", "InitobjTests", "InitobjTestU4"));
		}

		[Test]
		public void InitobjU8()
		{
			Assert.AreEqual(InitobjTests.InitobjTestU8(), Run<ulong>("Mosa.Test.Collection", "InitobjTests", "InitobjTestU8"));
		}

		[Test]
		public void InitobjI1()
		{
			Assert.AreEqual(InitobjTests.InitobjTestI1(), Run<sbyte>("Mosa.Test.Collection", "InitobjTests", "InitobjTestI1"));
		}

		[Test]
		public void InitobjI2()
		{
			Assert.AreEqual(InitobjTests.InitobjTestI2(), Run<short>("Mosa.Test.Collection", "InitobjTests", "InitobjTestI2"));
		}

		[Test]
		public void InitobjI4()
		{
			Assert.AreEqual(InitobjTests.InitobjTestI4(), Run<int>("Mosa.Test.Collection", "InitobjTests", "InitobjTestI4"));
		}

		[Test]
		public void InitobjI8()
		{
			Assert.AreEqual(InitobjTests.InitobjTestI8(), Run<long>("Mosa.Test.Collection", "InitobjTests", "InitobjTestI8"));
		}

		[Test]
		public void InitobjR4()
		{
			Assert.AreEqual(InitobjTests.InitobjTestR4(), Run<float>("Mosa.Test.Collection", "InitobjTests", "InitobjTestR4"));
		}

		[Test]
		public void InitobjR8()
		{
			Assert.AreEqual(InitobjTests.InitobjTestR8(), Run<double>("Mosa.Test.Collection", "InitobjTests", "InitobjTestR8"));
		}

		[Test]
		public void InitobjB()
		{
			Assert.AreEqual(InitobjTests.InitobjTestB(), Run<bool>("Mosa.Test.Collection", "InitobjTests", "InitobjTestB"));
		}

		[Test]
		public void InitobjC()
		{
			Assert.AreEqual(InitobjTests.InitobjTestC(), Run<char>("Mosa.Test.Collection", "InitobjTests", "InitobjTestC"));
		}

		[Test]
		public void InitobjO()
		{
			Assert.AreEqual(InitobjTests.InitobjTestO(), Run<object>("Mosa.Test.Collection", "InitobjTests", "InitobjTestO"));
		}

	}
}
