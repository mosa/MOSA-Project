 

using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class Structures : TestCompilerAdapter
	{
		public Structures()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void StructTestSet1U1([U1]byte one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1U1", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3U1([U1]byte one, [U1]byte two, [U1]byte three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3U1", one, two, three));
		}

		[Test]
		public void StructTestSet1U2([U2]ushort one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1U2", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3U2([U2]ushort one, [U2]ushort two, [U2]ushort three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3U2", one, two, three));
		}

		[Test]
		public void StructTestSet1U4([U4]uint one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1U4", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3U4([U4]uint one, [U4]uint two, [U4]uint three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3U4", one, two, three));
		}
		[Pending("See Bug #52")]
		[Test]
		public void StructTestSet1U8([U8]ulong one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1U8", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3U8([U8]ulong one, [U8]ulong two, [U8]ulong three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3U8", one, two, three));
		}

		[Test]
		public void StructTestSet1I1([I1]sbyte one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1I1", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3I1([I1]sbyte one, [I1]sbyte two, [I1]sbyte three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3I1", one, two, three));
		}

		[Test]
		public void StructTestSet1I2([I2]short one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1I2", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3I2([I2]short one, [I2]short two, [I2]short three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3I2", one, two, three));
		}

		[Test]
		public void StructTestSet1I4([I4]int one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1I4", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3I4([I4]int one, [I4]int two, [I4]int three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3I4", one, two, three));
		}
		[Pending("See Bug #52")]
		[Test]
		public void StructTestSet1I8([I8]long one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1I8", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3I8([I8]long one, [I8]long two, [I8]long three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3I8", one, two, three));
		}

		[Test]
		public void StructTestSet1R4([R4]float one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1R4", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3R4([R4]float one, [R4]float two, [R4]float three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3R4", one, two, three));
		}
		[Pending("See Bug #52")]
		[Test]
		public void StructTestSet1R8([R8]double one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1R8", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3R8([R8]double one, [R8]double two, [R8]double three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3R8", one, two, three));
		}

		[Test]
		public void StructTestSet1C([C]char one)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet1C", one));
		}
		
		[Test]
		[Pending("See Bug #52")]
		public void StructTestSet3C([C]char one, [C]char two, [C]char three)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StructTests", "StructTestSet3C", one, two, three));
		}
	}
}
