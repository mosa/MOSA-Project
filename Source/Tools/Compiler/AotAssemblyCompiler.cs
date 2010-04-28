/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;
using Mosa.Tools.Compiler.Linkers;
using Mosa.Tools.Compiler.TypeInitializers;

namespace Mosa.Tools.Compiler
{
    public class AotAssemblyCompiler : AssemblyCompiler
    {
        public AotAssemblyCompiler(IArchitecture architecture, IMetadataModule assembly, ITypeInitializerSchedulerStage typeInitializerSchedulerStage, IAssemblyLinker linker)
            : base(architecture, assembly)
        {
                this.Pipeline.AddRange(
                    new IAssemblyCompilerStage[] 
                    {
                        new TypeLayoutStage(),
                        new AssemblyMemberCompilationSchedulerStage(),
                        new MethodCompilerSchedulerStage(),
                        new TypeInitializerSchedulerStageProxy(typeInitializerSchedulerStage),
                        new LinkerProxy(linker)
                    });
        }

        public void Run()
        {
            // Build the default assembly compiler pipeline
            this.Architecture.ExtendAssemblyCompilerPipeline(this.Pipeline);

            // Run the compiler
            this.Compile();            
        }

        public override MethodCompilerBase CreateMethodCompiler(ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method)
        {
            MethodCompilerBase mc = new AotMethodCompiler(
                this,
                compilationScheduler,
                type,
                method
            );
            this.Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
            return mc;            
        }
    }
}
