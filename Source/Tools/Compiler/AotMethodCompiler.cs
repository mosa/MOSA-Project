/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;
using Mosa.Tools.Compiler.Stages;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Specializes <see cref="AotMethodCompiler"/> for AOT purposes.
    /// </summary>
    public sealed class AotMethodCompiler : MethodCompilerBase
    {
        #region Data Members

        /// <summary>
        /// Holds the aot compiler, which started this method compiler.
        /// </summary>
        private readonly AssemblyCompiler assemblyCompiler;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AotMethodCompiler"/> class.
        /// </summary>
        public AotMethodCompiler(AssemblyCompiler compiler, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method)
            : base(compiler.Pipeline.FindFirst<IAssemblyLinker>(), compiler.Architecture, compilationScheduler, type, method)
        {
            this.assemblyCompiler = compiler;
            this.Pipeline.AddRange(
                new IMethodCompilerStage[] 
                {
    				new DecodingStage(),
    				new InstructionLogger(),
    				new BasicBlockBuilderStage(),
    				new InstructionLogger(),
    				new OperandDeterminationStage(),
                    StaticAllocationResolutionStageWrapper.Instance,
    				new InstructionLogger(),
    				new CILTransformationStage(),
    				new InstructionLogger(),
    				//InstructionStatisticsStage.Instance,
    				//new DominanceCalculationStage(),
    				//new InstructionLogger(),
    				//new EnterSSA(),
    				//new InstructionLogger(),
    				//new ConstantPropagationStage(),
    				//new InstructionLogger(),
    				//new ConstantFoldingStage(),
    				//new StrengthReductionStage(),
    				//new InstructionLogger(),
    				//new LeaveSSA(),
    				//new InstructionLogger(),
    				new StackLayoutStage(),
    				new PlatformStubStage(),
    				//new InstructionLogger(),
    				//new BlockReductionStage(),
    				new LoopAwareBlockOrderStage(),
    				new InstructionLogger(),
    				//new SimpleTraceBlockOrderStage(),
    				//new ReverseBlockOrderStage(),	
    				//new LocalCSE(),
    				new CodeGenerationStage(),
                });
        }

        #endregion // Construction

        #region MethodCompilerBase Overrides

        /// <summary>
        /// Called after the method compiler has finished compiling the method.
        /// </summary>
        protected override void EndCompile()
        {
            // If we're compiling a type initializer, run it immediately.
            const MethodAttributes attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
            if ((Method.Attributes & attrs) == attrs && Method.Name == ".cctor")
            {
                var tiss = this.assemblyCompiler.Pipeline.FindFirst<TypeInitializers.ITypeInitializerSchedulerStage>();
                tiss.Schedule(Method);
            }

            base.EndCompile();
        }

        #endregion // MethodCompilerBase Overrides
    }
}
