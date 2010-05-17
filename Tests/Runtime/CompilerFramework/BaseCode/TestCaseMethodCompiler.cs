/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.IO;
using System.Runtime.InteropServices;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Tools.Compiler;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
    class TestCaseMethodCompiler : MethodCompilerBase
    {
        private readonly TestCaseAssemblyCompiler assemblyCompiler;

        public TestCaseMethodCompiler(TestCaseAssemblyCompiler assemblyCompiler, IAssemblyLinker linker, IArchitecture architecture, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method) :
            base(linker, architecture, compilationScheduler, type, method)
        {
            this.assemblyCompiler = assemblyCompiler;

            // Populate the pipeline
            this.Pipeline.AddRange(new IMethodCompilerStage[] {
                new DecodingStage(),
                //new InstructionLogger(),
                new BasicBlockBuilderStage(),
                //new InstructionLogger(),
				new OperandDeterminationStage(),
                new InstructionLogger(),
                new StaticAllocationResolutionStage(),
                //new InstructionLogger(),
                //new ConstantFoldingStage(),
                new CILTransformationStage(),
                //new InstructionLogger(),
                new CILLeakGuardStage() { MustThrowCompilationException = true },
                //new InstructionLogger(),
				//InstructionStatisticsStage.Instance,
                //new DominanceCalculationStage(),
                //new EnterSSA(),
                //new ConstantPropagationStage(),
                //new ConstantFoldingStage(),
                //new LeaveSSA(),
				new StackLayoutStage(),
				new PlatformStubStage(),
                //new InstructionLogger(),
				//new BlockReductionStage(),
				new LoopAwareBlockOrderStage(),
				//new SimpleTraceBlockOrderStage(),
				//new ReverseBlockOrderStage(),  // reverse all the basic blocks and see if it breaks anything
				//new BasicBlockOrderStage()	
				new CodeGenerationStage(),
                //new InstructionLogger(),
            });
        }

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
                this.assemblyCompiler.QueueCCtorForInvocationAfterCompilation(cctor);
            }

            base.EndCompile();
        }
    }
}
