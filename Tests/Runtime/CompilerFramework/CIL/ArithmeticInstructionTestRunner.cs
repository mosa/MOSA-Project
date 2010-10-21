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
			this.IncludeAdd = true;
			this.IncludeSub = true;
			this.IncludeMul = true;
			this.IncludeDiv = true;
			this.IncludeRem = true;
			this.IncludeNeg = true;
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
			string returnMarshalType = this.CreateMarshalAttribute(@"return:", FirstType);
			string marshalFirstType = this.CreateMarshalAttribute(String.Empty, FirstType);
			string marshalSecondType = this.CreateMarshalAttribute(String.Empty, SecondType);
			string marshalExpectedType = this.CreateMarshalAttribute(String.Empty, ExpectedType);

			StringBuilder codeBuilder = new StringBuilder();
			codeBuilder.Append(TestCodeHeader);
			if (this.IncludeAdd)
				codeBuilder.Append(TestCodeAdd);
			if (this.IncludeSub)
				codeBuilder.Append(TestCodeSub);
			if (this.IncludeMul)
				codeBuilder.Append(TestCodeMul);
			if (this.IncludeDiv)
				codeBuilder.Append(TestCodeDiv);
			if (this.IncludeRem)
				codeBuilder.Append(TestCodeRem);
			if (this.IncludeNeg)
				codeBuilder.Append(TestCodeNeg);

			// FIXME: Ret didn't yet fit anywhere else, move it once something shows up (e.g. call support?)
			codeBuilder.Append(TestCodeRet);

			codeBuilder.Append(TestCodeFooter);

			codeBuilder.Append(Code.ObjectClassDefinition);

			codeBuilder
				.Replace(@"[[expectedtype]]", ExpectedType)
				.Replace(@"[[firsttype]]", FirstType)
				.Replace(@"[[secondtype]]", SecondType)
				.Replace(@"[[returnmarshal-type]]", returnMarshalType)
				.Replace(@"[[marshal-expectedtype]]", marshalExpectedType)
				.Replace(@"[[marshal-firsttype]]", marshalFirstType)
				.Replace(@"[[marshal-secondtype]]", marshalSecondType);

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
				public delegate bool R_AddTest([[marshal-expectedtype]][[expectedtype]] expectedValue, [[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool AddTest([[expectedtype]] expectedValue, [[firsttype]] first, [[secondtype]] second)
				{
					return expectedValue == (first + second);   
				}
			";

		private const string TestCodeSub = @"
				public delegate bool R_SubTest([[marshal-expectedtype]][[expectedtype]] expectedValue, [[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool SubTest([[expectedtype]] expectedValue, [[firsttype]] first, [[secondtype]] second)
				{
					return expectedValue == (first - second);   
				}
			";

		private const string TestCodeMul = @"
				public delegate bool R_MulTest([[marshal-expectedtype]][[expectedtype]] expectedValue, [[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool MulTest([[expectedtype]] expectedValue, [[firsttype]] first, [[secondtype]] second)
				{
					return expectedValue == (first * second);
				}
			";

		private const string TestCodeDiv = @"
				public delegate bool R_DivTest([[marshal-expectedtype]][[expectedtype]] expectedValue, [[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool DivTest([[expectedtype]] expectedValue, [[firsttype]] first, [[secondtype]] second)
				{
					return expectedValue == (first / second);
				}
			";

		private const string TestCodeRem = @"
				public delegate bool R_RemTest([[marshal-expectedtype]][[expectedtype]] expectedValue, [[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool RemTest([[expectedtype]] expectedValue, [[firsttype]] first, [[secondtype]] second)
				{
					return expectedValue == (first % second);
				}
			";

		private const string TestCodeNeg = @"
				public delegate bool R_NegTest([[marshal-expectedtype]][[expectedtype]] expectedValue, [[marshal-firsttype]][[firsttype]] first);

				public static bool NegTest([[expectedtype]] expectedValue, [[firsttype]] first)
				{
					return expectedValue == (-first);
				}
			";

		private const string TestCodeRet = @"
				[[returnmarshal-type]]
				public delegate [[firsttype]] R_RetTest([[marshal-firsttype]][[firsttype]] first);

				public static [[firsttype]] RetTest([[firsttype]] first)
				{
					return first;
				}
			";

		private const string TestCodeFooter = @"
			}
		";
	}
}
