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
using Test.Mosa.Runtime.CompilerFramework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{

	[TestFixture]
	public class AddPtr : CodeDomTestRunner
	{
		private static string TestCode = @"
			static class Test
				{
					static unsafe bool AddPtr(int a, int b, int expect)
					{
						return expect == (int)(DoAddPtr((#typeInA)a, (#typeInB)b));
					}

					static unsafe #typeOut DoAddPtr(#typeInA a, #typeInB b)
					{
						return (a + b);
					}
				}";

		private static string CreateTestCode(string typeInA, string typeInB, string typeOut)
		{
			return TestCode
				.Replace("#typeInA", typeInA)
				.Replace("#typeInB", typeInB)
				.Replace("#typeOut", typeOut)
				+ Code.AllTestCode;
		}

		private static string TestCodeConstantRight = @"
			static class Test
				{
					static unsafe bool AddPtr(int a, int expect)
					{
						return expect == (int)(DoAddPtr((#type)a));
					}

					static unsafe #type DoAddPtr(#type a)
					{
						return a + ((#constantType)#constantValue);
					}
				}";

		private static string TestCodeConstantLeft = @"
			static class Test
				{
					static unsafe bool AddPtr(int a, int expect)
					{
						return expect == (int)(DoAddPtr((#type)a));
					}

					static unsafe #type DoAddPtr(#type a)
					{
						return ((#constantType)#constantValue) + a;
					}
				}";

		private static string CreateConstantRightTestCode(string type, string constantType, string constantValue)
		{
			return TestCodeConstantRight
				.Replace("#type", type)
				.Replace("#constantType", constantType)
				.Replace("#constantValue", constantValue)
				+ Code.AllTestCode;
		}

		private static string CreateConstantLeftTestCode(string type, string constantType, string constantValue)
		{
			return TestCodeConstantLeft
				.Replace("#type", type)
				.Replace("#constantType", constantType)
				.Replace("#constantValue", constantValue)
				+ Code.AllTestCode;
		}

		#region C

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddCPtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("char*", "int", "char*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((char*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddCPtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("char*", "long", "char*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((char*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddCPtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("int", "char*", "char*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((char*)a + (int)b)));
		}

		//[Row(0, 42)]
		//[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddCPtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "char*", "char*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((char*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantCPtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("char*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((char*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantCPtrI8Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("char*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((char*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantCPtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("char*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((char*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantCPtrI8Right(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("char*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((char*)a + (long)b)));
		}
		#endregion

		#region U1

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddU1PtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("byte*", "int", "byte*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddU1PtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("byte*", "long", "byte*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((byte*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddU1PtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("int", "byte*", "byte*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddU1PtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "byte*", "byte*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantU1PtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("byte*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantU1PtrI8Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("byte*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((byte*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantU1PtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("byte*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantU1PtrI8Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("byte*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((byte*)a + (long)b)));
		}

		#endregion

		#region I4

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI4PtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("int*", "int", "int*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((int*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI4PtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("int*", "long", "int*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((int*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI4PtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("int", "int*", "int*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((int*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI4PtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "int*", "int*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((int*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI4PtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("int*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((int*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI4PtrI8Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("int*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((int*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI4PtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("int*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((int*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI4PtrI8Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("int*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((int*)a + (long)b)));
		}
		#endregion

		#region I8

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI8PtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("long*", "int", "long*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((long*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI8PtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("long*", "long", "long*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((long*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI8PtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("int", "long*", "long*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((long*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI8PtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "long*", "long*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((long*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI8PtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("long*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((long*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI8PtrI8Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("long*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((long*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI8PtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("long*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((long*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI8PtrI8Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("long*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((long*)a + (long)b)));
		}
		#endregion

		#region R8

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddR8PtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("double*", "int", "double*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((double*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddR8PtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("double*", "long", "double*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, b, (int)((double*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddR8PtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "double*", "double*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((double*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddR8PtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "double*", "double*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, (int)((double*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantR8PtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("double*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((double*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantR8PtrI8Left(int a, int b)
		{
			CodeSource = CreateConstantLeftTestCode("double*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((double*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantR8PtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("double*", "int", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((double*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantR8PtrI8Right(int a, int b)
		{
			CodeSource = CreateConstantRightTestCode("double*", "long", b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, (int)((double*)a + (long)b)));
		}

		#endregion
	}
}
