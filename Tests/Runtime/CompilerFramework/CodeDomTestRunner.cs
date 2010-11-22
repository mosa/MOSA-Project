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
		private static TempFileCollection temps = new TempFileCollection(TempDirectory, false);

		/// <summary>
		/// Determines if unsafe code is allowed in the test.
		/// </summary>
		private bool unsafeCode;

		private static string tempDirectory;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeDomTestRunner"/> class.
		/// </summary>
		public CodeDomTestRunner()
		{
			language = "C#";
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
		public string Language
		{
			get { return language; }
			set
			{
				if (language != value)
				{
					language = value;
					NeedCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the code filename.
		/// </summary>
		/// <value>The code filename.</value>
		public string CodeFilename
		{
			get { return codeFilename; }
			set
			{
				if (codeFilename != value)
				{
					codeFilename = value;
					NeedCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the code source.
		/// </summary>
		/// <value>The code source.</value>
		public string CodeSource
		{
			get { return codeSource; }
			set
			{
				if (codeSource != value)
				{
					codeSource = value;
					NeedCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether unsafe code is used in the test.
		/// </summary>
		/// <value><c>true</c> if unsafe code is used in the test; otherwise, <c>false</c>.</value>
		public bool UnsafeCode
		{
			get { return unsafeCode; }
			set
			{
				if (unsafeCode != value)
				{
					unsafeCode = value;
					NeedCompile = true;
				}
			}
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
			Console.WriteLine("Executing {0} compiler...", Language);
			if (!providerCache.TryGetValue(language, out provider))
				provider = CodeDomProvider.CreateProvider(Language);
			if (provider == null)
				throw new NotSupportedException("The language '" + Language + "' is not supported on this machine.");

			string filename = Path.Combine(TempDirectory, Path.ChangeExtension(Path.GetRandomFileName(), "dll"));
			temps.AddFile(filename, false);

			CompilerResults compileResults;
			CompilerParameters parameters = new CompilerParameters(References, filename);
			parameters.CompilerOptions = "/optimize-";

			unsafeCode = true;
			if (unsafeCode)
			{
				if (Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /unsafe+";
				else
					throw new NotSupportedException();
			}

			if (DoNotReferenceMsCorlib)
			{
				if (Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /nostdlib";
				else
					throw new NotSupportedException();
			}

			parameters.GenerateInMemory = false;

			Console.WriteLine("Compiler Options: {0}", parameters.CompilerOptions);

			if (CodeSource != null)
			{
				Console.WriteLine("Code: {0}", CodeSource);
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
