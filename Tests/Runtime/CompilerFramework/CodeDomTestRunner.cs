/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.IO;

namespace Test.Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// A test runner base class for tests using the CodeDom compilers.
	/// </summary>
	public abstract class CodeDomTestRunner : MosaCompilerTestRunner
	{
		#region Data members

		/// <summary>
		/// The filename of the test code to compile.
		/// </summary>
		string codeFilename;

		/// <summary>
		/// The source text of the test code to compile.
		/// </summary>
		string codeSource;

		/// <summary>
		/// Holds the target language of this test runner.
		/// </summary>
		string language;

		/// <summary>
		/// A cache of CodeDom providers.
		/// </summary>
		private static Dictionary<string, CodeDomProvider> providerCache = new Dictionary<string, CodeDomProvider>();

		/// <summary>
		/// Holds the temporary files collection.
		/// </summary>
		private TempFileCollection temps = new TempFileCollection();

		/// <summary>
		/// Determines if unsafe code is allowed in the test.
		/// </summary>
		private bool unsafeCode;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeDomTestRunner"/> class.
		/// </summary>
		public CodeDomTestRunner()
		{
			this.language = "C#";
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
		public string Language
		{
			get { return this.language; }
			set
			{
				if (this.language != value)
				{
					this.language = value;
					this.NeedCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the code filename.
		/// </summary>
		/// <value>The code filename.</value>
		public string CodeFilename
		{
			get { return this.codeFilename; }
			set
			{
				if (this.codeFilename != value)
				{
					this.codeFilename = value;
					this.NeedCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the code source.
		/// </summary>
		/// <value>The code source.</value>
		public string CodeSource
		{
			get { return this.codeSource; }
			set
			{
				if (this.codeSource != value)
				{
					this.codeSource = value;
					this.NeedCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether unsafe code is used in the test.
		/// </summary>
		/// <value><c>true</c> if unsafe code is used in the test; otherwise, <c>false</c>.</value>
		public bool UnsafeCode
		{
			get { return this.unsafeCode; }
			set
			{
				if (this.unsafeCode != value)
				{
					this.unsafeCode = value;
					this.NeedCompile = true;
				}
			}
		}

		#endregion // Properties

		#region MosaCompilerTestRunner Overrides

		/// <summary>
		/// Compiles the test code.
		/// </summary>
		/// <param name="ns">The namespace of the test.</param>
		/// <param name="type">The type, which contains the test.</param>
		/// <param name="method">The name of the method of the test.</param>
		/// <returns>The name of the assembly file.</returns>
		protected override string CompileTestCode<TDelegate>(string ns, string type, string method)
		{
			CodeDomProvider provider;
			Console.WriteLine("Executing {0} compiler...", this.Language);
			if (!providerCache.TryGetValue(this.language, out provider))
				provider = CodeDomProvider.CreateProvider(Language);
			if (provider == null)
				throw new NotSupportedException("The language '" + Language + "' is not supported on this machine.");
			CompilerResults compileResults;
			CompilerParameters parameters = new CompilerParameters(this.References, Path.GetTempFileName());
			parameters.CompilerOptions = "/optimize- /debug+ /debug:full";

			if (this.unsafeCode)
			{
				if (this.Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /unsafe+";
				else
					throw new NotSupportedException();
			}

			if (this.DoNotReferenceMsCorlib)
			{
				if (this.Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /nostdlib";
				else
					throw new NotSupportedException();
			}

			parameters.GenerateInMemory = false;

			if (CodeSource != null)
			{
				Console.Write("From Source: ");
				Console.WriteLine(new string('-', 40 - 13));
				Console.WriteLine(codeSource);
				Console.WriteLine(new string('-', 40));
				compileResults = provider.CompileAssemblyFromSource(
					parameters,
					codeSource
				);
			}
			else if (CodeFilename != null)
			{
				Console.WriteLine("From File: {0}", codeFilename);
				compileResults = provider.CompileAssemblyFromFile(
					parameters,
					codeFilename
				);
			}
			else
				throw new NotSupportedException();
			if (compileResults.Errors.HasErrors)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Code compile errors:");
				foreach (CompilerError error in compileResults.Errors)
				{
					sb.AppendLine(error.ToString());
				}
				throw new Exception(sb.ToString());
			}
			return compileResults.PathToAssembly;
		}

		#endregion // MosaCompilerTestRunner Overrides

		public bool DoNotReferenceMsCorlib { get; set; }
	}
}
