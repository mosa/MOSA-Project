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
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class Switch : TestCompilerAdapter
	{
		public Switch()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
		
		[Test, Factory(typeof(U1), "Samples")]
		public void SwitchU1(byte a)
		{
			Assert.AreEqual(SwitchTests.SwitchU1(a), Run<byte>("Mosa.Test.Collection", "SwitchTests", "SwitchU1", a));
		}
		
		[Test, Factory(typeof(U2), "Samples")]
		public void SwitchU2(ushort a)
		{
			Assert.AreEqual(SwitchTests.SwitchU2(a), Run<ushort>("Mosa.Test.Collection", "SwitchTests", "SwitchU2", a));
		}
		
		[Test, Factory(typeof(U4), "Samples")]
		public void SwitchU4(uint a)
		{
			Assert.AreEqual(SwitchTests.SwitchU4(a), Run<uint>("Mosa.Test.Collection", "SwitchTests", "SwitchU4", a));
		}
		
		[Test, Factory(typeof(U8), "Samples")]
		public void SwitchU8(ulong a)
		{
			Assert.AreEqual(SwitchTests.SwitchU8(a), Run<ulong>("Mosa.Test.Collection", "SwitchTests", "SwitchU8", a));
		}
		
		[Test, Factory(typeof(I1), "Samples")]
		public void SwitchI1(sbyte a)
		{
			Assert.AreEqual(SwitchTests.SwitchI1(a), Run<sbyte>("Mosa.Test.Collection", "SwitchTests", "SwitchI1", a));
		}
		
		[Test, Factory(typeof(I2), "Samples")]
		public void SwitchI2(short a)
		{
			Assert.AreEqual(SwitchTests.SwitchI2(a), Run<short>("Mosa.Test.Collection", "SwitchTests", "SwitchI2", a));
		}
		
		[Test, Factory(typeof(I4), "Samples")]
		public void SwitchI4(int a)
		{
			Assert.AreEqual(SwitchTests.SwitchI4(a), Run<int>("Mosa.Test.Collection", "SwitchTests", "SwitchI4", a));
		}
		
		[Test, Factory(typeof(I8), "Samples")]
		public void SwitchI8(long a)
		{
			Assert.AreEqual(SwitchTests.SwitchI8(a), Run<long>("Mosa.Test.Collection", "SwitchTests", "SwitchI8", a));
		}
		
	}
}
