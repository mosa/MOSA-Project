/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AotMethodCompiler : MethodCompilerBase
    {
        /// <summary>
        /// 
        /// </summary>
        MemoryStream stream = new MemoryStream();

        /// <summary>
        /// 
        /// </summary>
        ObjectFileBuilderBase _objectFileBuilder;

        #region Construction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linker"></param>
        /// <param name="architecture"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="objectFileBuilder"></param>
        public AotMethodCompiler(IAssemblyLinker linker, IArchitecture architecture, IMetadataModule module, RuntimeType type, RuntimeMethod method, ObjectFileBuilderBase objectFileBuilder)
            : base(linker, architecture, module, type, method)
        {
            _objectFileBuilder = objectFileBuilder;
            Pipeline.AddRange(new IMethodCompilerStage[] {
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

        /// <summary>
        /// 
        /// </summary>
        protected override void BeginCompile()
        {
            _objectFileBuilder.OnMethodCompileBegin(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void EndCompile()
        {
            _objectFileBuilder.OnMethodCompileEnd(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Stream RequestCodeStream()
        {
            // FIXME: Request a stream from the AOT assembly compiler to place the method into, save the rva address 
            // of the native code.
            return stream;
        }

        #endregion // Methods
    }
}
