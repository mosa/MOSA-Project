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
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

using MbUnit.Framework;

using Mosa.Runtime;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;

namespace Test.Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Interface class for MbUnit3 to run our testcases.
	/// </summary>
	public abstract class MosaCompilerTestRunner
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		ITypeSystem typeSystem;

		/// <summary>
		/// The filename of the assembly, which contains the test case.
		/// </summary>
		string assembly = null;

		/// <summary>
		/// Flag, which determines if the compiler needs to run.
		/// </summary>
		bool needCompile = true;

		/// <summary>
		/// An array of assembly references to include in the compilation.
		/// </summary>
		string[] references;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MosaCompilerTestRunner"/> class.
		/// </summary>
		public MosaCompilerTestRunner()
		{
			references = new string[0];
			needCompile = true;
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
		/// Gets or sets the references.
		/// </summary>
		/// <value>The references.</value>
		protected string[] References
		{
			get { return references; }
			set { if (references != value) needCompile = true; references = value; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Builds the test runtime used to execute tests.
		/// </summary>
		[FixtureSetUp]
		public void Begin()
		{
			Console.WriteLine("Building runtime...");
		}

		/// <summary>
		/// Runs a test case.
		/// </summary>
		/// <typeparam name="T">The return type of the test method.</typeparam>
		/// <param name="ns">The namespace of the test.</param>
		/// <param name="type">The type, which contains the test.</param>
		/// <param name="method">The name of the method of the test.</param>
		/// <param name="parameters">The parameters to pass to the test.</param>
		/// <returns>The result of the test.</returns>
		public T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			// Do we need to compile the code?
			if (needCompile)
			{
				assembly = CompileTestCode(ns, type, method);
				Console.WriteLine("Executing MOSA compiler...");
				RunMosaCompiler(assembly);
				needCompile = false;
			}

			// Find the test method to execute
			RuntimeMethod runtimeMethod = FindMethod(
				ns,
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

			if (delegateType == null)
				throw new System.Exception(delegateName);

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

		/// <summary>
		/// Compiles the test code.
		/// </summary>
		/// <param name="ns">The namespace of the test.</param>
		/// <param name="type">The type, which contains the test.</param>
		/// <param name="method">The name of the method of the test.</param>
		/// <exception cref="NotSupportedException">Compilation is not supported.</exception>
		/// <exception cref="Exception">A generic exception during compilation.</exception>
		/// <returns>The name of the compiled assembly file.</returns>
		protected abstract string CompileTestCode(string ns, string type, string method);

		/// <summary>
		/// Loads the specified assembly into the mosa runtime and executes the mosa compiler.
		/// </summary>
		/// <param name="assemblyFile">The assembly file name.</param>
		/// <returns>The metadata module, which represents the loaded assembly.</returns>
		private void RunMosaCompiler(string assemblyFile)
		{
			List<string> files = new List<string>();
			files.Add(assemblyFile);

			IAssemblyLoader assemblyLoader = new AssemblyLoader();

			typeSystem = new DefaultTypeSystem(assemblyLoader);
			typeSystem.LoadModules(files);

			TestCaseAssemblyCompiler.Compile(typeSystem, assemblyLoader);
		}

		#endregion // Methods

	}
}
