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
using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Runtime.CompilerFramework.IL
{

	[TestFixture]
	public class AddPtr : TestCompilerAdapter
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
			compiler.CodeSource = CreateTestCode("char*", "int", "char*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((char*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddCPtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("char*", "long", "char*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((char*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddCPtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("int", "char*", "char*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((char*)a + (int)b)));
		}

		//[Row(0, 42)]
		//[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddCPtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("long", "char*", "char*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((char*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantCPtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("char*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((char*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantCPtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("char*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((char*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantCPtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("char*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((char*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantCPtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("char*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((char*)a + (long)b)));
		}
		#endregion

		#region U1

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddU1PtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("byte*", "int", "byte*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddU1PtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("byte*", "long", "byte*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((byte*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddU1PtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("int", "byte*", "byte*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddU1PtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("long", "byte*", "byte*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantU1PtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("byte*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantU1PtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("byte*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((byte*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantU1PtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("byte*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((byte*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantU1PtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("byte*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((byte*)a + (long)b)));
		}

		#endregion

		#region I4

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI4PtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("int*", "int", "int*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((int*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI4PtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("int*", "long", "int*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((int*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI4PtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("int", "int*", "int*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((int*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI4PtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("long", "int*", "int*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((int*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI4PtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("int*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((int*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI4PtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("int*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((int*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI4PtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("int*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((int*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI4PtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("int*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((int*)a + (long)b)));
		}
		#endregion

		#region I8

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI8PtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("long*", "int", "long*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((long*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI8PtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("long*", "long", "long*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((long*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI8PtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("int", "long*", "long*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((long*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddI8PtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("long", "long*", "long*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((long*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI8PtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("long*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((long*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI8PtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("long*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((long*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI8PtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("long*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((long*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantI8PtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("long*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((long*)a + (long)b)));
		}
		#endregion

		#region R8

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddR8PtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("double*", "int", "double*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((double*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddR8PtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("double*", "long", "double*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, b, (int)((double*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddR8PtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("long", "double*", "double*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((double*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddR8PtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("long", "double*", "double*");
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", b, a, (int)((double*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantR8PtrI4Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("double*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((double*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantR8PtrI8Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantLeftTestCode("double*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((double*)a + (long)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantR8PtrI4Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("double*", "int", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((double*)a + (int)b)));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test]
		public unsafe void AddConstantR8PtrI8Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantRightTestCode("double*", "long", b.ToString());
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddPtr", a, (int)((double*)a + (long)b)));
		}

		#endregion
	}
}
