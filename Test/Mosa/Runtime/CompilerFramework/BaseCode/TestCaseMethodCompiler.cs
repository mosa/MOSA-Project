/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System.IO;
using System.Runtime.InteropServices;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
    class TestCaseMethodCompiler : MethodCompilerBase
    {
        public TestCaseMethodCompiler(IAssemblyLinker linker, IArchitecture architecture, IMetadataModule module, RuntimeType type, RuntimeMethod method) :
            base(linker, architecture, module, type, method)
        {
            // Populate the pipeline
            this.Pipeline.AddRange(new IMethodCompilerStage[] {
                new ILDecodingStage(),
                //InstructionLogger.Instance,
                new BasicBlockBuilderStage(),
                //InstructionLogger.Instance,
                new CilToIrTransformationStage(),
                //InstructionLogger.Instance,

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
				//new BasicBlockReduction(),
                //InstructionLogger.Instance,
                new StackLayoutStage(),
                //InstructionLogger.Instance,
            });
        }

        private delegate void CCtor();

        /// <summary>
        /// Called after the method compiler has finished compiling the method.
        /// </summary>
        protected override void EndCompile()
        {
            // If we're compiling a type initializer, run it immediately.
            MethodAttributes attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
            if ((this.Method.Attributes & attrs) == attrs && this.Method.Name == ".cctor")
            {
                CCtor cctor = (CCtor)Marshal.GetDelegateForFunctionPointer(this.Method.Address, typeof(CCtor));
                cctor();
            }

            base.EndCompile();
        }
    }
}
