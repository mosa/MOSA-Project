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

namespace Mosa.Test.Runtime.CompilerFramework.CIL
{
	public class ComparisonInstructionTestRunner<T> : TestCompilerAdapter
	{
		private const string TestClassName = @"ComparisonTestClass";

		public ComparisonInstructionTestRunner()
		{
			IncludeCeq = true;
			IncludeClt = true;
			IncludeCgt = true;
			IncludeCle = true;
			IncludeCge = true;
			IncludeNaN = false;
			IncudePositiveInfinity = false;
		}

		public string FirstType { get; set; }

		public bool IncludeCeq { get; set; }
		public bool IncludeClt { get; set; }
		public bool IncludeCgt { get; set; }
		public bool IncludeCle { get; set; }
		public bool IncludeCge { get; set; }
		public bool IncludeNaN { get; set; }
		public bool IncudePositiveInfinity { get; set; }

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
			if (IncludeNaN)
				codeBuilder.Append(TestCodeNaN);
			if (IncudePositiveInfinity)
				codeBuilder.Append(TestCodePositiveInfity);

			codeBuilder.Append(TestCodeFooter);
			

			codeBuilder
				.Replace(@"#firsttype", FirstType)
				.Replace(@"#secondtype", FirstType);

			settings.CodeSource = codeBuilder.ToString();
		}

		public void Ceq(bool expected, T first, T second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty, TestClassName, @"CeqTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Clt(bool expected, T first, T second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty, TestClassName, @"CltTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cgt(bool expected, T first, T second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty, TestClassName, @"CgtTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cle(bool expected, T first, T second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty, TestClassName, @"CleTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cge(bool expected, T first, T second)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty, TestClassName, @"CgeTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void NaN(bool expected, T first)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty, TestClassName, @"NaNTest", first);
			Assert.AreEqual(expected, result);
		}

		public void PositiveInfinity(bool expected, T first)
		{
			SetTestCode();
			bool result = Run<bool>(string.Empty, TestClassName, @"PositiveInfinityTest", first);
			Assert.AreEqual(expected, result);
		}

		private const string TestCodeHeader = @"
			using System.Runtime.InteropServices;

			public static class ComparisonTestClass
			{
		";

		private const string TestCodeCeq = @"
				public static bool CeqTest(#firsttype first, #secondtype second)
				{
					return (first.CompareTo(second) == 0);
				}
			";

		private const string TestCodeClt = @"
				public static bool CltTest(#firsttype first, #secondtype second)
				{
					return (first.CompareTo(second) < 0);
				}
			";

		private const string TestCodeCgt = @"
				public static bool CgtTest(#firsttype first, #secondtype second)
				{
					return (first.CompareTo(second) > 0);
				}
			";

		private const string TestCodeCle = @"
				public static bool CleTest(#firsttype first, #secondtype second)
				{
					return (first.CompareTo(second) <= 0);
				}
			";

		private const string TestCodeCge = @"
				public static bool CgeTest(#firsttype first, #secondtype second)
				{
					return (first.CompareTo(second) >= 0);
				}
			";

		private const string TestCodeNaN = @"
				public static bool NaNTest(#firsttype first)
				{
					return (double.IsNaN(first));
				}
			";

		private const string TestCodePositiveInfity = @"
				public static bool PositiveInfinityTest(#firsttype first)
				{
					return (double.IsPositiveInfinity(first));
				}
			";

		private const string TestCodeFooter = @"
			}
		";
	}
}
