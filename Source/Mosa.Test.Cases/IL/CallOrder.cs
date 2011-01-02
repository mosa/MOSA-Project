/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lymangmail.com>
 *  Simon Wollwage (rootnode) <kintarothink-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruckmichaelruck.de>
 *  Kai P. Reisert <kpreisertgooglemail.com>
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class CallOrder : TestCompilerAdapter
	{

		public CallOrder()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void CallEmpty()
		{
			Run<object>("Mosa.Test.Collection", "CallOrderTests", "CallEmpty");
		}

		[Test]
		public void CallOrderI4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4", 1));
		}

		[Test]
		public void CallOrderI4I4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4I4", 1, 2));
		}

		[Test]
		public void CallOrderI4I4I4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4I4I4", 1, 2, 3));
		}

		[Test]
		public void CallOrderI4I4I4I4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4I4I4I4", 1, 2, 3, 4));
		}

		[Test]
		public void CallOrderU8()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderU8", (ulong)1, (ulong)2, (ulong)3, (ulong)4));
		}

		[Test]
		public void CallOrderU4_U8_U8_U8()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderU4_U8_U8_U8", (uint)1, (ulong)2, (ulong)3, (ulong)4));
		}

	}
}
