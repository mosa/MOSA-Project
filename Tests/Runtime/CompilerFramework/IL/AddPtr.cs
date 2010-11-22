/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Kai P. Reisert <kpreisert@googlemail.com>
 *
 */

using System;
using System.Runtime.InteropServices;
using Gallio.Framework;
using MbUnit.Framework;
using Test.Mosa.Runtime.CompilerFramework.BaseCode;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
	/// <summary>
	/// Testcase for the AddInstruction on pointers
	/// </summary>
	[TestFixture]
	public class AddPtr : CodeDomTestRunner
	{
		private static string CreateTestCode(string typeInA, string typeInB, string typeOut)
		{
			return @"
				static class Test
				{
					static unsafe " + typeOut + " AddPtr(" + typeInA + " a, " + typeInB + @" b)
					{
						return (a + b);
					}
				}" + Code.AllTestCode;
		}

		private static string CreateConstantTestCode(string typeIn, string typeOut, string constantLeft, string constantRight)
		{
			if (constantLeft == null)
			{
				return @"
					static class Test
					{
						static unsafe " + typeOut + " AddPtr(" + typeIn + @" a)
						{
							return (a + " + constantRight + @");
						}
					}" + Code.AllTestCode;
			}
			else if (constantRight == null)
			{
				return @"
					static class Test
					{
						static unsafe " + typeOut + " AddPtr(" + typeIn + @" a)
						{
							return (" + constantLeft + @" + a);
						}
					}" + Code.AllTestCode;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		delegate int I4_I4_I4(int a, int b);
		delegate int I4_I4_I4_Ptr(int a, int b);
		delegate int I4_I4_I8(int a, long b);
		delegate int I4_I8_I4(long a, int b);
		delegate int I4_I4_Constant(int x);

		#region C
		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddCPtrI4Left(int a, int b)
		{
			char* pa = (char*)a;
			CodeSource = CreateTestCode("char*", "int", "char*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I4>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddCPtrI8Left(int a, long b)
		{
			char* pa = (char*)a;
			CodeSource = CreateTestCode("char*", "long", "char*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I8>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddCPtrI4Right(int a, int b)
		{
			char* pa = (char*)a;
			CodeSource = CreateTestCode("int", "char*", "char*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		//[Row(0, 42)]
		//[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddCPtrI8Right(int a, long b)
		{
			char* pa = (char*)a;
			CodeSource = CreateTestCode("long", "char*", "char*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I8_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantCPtrI4Left(int a, int b)
		{
			char* pa = (char*)a;
			CodeSource = CreateConstantTestCode("char*", "char*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantCPtrI8Left(int a, long b)
		{
			char* pa = (char*)a;
			CodeSource = CreateConstantTestCode("char*", "char*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantCPtrI4Right(int a, int b)
		{
			char* pa = (char*)a;
			CodeSource = CreateConstantTestCode("char*", "char*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantCPtrI8Right(int a, long b)
		{
			char* pa = (char*)a;
			CodeSource = CreateConstantTestCode("char*", "char*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}
		#endregion

		#region U1
		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddU1PtrI4Left(int a, int b)
		{
			byte* pa = (byte*)a;
			CodeSource = CreateTestCode("byte*", "int", "byte*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I4>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddU1PtrI8Left(int a, int b)
		{
			byte* pa = (byte*)a;
			CodeSource = CreateTestCode("byte*", "long", "byte*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I8>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddU1PtrI4Right(int a, int b)
		{
			byte* pa = (byte*)a;
			CodeSource = CreateTestCode("int", "byte*", "byte*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddU1PtrI8Right(int a, int b)
		{
			byte* pa = (byte*)a;
			CodeSource = CreateTestCode("long", "byte*", "byte*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I8_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantU1PtrI4Left(int a, int b)
		{
			byte* pa = (byte*)a;
			CodeSource = CreateConstantTestCode("byte*", "byte*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantU1PtrI8Left(int a, long b)
		{
			byte* pa = (byte*)a;
			CodeSource = CreateConstantTestCode("byte*", "byte*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantU1PtrI4Right(int a, int b)
		{
			byte* pa = (byte*)a;
			CodeSource = CreateConstantTestCode("byte*", "byte*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantU1PtrI8Right(int a, long b)
		{
			byte* pa = (byte*)a;
			CodeSource = CreateConstantTestCode("byte*", "byte*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}
		#endregion

		#region I4
		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI4PtrI4Left(int a, int b)
		{
			int* pa = (int*)a;
			CodeSource = CreateTestCode("int*", "int", "int*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I4>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI4PtrI8Left(int a, int b)
		{
			int* pa = (int*)a;
			CodeSource = CreateTestCode("int*", "long", "int*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I8>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI4PtrI4Right(int a, int b)
		{
			int* pa = (int*)a;
			CodeSource = CreateTestCode("int", "int*", "int*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI4PtrI8Right(int a, int b)
		{
			int* pa = (int*)a;
			CodeSource = CreateTestCode("long", "int*", "int*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I8_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI4PtrI4Left(int a, int b)
		{
			int* pa = (int*)a;
			CodeSource = CreateConstantTestCode("int*", "int*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI4PtrI8Left(int a, long b)
		{
			int* pa = (int*)a;
			CodeSource = CreateConstantTestCode("int*", "int*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI4PtrI4Right(int a, int b)
		{
			int* pa = (int*)a;
			CodeSource = CreateConstantTestCode("int*", "int*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI4PtrI8Right(int a, long b)
		{
			int* pa = (int*)a;
			CodeSource = CreateConstantTestCode("int*", "int*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}
		#endregion

		#region I8
		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI8PtrI4Left(int a, int b)
		{
			long* pa = (long*)a;
			CodeSource = CreateTestCode("long*", "int", "long*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I4>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI8PtrI8Left(int a, int b)
		{
			long* pa = (long*)a;
			CodeSource = CreateTestCode("long*", "long", "long*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I8>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI8PtrI4Right(int a, int b)
		{
			long* pa = (long*)a;
			CodeSource = CreateTestCode("int", "long*", "long*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI8PtrI8Right(int a, int b)
		{
			long* pa = (long*)a;
			CodeSource = CreateTestCode("long", "long*", "long*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I8_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI8PtrI4Left(int a, int b)
		{
			long* pa = (long*)a;
			CodeSource = CreateConstantTestCode("long*", "long*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI8PtrI8Left(int a, long b)
		{
			long* pa = (long*)a;
			CodeSource = CreateConstantTestCode("long*", "long*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI8PtrI4Right(int a, int b)
		{
			long* pa = (long*)a;
			CodeSource = CreateConstantTestCode("long*", "long*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI8PtrI8Right(int a, long b)
		{
			long* pa = (long*)a;
			CodeSource = CreateConstantTestCode("long*", "long*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}
		#endregion

		#region R8
		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddR8PtrI4Left(int a, int b)
		{
			double* pa = (double*)a;
			CodeSource = CreateTestCode("double*", "int", "double*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I4>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddR8PtrI8Left(int a, int b)
		{
			long* pa = (long*)a;
			CodeSource = CreateTestCode("double*", "long", "double*");
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_I8>("", "Test", "AddPtr", a, b));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddR8PtrI4Right(int a, int b)
		{
			double* pa = (double*)a;
			CodeSource = CreateTestCode("long", "double*", "double*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I8_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddR8PtrI8Right(int a, int b)
		{
			double* pa = (double*)a;
			CodeSource = CreateTestCode("long", "double*", "double*");
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I8_I4>("", "Test", "AddPtr", b, a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantR8PtrI4Left(int a, int b)
		{
			double* pa = (double*)a;
			CodeSource = CreateConstantTestCode("double*", "double*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantR8PtrI8Left(int a, long b)
		{
			double* pa = (double*)a;
			CodeSource = CreateConstantTestCode("double*", "double*", null, b.ToString());
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of an integer to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The integer to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantR8PtrI4Right(int a, int b)
		{
			double* pa = (double*)a;
			CodeSource = CreateConstantTestCode("double*", "double*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(b + pa), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}

		/// <summary>
		/// Tests addition of a long to a pointer.
		/// </summary>
		/// <param name="a">The pointer value.</param>
		/// <param name="b">The long to add to the pointer.</param>
		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantR8PtrI8Right(int a, long b)
		{
			double* pa = (double*)a;
			CodeSource = CreateConstantTestCode("double*", "double*", b.ToString(), null);
			UnsafeCode = true;
			Assert.AreEqual((int)(pa + b), (int)Run<I4_I4_Constant>("", "Test", "AddPtr", a));
		}
		#endregion
	}
}
