/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *
 */


using System;
using MbUnit.Framework;

namespace Mosa.Test.Runtime.CompilerFramework
{
	[TestFixture]
	public class GenericTypeFixture : TestCompilerAdapter
	{
		private static string CreateTestCode(string type)
		{
			return @"
				class GenericType<T>
				{
					public static T StaticMethodInGenericType(T value)
					{
						return value;
					}
				}

				static class Test
				{
					public static bool TestCallStaticMethodInGenericTypeWith(" + type + @" value)
					{
						return value == GenericType<" + type + @">.StaticMethodInGenericType(value);
					}
				}" + Code.AllTestCode;
		}

		[Test]
		[Row(true)]
		[Row(false)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithB(bool value)
		{
			compiler.CodeSource = CreateTestCode("bool");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row(0)]
		[Row(Char.MaxValue)]
		[Row(Char.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithC(char value)
		{
			compiler.CodeSource = CreateTestCode("char");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Row(0)]
		[Row(Int32.MaxValue)]
		[Row(Int32.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithI(int value)
		{
			compiler.CodeSource = CreateTestCode("IntPtr");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", new IntPtr(value)));
		}

		[Test]
		[Row((sbyte)0)]
		[Row(SByte.MaxValue)]
		[Row(SByte.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithI1(sbyte value)
		{
			compiler.CodeSource = CreateTestCode("sbyte");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row((short)0)]
		[Row(Int16.MaxValue)]
		[Row(Int16.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithI2(short value)
		{
			compiler.CodeSource = CreateTestCode("short");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row(0)]
		[Row(5)]
		[Row(Int32.MaxValue)]
		[Row(Int32.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithI4(int value)
		{
			compiler.CodeSource = CreateTestCode("int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row(0L)]
		[Row(Int64.MaxValue)]
		[Row(Int64.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithI8(long value)
		{
			compiler.CodeSource = CreateTestCode("long");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Row(0U)]
		[Row(UInt32.MaxValue)]
		[Row(UInt32.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithU(uint value)
		{
			compiler.CodeSource = CreateTestCode("UIntPtr");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", new UIntPtr(value)));
		}

		[Test]
		[Row((byte)0)]
		[Row(Byte.MaxValue)]
		[Row(Byte.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithU1(byte value)
		{
			compiler.CodeSource = CreateTestCode("byte");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row((ushort)0U)]
		[Row(UInt16.MaxValue)]
		[Row(UInt16.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithU2(ushort value)
		{
			compiler.CodeSource = CreateTestCode("ushort");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row(0U)]
		[Row(UInt32.MaxValue)]
		[Row(UInt32.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithU4(uint value)
		{
			compiler.CodeSource = CreateTestCode("uint");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row(0UL)]
		[Row(UInt64.MaxValue)]
		[Row(UInt64.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithU8(ulong value)
		{
			compiler.CodeSource = CreateTestCode("ulong");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row(0f)]
		[Row(Single.MaxValue)]
		[Row(Single.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithR4(float value)
		{
			compiler.CodeSource = CreateTestCode("float");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row(0.0)]
		[Row(Double.MaxValue)]
		[Row(Double.MinValue)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithR8(double value)
		{
			compiler.CodeSource = CreateTestCode("double");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}

		[Test]
		[Row(null)]
		public void MustMakeSuccessfulCallToStaticMethodInGenericTypeWithO(object value)
		{
			compiler.CodeSource = CreateTestCode("object");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "TestCallStaticMethodInGenericTypeWith", value));
		}
	}
}
