 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.FIX.IL
{
	[TestFixture]
	public class Conv : TestCompilerAdapter
	{
		public Conv()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
				
		[Test]
		public void ConvU1I1([U1]byte a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU1I1", a, b));
		}
				
		[Test]
		public void ConvU1I2([U1]byte a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU1I2", a, b));
		}
				
		[Test]
		public void ConvU1I4([U1]byte a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU1I4", a, b));
		}
				
		[Test]
		public void ConvU1I8([U1]byte a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU1I8", a, b));
		}
				
		[Test]
		public void ConvU2I1([U2]ushort a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU2I1", a, b));
		}
				
		[Test]
		public void ConvU2I2([U2]ushort a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU2I2", a, b));
		}
				
		[Test]
		public void ConvU2I4([U2]ushort a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU2I4", a, b));
		}
				
		[Test]
		public void ConvU2I8([U2]ushort a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU2I8", a, b));
		}
				
		[Test]
		public void ConvU4I1([U4]uint a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU4I1", a, b));
		}
				
		[Test]
		public void ConvU4I2([U4]uint a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU4I2", a, b));
		}
				
		[Test]
		public void ConvU4I4([U4]uint a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU4I4", a, b));
		}
				
		[Test]
		public void ConvU4I8([U4]uint a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU4I8", a, b));
		}
				
		[Test]
		public void ConvU8I1([U8]ulong a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU8I1", a, b));
		}
				
		[Test]
		public void ConvU8I2([U8]ulong a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU8I2", a, b));
		}
				
		[Test]
		public void ConvU8I4([U8]ulong a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU8I4", a, b));
		}
				
		[Test]
		public void ConvU8I8([U8]ulong a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvU8I8", a, b));
		}
				
		[Test]
		public void ConvI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI1I1", a, b));
		}
				
		[Test]
		public void ConvI1I2([I1]sbyte a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI1I2", a, b));
		}
				
		[Test]
		public void ConvI1I4([I1]sbyte a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI1I4", a, b));
		}
				
		[Test]
		public void ConvI1I8([I1]sbyte a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI1I8", a, b));
		}
				
		[Test]
		public void ConvI2I1([I2]short a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI2I1", a, b));
		}
				
		[Test]
		public void ConvI2I2([I2]short a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI2I2", a, b));
		}
				
		[Test]
		public void ConvI2I4([I2]short a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI2I4", a, b));
		}
				
		[Test]
		public void ConvI2I8([I2]short a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI2I8", a, b));
		}
				
		[Test]
		public void ConvI4I1([I4]int a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI4I1", a, b));
		}
				
		[Test]
		public void ConvI4I2([I4]int a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI4I2", a, b));
		}
				
		[Test]
		public void ConvI4I4([I4]int a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI4I4", a, b));
		}
				
		[Test]
		public void ConvI4I8([I4]int a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI4I8", a, b));
		}
				
		[Test]
		public void ConvI8I1([I8]long a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI8I1", a, b));
		}
				
		[Test]
		public void ConvI8I2([I8]long a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI8I2", a, b));
		}
				
		[Test]
		public void ConvI8I4([I8]long a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI8I4", a, b));
		}
				
		[Test]
		public void ConvI8I8([I8]long a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvI8I8", a, b));
		}
				
		[Test]
		public void ConvR4I1([R4]float a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvR4I1", a, b));
		}
				
		[Test]
		public void ConvR4I2([R4]float a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvR4I2", a, b));
		}
				
		[Test]
		public void ConvR4I4([R4]float a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvR4I4", a, b));
		}
				
		[Test]
		public void ConvR4I8([R4]float a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvR4I8", a, b));
		}
				
		[Test]
		public void ConvR8I1([R8]double a, [I1]sbyte b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvR8I1", a, b));
		}
				
		[Test]
		public void ConvR8I2([R8]double a, [I2]short b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvR8I2", a, b));
		}
				
		[Test]
		public void ConvR8I4([R8]double a, [I4]int b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvR8I4", a, b));
		}
				
		[Test]
		public void ConvR8I8([R8]double a, [I8]long b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvTests", "ConvR8I8", a, b));
		}
		
	}
}
