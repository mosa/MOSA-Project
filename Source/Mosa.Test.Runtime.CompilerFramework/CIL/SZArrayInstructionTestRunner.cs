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

namespace Mosa.Test.Runtime.CompilerFramework.CIL
{
	public class SZArrayInstructionTestRunner<T> : TestCompilerAdapter
	{
		private const string TestClassName = @"ArrayTestClass";

		public SZArrayInstructionTestRunner()
		{
			IncludeNewarr = true;
			IncludeLdlen = true;
			IncludeLdelem = true;
			IncludeStelem = true;
			IncludeLdelema = true;
		}

		public string FirstType { get; set; }

		public bool IncludeNewarr { get; set; }
		public bool IncludeLdlen { get; set; }
		public bool IncludeLdelem { get; set; }
		public bool IncludeStelem { get; set; }
		public bool IncludeLdelema { get; set; }

		private void SetTestCode()
		{
			StringBuilder codeBuilder = new StringBuilder();

			codeBuilder.Append(TestCodeHeader);

			if (this.IncludeNewarr)
				codeBuilder.Append(TestCodeNewarr);

			if (this.IncludeLdlen)
				codeBuilder.Append(TestCodeLdlen);
			if (this.IncludeLdelem)
				codeBuilder.Append(TestCodeLdelem);
			if (this.IncludeStelem)
				codeBuilder.Append(TestCodeStelem);
			if (this.IncludeLdelema)
				codeBuilder.Append(TestCodeLdelema);

			codeBuilder.Append(TestCodeFooter);
			codeBuilder.Append(Code.AllTestCode);

			codeBuilder
				.Replace(@"#firsttype", FirstType);

			compiler.CodeSource = codeBuilder.ToString();
		}

		public void Newarr()
		{
			this.EnsureCodeSourceIsSet();
			bool result = compiler.Run<bool>(string.Empty, TestClassName, @"NewarrTest");
			Assert.IsTrue(result);
		}

		public void Ldlen(int length)
		{
			this.EnsureCodeSourceIsSet();
			bool result = compiler.Run<bool>(string.Empty, TestClassName, @"LdlenTest", length);
			Assert.IsTrue(result);
		}

		public void Stelem(int index, T value)
		{
			this.EnsureCodeSourceIsSet();
			bool result = compiler.Run<bool>(string.Empty, TestClassName, @"StelemTest", index, value);
			Assert.IsTrue(result);
		}

		public void Ldelem(int index, T value)
		{
			this.EnsureCodeSourceIsSet();
			bool result = compiler.Run<bool>(string.Empty, TestClassName, @"LdelemTest", index, value);
			Assert.IsTrue(result);
		}

		public void Ldelema(int index, T value)
		{
			this.EnsureCodeSourceIsSet();
			bool result = compiler.Run<bool>(string.Empty, TestClassName, @"LdelemaTest", index, value);
			Assert.IsTrue(result);
		}

		private void EnsureCodeSourceIsSet()
		{
			this.SetTestCode();
		}

		private const string TestCodeHeader = @"
			using System.Runtime.InteropServices;

			public static class ArrayTestClass
			{
		";

		private const string TestCodeNewarr = @"
				public static bool NewarrTest()
				{
					#firsttype[] arr = new #firsttype[0];
					return arr != null;
				}
			";

		private const string TestCodeLdlen = @"
				public static bool LdlenTest(int length)
				{
					#firsttype[] arr = new #firsttype[length];
					return arr.Length == length;   
				}
			";

		private const string TestCodeLdelem = @"
				public static bool LdelemTest(int index, #firsttype value)
				{
					#firsttype[] arr = new #firsttype[index + 1];
					arr[index] = value;
					return value == arr[index];
				}
			";

		private const string TestCodeStelem = @"
				public static bool StelemTest(int index, #firsttype value)
				{
					#firsttype[] arr = new #firsttype[index + 1];
					arr[index] = value;
					return true;
				}
			";

		private const string TestCodeLdelema = @"
				public static bool LdelemaTest(int index, #firsttype value)
				{
					#firsttype[] arr = new #firsttype[index + 1];
					SetValueInRefValue(ref arr[index], value);
					return arr[index] == value;
				}

				private static void SetValueInRefValue(ref #firsttype destination, #firsttype value)
				{
					destination = value;
				}
			";

		private const string TestCodeFooter = @"
			}
		";
	}
}
