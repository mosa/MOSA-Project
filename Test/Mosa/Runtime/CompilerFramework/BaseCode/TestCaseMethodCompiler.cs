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
using System;

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
				new OperandDeterminationStage(),
                //InstructionLogger.Instance,
                //new ConstantFoldingStage(),
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
                //InstructionLogger.Instance,
                //new LeaveSSA(),
                //InstructionLogger.Instance,
				new StackLayoutStage(),
				new BlockReductionStage(),
				new LoopAwareBlockOrderStage(),
				//new SimpleTraceBlockOrderStage(),
				//new ReverseBlockOrderStage(),  // reverse all the basic Blocks and see if it breaks anything
				new BasicBlockOrderStage(),
                new LocalCSE()
            });
        }

        private delegate void CCtor();

        public override Stream RequestCodeStream()
        {
            LinkerStream stream = base.RequestCodeStream() as LinkerStream;
            VirtualMemoryStream vms = (VirtualMemoryStream)stream.BaseStream;
            if (this.Method.Address == IntPtr.Zero)
                this.Method.Address = new IntPtr(vms.Base.ToInt64() + vms.Position);
            return stream;
        }

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
            //InstructionStatisticsStage.Instance.PrintStatistics();
        }
    }
}
