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

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.CIL
{
	public sealed class BinaryLogicInstructionTestRunner<R, T, S> : TestCompilerAdapter
	{
		private const string TestClassName = @"BinaryLogicTestClass";

		public BinaryLogicInstructionTestRunner()
		{
			IncludeAnd = true;
			IncludeOr = true;
			IncludeXor = true;
			IncludeNot = true;
			IncludeShl = true;
			IncludeShr = true;
			IncludeComp = true;
		}

		private void SetTestCode()
		{
			StringBuilder codeBuilder = new StringBuilder();

			codeBuilder.Append(TestCodeHeader);

			if (IncludeAnd)
				codeBuilder.Append(TestCodeAnd);
			if (IncludeOr)
				codeBuilder.Append(TestCodeOr);
			if (IncludeXor)
				codeBuilder.Append(TestCodeXor);
			if (IncludeNot)
				codeBuilder.Append(TestCodeNot);
			if (IncludeComp)
				codeBuilder.Append(TestCodeComp);
			if (IncludeShl)
				codeBuilder.Append(TestCodeShl);
			if (IncludeShr)
				codeBuilder.Append(TestCodeShr);

			codeBuilder.Append(TestCodeFooter);
			

			codeBuilder
				.Replace(@"#expectedtype", ExpectedType)
				.Replace(@"#firsttype", FirstType)
				.Replace(@"#secondtype", SecondType)
				.Replace(@"#shifttypename", ShiftType);

			settings.CodeSource = codeBuilder.ToString();
		}

		public string ExpectedType { get; set; }
		public string FirstType { get; set; }
		public string SecondType { get; set; }
		public string ShiftType { get; set; }

		public bool IncludeAnd { get; set; }
		public bool IncludeOr { get; set; }
		public bool IncludeXor { get; set; }
		public bool IncludeNot { get; set; }
		public bool IncludeShl { get; set; }
		public bool IncludeShr { get; set; }
		public bool IncludeComp { get; set; }

		public void And(R expectedValue, T first, T second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty,TestClassName, @"AndTest", expectedValue, first, second);
			Assert.IsTrue(result);
		}

		public void Or(R expectedValue, T first, T second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty,TestClassName, @"OrTest", expectedValue, first, second);
			Assert.IsTrue(result);
		}

		public void Xor(R expectedValue, T first, T second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty,TestClassName, @"XorTest", expectedValue, first, second);
			Assert.IsTrue(result);
		}

		public void Not(R expectedValue, T first)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty,TestClassName, @"NotTest", expectedValue, first);
			Assert.IsTrue(result);
		}

		public void Comp(R expectedValue, T first)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty,TestClassName, @"CompTest", expectedValue, first);
			Assert.IsTrue(result);
		}

		public void Shl(R expectedValue, T first, S second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty,TestClassName, @"ShiftLeftTest", expectedValue, first, second);
			Assert.IsTrue(result);
		}

		public void Shr(R expectedValue, T first, S second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty,TestClassName, @"ShiftRightTest", expectedValue, first, second);
			Assert.IsTrue(result);
		}

		private void EnsureCodeSourceIsSet()
		{
			this.SetTestCode();
		}

		private const string TestCodeHeader = @"
			using System.Runtime.InteropServices;

			public static class BinaryLogicTestClass
			{
		";
				
		private const string TestCodeAnd = @"
				public static bool AndTest(#expectedtype expectedValue, #firsttype first, #secondtype second)
				{
					return expectedValue == (first & second);
				}
		";

		private const string TestCodeOr = @"
				public static bool OrTest(#expectedtype expectedValue, #firsttype first, #secondtype second)
				{
					return expectedValue == (first | second);
				}
		";

		private const string TestCodeXor = @"
				public static bool XorTest(#expectedtype expectedValue, #firsttype first, #secondtype second)
				{
					return expectedValue == (first ^ second);
				}
		";

		private const string TestCodeNot = @"
				public static bool NotTest(#expectedtype expectedValue, #firsttype first)
				{
					return expectedValue == (!first);
				}
		";

		private const string TestCodeComp = @"
				public static bool CompTest(#expectedtype expectedValue, #firsttype first)
				{
					return expectedValue == (~first);
				}
		";

		private const string TestCodeShl = @"
				public static bool ShiftLeftTest(#expectedtype expectedValue, #firsttype first, #shifttypename second)
				{
					return expectedValue == (#expectedtype)(first << second);
				}
		";

		private const string TestCodeShr = @"
				public static bool ShiftRightTest(#expectedtype expectedValue, #firsttype first, #shifttypename second)
				{
					return expectedValue == (#expectedtype)(first >> second);
				}
		";

		private const string TestCodeFooter = @"
			}
		";
	}
}
