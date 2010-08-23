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
	public class SZArrayInstructionTestRunner<T> : TestFixtureBase
	{
		private const string TestClassName = @"ArrayTestClass";

		public SZArrayInstructionTestRunner()
		{
			this.IncludeNewarr = true;
			this.IncludeLdlen = true;
			this.IncludeLdelem = true;
			this.IncludeStelem = true;
			this.IncludeLdelema = true;
		}

		public string TypeName { get; set; }

		public bool IncludeNewarr { get; set; }
		public bool IncludeLdlen { get; set; }
		public bool IncludeLdelem { get; set; }
		public bool IncludeStelem { get; set; }
		public bool IncludeLdelema { get; set; }

		private void SetTestCode(string typeName)
		{
			string marshalType = this.CreateMarshalAttribute(String.Empty, typeName);

			StringBuilder codeBuilder = new StringBuilder();
			codeBuilder.Append(TestCodeHeader);

			if (this.IncludeNewarr == true)
			{
				codeBuilder.Append(TestCodeNewarr);
			}

			if (this.IncludeLdlen == true)
			{
				codeBuilder.Append(TestCodeLdlen);
			}

			if (this.IncludeLdelem == true)
			{
				codeBuilder.Append(TestCodeLdelem);
			}

			if (this.IncludeStelem == true)
			{
				codeBuilder.Append(TestCodeStelem);
			}

			if (this.IncludeLdelema == true)
			{
				codeBuilder.Append(TestCodeLdelema);
			}

			codeBuilder.Append(TestCodeFooter);

			codeBuilder.Append(Code.ObjectClassDefinition);

			codeBuilder
				.Replace(@"[[typename]]", typeName)
				.Replace(@"[[marshal-typename]]", marshalType);

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

		public void Newarr()
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"NewarrTest");
			Assert.IsTrue(result);
		}

		public void Ldlen(int length)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"LdlenTest", length);
			Assert.IsTrue(result);
		}

		public void Stelem(int index, T value)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"StelemTest", index, value);
			Assert.IsTrue(result);
		}

		public void Ldelem(int index, T value)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"LdelemTest", index, value);
			Assert.IsTrue(result);
		}

		public void Ldelema(int index, T value)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"LdelemaTest", index, value);
			Assert.IsTrue(result);
		}

		private void EnsureCodeSourceIsSet()
		{
			if (this.CodeSource == null)
			{
				this.SetTestCode(this.TypeName);
			}
		}

		private const string TestCodeHeader = @"
			using System.Runtime.InteropServices;

			public delegate bool R();

			public delegate bool R_T(int index);

			public delegate bool R_T_T(int index, [[marshal-typename]][[typename]] value);

			public static class ArrayTestClass
			{
		";

		private const string TestCodeNewarr = @"
				public static bool NewarrTest()
				{
					[[typename]][] arr = new [[typename]][0];
					return arr != null;
				}
			";

		private const string TestCodeLdlen = @"
				public static bool LdlenTest(int length)
				{
					[[typename]][] arr = new [[typename]][length];
					return arr.Length == length;   
				}
			";

		private const string TestCodeLdelem = @"
				public static bool LdelemTest(int index, [[typename]] value)
				{
					[[typename]][] arr = new [[typename]][index + 1];
					arr[index] = value;
					return value == arr[index];
				}
			";

		private const string TestCodeStelem = @"
				public static bool StelemTest(int index, [[typename]] value)
				{
					[[typename]][] arr = new [[typename]][index + 1];
					arr[index] = value;
					return true;
				}
			";

		private const string TestCodeLdelema = @"
				public static bool LdelemaTest(int index, [[typename]] value)
				{
					[[typename]][] arr = new [[typename]][index + 1];
					SetValueInRefValue(ref arr[index], value);
					return arr[index] == value;
				}

				private static void SetValueInRefValue(ref [[typename]] destination, [[typename]] value)
				{
					destination = value;
				}
			";

		private const string TestCodeFooter = @"
			}
		";
	}
}
