/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Michael Fröhlich (aka Michael Ruck, grover <mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Loader;
using Mosa.Runtime;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;
using MbUnit.Framework;
using System.Runtime.InteropServices;
using Test.Mosa.Runtime.CompilerFramework.BaseCode;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;

namespace Test.Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Interface class for MbUnit3 to run our testcases.
	/// </summary>
	public abstract class TestFixtureBase
	{
		private Assembly loadedAssembly;

		/// <summary>
		/// The filename of the assembly, which contains the test case.
		/// </summary>
		private string assembly = null;

		/// <summary>
		/// Flag, which determines if the compiler needs to run.
		/// </summary>
		private bool needCompile = true;

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
		private TempFileCollection temps = new TempFileCollection();

		/// <summary>
		/// Determines if unsafe code is allowed in the test.
		/// </summary>
		private bool unsafeCode;

		/// <summary>
		/// Initializes a new instance of the <see cref="TestFixtureBase"/> class.
		/// </summary>
		public TestFixtureBase()
		{
			this.references = new string[0];
			this.language = "C#";
		}

		/// <summary>
		/// Gets or sets a value indicating whether the test needs to be compiled.
		/// </summary>
		/// <value><c>true</c> if a compilation is needed; otherwise, <c>false</c>.</value>
		protected bool NeedCompile
		{
			get { return this.needCompile; }
			set { this.needCompile = value; }
		}

		/// <summary>
		/// Gets or sets the references.
		/// </summary>
		/// <value>The references.</value>
		public string[] References
		{
			get { return this.references; }
			set
			{
				if (this.references != value)
				{
					this.references = value;
					this.needCompile = true;
				}
			}
		}

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

		/// <summary>
		/// Disposes the test runtime and deletes the compiled assembly.
		/// </summary>
		public void End()
		{
			// Try to delete the compiled assembly...
			if (null != this.assembly)
			{
				try
				{
					File.Delete(this.assembly);
				}
				catch
				{
				}
			}
		}

		public T Run<T>(string type, string method, params object[] parameters)
		{
			this.CompileTestCodeIfNecessary();

			Type delegateType = this.LocateDelegateInCompiledAssembly(parameters.Length);

			IntPtr address = FindTestMethod(String.Empty, type, method);

			T result = default(T);
			object tempResult = ExecuteTestMethod(delegateType, parameters, address);
			try
			{
				result = (T)tempResult;
			}
			catch (InvalidCastException)
			{
				Assert.Fail(@"Failed to convert result {0} of type {1} to type {2}.", tempResult, tempResult.GetType(), typeof(T));
			}

			return result;
		}

		private Type LocateDelegateInCompiledAssembly(int parameterCount)
		{
			string result = BuildDelegateName(parameterCount);
			return GetDelegateType(result);
		}

		private Type GetDelegateType(string result)
		{
			if (this.loadedAssembly == null)
			{
				this.loadedAssembly = Assembly.LoadFile(this.assembly);
			}

			Type delegateType = this.loadedAssembly.GetType(result, true);
			return delegateType;
		}

		private static string BuildDelegateName(int parameterCount)
		{
			StringBuilder delegateName = new StringBuilder();
			delegateName.Append(@"R_");

			for (int index = 0; index < parameterCount; index++)
			{
				delegateName.Append("T_");
			}

			delegateName.Length = delegateName.Length - 1;
			string result = delegateName.ToString();
			return result;
		}

		private static object ExecuteTestMethod(Type delegateType, object[] parameters, IntPtr address)
		{
			// Create a delegate for the test method
			Delegate fn = Marshal.GetDelegateForFunctionPointer(
				address,
				delegateType
			);

			// Execute the test method
			return fn.DynamicInvoke(parameters);
		}

		private IntPtr FindTestMethod(string ns, string type, string method)
		{
			// Find the test method to execute
			RuntimeMethod runtimeMethod = FindMethod(ns, type, method);
			IntPtr address = runtimeMethod.Address;
			return address;
		}

		protected void CompileTestCodeIfNecessary()
		{
			// Do we need to compile the code?
			if (this.needCompile == true)
			{
				this.CompileTestCode();

				this.needCompile = false;
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
			foreach (RuntimeType t in StaticRuntime.TypeSystem.GetCompiledTypes())
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

			throw new MissingMethodException(ns + @"." + type, method);
		}

		protected void CompileTestCode()
		{
			if (this.loadedAssembly != null)
			{
				this.loadedAssembly = null;
			}

			this.assembly = this.RunCodeDomCompiler();

			Console.WriteLine("Executing MOSA compiler...");
			RunMosaCompiler(this.assembly);
		}

		private string RunCodeDomCompiler()
		{
			CodeDomProvider provider;
			Console.WriteLine("Executing {0} compiler...", this.Language);
			if (!providerCache.TryGetValue(this.language, out provider))
				provider = CodeDomProvider.CreateProvider(this.Language);
			if (provider == null)
				throw new NotSupportedException("The language '" + this.Language + "' is not supported on this machine.");

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
			parameters.GenerateInMemory = false;
			if (this.codeSource != null)
			{
				Console.Write("From Source: ");
				Console.WriteLine(new string('-', 40 - 13));
				Console.WriteLine(this.codeSource);
				Console.WriteLine(new string('-', 40));
				compileResults = provider.CompileAssemblyFromSource(parameters, this.codeSource);
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
			//IMetadataModule rtModule = StaticRuntime.AssemblyLoader.Load(StaticRuntime.TypeSystem,typeof(global::Mosa.Runtime.Runtime).Module.FullyQualifiedName);
			//IMetadataModule module = StaticRuntime.AssemblyLoader.Load(StaticRuntime.TypeSystem, assemblyFile);

			List<string> files = new List<string>();

			files.Add(assemblyFile);
			files.Add(typeof(global::Mosa.Runtime.Runtime).Module.FullyQualifiedName);

			StaticRuntime.TypeSystem.LoadModules(files);

			TestCaseAssemblyCompiler.Compile(StaticRuntime.TypeSystem, StaticRuntime.AssemblyLoader);
		}

		public void Dispose()
		{
			try
			{
				this.End();
			}
			finally
			{
				GC.SuppressFinalize(this);
			}
		}
	}
}
