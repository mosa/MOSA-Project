 
using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class SwitchFixture : TestCompilerAdapter
	{
		public SwitchFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
		
		[Test]
		public void SwitchU1([U1]byte a)
		{
			Assert.AreEqual(SwitchTests.SwitchU1(a), Run<byte>("Mosa.Test.Collection", "SwitchTests", "SwitchU1", a));
		}
		
		[Test]
		public void SwitchU2([U2]ushort a)
		{
			Assert.AreEqual(SwitchTests.SwitchU2(a), Run<ushort>("Mosa.Test.Collection", "SwitchTests", "SwitchU2", a));
		}
		
		[Test]
		public void SwitchU4([U4]uint a)
		{
			Assert.AreEqual(SwitchTests.SwitchU4(a), Run<uint>("Mosa.Test.Collection", "SwitchTests", "SwitchU4", a));
		}
		
		[Test]
		public void SwitchU8([U8]ulong a)
		{
			Assert.AreEqual(SwitchTests.SwitchU8(a), Run<ulong>("Mosa.Test.Collection", "SwitchTests", "SwitchU8", a));
		}
		
		[Test]
		public void SwitchI1([I1]sbyte a)
		{
			Assert.AreEqual(SwitchTests.SwitchI1(a), Run<sbyte>("Mosa.Test.Collection", "SwitchTests", "SwitchI1", a));
		}
		
		[Test]
		public void SwitchI2([I2]short a)
		{
			Assert.AreEqual(SwitchTests.SwitchI2(a), Run<short>("Mosa.Test.Collection", "SwitchTests", "SwitchI2", a));
		}
		
		[Test]
		public void SwitchI4([I4]int a)
		{
			Assert.AreEqual(SwitchTests.SwitchI4(a), Run<int>("Mosa.Test.Collection", "SwitchTests", "SwitchI4", a));
		}
		
		[Test]
		public void SwitchI8([I8]long a)
		{
			Assert.AreEqual(SwitchTests.SwitchI8(a), Run<long>("Mosa.Test.Collection", "SwitchTests", "SwitchI8", a));
		}
		
	}
}
