/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Reflection.Emit;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.OLD.IL
{
	[TestFixture]
	public class ConvI4 : TestCompilerAdapter
	{
		
		public ConvI4()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ConvI4_I1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI4Tests", "ConvI4_I1", (int)a, a));
		}

		[Column(0, 1, 2, short.MinValue, short.MaxValue)]
		[Test]
		public void ConvI4_I2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI4Tests", "ConvI4_I2", (int)a, a));
		}

		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test]
		public void ConvI4_I4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI4Tests", "ConvI4_I4", (int)a, a));
		}

		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test]
		public void ConvI4_I8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI4Tests", "ConvI4_I8", (int)a, a));
		}

		[Column(0.0f, 1.0f, 2.0f, Single.MinValue, Single.MaxValue)]
		[Test]
		public void ConvI4_R4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI4Tests", "ConvI4_R4", (int)a, a));
		}

		[Column(0.0f, 1.0f, 2.0f, Double.MinValue, Double.MaxValue)]
		[Test]
		public void ConvI4_R8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI4Tests", "ConvI4_R8", (int)a, a));
		}
	}
}
