/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Ir;
using Mosa.Platforms.x86;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler {

	/// <summary>
	/// Implements the ahead of time compiler.
	/// </summary>
	/// <remarks>
    /// This class implements the ahead of time compiler for MOSA. The AoT uses 
	/// the compiler services offered in Mosa.Runtime.CompilerFramework in order
	/// to share as much code as possible with assembly jit compiler in MOSA. The 
	/// primary difference between the two compilers is primarily the number and
	/// quality of compilation stages used. The AoT compiler makes use of assembly lot
	/// more optimizations than the jit. The jit is tweaked for execution speed. 
	/// </remarks>
	sealed class AotCompiler : AssemblyCompiler {

        public AotCompiler(IArchitecture architecture, IMetadataModule assembly)
			: base(architecture, assembly)
		{
            // Build the assembly compiler pipeline
            CompilerPipeline<IAssemblyCompilerStage> pipeline = this.Pipeline;
            pipeline.AddRange(new IAssemblyCompilerStage[] {
                new TypeLayoutStage(),
                new MethodCompilerBuilderStage()
            });
            architecture.ExtendAssemblyCompilerPipeline(pipeline);
		}

		/// <summary>
		/// Compiles an entire assemblyName.
		/// </summary>
		/// <param name="assemblyName">The compiled assemblyName.</param>
		public static void Compile(string assemblyName)
		{
            IMetadataModule assembly = RuntimeBase.Instance.AssemblyLoader.Load(assemblyName);

            // FIXME: Use compiler command line options to determine architecture and feature flags
            IArchitecture architecture = Mosa.Platforms.x86.Architecture.CreateArchitecture(assembly.Metadata, ArchitectureFeatureFlags.AutoDetect);

            AotCompiler c = new AotCompiler(architecture, assembly);
            c.Compile();
		}

        public override MethodCompilerBase CreateMethodCompiler(RuntimeType type, RuntimeMethod method)
        {
            IArchitecture arch = this.Architecture;
            MethodCompilerBase mc = new AotMethodCompiler(this.Architecture, this.Assembly, type, method);
            arch.ExtendMethodCompilerPipeline(mc.Pipeline);
            return mc;
        }
	}
}
