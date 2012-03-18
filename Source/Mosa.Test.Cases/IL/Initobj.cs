/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai Patrick Reisert (Boddlnagg) <kpreisert@googlemail.com> 
 *
 */
  

using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class Initobj : TestCompilerAdapter
	{
		public Initobj()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjU1()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestU1(), Run<byte>("Mosa.Test.Collection", "InitobjTests", "InitobjTestU1"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjU2()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestU2(), Run<ushort>("Mosa.Test.Collection", "InitobjTests", "InitobjTestU2"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjU4()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestU4(), Run<uint>("Mosa.Test.Collection", "InitobjTests", "InitobjTestU4"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjU8()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestU8(), Run<ulong>("Mosa.Test.Collection", "InitobjTests", "InitobjTestU8"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjI1()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestI1(), Run<sbyte>("Mosa.Test.Collection", "InitobjTests", "InitobjTestI1"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjI2()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestI2(), Run<short>("Mosa.Test.Collection", "InitobjTests", "InitobjTestI2"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjI4()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestI4(), Run<int>("Mosa.Test.Collection", "InitobjTests", "InitobjTestI4"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjI8()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestI8(), Run<long>("Mosa.Test.Collection", "InitobjTests", "InitobjTestI8"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjR4()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestR4(), Run<float>("Mosa.Test.Collection", "InitobjTests", "InitobjTestR4"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjR8()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestR8(), Run<double>("Mosa.Test.Collection", "InitobjTests", "InitobjTestR8"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjB()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestB(), Run<bool>("Mosa.Test.Collection", "InitobjTests", "InitobjTestB"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjC()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestC(), Run<char>("Mosa.Test.Collection", "InitobjTests", "InitobjTestC"));
		}

		[Test]
		[Pending("Commented out until implemented")]
		public void InitobjO()
		{
			//Assert.AreEqual(InitobjTests.InitobjTestO(), Run<object>("Mosa.Test.Collection", "InitobjTests", "InitobjTestO"));
		}

	}
}
