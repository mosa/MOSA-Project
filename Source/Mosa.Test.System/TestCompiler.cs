/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using MbUnit.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Test.CodeDomCompiler;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mosa.Test.System
{
	public class TestCompiler
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		private TypeSystem typeSystem;

		/// <summary>
		///
		/// </summary>
		private CompilerSettings cacheSettings = null;

		/// <summary>
		///
		/// </summary>
		private ILinker linker;

		/// <summary>
		/// The memory size to allocate
		/// </summary>
		private const uint memorySize = 1024 * 1024 * 128;

		/// <summary>
		/// The memory allocated
		/// </summary>
		private long memoryAllocated = 0;

		private bool setKernelMemory = false;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCompiler"/> class.
		/// </summary>
		public TestCompiler()
		{
			AllocateMemory();
		}

		#endregion Construction

		protected void AllocateMemory()
		{
			if (memoryAllocated == 0)
			{
				setKernelMemory = false;

				memoryAllocated = Win32Memory.Allocate(0, memorySize, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.WriteCombine);

				if (memoryAllocated == 0)
					throw new OutOfMemoryException();
			}
		}

		protected void SetKernelMemory(CompilerSettings settings)
		{
			setKernelMemory = true; // must be before the Run method

			Run<uint>(settings, "Mosa.Kernel.x86Test", "KernelMemory", "SetMemory", new object[] { (uint)memoryAllocated });
		}

		public T Run<T>(CompilerSettings settings, string ns, string type, string method, params object[] parameters)
		{
			CompileTestCode(settings);

			if (!setKernelMemory)
			{
				SetKernelMemory(settings);
			}

			// Find the test method to execute
			MosaMethod runtimeMethod = FindMethod(
				ns,
				type,
				method,
				parameters
			);

			Debug.Assert(runtimeMethod != null, runtimeMethod.ToString());

			// Get delegate name
			string delegateName;

			if (default(T) is ValueType)
				delegateName = "Mosa.Test.Prebuilt.Delegates+" + DelegateUtility.GetDelegteName(default(T), parameters);
			else
				delegateName = "Mosa.Test.Prebuilt.Delegates+" + DelegateUtility.GetDelegteName(null, parameters);

			// Get the prebuilt delegate type
			Type delegateType = Prebuilt.GetType(delegateName);

			Debug.Assert(delegateType != null, delegateName);

			LinkerSymbol symbol = linker.GetSymbol(runtimeMethod.FullName);
			LinkerSection section = linker.GetSection(symbol.SectionKind);

			long address = symbol.VirtualAddress;

			// Create a delegate for the test method
			Delegate fn = Marshal.GetDelegateForFunctionPointer(new IntPtr(address), delegateType);

			// Execute the test method
			object tempResult = fn.DynamicInvoke(parameters);

			try
			{
				if (default(T) is ValueType)
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
		public void CompileTestCode(CompilerSettings settings)
		{
			if (cacheSettings == null || !cacheSettings.IsEqual(settings))
			{
				cacheSettings = new CompilerSettings(settings);

				CompilerResults results = Mosa.Test.CodeDomCompiler.Compiler.ExecuteCompiler(cacheSettings);

				Assert.IsFalse(results.Errors.HasErrors, "Failed to compile source codeReader with native compiler");

				linker = RunMosaCompiler(settings, results.PathToAssembly);

				setKernelMemory = false;
			}
		}

		/// <summary>
		/// Finds a runtime method, which represents the requested method.
		/// </summary>
		/// <param name="ns">The namespace of the sought method.</param>
		/// <param name="type">The type, which contains the sought method.</param>
		/// <param name="method">The method to find.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>
		/// An instance of <see cref="MosaMethod" />.
		/// </returns>
		/// <exception cref="System.MissingMethodException"></exception>
		/// <exception cref="MissingMethodException">The sought method is not found.</exception>
		private MosaMethod FindMethod(string ns, string type, string method, params object[] parameters)
		{
			foreach (MosaType t in typeSystem.AllTypes)
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns))
					if (t.Namespace != ns)
						continue;

				foreach (MosaMethod m in t.Methods)
				{
					if (m.Name == method)
					{
						if (m.Signature.Parameters.Count == parameters.Length)
						{
							return m;
						}
					}
				}

				break;
			}

			throw new MissingMethodException(ns + "." + type, method);
		}

		private TestLinker RunMosaCompiler(CompilerSettings settings, string assemblyFile)
		{
			MosaModuleLoader moduleLoader = new MosaModuleLoader();

			moduleLoader.AddPrivatePath(settings.References);

			moduleLoader.LoadModuleFromFile(assemblyFile);

			foreach (string file in settings.References)
			{
				moduleLoader.LoadModuleFromFile(file);
			}

			typeSystem = TypeSystem.Load(moduleLoader.CreateMetadata());

			return TestCaseCompiler.Compile(typeSystem);
		}
	}
}