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

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class ConvI1 : TestCompilerAdapter
	{
		
		public ConvI1()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ConvI1_I1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI1Tests", "ConvI1_I1", a, a));
		}

		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue, short.MinValue, short.MaxValue)]
		[Test]
		public void ConvI1_I2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI1Tests", "ConvI1_I2", (sbyte)a, a));
		}

		[Column(/*0, 1, 2, sbyte.MinValue, sbyte.MaxValue, */int.MinValue, int.MaxValue)]
		[Test]
		public void ConvI1_I4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI1Tests", "ConvI1_I4", (sbyte)a, a));
		}

		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue, long.MinValue, long.MaxValue)]
		[Test]
		public void ConvI1_I8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI1Tests", "ConvI1_I8", (sbyte)a, a));
		}

		[Column(0.0f, 1.0f, 2.0f, (float)sbyte.MinValue, (float)sbyte.MaxValue, Single.MinValue, Single.MaxValue)]
		[Test]
		public void ConvI1_R4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI1Tests", "ConvI1_R4", (sbyte)a, a));
		}

		[Column(0, 1, 2, (double)sbyte.MinValue, (double)sbyte.MaxValue, Double.MinValue, Double.MaxValue)]
		[Test]
		public void ConvI1_R8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI1Tests", "ConvI1_R8", (sbyte)a, a));
		}
	}
}
