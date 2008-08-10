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

using Mosa.Runtime.CompilerFramework;
using Mosa.Platforms.x86;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;

namespace cltester
{
    class TestCaseAssemblyCompiler : AssemblyCompiler
    {
        private TestCaseAssemblyCompiler(IArchitecture architecture, IMetadataModule module) :
            base(architecture, module)
        {
            // Build the assembly compiler pipeline
            CompilerPipeline<IAssemblyCompilerStage> pipeline = this.Pipeline;
            pipeline.AddRange(new IAssemblyCompilerStage[] {
                new TypeLayoutStage(),
                new MethodCompilerBuilderStage()
            });
            architecture.ExtendAssemblyCompilerPipeline(pipeline);
        }

        public static void Compile(IMetadataModule module)
        {
            IArchitecture architecture = Mosa.Platforms.x86.Architecture.CreateArchitecture(module.Metadata, ArchitectureFeatureFlags.AutoDetect);
            new TestCaseAssemblyCompiler(architecture, module).Compile();
        }

        public override MethodCompilerBase CreateMethodCompiler(RuntimeType type, RuntimeMethod method)
        {
            IArchitecture arch = this.Architecture;
            MethodCompilerBase mc = new TestCaseMethodCompiler(this.Architecture, this.Assembly, type, method);
            arch.ExtendMethodCompilerPipeline(mc.Pipeline);
            return mc;
        }
    }
}
