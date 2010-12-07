/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.IO;

namespace Mosa.Test.Runtime.CompilerFramework
{

	/// <summary>
	/// A test runner base class for tests using the CodeDom compilers.
	/// </summary>
	public abstract class CodeDomTestRunner : MosaCompilerTestRunner
	{
		#region Data members

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
		private static TempFileCollection temps = new TempFileCollection(TempDirectory, false);

		/// <summary>
		/// Determines if unsafe code is allowed in the test.
		/// </summary>
		private bool unsafeCode;

		/// <summary>
		/// Determines if mscorlib is referenced in the test.
		/// </summary>
		private bool doNotReferenceMscorlib;

		/// <summary>
		/// 
		/// </summary>
		private static string tempDirectory;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeDomTestRunner"/> class.
		/// </summary>
		public CodeDomTestRunner()
		{
			language = "C#";
			unsafeCode = true;
			doNotReferenceMscorlib = true;
			NeedCompile = true;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
		protected string Language
		{
			get { return language; }
			set { if (language != value) NeedCompile = true; language = value; }
		}

		/// <summary>
		/// Gets or sets the code source.
		/// </summary>
		/// <value>The code source.</value>
		protected string CodeSource
		{
			get { return codeSource; }
			set { if (codeSource != value) NeedCompile = true; codeSource = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether unsafe code is used in the test.
		/// </summary>
		/// <value><c>true</c> if unsafe code is used in the test; otherwise, <c>false</c>.</value>
		protected bool UnsafeCode
		{
			get { return unsafeCode; }
			set { if (unsafeCode != value) NeedCompile = true; unsafeCode = value; }
		}

		protected bool DoNotReferenceMscorlib
		{
			get { return doNotReferenceMscorlib; }
			set { if (doNotReferenceMscorlib != value) NeedCompile = true; doNotReferenceMscorlib = value; }
		}

		private static string TempDirectory
		{
			get
			{
				if (tempDirectory == null)
				{
					tempDirectory = Path.Combine(Path.GetTempPath(), "mosa");
					if (!Directory.Exists(tempDirectory))
					{
						Directory.CreateDirectory(tempDirectory);
					}
				}
				return tempDirectory;
			}
		}

		#endregion Properties

		#region MosaCompilerTestRunner Overrides

		/// <summary>
		/// Compiles the test code.
		/// </summary>
		/// <param name="ns">The namespace of the test.</param>
		/// <param name="type">The type, which contains the test.</param>
		/// <param name="method">The name of the method of the test.</param>
		/// <returns>The name of the assembly file.</returns>
		protected override string CompileTestCode(string ns, string type, string method)
		{
			Console.WriteLine("Executing {0} compiler...", Language);

			CodeDomProvider provider;
			if (!providerCache.TryGetValue(language, out provider))
			{
				provider = CodeDomProvider.CreateProvider(Language);
				if (provider == null)
					throw new NotSupportedException("The language '" + Language + "' is not supported on this machine.");
				providerCache.Add(Language, provider);
			}

			string filename = Path.Combine(TempDirectory, Path.ChangeExtension(Path.GetRandomFileName(), "dll"));
			temps.AddFile(filename, false);

			CompilerResults compileResults;
			CompilerParameters parameters = new CompilerParameters(References, filename);
			parameters.CompilerOptions = "/optimize-";

			if (unsafeCode)
			{
				if (Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /unsafe+";
				else
					throw new NotSupportedException();
			}

			if (doNotReferenceMscorlib)
			{
				if (Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /nostdlib";
				else
					throw new NotSupportedException();
			}

			parameters.GenerateInMemory = false;

			if (codeSource != null)
			{
				Console.WriteLine("Code: {0}", codeSource);
				compileResults = provider.CompileAssemblyFromSource(
					parameters,
					codeSource
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

	}
}
