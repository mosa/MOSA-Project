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
						return (a + #value);
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
						return (#value + a);
					}
				}";

		private static string CreateConstantTestCode(string type, string constantLeft, string constantRight)
		{
			if (constantLeft == null)
			{
				return TestCodeConstantRight
					.Replace("#type", type)
					.Replace("#value", constantRight)
					+ Code.AllTestCode;
			}
			else if (constantRight == null)
			{
				return TestCodeConstantLeft
					.Replace("#type", type)
					.Replace("#value", constantLeft)
					+ Code.AllTestCode;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region C

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddCPtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("char*", "int", "char*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddCPtrI8Left(int a, long b)
		{
			CodeSource = CreateTestCode("char*", "long", "char*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddCPtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("int", "char*", "char*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		//[Row(0, 42)]
		//[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddCPtrI8Right(int a, long b)
		{
			CodeSource = CreateTestCode("long", "char*", "char*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantCPtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("char*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantCPtrI8Left(int a, long b)
		{
			CodeSource = CreateConstantTestCode("char*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantCPtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("char*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantCPtrI8Right(int a, long b)
		{
			CodeSource = CreateConstantTestCode("char*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}
		#endregion

		#region U1

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddU1PtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("byte*", "int", "byte*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddU1PtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("byte*", "long", "byte*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddU1PtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("int", "byte*", "byte*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddU1PtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "byte*", "byte*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantU1PtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("byte*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantU1PtrI8Left(int a, long b)
		{
			CodeSource = CreateConstantTestCode("byte*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantU1PtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("byte*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantU1PtrI8Right(int a, long b)
		{
			CodeSource = CreateConstantTestCode("byte*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}
		#endregion

		#region I4

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI4PtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("int*", "int", "int*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI4PtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("int*", "long", "int*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI4PtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("int", "int*", "int*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI4PtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "int*", "int*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI4PtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("int*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI4PtrI8Left(int a, long b)
		{
			CodeSource = CreateConstantTestCode("int*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI4PtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("int*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI4PtrI8Right(int a, long b)
		{
			CodeSource = CreateConstantTestCode("int*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}
		#endregion

		#region I8

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI8PtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("long*", "int", "long*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI8PtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("long*", "long", "long*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI8PtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("int", "long*", "long*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddI8PtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "long*", "long*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI8PtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("long*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI8PtrI8Left(int a, long b)
		{
			CodeSource = CreateConstantTestCode("long*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI8PtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("long*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantI8PtrI8Right(int a, long b)
		{
			CodeSource = CreateConstantTestCode("long*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}
		#endregion

		#region R8

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddR8PtrI4Left(int a, int b)
		{
			CodeSource = CreateTestCode("double*", "int", "double*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddR8PtrI8Left(int a, int b)
		{
			CodeSource = CreateTestCode("double*", "long", "double*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddR8PtrI4Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "double*", "double*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddR8PtrI8Right(int a, int b)
		{
			CodeSource = CreateTestCode("long", "double*", "double*");
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", b, a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantR8PtrI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("double*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantR8PtrI8Left(int a, long b)
		{
			CodeSource = CreateConstantTestCode("double*", null, b.ToString());
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantR8PtrI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("double*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		[Row(0, 42)]
		[Row(int.MaxValue, 42)]
		[Row(42, 0)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public unsafe void AddConstantR8PtrI8Right(int a, long b)
		{
			CodeSource = CreateConstantTestCode("double*", b.ToString(), null);
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "AddPtr", a, a + b));
		}

		#endregion
	}
}
