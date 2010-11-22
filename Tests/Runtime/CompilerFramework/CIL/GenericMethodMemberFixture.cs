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

namespace Test.Mosa.Runtime.CompilerFramework
{
	[TestFixture]
	//[Parallelizable]
	public class GenericMethodMemberFixture : CodeDomTestRunner
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
				}" + Code.AllTestCode;
		}

		private delegate bool B_B(bool value);
		private delegate bool B_C(char value);

		private delegate bool B_I(IntPtr value);
		private delegate bool B_I1(sbyte value);
		private delegate bool B_I2(short value);
		private delegate bool B_I4(int value);
		private delegate bool B_I8(long value);

		private delegate bool B_U(byte value);
		private delegate bool B_U1(byte value);
		private delegate bool B_U2(ushort value);
		private delegate bool B_U4(uint value);
		private delegate bool B_U8(ulong value);

		private delegate bool B_R4(float value);
		private delegate bool B_R8(double value);

		private delegate bool B_O(object value);


		[Test]
		[Row(true)]
		[Row(false)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithB(bool value)
		{
			CodeSource = CreateTestCode("bool");
			Assert.IsTrue((bool)Run<B_B>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0)]
		[Row(Char.MaxValue)]
		[Row(Char.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithC(char value)
		{
			CodeSource = CreateTestCode("char");
			Assert.IsTrue((bool)Run<B_C>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Row(0)]
		[Row(Int32.MaxValue)]
		[Row(Int32.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI(int value)
		{
			CodeSource = CreateTestCode("IntPtr");
			Assert.IsTrue((bool)Run<B_I>("", "Test", "TestCallGenericMethodWith", new IntPtr(value)));
		}

		[Test]
		[Row((sbyte)0)]
		[Row(SByte.MaxValue)]
		[Row(SByte.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI1(sbyte value)
		{
			CodeSource = CreateTestCode("sbyte");
			Assert.IsTrue((bool)Run<B_I1>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row((short)0)]
		[Row(Int16.MaxValue)]
		[Row(Int16.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI2(short value)
		{
			CodeSource = CreateTestCode("short");
			Assert.IsTrue((bool)Run<B_I2>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0)]
		[Row(5)]
		[Row(Int32.MaxValue)]
		[Row(Int32.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI4(int value)
		{
			CodeSource = CreateTestCode("int");
			Assert.IsTrue((bool)Run<B_I4>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0L)]
		[Row(Int64.MaxValue)]
		[Row(Int64.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithI8(long value)
		{
			CodeSource = CreateTestCode("long");
			Assert.IsTrue((bool)Run<B_I8>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Row(0U)]
		[Row(UInt32.MaxValue)]
		[Row(UInt32.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU(uint value)
		{
			CodeSource = CreateTestCode("UIntPtr");
			Assert.IsTrue((bool)Run<B_U>("", "Test", "TestCallGenericMethodWith", new UIntPtr(value)));
		}

		[Test]
		[Row((byte)0)]
		[Row(Byte.MaxValue)]
		[Row(Byte.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU1(byte value)
		{
			CodeSource = CreateTestCode("byte");
			Assert.IsTrue((bool)Run<B_U1>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row((ushort)0U)]
		[Row(UInt16.MaxValue)]
		[Row(UInt16.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU2(ushort value)
		{
			CodeSource = CreateTestCode("ushort");
			Assert.IsTrue((bool)Run<B_U2>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0U)]
		[Row(UInt32.MaxValue)]
		[Row(UInt32.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU4(uint value)
		{
			CodeSource = CreateTestCode("uint");
			Assert.IsTrue((bool)Run<B_U4>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0UL)]
		[Row(UInt64.MaxValue)]
		[Row(UInt64.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithU8(ulong value)
		{
			CodeSource = CreateTestCode("ulong");
			Assert.IsTrue((bool)Run<B_U8>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0f)]
		[Row(Single.MaxValue)]
		[Row(Single.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithR4(float value)
		{
			CodeSource = CreateTestCode("float");
			Assert.IsTrue((bool)Run<B_R4>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(0.0)]
		[Row(Double.MaxValue)]
		[Row(Double.MinValue)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithR8(double value)
		{
			CodeSource = CreateTestCode("double");
			Assert.IsTrue((bool)Run<B_R8>("", "Test", "TestCallGenericMethodWith", value));
		}

		[Test]
		[Row(null)]
		public void MustMakeSuccessfulCallToStaticGenericMethodWithO(object value)
		{
			CodeSource = CreateTestCode("object");
			Assert.IsTrue((bool)Run<B_O>("", "Test", "TestCallGenericMethodWith", value));
		}
	}
}
