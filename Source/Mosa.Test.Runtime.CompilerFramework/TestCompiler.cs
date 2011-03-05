/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
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

using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Test.Runtime.CompilerFramework
{
	public class TestCompiler
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		private ITypeSystem typeSystem;

		/// <summary>
		/// 
		/// </summary>
		private TestCompilerSettings cacheSettings = null;

		/// <summary>
		/// A cache of CodeDom providers.
		/// </summary>
		private static Dictionary<string, CodeDomProvider> providerCache = new Dictionary<string, CodeDomProvider>();

		/// <summary>
		/// 
		/// </summary>
		private static string tempDirectory;

		/// <summary>
		/// Holds the temporary files collection.
		/// </summary>
		private static TempFileCollection temps = new TempFileCollection(TempDirectory, false);

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCompiler"/> class.
		/// </summary>
		public TestCompiler()
		{
		}

		#endregion // Construction

		#region Properties

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

		public T Run<T>(TestCompilerSettings settings, string ns, string type, string method, params object[] parameters)
		{
			CompileTestCode(settings);

			// Find the test method to execute
			RuntimeMethod runtimeMethod = FindMethod(
				ns,
				type,
				method
			);

			Debug.Assert(runtimeMethod != null, runtimeMethod.ToString());
			//Debug.Assert(runtimeMethod.Address != null, runtimeMethod.ToString());
			//Debug.Assert(runtimeMethod.Address != IntPtr.Zero, runtimeMethod.ToString());

			// Get delegate name
			string delegateName;

			if (default(T) is System.ValueType)
				delegateName = "Mosa.Test.Prebuilt.Delegates+" + DelegateUtility.GetDelegteName(default(T), parameters);
			else
				delegateName = "Mosa.Test.Prebuilt.Delegates+" + DelegateUtility.GetDelegteName(null, parameters);

			// Get the prebuilt delegate type
			Type delegateType = Prebuilt.GetType(delegateName);

			Debug.Assert(delegateType != null, delegateName);

			//NOTES: Would it be better to get the address from the linker?

			// Create a delegate for the test method
			Delegate fn = Marshal.GetDelegateForFunctionPointer(
				IntPtr.Zero, // runtimeMethod.Address,
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

		// Might not keep this as a public method
		public void CompileTestCode(TestCompilerSettings settings)
		{
			if (cacheSettings == null || !cacheSettings.IsEqual(settings))
			{
				cacheSettings = new TestCompilerSettings(settings);

				string assembly = RunCodeDomCompiler(settings);

				Console.WriteLine("Executing MOSA compiler...");
				RunMosaCompiler(settings, assembly);
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
			foreach (RuntimeType t in typeSystem.GetAllTypes())
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns))
					if (t.Namespace != ns)
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

		private string RunCodeDomCompiler(TestCompilerSettings settings)
		{
			Console.WriteLine("Executing {0} compiler...", settings.Language);

			CodeDomProvider provider;
			if (!providerCache.TryGetValue(settings.Language, out provider))
			{
				provider = CodeDomProvider.CreateProvider(settings.Language);
				if (provider == null)
					throw new NotSupportedException("The language '" + settings.Language + "' is not supported on this machine.");
				providerCache.Add(settings.Language, provider);
			}

			string filename = Path.Combine(TempDirectory, Path.ChangeExtension(Path.GetRandomFileName(), "dll"));
			temps.AddFile(filename, false);

			string[] references = new string[settings.References.Count];
			settings.References.CopyTo(references, 0);

			CompilerResults compileResults;
			CompilerParameters parameters = new CompilerParameters(references, filename, false);
			parameters.CompilerOptions = "/optimize-";

			if (settings.UnsafeCode)
			{
				if (settings.Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /unsafe+";
				else
					throw new NotSupportedException();
			}

			if (settings.DoNotReferenceMscorlib)
			{
				if (settings.Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /nostdlib";
				else
					throw new NotSupportedException();
			}

			parameters.GenerateInMemory = false;

			if (settings.CodeSource != null)
			{
				Console.WriteLine("Code: {0}", settings.CodeSource + settings.AdditionalSource);
				compileResults = provider.CompileAssemblyFromSource(parameters, settings.CodeSource + settings.AdditionalSource);
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

		private void RunMosaCompiler(TestCompilerSettings settings, string assemblyFile)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.InitializePrivatePaths(settings.References);

			foreach (string file in settings.References)
			{
				assemblyLoader.LoadModule(file);
			}

			typeSystem = new TypeSystem();
			typeSystem.LoadModules(assemblyLoader.Modules);

			TestCaseAssemblyCompiler.Compile(typeSystem);
		}

	}
}
