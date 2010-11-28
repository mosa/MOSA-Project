/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>  
 */

using System;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.CLI
{
	public class ArithmeticInstructionTestRunner<R, T> : TestFixtureBase
	{
		private const string TestClassName = @"ArithmeticsTestClass";

		public ArithmeticInstructionTestRunner()
		{
			IncludeAdd = true;
			IncludeSub = true;
			IncludeMul = true;
			IncludeDiv = true;
			IncludeRem = true;
			IncludeNeg = true;
		}

		public string ExpectedType { get; set; }
		public string FirstType { get; set; }
		public string SecondType { get; set; }

		public bool IncludeAdd { get; set; }
		public bool IncludeSub { get; set; }
		public bool IncludeMul { get; set; }
		public bool IncludeDiv { get; set; }
		public bool IncludeRem { get; set; }
		public bool IncludeNeg { get; set; }

		private void SetTestCode()
		{
			StringBuilder codeBuilder = new StringBuilder();

			codeBuilder.Append(TestCodeHeader);

			if (IncludeAdd)
				codeBuilder.Append(TestCodeAdd);
			if (IncludeSub)
				codeBuilder.Append(TestCodeSub);
			if (IncludeMul)
				codeBuilder.Append(TestCodeMul);
			if (IncludeDiv)
				codeBuilder.Append(TestCodeDiv);
			if (IncludeRem)
				codeBuilder.Append(TestCodeRem);
			if (IncludeNeg)
				codeBuilder.Append(TestCodeNeg);

			codeBuilder.Append(TestCodeRet);
			codeBuilder.Append(TestCodeFooter);
			codeBuilder.Append(Code.AllTestCode);

			codeBuilder
				.Replace(@"#expectedtype", ExpectedType)
				.Replace(@"#firsttype", FirstType)
				.Replace(@"#secondtype", SecondType);

			CodeSource = codeBuilder.ToString();
		}

		public void Add(R expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"AddTest", expected, first, second);
			Assert.IsTrue(result);
		}

		public void Sub(R expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"SubTest", expected, first, second);
			Assert.IsTrue(result);
		}

		public void Mul(R expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"MulTest", expected, first, second);
			Assert.IsTrue(result);
		}

		public void Div(R expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"DivTest", expected, first, second);
			Assert.IsTrue(result);
		}

		public void Rem(R expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"RemTest", expected, first, second);
			Assert.IsTrue(result);
		}

		public void Neg(R expectedValue, T first)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"NegTest", expectedValue, first);
			Assert.IsTrue(result);
		}

		public void Ret(T value)
		{
			this.EnsureCodeSourceIsSet();
			T result = this.Run<T>(TestClassName, @"RetTest", value);
			Assert.AreEqual(value, result);
		}

		private void EnsureCodeSourceIsSet()
		{
			if (CodeSource == null)
			{
				this.SetTestCode();
			}
		}

		private const string TestCodeHeader = @"
			using System.Runtime.InteropServices;
		
			public static class ArithmeticsTestClass
			{
		";

		private const string TestCodeAdd = @"
				public static bool AddTest(#expectedtype expectedValue, #firsttype first, #secondtype second)
				{
					return expectedValue == (first + second);   
				}
			";

		private const string TestCodeSub = @"
				public static bool SubTest(#expectedtype expectedValue, #firsttype first, #secondtype second)
				{
					return expectedValue == (first - second);   
				}
			";

		private const string TestCodeMul = @"
				public static bool MulTest(#expectedtype expectedValue, #firsttype first, #secondtype second)
				{
					return expectedValue == (first * second);
				}
			";

		private const string TestCodeDiv = @"
				public static bool DivTest(#expectedtype expectedValue, #firsttype first, #secondtype second)
				{
					return expectedValue == (first / second);
				}
			";

		private const string TestCodeRem = @"
				public static bool RemTest(#expectedtype expectedValue, #firsttype first, #secondtype second)
				{
					return expectedValue == (first % second);
				}
			";

		private const string TestCodeNeg = @"
				public static bool NegTest(#expectedtype expectedValue, #firsttype first)
				{
					return expectedValue == (-first);
				}
			";

		private const string TestCodeRet = @"
				public static #firsttype RetTest(#firsttype first)
				{
					return first;
				}
			";

		private const string TestCodeFooter = @"
			}
		";
	}
}
