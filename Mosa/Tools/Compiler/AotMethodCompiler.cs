/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.CIL;
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
            aotCompiler = compiler;
            Pipeline.AddRange(new IMethodCompilerStage[] {
				new DecodingStage(),
                //InstructionLogger.Instance,
                new BasicBlockBuilderStage(),
				new OperandDeterminationStage(),
                //InstructionLogger.Instance,
                new CilToIrTransformationStage(),
                //InstructionLogger.Instance,
                //InstructionStatisticsStage.Instance,
                //new DominanceCalculationStage(),
                //InstructionLogger.Instance,
                //new EnterSSA(),
                //InstructionLogger.Instance,
                //new ConstantPropagationStage(),
                //InstructionLogger.Instance,
                //new ConstantFoldingStage(),
				//new StrengthReductionStage(),
                //InstructionLogger.Instance,
                //new LeaveSSA(),
                //InstructionLogger.Instance,
				//InstructionLogger.Instance,
                new StackLayoutStage(),
				//InstructionLogger.Instance,
				//new BlockReductionStage(),
				new LoopAwareBlockOrderStage(),
				new SimpleTraceBlockOrderStage(),
				new BasicBlockOrderStage(),		
                InstructionStatisticsStage.Instance,
		        //new LocalCSE(),
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
            if ((Method.Attributes & attrs) == attrs && Method.Name == ".cctor")
            {
                TypeInitializers.TypeInitializerSchedulerStage tiss = aotCompiler.Pipeline.Find<TypeInitializers.TypeInitializerSchedulerStage>();
                tiss.Schedule(Method);
            }

            base.EndCompile();
        }

        #endregion // MethodCompilerBase Overrides
    }
}
