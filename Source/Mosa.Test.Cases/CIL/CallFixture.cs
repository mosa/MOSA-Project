 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class CallFixture : TestCompilerAdapter
	{
		public CallFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
		
		[Test]
		public void CallU1([U1]byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallU1", a));
		}
		
		[Test]
		public void CallU2([U2]ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallU2", a));
		}
		
		[Test]
		public void CallU4([U4]uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallU4", a));
		}
		
		[Test]
		public void CallU8([U8]ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallU8", a));
		}
		
		[Test]
		public void CallI1([I1]sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallI1", a));
		}
		
		[Test]
		public void CallI2([I2]short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallI2", a));
		}
		
		[Test]
		public void CallI4([I4]int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallI4", a));
		}
		
		[Test]
		public void CallI8([I8]long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallI8", a));
		}
		
		[Test]
		public void CallC([C]char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallTests", "CallC", a));
		}
		
	}
}
