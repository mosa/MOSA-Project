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
using System.Text;

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.CIL
{
	public class ComparisonInstructionTestRunner<T> : TestFixtureBase
	{
		private const string TestClassName = @"ComparisonTestClass";

		public ComparisonInstructionTestRunner()
		{
			IncludeCeq = true;
			IncludeClt = true;
			IncludeCgt = true;
			IncludeCle = true;
			IncludeCge = true;
		}

		public string FirstType { get; set; }

		public bool IncludeCeq { get; set; }
		public bool IncludeClt { get; set; }
		public bool IncludeCgt { get; set; }
		public bool IncludeCle { get; set; }
		public bool IncludeCge { get; set; }

		private void SetTestCode()
		{
			StringBuilder codeBuilder = new StringBuilder();

			codeBuilder.Append(TestCodeHeader);

			if (IncludeCeq)
				codeBuilder.Append(TestCodeCeq);
			if (IncludeClt)
				codeBuilder.Append(TestCodeClt);
			if (IncludeCgt)
				codeBuilder.Append(TestCodeCgt);
			if (IncludeCle)
				codeBuilder.Append(TestCodeCle);
			if (IncludeCge)
				codeBuilder.Append(TestCodeCge);

			codeBuilder.Append(TestCodeFooter);
			codeBuilder.Append(Code.AllTestCode);

			codeBuilder
				.Replace(@"#firsttype", FirstType)
				.Replace(@"#secondtype", FirstType);

			CodeSource = codeBuilder.ToString();
		}

		public void Ceq(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CeqTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Clt(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CltTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cgt(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CgtTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cle(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CleTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cge(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CgeTest", first, second);
			Assert.AreEqual(expected, result);
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

			public static class ComparisonTestClass
			{
		";

		private const string TestCodeCeq = @"
				public static bool CeqTest(#firsttype first, #secondtype second)
				{
					return (first == second);
				}
			";

		private const string TestCodeClt = @"
				public static bool CltTest(#firsttype first, #secondtype second)
				{
					return (first < second);
				}
			";

		private const string TestCodeCgt = @"
				public static bool CgtTest(#firsttype first, #secondtype second)
				{
					return (first > second);
				}
			";

		private const string TestCodeCle = @"
				public static bool CleTest(#firsttype first, #secondtype second)
				{
					return (first <= second);
				}
			";

		private const string TestCodeCge = @"
				public static bool CgeTest(#firsttype first, #secondtype second)
				{
					return (first >= second);
				}
			";

		private const string TestCodeFooter = @"
			}
		";
	}
}
