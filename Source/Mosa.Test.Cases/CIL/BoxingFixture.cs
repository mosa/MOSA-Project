 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class BoxingFixture : TestCompilerAdapter
	{
		public BoxingFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
				
		[Test]
		public void BoxU1([U1]byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxU1", value));
		}
				
		[Test]
		public void BoxU2([U2]ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxU2", value));
		}
				
		[Test]
		public void BoxU4([U4]uint value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxU4", value));
		}
				
		[Test]
		public void BoxU8([U8]ulong value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxU8", value));
		}
				
		[Test]
		public void BoxI1([I1]sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxI1", value));
		}
				
		[Test]
		public void BoxI2([I2]short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxI2", value));
		}
				
		[Test]
		public void BoxI4([I4]int value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxI4", value));
		}
				
		[Test]
		public void BoxI8([I8]long value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxI8", value));
		}
				
		[Test]
		public void BoxR4([R4]float value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxR4", value));
		}
				
		[Test]
		public void BoxR8([R8]double value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxR8", value));
		}
				
		[Test]
		public void BoxC([C]char value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BoxingTests", "BoxC", value));
		}
			}
}
