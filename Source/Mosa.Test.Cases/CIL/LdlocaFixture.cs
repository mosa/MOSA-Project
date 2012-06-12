 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class Ldloca : TestCompilerAdapter
	{
		public Ldloca()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
				[Test]
		public void LdlocaCheckValueU1([U1]byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU1", a, a));
		}
				[Test]
		public void LdlocaCheckValueU2([U2]ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU2", a, a));
		}
				[Test]
		public void LdlocaCheckValueU4([U4]uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU4", a, a));
		}
				[Test]
		public void LdlocaCheckValueU8([U8]ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU8", a, a));
		}
				[Test]
		public void LdlocaCheckValueI1([I1]sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI1", a, a));
		}
				[Test]
		public void LdlocaCheckValueI2([I2]short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI2", a, a));
		}
				[Test]
		public void LdlocaCheckValueI4([I4]int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI4", a, a));
		}
				[Test]
		public void LdlocaCheckValueI8([I8]long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI8", a, a));
		}
				[Test]
		public void LdlocaCheckValueR4([R4]float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueR4", a, a));
		}
				[Test]
		public void LdlocaCheckValueR8([R8]double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueR8", a, a));
		}
				[Test]
		public void LdlocaCheckValueC([C]char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueC", a, a));
		}
			}
}
