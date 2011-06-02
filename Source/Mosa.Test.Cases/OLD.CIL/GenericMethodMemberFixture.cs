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

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD.CIL
{
	[TestFixture]
	public class GenericMethodMemberFixture : TestCompilerAdapter
	{
		private static string CreateTestCode(string type)
		{
			return @"
				using System;

				static class Test
				{
					private static T GenericMethod<T>(T value)
					{
						return value;
					}

					public static bool TestCallGenericMethodWith(" + type + @" value)
					{
						return value == GenericMethod(value);
					}
				}";
		}
		
		[Test]
		[Row(true)]
		[Row(false)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithB(bool value)
		{
			settings.CodeSource = CreateTestCode("bool");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0)]
		[Row(Char.MaxValue)]
		[Row(Char.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithC(char value)
		{
			settings.CodeSource = CreateTestCode("char");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Row(0)]
		[Row(Int32.MaxValue)]
		[Row(Int32.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI(int value)
		{
			settings.CodeSource = CreateTestCode("IntPtr");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", new IntPtr(value)));
		}

		[Test]
		[Row((sbyte)0)]
		[Row(SByte.MaxValue)]
		[Row(SByte.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI1(sbyte value)
		{
			settings.CodeSource = CreateTestCode("sbyte");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row((short)0)]
		[Row(Int16.MaxValue)]
		[Row(Int16.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI2(short value)
		{
			settings.CodeSource = CreateTestCode("short");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0)]
		[Row(5)]
		[Row(Int32.MaxValue)]
		[Row(Int32.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI4(int value)
		{
			settings.CodeSource = CreateTestCode("int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0L)]
		[Row(Int64.MaxValue)]
		[Row(Int64.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI8(long value)
		{
			settings.CodeSource = CreateTestCode("long");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Row(0U)]
		[Row(UInt32.MaxValue)]
		[Row(UInt32.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU(uint value)
		{
			settings.CodeSource = CreateTestCode("UIntPtr");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", new UIntPtr(value)));
		}

		[Test]
		[Row((byte)0)]
		[Row(Byte.MaxValue)]
		[Row(Byte.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU1(byte value)
		{
			settings.CodeSource = CreateTestCode("byte");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row((ushort)0U)]
		[Row(UInt16.MaxValue)]
		[Row(UInt16.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU2(ushort value)
		{
			settings.CodeSource = CreateTestCode("ushort");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0U)]
		[Row(UInt32.MaxValue)]
		[Row(UInt32.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU4(uint value)
		{
			settings.CodeSource = CreateTestCode("uint");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0UL)]
		[Row(UInt64.MaxValue)]
		[Row(UInt64.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU8(ulong value)
		{
			settings.CodeSource = CreateTestCode("ulong");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0f)]
		[Row(Single.MaxValue)]
		[Row(Single.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithR4(float value)
		{
			settings.CodeSource = CreateTestCode("float");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0.0)]
		[Row(Double.MaxValue)]
		[Row(Double.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithR8(double value)
		{
			settings.CodeSource = CreateTestCode("double");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(null)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithO(object value)
		{
			settings.CodeSource = CreateTestCode("object");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "TestCallGenericMethodWith", value));
		}
	}
}
