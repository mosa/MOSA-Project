/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Specializes <see cref="MethodCompilerBase"/> for AOT purposes.
    /// </summary>
    public sealed class AotMethodCompiler : MethodCompilerBase
    {
        #region Data Members

        /// <summary>
        /// Holds the aot compiler, which started this method compiler.
        /// </summary>
        private AotCompiler aotCompiler;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AotMethodCompiler"/> class.
        /// </summary>
        /// <param name="compiler">The AOT assembly compiler.</param>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        public AotMethodCompiler(AotCompiler compiler, RuntimeType type, RuntimeMethod method)
            : base(compiler.Pipeline.Find<IAssemblyLinker>(), compiler.Architecture, compiler.Assembly, type, method)
        {
            this.aotCompiler = compiler;
            this.Pipeline.AddRange(new IMethodCompilerStage[] {
                new ILDecodingStage(),
                //InstructionLogger.Instance,
                new BasicBlockBuilderStage(),
                //InstructionLogger.Instance,
                new ConstantFoldingStage(),
                new CilToIrTransformationStage(),
                //InstructionLogger.Instance,
                InstructionStatisticsStage.Instance,
                new DominanceCalculationStage(),
                //InstructionLogger.Instance,
                new EnterSSA(),
                //InstructionLogger.Instance,
                new ConstantPropagationStage(),
                //InstructionLogger.Instance,
                new ConstantFoldingStage(),
                //InstructionLogger.Instance,
                new LeaveSSA(),
                //InstructionLogger.Instance,
                //InstructionLogger.Instance,
                new StackLayoutStage(),
				new BasicBlockReduction(),
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
            MethodAttributes attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
            if ((this.Method.Attributes & attrs) == attrs && this.Method.Name == ".cctor")
            {
                TypeInitializers.TypeInitializerSchedulerStage tiss = this.aotCompiler.Pipeline.Find<TypeInitializers.TypeInitializerSchedulerStage>();
                tiss.Schedule(this.Method);
            }

            base.EndCompile();

            InstructionStatisticsStage.Instance.PrintStatistics();
        }

        #endregion // MethodCompilerBase Overrides
    }
}
