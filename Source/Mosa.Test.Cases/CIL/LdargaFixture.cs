 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class Ldarga : TestCompilerAdapter
	{
		public Ldarga()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		#region CheckValue
				[Test]
		public void LdargaCheckValueU1([U1]byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU1", a, a));
		}
				[Test]
		public void LdargaCheckValueU2([U2]ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU2", a, a));
		}
				[Test]
		public void LdargaCheckValueU4([U4]uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU4", a, a));
		}
				[Test]
		public void LdargaCheckValueU8([U8]ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU8", a, a));
		}
				[Test]
		public void LdargaCheckValueI1([I1]sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI1", a, a));
		}
				[Test]
		public void LdargaCheckValueI2([I2]short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI2", a, a));
		}
				[Test]
		public void LdargaCheckValueI4([I4]int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI4", a, a));
		}
				[Test]
		public void LdargaCheckValueI8([I8]long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI8", a, a));
		}
				[Test]
		public void LdargaCheckValueR4([R4]float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueR4", a, a));
		}
				[Test]
		public void LdargaCheckValueR8([R8]double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueR8", a, a));
		}
				[Test]
		public void LdargaCheckValueC([C]char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueC", a, a));
		}
				#endregion

		#region ChangeValue
				[Test]
		public void LdargaChangeValueU1([U1]byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU1", a, a));
		}
				[Test]
		public void LdargaChangeValueU2([U2]ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU2", a, a));
		}
				[Test]
		public void LdargaChangeValueU4([U4]uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU4", a, a));
		}
				[Test]
		public void LdargaChangeValueU8([U8]ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU8", a, a));
		}
				[Test]
		public void LdargaChangeValueI1([I1]sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI1", a, a));
		}
				[Test]
		public void LdargaChangeValueI2([I2]short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI2", a, a));
		}
				[Test]
		public void LdargaChangeValueI4([I4]int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI4", a, a));
		}
				[Test]
		public void LdargaChangeValueI8([I8]long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI8", a, a));
		}
				[Test]
		public void LdargaChangeValueR4([R4]float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueR4", a, a));
		}
				[Test]
		public void LdargaChangeValueR8([R8]double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueR8", a, a));
		}
				#endregion
	}
}
