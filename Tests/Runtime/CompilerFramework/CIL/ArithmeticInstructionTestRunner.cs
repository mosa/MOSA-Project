/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka grover, <mailto:sharpos@michaelruck.de>)
 *  
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

		public string ExpectedTypeName { get; set; }
		public string TypeName { get; set; }

		public bool IncludeAdd { get; set; }
		public bool IncludeSub { get; set; }
		public bool IncludeMul { get; set; }
		public bool IncludeDiv { get; set; }
		public bool IncludeRem { get; set; }
		public bool IncludeNeg { get; set; }

		private void SetTestCode(string expectedType, string typeName)
		{
			string marshalType = this.CreateMarshalAttribute(String.Empty, typeName);
			string returnMarshalType = this.CreateMarshalAttribute(@"return:", typeName);
			string marshalExpectedType = this.CreateMarshalAttribute(String.Empty, expectedType);

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
				.Replace(@"[[typename]]", typeName)
				.Replace(@"[[expectedType]]", expectedType)
				.Replace(@"[[returnmarshal-typename]]", returnMarshalType)
				.Replace(@"[[marshal-typename]]", marshalType)
				.Replace(@"[[marshal-expectedType]]", marshalExpectedType);

			this.CodeSource = codeBuilder.ToString();
		}

		private string CreateMarshalAttribute(string prefix, string typeName)
		{
			string result = String.Empty;
			string marshalDirective = this.GetMarshalDirective(typeName);
			if (marshalDirective != null)
			{
				result = @"[" + prefix + marshalDirective + @"]";
			}

			return result;
		}

		private string GetMarshalDirective(string typeName)
		{
			string marshalDirective = null;

			if (typeName == @"char")
			{
				marshalDirective = @"MarshalAs(UnmanagedType.U2)";
			}

			return marshalDirective;
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
			if (this.CodeSource == null)
			{
				this.SetTestCode(this.ExpectedTypeName, this.TypeName);
			}
		}

		private const string TestCodeHeader = @"
			using System.Runtime.InteropServices;

			[[returnmarshal-typename]]
			public delegate [[typename]] R_T([[marshal-typename]][[typename]] value);

			public delegate bool R_T_T([[marshal-expectedType]][[expectedType]] first, [[marshal-typename]][[typename]] second);

			public delegate bool R_T_T_T([[marshal-expectedType]][[expectedType]] expectedValue, [[marshal-typename]][[typename]] first, [[marshal-typename]][[typename]] second);

			public static class ArithmeticsTestClass
			{
		";

		private const string TestCodeAdd = @"
				public static bool AddTest([[expectedType]] expectedValue, [[typename]] first, [[typename]] second)
				{
					return expectedValue == (first + second);   
				}
			";

		private const string TestCodeSub = @"
				public static bool SubTest([[expectedType]] expectedValue, [[typename]] first, [[typename]] second)
				{
					return expectedValue == (first - second);   
				}
			";

		private const string TestCodeMul = @"
				public static bool MulTest([[expectedType]] expectedValue, [[typename]] first, [[typename]] second)
				{
					return expectedValue == (first * second);
				}
			";

		private const string TestCodeDiv = @"
				public static bool DivTest([[expectedType]] expectedValue, [[typename]] first, [[typename]] second)
				{
					return expectedValue == (first / second);
				}
			";

		private const string TestCodeRem = @"
				public static bool RemTest([[expectedType]] expectedValue, [[typename]] first, [[typename]] second)
				{
					return expectedValue == (first % second);
				}
			";

		private const string TestCodeNeg = @"
				public static bool NegTest([[expectedType]] expectedValue, [[typename]] first)
				{
					return expectedValue == (-first);
				}
			";

		private const string TestCodeRet = @"
				public static [[typename]] RetTest([[typename]] value)
				{
					return value;
				}
			";

		private const string TestCodeFooter = @"
			}
		";
	}
}
