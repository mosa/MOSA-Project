/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler
{

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
    public sealed class AotCompiler : AssemblyCompiler
    {
        /// <summary>
        /// 
        /// </summary>
        ObjectFileBuilderBase _objectFileBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="architecture"></param>
        /// <param name="assembly"></param>
        /// <param name="objectFileBuilder"></param>
        public AotCompiler(IArchitecture architecture, IMetadataModule assembly, ObjectFileBuilderBase objectFileBuilder)
            : base(architecture, assembly)
        {
            this._objectFileBuilder = objectFileBuilder;
            // Build the assembly compiler pipeline
            Pipeline.AddRange(new IAssemblyCompilerStage[] {
                new TypeLayoutStage(),
                new MethodCompilerBuilderStage(),
                new MethodCompilerRunnerStage(),
                new AotLinkerStage(objectFileBuilder),
            });
            architecture.ExtendAssemblyCompilerPipeline(Pipeline);
        }

        /// <summary>
        /// Compiles an entire assemblyName.
        /// </summary>
        /// <param name="architecture">The architecture to compile for</param>
        /// <param name="assemblyName">The compiled assembly name</param>
        /// <param name="objectFileBuilder">The objectfilebuilder to use</param>
        public static void Compile(IArchitecture architecture, string assemblyName, ObjectFileBuilderBase objectFileBuilder)
        {
            IMetadataModule assembly = RuntimeBase.Instance.AssemblyLoader.Load(assemblyName);
            AotCompiler c = new AotCompiler(architecture, assembly, objectFileBuilder);
            c.Compile();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void BeginCompile()
        {
            _objectFileBuilder.OnAssemblyCompileBegin(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void EndCompile()
        {
            _objectFileBuilder.OnAssemblyCompileEnd(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public override MethodCompilerBase CreateMethodCompiler(RuntimeType type, RuntimeMethod method)
        {
            IArchitecture arch = this.Architecture;
            MethodCompilerBase mc = new AotMethodCompiler(
                this.Pipeline.Find<IAssemblyLinker>(),
                this.Architecture,
                this.Assembly, 
                type,
                method,
                _objectFileBuilder
            );
            arch.ExtendMethodCompilerPipeline(mc.Pipeline);
            return mc;
        }
    }
}
