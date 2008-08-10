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
using System.Text;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;
using System.IO;

namespace Mosa.Tools.Compiler
{
    public sealed class AotMethodCompiler : MethodCompilerBase
    {
        #region Construction

        public AotMethodCompiler(IArchitecture architecture, IMetadataModule module, RuntimeType type, RuntimeMethod method)
            : base(architecture, module, type, method)
        {
            this.Pipeline.AddRange(new IMethodCompilerStage[] {
                new ILDecodingStage(),
                new BasicBlockBuilderStage(),
                //InstructionLogger.Instance,
                new InstructionExpansionStage(),
                InstructionLogger.Instance,
                //new StackResolutionStage(),
                //InstructionLogger.Instance,
                //new FunctionCallInliningProcessor(),
                //InstructionLogger.Instance,
                //new EnterSSA(),
                //InstructionLogger.Instance,
                //new ScheduleBasicBlocks(),
                //new LeaveSSA(),
                //new LinearScanRegisterAllocator(),
            });
        }

        #endregion // Construction

        #region Methods

        public override Stream RequestCodeStream()
        {
            // FIXME: Request a stream from the AOT assembly compiler to place the method into, save the rva address 
            // of the native code.
            return Stream.Null;
        }

        #endregion // Methods
    }
}
