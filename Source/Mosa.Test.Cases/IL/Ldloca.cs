/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 *
 */
 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class Ldloca : TestCompilerAdapter
	{
		public Ldloca()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
		
		[Column(0, 1, 2, byte.MinValue, byte.MaxValue, byte.MinValue + 1, byte.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU1", a, a));
		}
		
		[Column(0, 1, 2, ushort.MinValue, ushort.MaxValue, ushort.MinValue + 1, ushort.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU2", a, a));
		}
		
		[Column(0, 1, 2, uint.MinValue, uint.MaxValue, uint.MinValue + 1, uint.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU4", a, a));
		}
		
		[Column(0, 1, 2, ulong.MinValue, ulong.MaxValue, ulong.MinValue + 1, ulong.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueU8(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU8", a, a));
		}
		
		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue, sbyte.MinValue + 1, sbyte.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueI1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI1", a, a));
		}
		
		[Column(0, 1, 2, short.MinValue, short.MaxValue, short.MinValue + 1, short.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueI2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI2", a, a));
		}
		
		[Column(0, 1, 2, int.MinValue, int.MaxValue, int.MinValue + 1, int.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI4", a, a));
		}
		
		[Column(0, 1, 2, long.MinValue, long.MaxValue, long.MinValue + 1, long.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueI8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI8", a, a));
		}
		
		[Column(0, 1, 2, float.MinValue, float.MaxValue, float.MinValue + 1, float.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueR4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueR4", a, a));
		}
		
		[Column(0, 1, 2, double.MinValue, double.MaxValue, double.MinValue + 1, double.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueR8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueR8", a, a));
		}
		
		[Column(0, 1, 2, char.MinValue, char.MaxValue, char.MinValue + 1, char.MaxValue - 1)]
		[Test]
		public void LdlocaCheckValueC(char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueC", a, a));
		}
		
	}
}
