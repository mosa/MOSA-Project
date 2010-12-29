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
	public class ConvI4 : TestCompilerAdapter
	{
		
		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ConvI4_I1(sbyte a)
		{
			settings.CodeSource = "static class Test { static bool ConvI4_I1(int expect, sbyte a) { return expect == ((int)a); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ConvI4_I1", ((int)a), a));
		}

		[Column(0, 1, 2, short.MinValue, short.MaxValue)]
		[Test]
		public void ConvI4_I2(short a)
		{
			settings.CodeSource = "static class Test { static bool ConvI4_I2(int expect, short a) { return expect == ((int)a); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ConvI4_I2", ((int)a), a));
		}

		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test]
		public void ConvI4_I4(int a)
		{
			settings.CodeSource = "static class Test { static bool ConvI4_I4(int expect, int a) { return expect == ((int)a); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ConvI4_I4", ((int)a), a));
		}

		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test]
		public void ConvI4_I8(long a)
		{
			settings.CodeSource = "static class Test { static bool ConvI4_I8(int expect, long a) { return expect == ((int)a); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ConvI4_I8", ((int)a), a));
		}

		[Column(0.0f, 1.0f, 2.0f, Single.MinValue, Single.MaxValue)]
		[Test]
		public void ConvI4_R4(float a)
		{
			settings.CodeSource = "static class Test { static bool ConvI1_R4(int expect, float a) { return expect == ((int)a); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ConvI1_R4", ((int)a), a));
		}

		[Column(0.0f, 1.0f, 2.0f, Double.MinValue, Double.MaxValue)]
		[Test]
		public void ConvI4_R8(double a)
		{
			settings.CodeSource = "static class Test { static bool ConvI1_R8(int expect, double a) { return expect == ((int)a); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ConvI1_R8", ((int)a), a));
		}
	}
}
