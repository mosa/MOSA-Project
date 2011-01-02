/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 *
 */
 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;
using Mosa.Test.Runtime.CompilerFramework.Numbers;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.FIX.IL
{
	[TestFixture]
	public class ConditionalOperator : TestCompilerAdapter
	{
		public ConditionalOperator()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
				
		[Test, Factory(typeof(Variations), "U1_U1_U1_U1")]
		public void CompareEqualU1(byte a, byte b, byte c, byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualU1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U1_U1_U1_U1")]
		public void CompareNotEqualU1(byte a, byte b, byte c, byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualU1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U1_U1_U1_U1")]
		public void CompareGreaterThanU1(byte a, byte b, byte c, byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanU1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U1_U1_U1_U1")]
		public void CompareLessThanU1(byte a, byte b, byte c, byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanU1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U1_U1_U1_U1")]
		public void CompareGreaterThanOrEqualU1(byte a, byte b, byte c, byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualU1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U1_U1_U1_U1")]
		public void CompareLessThanOrEqualU1(byte a, byte b, byte c, byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualU1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U2_U2_U2_U2")]
		public void CompareEqualU2(ushort a, ushort b, ushort c, ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualU2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U2_U2_U2_U2")]
		public void CompareNotEqualU2(ushort a, ushort b, ushort c, ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualU2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U2_U2_U2_U2")]
		public void CompareGreaterThanU2(ushort a, ushort b, ushort c, ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanU2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U2_U2_U2_U2")]
		public void CompareLessThanU2(ushort a, ushort b, ushort c, ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanU2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U2_U2_U2_U2")]
		public void CompareGreaterThanOrEqualU2(ushort a, ushort b, ushort c, ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualU2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U2_U2_U2_U2")]
		public void CompareLessThanOrEqualU2(ushort a, ushort b, ushort c, ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualU2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U4_U4_U4_U4")]
		public void CompareEqualU4(uint a, uint b, uint c, uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualU4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U4_U4_U4_U4")]
		public void CompareNotEqualU4(uint a, uint b, uint c, uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualU4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U4_U4_U4_U4")]
		public void CompareGreaterThanU4(uint a, uint b, uint c, uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanU4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U4_U4_U4_U4")]
		public void CompareLessThanU4(uint a, uint b, uint c, uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanU4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U4_U4_U4_U4")]
		public void CompareGreaterThanOrEqualU4(uint a, uint b, uint c, uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualU4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U4_U4_U4_U4")]
		public void CompareLessThanOrEqualU4(uint a, uint b, uint c, uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualU4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U8_U8_U8_U8")]
		public void CompareEqualU8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualU8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U8_U8_U8_U8")]
		public void CompareNotEqualU8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualU8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U8_U8_U8_U8")]
		public void CompareGreaterThanU8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanU8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U8_U8_U8_U8")]
		public void CompareLessThanU8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanU8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U8_U8_U8_U8")]
		public void CompareGreaterThanOrEqualU8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualU8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "U8_U8_U8_U8")]
		public void CompareLessThanOrEqualU8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualU8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I1_I1_I1_I1")]
		public void CompareEqualI1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualI1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I1_I1_I1_I1")]
		public void CompareNotEqualI1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualI1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I1_I1_I1_I1")]
		public void CompareGreaterThanI1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanI1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I1_I1_I1_I1")]
		public void CompareLessThanI1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanI1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I1_I1_I1_I1")]
		public void CompareGreaterThanOrEqualI1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualI1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I1_I1_I1_I1")]
		public void CompareLessThanOrEqualI1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualI1", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I2_I2_I2_I2")]
		public void CompareEqualI2(short a, short b, short c, short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualI2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I2_I2_I2_I2")]
		public void CompareNotEqualI2(short a, short b, short c, short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualI2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I2_I2_I2_I2")]
		public void CompareGreaterThanI2(short a, short b, short c, short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanI2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I2_I2_I2_I2")]
		public void CompareLessThanI2(short a, short b, short c, short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanI2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I2_I2_I2_I2")]
		public void CompareGreaterThanOrEqualI2(short a, short b, short c, short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualI2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I2_I2_I2_I2")]
		public void CompareLessThanOrEqualI2(short a, short b, short c, short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualI2", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I4_I4_I4_I4")]
		public void CompareEqualI4(int a, int b, int c, int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualI4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I4_I4_I4_I4")]
		public void CompareNotEqualI4(int a, int b, int c, int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualI4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I4_I4_I4_I4")]
		public void CompareGreaterThanI4(int a, int b, int c, int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanI4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I4_I4_I4_I4")]
		public void CompareLessThanI4(int a, int b, int c, int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanI4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I4_I4_I4_I4")]
		public void CompareGreaterThanOrEqualI4(int a, int b, int c, int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualI4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I4_I4_I4_I4")]
		public void CompareLessThanOrEqualI4(int a, int b, int c, int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualI4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I8_I8_I8_I8")]
		public void CompareEqualI8(long a, long b, long c, long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualI8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I8_I8_I8_I8")]
		public void CompareNotEqualI8(long a, long b, long c, long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualI8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I8_I8_I8_I8")]
		public void CompareGreaterThanI8(long a, long b, long c, long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanI8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I8_I8_I8_I8")]
		public void CompareLessThanI8(long a, long b, long c, long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanI8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I8_I8_I8_I8")]
		public void CompareGreaterThanOrEqualI8(long a, long b, long c, long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualI8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "I8_I8_I8_I8")]
		public void CompareLessThanOrEqualI8(long a, long b, long c, long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualI8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R4_R4_R4_R4")]
		public void CompareEqualR4(float a, float b, float c, float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualR4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R4_R4_R4_R4")]
		public void CompareNotEqualR4(float a, float b, float c, float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualR4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R4_R4_R4_R4")]
		public void CompareGreaterThanR4(float a, float b, float c, float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanR4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R4_R4_R4_R4")]
		public void CompareLessThanR4(float a, float b, float c, float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanR4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R4_R4_R4_R4")]
		public void CompareGreaterThanOrEqualR4(float a, float b, float c, float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualR4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R4_R4_R4_R4")]
		public void CompareLessThanOrEqualR4(float a, float b, float c, float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualR4", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R8_R8_R8_R8")]
		public void CompareEqualR8(double a, double b, double c, double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualR8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R8_R8_R8_R8")]
		public void CompareNotEqualR8(double a, double b, double c, double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualR8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R8_R8_R8_R8")]
		public void CompareGreaterThanR8(double a, double b, double c, double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanR8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R8_R8_R8_R8")]
		public void CompareLessThanR8(double a, double b, double c, double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanR8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R8_R8_R8_R8")]
		public void CompareGreaterThanOrEqualR8(double a, double b, double c, double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualR8", a, b, c, d));
		}
				
		[Test, Factory(typeof(Variations), "R8_R8_R8_R8")]
		public void CompareLessThanOrEqualR8(double a, double b, double c, double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualR8", a, b, c, d));
		}
		
	}
}
