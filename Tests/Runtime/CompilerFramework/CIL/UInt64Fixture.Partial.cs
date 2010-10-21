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
	public partial class UInt64Fixture
	{
		#region Add
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void AddUlongUlong(ulong a, ulong b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}
		
		#endregion // Add
		
		#region Sub
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void SubUlongUlong(ulong a, ulong b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}
		
		#endregion // Sub
		
		#region Mul
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void MulUlongUlong(ulong a, ulong b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}
		
		#endregion // Mul
		
		#region Div
		
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void DivUlongUlong(ulong a, ulong b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}
		
		#endregion // Div
		
		#region Rem
		
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt64.MaxValue - 1, 1)]
		//[Row(UInt64.MaxValue - 1, UInt64.MaxValue)] // Crashes test runner
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void RemUlongUlong(ulong a, ulong b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}
		
		#endregion // Rem
		
		#region Ret
		
		[Row(0)]
		[Row(1)]
		[Row(UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void RetUlong(ulong value)
		{
			this.arithmeticTests.Ret(value);
		}
		
		#endregion // Ret
		
		#region And
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void AndUlongUlong(ulong first, ulong second)
		{
			this.logicTests.And((first & second), first, second);
		}
		
		#endregion // And
		
		#region Or
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void OrUlongUlong(ulong first, ulong second)
		{
			this.logicTests.Or((first | second), first, second);
		}
		
		#endregion // Or
		
		#region Xor
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void XorUlongUlong(ulong first, ulong second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}
		
		#endregion // Xor
		
		#region Comp
		
		[Row(0)]
		[Row(1)]
		[Row(UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void BitwiseNotUlong(ulong first)
		{
			this.logicTests.Comp(~first, first);
		}
		
		#endregion // Comp
		
		#region Ceq
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CeqUlongUlong(ulong first, ulong second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}
		
		#endregion // Ceq
		
		#region Cgt
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CgtUlongUlong(ulong first, ulong second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}
		
		#endregion // Cgt
		
		#region Clt
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CltUlongUlong(ulong first, ulong second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}
		
		#endregion // Clt
		
		#region Cge
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CgeUlongUlong(ulong first, ulong second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}
		
		#endregion // Cge
		
		#region Cle
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(UInt64.MaxValue, 1)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(UInt64.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue, UInt32.MaxValue)]
		[Row(UInt64.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, 0)]
		[Row(UInt64.MaxValue - 1, 1)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt64.MaxValue - 1, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(UInt32.MaxValue, 1)]
		[Row(UInt32.MaxValue, UInt64.MaxValue)]
		[Row(UInt32.MaxValue, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(UInt32.MaxValue, UInt32.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, 0)]
		[Row(UInt32.MaxValue - 1, 1)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt64.MaxValue - 1)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue)]
		[Row(UInt32.MaxValue - 1, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CleUlongUlong(ulong first, ulong second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}
		
		#endregion // Cle
		
		#region Newarr
		
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void NewarrUlong()
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
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void LdlenUlong(int length)
		{
			this.arrayTests.Ldlen(length);
		}
		
		#endregion // Ldlen
		
		#region Stelem
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, UInt64.MaxValue)]
		[Row(2, UInt64.MaxValue - 1)]
		[Row(2, UInt32.MaxValue)]
		[Row(2, UInt32.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, UInt64.MaxValue)]
		[Row(4, UInt64.MaxValue - 1)]
		[Row(4, UInt32.MaxValue)]
		[Row(4, UInt32.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, UInt64.MaxValue)]
		[Row(7, UInt64.MaxValue - 1)]
		[Row(7, UInt32.MaxValue)]
		[Row(7, UInt32.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, UInt64.MaxValue)]
		[Row(8, UInt64.MaxValue - 1)]
		[Row(8, UInt32.MaxValue)]
		[Row(8, UInt32.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, UInt64.MaxValue)]
		[Row(10, UInt64.MaxValue - 1)]
		[Row(10, UInt32.MaxValue)]
		[Row(10, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void StelemUlong(int index, ulong value)
		{
			this.arrayTests.Stelem(index, value);
		}
		
		#endregion // Stelem
		
		#region Ldelem
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, UInt64.MaxValue)]
		[Row(2, UInt64.MaxValue - 1)]
		[Row(2, UInt32.MaxValue)]
		[Row(2, UInt32.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, UInt64.MaxValue)]
		[Row(4, UInt64.MaxValue - 1)]
		[Row(4, UInt32.MaxValue)]
		[Row(4, UInt32.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, UInt64.MaxValue)]
		[Row(7, UInt64.MaxValue - 1)]
		[Row(7, UInt32.MaxValue)]
		[Row(7, UInt32.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, UInt64.MaxValue)]
		[Row(8, UInt64.MaxValue - 1)]
		[Row(8, UInt32.MaxValue)]
		[Row(8, UInt32.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, UInt64.MaxValue)]
		[Row(10, UInt64.MaxValue - 1)]
		[Row(10, UInt32.MaxValue)]
		[Row(10, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void LdelemUlong(int index, ulong value)
		{
			this.arrayTests.Ldelem(index, value);
		}
		
		#endregion // Ldelem
		
		#region Ldelema
		
		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, UInt64.MaxValue - 1)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, UInt32.MaxValue - 1)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, UInt64.MaxValue)]
		[Row(1, UInt64.MaxValue - 1)]
		[Row(1, UInt32.MaxValue)]
		[Row(1, UInt32.MaxValue - 1)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, UInt64.MaxValue)]
		[Row(2, UInt64.MaxValue - 1)]
		[Row(2, UInt32.MaxValue)]
		[Row(2, UInt32.MaxValue - 1)]
		[Row(4, 0)]
		[Row(4, 1)]
		[Row(4, UInt64.MaxValue)]
		[Row(4, UInt64.MaxValue - 1)]
		[Row(4, UInt32.MaxValue)]
		[Row(4, UInt32.MaxValue - 1)]
		[Row(7, 0)]
		[Row(7, 1)]
		[Row(7, UInt64.MaxValue)]
		[Row(7, UInt64.MaxValue - 1)]
		[Row(7, UInt32.MaxValue)]
		[Row(7, UInt32.MaxValue - 1)]
		[Row(8, 0)]
		[Row(8, 1)]
		[Row(8, UInt64.MaxValue)]
		[Row(8, UInt64.MaxValue - 1)]
		[Row(8, UInt32.MaxValue)]
		[Row(8, UInt32.MaxValue - 1)]
		[Row(10, 0)]
		[Row(10, 1)]
		[Row(10, UInt64.MaxValue)]
		[Row(10, UInt64.MaxValue - 1)]
		[Row(10, UInt32.MaxValue)]
		[Row(10, UInt32.MaxValue - 1)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void LdelemaUlong(int index, ulong value)
		{
			this.arrayTests.Ldelema(index, value);
		}
		
		#endregion // Ldelema
		
	}
}
