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

using Test.Mosa.Runtime.CompilerFramework.Numbers;

namespace Test.Mosa.Runtime.CompilerFramework.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.Single")]
	public class SingleFixture
	{
		private readonly FloatingArithmeticInstructionTestRunner<float, float> arithmeticTests = new FloatingArithmeticInstructionTestRunner<float, float>
		{
			ExpectedType = "float",
			FirstType = "float",
			SecondType = "float",
		};

		private readonly ComparisonInstructionTestRunner<float> comparisonTests = new ComparisonInstructionTestRunner<float>
		{
			FirstType = "float"
		};

		private readonly SZArrayInstructionTestRunner<float> arrayTests = new SZArrayInstructionTestRunner<float>
		{
			FirstType = "float"
		};

		//#region Add

		//[Test, Factory(typeof(Variations), "R4_R4")]
		//public void AddR4R4(float a, float b)
		//{
		//    this.arithmeticTests.Add((a + b), a, b);
		//}

		//#endregion // Add

		//#region Sub

		//[Test, Factory(typeof(Variations), "R4_R4")]
		//public void SubR4R4(float a, float b)
		//{
		//    this.arithmeticTests.Sub((a - b), a, b);
		//}

		//#endregion // Sub

		//#region Mul

		//[Test, Factory(typeof(Variations), "R4_R4")]
		//public void MulR4R4(float a, float b)
		//{
		//    this.arithmeticTests.Mul((a * b), a, b);
		//}

		//#endregion // Mul

		//#region Div

		//[Test, Factory(typeof(Variations), "R4_R4WithoutZero")]
		//public void DivR4R4(float a, float b)
		//{
		//    this.arithmeticTests.Div((a / b), a, b);
		//}

		////[Test, Factory(typeof(Variations), "R4_R4Zero")]
		////[ExpectedException(typeof(DivideByZeroException))]
		////public void DivR4R4DivideByZeroException(float a, float b)
		////{
		////    this.arithmeticTests.Div((a / b), a, b);
		////}

		//#endregion // Div

		//#region Rem

		//[Test, Factory(typeof(Variations), "R4_R4WithoutZero")]
		//public void RemR4R4(float a, float b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		////[Test, Factory(typeof(Variations), "R4_R4Zero")]
		////[ExpectedException(typeof(DivideByZeroException))]
		////public void RemR4R4DivideByZeroException(float a, float b)
		////{
		////    this.arithmeticTests.Rem((a % b), a, b);
		////}

		////[Test, Factory(typeof(Variations), "R4_R4BelowZero")]
		////[ExpectedException(typeof(OverflowException))]
		////public void RemR4R4OverflowException(float a, float b)
		////{
		////    this.arithmeticTests.Rem((a % b), a, b);
		////}

		//#endregion // Rem

		//#region Neg

		//[Test, Factory(typeof(R4), "Samples")]
		//public void NegR4(float first)
		//{
		//    this.arithmeticTests.Neg(-first, first);
		//}

		//#endregion // Neg

		//#region Ret

		//[Test, Factory(typeof(R4), "Samples")]
		//public void RetR4(float value)
		//{
		//    this.arithmeticTests.Ret(value);
		//}

		//#endregion // Ret

		#region Ceq

		[Test, Factory(typeof(Variations), "R4_R4")]
		public void CeqR4R4(float first, float second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Test, Factory(typeof(Variations), "R4_R4")]
		public void CgtR4R4(float first, float second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "R4_R4")]
		public void CltR4R4(float first, float second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "R4_R4")]
		public void CgeR4R4(float first, float second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "R4_R4")]
		public void CleR4R4(float first, float second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrR4()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenR4(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_R4")]
		public void StelemR4(int index, float value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_R4")]
		public void LdelemR4(int index, float value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_R4")]
		public void LdelemaR4(int index, float value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
