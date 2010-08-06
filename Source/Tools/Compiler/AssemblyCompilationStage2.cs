/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Tools.Compiler.Symbols.Pdb;
using Mosa.Tools.Compiler.TypeInitializers;

namespace Mosa.Tools.Compiler
{
	public class AssemblyCompilationStage2 : BaseAssemblyCompilerStage, IAssemblyCompilerStage
	{
		private readonly List<IMetadataModule> modules = new List<IMetadataModule>();

		private IAssemblyLinker linker;

		private ITypeInitializerSchedulerStage typeInitializerSchedulerStage;

		public AssemblyCompilationStage2(IList<IMetadataModule> modules)
		{
			foreach (IMetadataModule module in modules)
				this.modules.Add(module);
		}

		#region IPipelineStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"AssemblyCompilationStage2"; } }

		#endregion // IPipelineStage

		#region IAssemblyCompilerStage

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			typeInitializerSchedulerStage = compiler.Pipeline.FindFirst<ITypeInitializerSchedulerStage>();

			if (typeInitializerSchedulerStage == null)
				throw new InvalidOperationException(@"AssemblyCompilationStage needs a ITypeInitializerSchedulerStage.");

			linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();

			if (linker == null)
				throw new InvalidOperationException(@"AssemblyCompilationStage needs a linker.");

		}

		void IAssemblyCompilerStage.Run()
		{
			foreach (IMetadataModule assembly in modules)
				CompileAssembly(assembly);
		}

		#endregion IAssemblyCompilerStage

		private void CompileAssembly(IMetadataModule assembly)
		{
			using (AotAssemblyCompiler assemblyCompiler = new AotAssemblyCompiler(architecture, assembly, typeInitializerSchedulerStage, linker, typeSystem, assemblyLoader))
			{
				assemblyCompiler.Run();
			}
		}
	}
}
