 

using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class StaticField : TestCompilerAdapter
	{
		public StaticField()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void StaticFieldU1([U1]byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestU1", "StaticFieldU1", a));
		}

		[Test]
		public void StaticFieldU2([U2]ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestU2", "StaticFieldU2", a));
		}

		[Test]
		public void StaticFieldU4([U4]uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestU4", "StaticFieldU4", a));
		}

		[Test]
		public void StaticFieldU8([U8]ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestU8", "StaticFieldU8", a));
		}

		[Test]
		public void StaticFieldI1([I1]sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestI1", "StaticFieldI1", a));
		}

		[Test]
		public void StaticFieldI2([I2]short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestI2", "StaticFieldI2", a));
		}

		[Test]
		public void StaticFieldI4([I4]int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestI4", "StaticFieldI4", a));
		}

		[Test]
		public void StaticFieldI8([I8]long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestI8", "StaticFieldI8", a));
		}

		[Test]
		public void StaticFieldR4([R4]float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestR4", "StaticFieldR4", a));
		}

		[Test]
		public void StaticFieldR8([R8]double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestR8", "StaticFieldR8", a));
		}

		[Test]
		public void StaticFieldB([B]bool a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestB", "StaticFieldB", a));
		}

		[Test]
		public void StaticFieldC([C]char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StaticFieldTestC", "StaticFieldC", a));
		}

	}
}
