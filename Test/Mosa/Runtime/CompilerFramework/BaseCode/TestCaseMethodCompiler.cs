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


        public override Stream RequestCodeStream()
        {
            return this.Linker.Allocate(this.Method, LinkerSection.Text, 0, 0);
        }
    }
}
