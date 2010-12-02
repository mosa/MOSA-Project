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
	public partial class UInt16Fixture
	{
		#region Add
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void AddUshortUshort(ushort a, ushort b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}
		
		#endregion // Add
		
		#region Sub
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void SubUshortUshort(ushort a, ushort b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}
		
		#endregion // Sub
		
		#region Mul
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void MulUshortUshort(ushort a, ushort b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}
		
		#endregion // Mul
		
		#region Div
		
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void DivUshortUshort(ushort a, ushort b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}
		
		#endregion // Div
		
		#region Rem
		
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void RemUshortUshort(ushort a, ushort b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}
		
		#endregion // Rem
		
		#region Ret
		
		[Row(0)]
		[Row(1)]
		[Row(UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1)]
		[Test]
		public void RetUshort(ushort value)
		{
			this.arithmeticTests.Ret(value);
		}
		
		#endregion // Ret
		
		#region And
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void AndUshortUshort(ushort first, ushort second)
		{
			this.logicTests.And((first & second), first, second);
		}
		
		#endregion // And
		
		#region Or
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void OrUshortUshort(ushort first, ushort second)
		{
			this.logicTests.Or((first | second), first, second);
		}
		
		#endregion // Or
		
		#region Xor
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void XorUshortUshort(ushort first, ushort second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}
		
		#endregion // Xor
		
		#region Ceq
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void CeqUshortUshort(ushort first, ushort second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}
		
		#endregion // Ceq
		
		#region Cgt
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void CgtUshortUshort(ushort first, ushort second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}
		
		#endregion // Cgt
		
		#region Clt
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void CltUshortUshort(ushort first, ushort second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}
		
		#endregion // Clt
		
		#region Cge
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void CgeUshortUshort(ushort first, ushort second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}
		
		#endregion // Cge
		
		#region Cle
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Test]
		public void CleUshortUshort(ushort first, ushort second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}
		
		#endregion // Cle
		
		#region Newarr
		
		[Test]
		public void NewarrUshort()
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
		public void LdlenUshort(int length)
		{
			this.arrayTests.Ldlen(length);
		}
		
		#endregion // Ldlen
		
		#region Stelem
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, UInt16.MaxValue)]
		[Row(4, UInt16.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, UInt16.MaxValue)]
		[Row(7, UInt16.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, UInt16.MaxValue)]
		[Row(8, UInt16.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, UInt16.MaxValue)]
		[Row(10, UInt16.MaxValue - 1)]
		[Test]
		public void StelemUshort(int index, ushort value)
		{
			this.arrayTests.Stelem(index, value);
		}
		
		#endregion // Stelem
		
		#region Ldelem
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, UInt16.MaxValue)]
		[Row(4, UInt16.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, UInt16.MaxValue)]
		[Row(7, UInt16.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, UInt16.MaxValue)]
		[Row(8, UInt16.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, UInt16.MaxValue)]
		[Row(10, UInt16.MaxValue - 1)]
		[Test]
		public void LdelemUshort(int index, ushort value)
		{
			this.arrayTests.Ldelem(index, value);
		}
		
		#endregion // Ldelem
		
		#region Ldelema
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, UInt16.MaxValue)]
		[Row(4, UInt16.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, UInt16.MaxValue)]
		[Row(7, UInt16.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, UInt16.MaxValue)]
		[Row(8, UInt16.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, UInt16.MaxValue)]
		[Row(10, UInt16.MaxValue - 1)]
		[Test]
		public void LdelemaUshort(int index, ushort value)
		{
			this.arrayTests.Ldelema(index, value);
		}
		
		#endregion // Ldelema
		
	}
}
