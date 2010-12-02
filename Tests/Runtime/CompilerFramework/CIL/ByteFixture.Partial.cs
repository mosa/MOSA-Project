/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

/* DO NOT MODIFY THIS FILE COMPUTER GENERATED CODE. */

using System;

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.CLI
{
	public partial class ByteFixture
	{
		#region Add
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void AddByteByte(byte a, byte b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}
		
		#endregion // Add
		
		#region Sub
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void SubByteByte(byte a, byte b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}
		
		#endregion // Sub
		
		#region Mul
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void MulByteByte(byte a, byte b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}
		
		#endregion // Mul
		
		#region Div
		
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void DivByteByte(byte a, byte b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}
		
		#endregion // Div
		
		#region Rem
		
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void RemByteByte(byte a, byte b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}
		
		#endregion // Rem
		
		#region Ret
		
		[Row(0)]
		[Row(1)]
		[Row(byte.MaxValue)]
		[Row(byte.MaxValue - 1)]
		[Test]
		public void RetByte(byte value)
		{
			this.arithmeticTests.Ret(value);
		}
		
		#endregion // Ret
		
		#region And
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void AndByteByte(byte first, byte second)
		{
			this.logicTests.And((first & second), first, second);
		}
		
		#endregion // And
		
		#region Or
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void OrByteByte(byte first, byte second)
		{
			this.logicTests.Or((first | second), first, second);
		}
		
		#endregion // Or
		
		#region Xor
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void XorByteByte(byte first, byte second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}
		
		#endregion // Xor
		
		#region Shl
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, 3)]
		[Row(0, 4)]
		[Row(0, 5)]
		[Row(0, 6)]
		[Row(0, 7)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, 3)]
		[Row(1, 4)]
		[Row(1, 5)]
		[Row(1, 6)]
		[Row(1, 7)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, 3)]
		[Row(byte.MaxValue, 4)]
		[Row(byte.MaxValue, 5)]
		[Row(byte.MaxValue, 6)]
		[Row(byte.MaxValue, 7)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, 3)]
		[Row(byte.MaxValue - 1, 4)]
		[Row(byte.MaxValue - 1, 5)]
		[Row(byte.MaxValue - 1, 6)]
		[Row(byte.MaxValue - 1, 7)]
		[Test]
		public void ShlByteByte(byte first, byte second)
		{
			this.logicTests.Shl((first << second), first, second);
		}
		
		#endregion // Shl
		
		#region Shr
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, 3)]
		[Row(0, 4)]
		[Row(0, 5)]
		[Row(0, 6)]
		[Row(0, 7)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, 3)]
		[Row(1, 4)]
		[Row(1, 5)]
		[Row(1, 6)]
		[Row(1, 7)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, 3)]
		[Row(byte.MaxValue, 4)]
		[Row(byte.MaxValue, 5)]
		[Row(byte.MaxValue, 6)]
		[Row(byte.MaxValue, 7)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, 3)]
		[Row(byte.MaxValue - 1, 4)]
		[Row(byte.MaxValue - 1, 5)]
		[Row(byte.MaxValue - 1, 6)]
		[Row(byte.MaxValue - 1, 7)]
		[Test]
		public void ShrByteByte(byte first, byte second)
		{
			this.logicTests.Shr((first >> second), first, second);
		}
		
		#endregion // Shr
		
		#region Ceq
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void CeqByteByte(byte first, byte second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}
		
		#endregion // Ceq
		
		#region Cgt
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void CgtByteByte(byte first, byte second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}
		
		#endregion // Cgt
		
		#region Clt
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void CltByteByte(byte first, byte second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}
		
		#endregion // Clt
		
		#region Cge
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void CgeByteByte(byte first, byte second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}
		
		#endregion // Cge
		
		#region Cle
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Test]
		public void CleByteByte(byte first, byte second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}
		
		#endregion // Cle
		
		#region Newarr
		
		[Test]
		public void NewarrByte()
		{
			this.arrayTests.Newarr();
		}
		
		#endregion // Newarr
		
		#region Ldlen
		
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(4)]
		[Row(7)]
		[Row(8)]
		[Row(10)]
		[Test]
		public void LdlenByte(int length)
		{
			this.arrayTests.Ldlen(length);
		}
		
		#endregion // Ldlen
		
		#region Stelem
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, byte.MaxValue)]
		[Row(4, byte.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, byte.MaxValue)]
		[Row(7, byte.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, byte.MaxValue)]
		[Row(8, byte.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, byte.MaxValue)]
		[Row(10, byte.MaxValue - 1)]
		[Test]
		public void StelemByte(int index, byte value)
		{
			this.arrayTests.Stelem(index, value);
		}
		
		#endregion // Stelem
		
		#region Ldelem
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, byte.MaxValue)]
		[Row(4, byte.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, byte.MaxValue)]
		[Row(7, byte.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, byte.MaxValue)]
		[Row(8, byte.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, byte.MaxValue)]
		[Row(10, byte.MaxValue - 1)]
		[Test]
		public void LdelemByte(int index, byte value)
		{
			this.arrayTests.Ldelem(index, value);
		}
		
		#endregion // Ldelem
		
		#region Ldelema
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, byte.MaxValue)]
		[Row(4, byte.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, byte.MaxValue)]
		[Row(7, byte.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, byte.MaxValue)]
		[Row(8, byte.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, byte.MaxValue)]
		[Row(10, byte.MaxValue - 1)]
		[Test]
		public void LdelemaByte(int index, byte value)
		{
			this.arrayTests.Ldelema(index, value);
		}
		
		#endregion // Ldelema
		
	}
}
