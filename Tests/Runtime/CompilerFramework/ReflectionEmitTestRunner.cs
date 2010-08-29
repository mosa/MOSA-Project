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
using System.Reflection.Emit;
using System.Text;
using System.Reflection;
using System.IO;

namespace Test.Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// A test runner base class for tests using Reflection.Emit.
	/// </summary>
	public abstract class ReflectionEmitTestRunner : MosaCompilerTestRunner
	{
		#region Types

		/// <summary>
		/// A delegate used to generate IL.
		/// </summary>
		protected delegate void GenerateIL(ILGenerator generator);

		#endregion // Types

		#region Data members

		/// <summary>
		/// Holds the generator for the current test.
		/// </summary>
		private GenerateIL generator;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ReflectionEmitTestRunner"/> class.
		/// </summary>
		protected ReflectionEmitTestRunner()
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the IL generator.
		/// </summary>
		/// <value>The generator.</value>
		protected GenerateIL Generator
		{
			get { return this.generator; }
			set
			{
				this.generator = value;
				this.NeedCompile = true;
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
		/// <returns>The name of the compiled assembly file.</returns>
		/// <exception cref="NotSupportedException">Compilation is not supported.</exception>
		/// <exception cref="Exception">A generic exception during compilation.</exception>
		protected override string CompileTestCode<TDelegate>(string ns, string type, string method)
		{
			string fileName = Path.GetTempFileName();
			File.Delete(fileName);
			fileName = fileName.Replace(".tmp", ".dll");
			Environment.CurrentDirectory = Path.GetDirectoryName(fileName);
			AssemblyName assemblyName = new AssemblyName(Path.GetFileName(fileName));
			AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
			ModuleBuilder moduleBuilder = builder.DefineDynamicModule(assemblyName.Name, Path.GetFileName(fileName));

			string typeName;
			if (String.IsNullOrEmpty(ns) == false)
				typeName = ns + '.' + type;
			else
				typeName = type;

			TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
			ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.RTSpecialName | MethodAttributes.SpecialName, CallingConventions.Standard, new Type[0]);
			ILGenerator ctorIL = ctorBuilder.GetILGenerator(1);
			ctorIL.Emit(OpCodes.Ret);

			MethodBuilder methodBuilder = typeBuilder.DefineMethod(method, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig);

			MethodInfo invokeMethod = typeof(TDelegate).GetMethod("Invoke");
			ParameterInfo[] paramInfos = invokeMethod.GetParameters();
			Type[] paramTypes = new Type[paramInfos.Length];
			int i = 0;
			foreach (ParameterInfo pi in paramInfos)
				paramTypes[i++] = pi.ParameterType;

			ILGenerator ilgen = methodBuilder.GetILGenerator();
			this.generator(ilgen);

			Type fullType = typeBuilder.CreateType();

			string curDir = Environment.CurrentDirectory;
			builder.Save(Path.GetFileName(fileName));
			Environment.CurrentDirectory = curDir;
			return fileName;
		}

		#endregion // MosaCompilerTestRunner Overrides
	}
}
