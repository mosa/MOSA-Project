 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class GenericCall : TestCompilerAdapter
	{
		public GenericCall()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
		
		[Test]
		[Pending]
		public void GenericCallU1([U1]byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallU1", a));
		}
		
		[Test]
		[Pending]
		public void GenericCallU2([U2]ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallU2", a));
		}
		
		[Test]
		[Pending]
		public void GenericCallU4([U4]uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallU4", a));
		}
		
		[Test]
		[Pending]
		public void GenericCallU8([U8]ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallU8", a));
		}
		
		[Test]
		[Pending]
		public void GenericCallI1([I1]sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallI1", a));
		}
		
		[Test]
		[Pending]
		public void GenericCallI2([I2]short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallI2", a));
		}
		
		[Test]
		[Pending]
		public void GenericCallI4([I4]int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallI4", a));
		}
		
		[Test]
		[Pending]
		public void GenericCallI8([I8]long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallI8", a));
		}
		
		[Test]
		[Pending]
		public void GenericCallC([C]char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallC", a));
		}
		
	}
}
