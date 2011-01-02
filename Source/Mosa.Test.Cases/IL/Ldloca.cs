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
using Mosa.Test.Runtime.CompilerFramework.Numbers;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class Ldloca : TestCompilerAdapter
	{
		public Ldloca()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
		
		[Test, Factory(typeof(U1), "Samples")]
		public void LdlocaCheckValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU1", a, a));
		}
		
		[Test, Factory(typeof(U2), "Samples")]
		public void LdlocaCheckValueU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU2", a, a));
		}
		
		[Test, Factory(typeof(U4), "Samples")]
		public void LdlocaCheckValueU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU4", a, a));
		}
		
		[Test, Factory(typeof(U8), "Samples")]
		public void LdlocaCheckValueU8(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueU8", a, a));
		}
		
		[Test, Factory(typeof(I1), "Samples")]
		public void LdlocaCheckValueI1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI1", a, a));
		}
		
		[Test, Factory(typeof(I2), "Samples")]
		public void LdlocaCheckValueI2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI2", a, a));
		}
		
		[Test, Factory(typeof(I4), "Samples")]
		public void LdlocaCheckValueI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI4", a, a));
		}
		
		[Test, Factory(typeof(I8), "Samples")]
		public void LdlocaCheckValueI8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueI8", a, a));
		}
		
		[Test, Factory(typeof(R4), "Samples")]
		public void LdlocaCheckValueR4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueR4", a, a));
		}
		
		[Test, Factory(typeof(R8), "Samples")]
		public void LdlocaCheckValueR8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueR8", a, a));
		}
		
		[Test, Factory(typeof(C), "Samples")]
		public void LdlocaCheckValueC(char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdlocaTests", "LdlocaCheckValueC", a, a));
		}
		
	}
}
