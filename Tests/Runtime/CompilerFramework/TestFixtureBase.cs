/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Diagnostics;

using MbUnit.Framework;

using Mosa.Runtime.Loader;
using Mosa.Runtime;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;

namespace Test.Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Interface class for MbUnit3 to run our testcases.
	/// </summary>
	public abstract class TestFixtureBase
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		ITypeSystem typeSystem;

		/// <summary>
		/// 
		/// </summary>
		private Assembly loadedAssembly;

		/// <summary>
		/// The filename of the assembly, which contains the test case.
		/// </summary>
		private string assembly = null;

		/// <summary>
		/// Flag, which determines if the compiler needs to run.
		/// </summary>
		private bool needCompile;

		/// <summary>
		/// An array of assembly references to include in the compilation.
		/// </summary>
		private string[] references;

		/// <summary>
		/// The source text of the test code to compile.
		/// </summary>
		private string codeSource;

		/// <summary>
		/// Holds the target language of this test runner.
		/// </summary>
		private string language;

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
		/// Initializes a new instance of the <see cref="TestFixtureBase"/> class.
		/// </summary>
		public TestFixtureBase()
		{
			references = new string[0];
			language = "C#";
			unsafeCode = true;
			doNotReferenceMscorlib = true;
			NeedCompile = true;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether the test needs to be compiled.
		/// </summary>
		/// <value><c>true</c> if a compilation is needed; otherwise, <c>false</c>.</value>
		protected bool NeedCompile
		{
			get { return needCompile; }
			set { needCompile = value; }
		}

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

		/// <summary>
		/// Gets or sets the references.
		/// </summary>
		/// <value>The references.</value>
		protected string[] References
		{
			get { return references; }
			set { if (references != value) NeedCompile = true; references = value; }
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

		public T Run<T>(string type, string method, params object[] parameters)
		{
			if (needCompile)
			{
				CompileTestCode();
				needCompile = false;
				Prebuilt.LoadPrebuiltDelegateAssembly();
			}

			// Find the test method to execute
			RuntimeMethod runtimeMethod = FindMethod(
				string.Empty,
				type,
				method
			);

			// Get delegate name
			string delegateName;

			if (default(T) is System.ValueType)
				delegateName = "Mosa.Test.Prebuilt.Delegates+" + DelegateUtility.GetDelegteName(default(T), parameters);
			else
				delegateName = "Mosa.Test.Prebuilt.Delegates+" + DelegateUtility.GetDelegteName(null, parameters);

			// Get the prebuilt delegate type
			//Type delegateType = Type.GetType(delegateName);
			Type delegateType = Prebuilt.GetType(delegateName);

			Debug.Assert(delegateType != null, delegateName);

			// Create a delegate for the test method
			Delegate fn = Marshal.GetDelegateForFunctionPointer(
				runtimeMethod.Address,
				delegateType
			);

			// Execute the test method
			object tempResult = fn.DynamicInvoke(parameters);

			try
			{
				if (default(T) is System.ValueType)
					return (T)tempResult;
				else
					return default(T);
			}
			catch (InvalidCastException e)
			{
				Assert.Fail(@"Failed to convert result {0} of type {1} to type {2}.", tempResult, tempResult.GetType(), typeof(T));
				throw e;
			}
		}

		/// <summary>
		/// Finds a runtime method, which represents the requested method.
		/// </summary>
		/// <exception cref="MissingMethodException">The sought method is not found.</exception>
		/// <param name="ns">The namespace of the sought method.</param>
		/// <param name="type">The type, which contains the sought method.</param>
		/// <param name="method">The method to find.</param>
		/// <returns>An instance of <see cref="RuntimeMethod"/>.</returns>
		private RuntimeMethod FindMethod(string ns, string type, string method)
		{
			foreach (RuntimeType t in typeSystem.GetCompiledTypes())
			{
				if (t.Namespace != ns || t.Name != type)
					continue;

				foreach (RuntimeMethod m in t.Methods)
				{
					if (m.Name == method)
					{
						return m;
					}
				}
			}

			throw new MissingMethodException(ns + "." + type, method);
		}

		protected void CompileTestCode()
		{
			if (loadedAssembly != null)
			{
				loadedAssembly = null;
			}

			assembly = RunCodeDomCompiler();

			Console.WriteLine("Executing MOSA compiler...");
			RunMosaCompiler(assembly);
		}

		private string RunCodeDomCompiler()
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
			CompilerParameters parameters = new CompilerParameters(References, filename, false);
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
				compileResults = provider.CompileAssemblyFromSource(parameters, codeSource);
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

		private void RunMosaCompiler(string assemblyFile)
		{
			List<string> files = new List<string>();
			files.Add(assemblyFile);

			IAssemblyLoader assemblyLoader = new AssemblyLoader();

			typeSystem = new DefaultTypeSystem(assemblyLoader);
			typeSystem.LoadModules(files);

			TestCaseAssemblyCompiler.Compile(typeSystem, assemblyLoader);
		}

		protected string CreateMarshalAttribute(string prefix, string typeName)
		{
			string result = String.Empty;
			string marshalDirective = GetMarshalDirective(typeName);
			if (marshalDirective != null)
			{
				result = @"[" + prefix + marshalDirective + @"]";
			}

			return result;
		}

		protected string GetMarshalDirective(string typeName)
		{
			string marshalDirective = null;

			if (typeName == "char")
			{
				marshalDirective = @"MarshalAs(UnmanagedType.U2)";
			}

			return marshalDirective;
		}

	}
}
