/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Reflection.Emit;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class ConvI4 : CodeDomTestRunner
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		delegate bool Native_ConvI4_I1(int expect, sbyte a);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ConvI4_I1(sbyte a)
		{
			CodeSource = "static class Test { static bool ConvI4_I1(int expect, sbyte a) { return expect == ((int)a); } }" + Code.ObjectClassDefinition;
			Assert.IsTrue((bool)Run<Native_ConvI4_I1>("", "Test", "ConvI4_I1", ((int)a), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		delegate bool Native_ConvI4_I2(int expect, short a);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		[Column(0, 1, 2, short.MinValue, short.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ConvI4_I2(short a)
		{
			CodeSource = "static class Test { static bool ConvI4_I2(int expect, short a) { return expect == ((int)a); } }" + Code.ObjectClassDefinition;
			Assert.IsTrue((bool)Run<Native_ConvI4_I2>("", "Test", "ConvI4_I2", ((int)a), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		delegate bool Native_ConvI4_I4(int expect, int a);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ConvI4_I4(int a)
		{
			CodeSource = "static class Test { static bool ConvI4_I4(int expect, int a) { return expect == ((int)a); } }" + Code.ObjectClassDefinition;
			Assert.IsTrue((bool)Run<Native_ConvI4_I4>("", "Test", "ConvI4_I4", ((int)a), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		delegate bool Native_ConvI4_I8(int expect, long a);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ConvI4_I8(long a)
		{
			CodeSource = "static class Test { static bool ConvI4_I8(int expect, long a) { return expect == ((int)a); } }" + Code.ObjectClassDefinition;
			Assert.IsTrue((bool)Run<Native_ConvI4_I8>("", "Test", "ConvI4_I8", ((int)a), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		delegate bool Native_ConvI4_R4(int expect, float a);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		[Column(0.0f, 1.0f, 2.0f, Single.MinValue, Single.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ConvI4_R4(float a)
		{
			CodeSource = "static class Test { static bool ConvI1_R4(int expect, float a) { return expect == ((int)a); } }" + Code.ObjectClassDefinition;
			Assert.IsTrue((bool)Run<Native_ConvI4_R4>("", "Test", "ConvI1_R4", ((int)a), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		delegate bool Native_ConvI4_R8(int expect, double a);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		[Column(0.0f, 1.0f, 2.0f, Double.MinValue, Double.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ConvI4_R8(double a)
		{
			CodeSource = "static class Test { static bool ConvI1_R8(int expect, double a) { return expect == ((int)a); } }" + Code.ObjectClassDefinition;
			Assert.IsTrue((bool)Run<Native_ConvI4_R8>("", "Test", "ConvI1_R8", ((int)a), a));
		}
	}
}
