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
	public class ConvI8 : TestCompilerAdapter
	{

		public ConvI8()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ConvI8_I1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI8Tests", "ConvI8_I1", (long)a, a));
		}

		[Column(0, 1, 2, short.MinValue, short.MaxValue)]
		[Test]
		public void ConvI8_I2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI8Tests", "ConvI8_I2", (long)a, a));
		}

		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test]
		public void ConvI8_I4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI8Tests", "ConvI8_I4", (long)a, a));
		}

		[Column(0, 1, 2, long.MinValue, long.MaxValue)]
		[Test]
		public void ConvI8_I8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI8Tests", "ConvI8_I8", (long)a, a));
		}

		//TODO:
		//[Column(0, 1, 2, long.MinValue, long.MaxValue)]
		//[Test]
		//public void ConvI8_R4(float a)
		//{
		//    Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI8Tests", "ConvI8_R4", (long)a, a));
		//}

		//TODO:
		//[Column(0, 1, 2, long.MinValue, long.MaxValue)]
		//[Test]
		//public void ConvI8_R8(double a)
		//{
		//    Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ConvI8Tests", "ConvI8_R8", (long)a, a));
		//}
	}
}
