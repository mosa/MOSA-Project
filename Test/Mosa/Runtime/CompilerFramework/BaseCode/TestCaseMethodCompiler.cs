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
using System.IO;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime;
using Mosa.Runtime.Loader;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
    class TestCaseMethodCompiler : MethodCompilerBase
    {
        private Stream _stream;

        public TestCaseMethodCompiler(IAssemblyLinker linker, IArchitecture architecture, IMetadataModule module, RuntimeType type, RuntimeMethod method) :
            base(linker, architecture, module, type, method)
        {
            // Request 64K of memory
            _stream = new VirtualMemoryStream(RuntimeBase.Instance.MemoryManager, 16 * 4096);

            // Populate the pipeline
            this.Pipeline.AddRange(new IMethodCompilerStage[] {
                new ILDecodingStage(),
                new BasicBlockBuilderStage(),
                //InstructionLogger.Instance,
                new InstructionExpansionStage(),
                //InstructionLogger.Instance,

                new DominanceCalculationStage(),
                new EnterSSA(),
                //InstructionLogger.Instance,
                new ConstantPropagationStage(),
                new ConstantFoldingStage(),
                new LeaveSSA(),
				//new BasicBlockReduction(),	// Experimental
                InstructionLogger.Instance,

                new StackLayoutStage(),
                new InstructionExpansionStage(),

                //InstructionLogger.Instance,
                //new FunctionCallInliningProcessor(),
                //InstructionLogger.Instance,
                //new ScheduleBasicBlocks(),
                //new LeaveSSA(),
                //new LinearScanRegisterAllocator(),
            });
        }


        public override Stream RequestCodeStream()
        {
            return _stream;
        }
    }
}
