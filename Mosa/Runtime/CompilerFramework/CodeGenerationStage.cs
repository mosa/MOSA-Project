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
using System.Diagnostics;
using System.IO;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Base class for code generation stages.
    /// </summary>
    /// <typeparam name="ContextType">Specifies the context type used by the code generation stage for its visitors.</typeparam>
    public abstract class CodeGenerationStage<ContextType> : ICodeGenerationStage, IMethodCompilerStage, IInstructionVisitor<ContextType>
    {
        #region Data members

        /// <summary>
        /// Holds the stream, where code is emitted to.
        /// </summary>
        protected Stream _codeStream;

        /// <summary>
        /// Holds the method compiler, which is executing this stage.
        /// </summary>
        protected IMethodCompiler _compiler;

        // <summary>
        // Maps label targets as instruction offsets.
        // </summary>
        //private Dictionary<int, int> _labels;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenerationStage{ContextType}"/> class.
        /// </summary>
        protected CodeGenerationStage()
        {
        }

        #endregion // Construction

        #region IMethodCompilerStage members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"CodeGeneration"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(IMethodCompiler compiler)
		{
            // Check preconditions
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            _compiler = compiler;

            // Retrieve a stream to place the code into
            using (_codeStream = _compiler.RequestCodeStream())
            {
                // HINT: We need seeking to resolve labels.
                Debug.Assert(true == _codeStream.CanSeek, @"Can't seek code output stream.");
                Debug.Assert(true == _codeStream.CanWrite, @"Can't write to code output stream.");
                if (false == _codeStream.CanSeek || false == _codeStream.CanWrite)
                    throw new NotSupportedException(@"Code stream doesn't support seeking or writing.");

                // Emit method prologue
                BeginGenerate();

                // Emit all instructions
                EmitInstructions();

                // Emit the method epilogue
                EndGenerate();
            }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public abstract void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline);

        #endregion // IMethodCompilerStage members

        #region Methods

        /// <summary>
        /// Notifies the derived class about the start of code generation.
        /// </summary>
        protected abstract void BeginGenerate();

        /// <summary>
        /// Called to emit a list of instructions offered by the instruction provider.
        /// </summary>
        protected virtual void EmitInstructions()
        {
            ContextType ct = default(ContextType);

            // Retrieve the latest basic block decoder
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)_compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            Debug.Assert(blockProvider != null, @"Code generation requires a basic block provider.");
            if (blockProvider == null)
                throw new InvalidOperationException(@"Code generation requires a basic block provider.");

            foreach (BasicBlock block in blockProvider)
            {
                BlockStart(block);
                foreach (Instruction instruction in block.Instructions)
                    if (!instruction.Ignore)
                        instruction.Visit<ContextType>(this, ct);
 
				BlockEnd(block);
            }
        }

        /// <summary>
        /// Notifies a derived class about start of code generation for a block.
        /// </summary>
        /// <param name="block">The started block.</param>
        protected virtual void BlockStart(BasicBlock block)
        {
        }

        /// <summary>
        /// Notifies a derived class about completion of code generation for a block.
        /// </summary>
        /// <param name="block">The completed block.</param>
        protected virtual void BlockEnd(BasicBlock block)
        {
        }

        /// <summary>
        /// Notifies the derived class the code generation completed.
        /// </summary>
        protected abstract void EndGenerate();

        #endregion // Methods

        #region IInstructionVisitor<ContextType> Members

        /// <summary>
        /// Visitation method for instructions not caught by more specific visitation methods.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">A visitation context argument.</param>
        void IInstructionVisitor<ContextType>.Visit(Instruction instruction, ContextType arg)
        {
            Trace.WriteLine(String.Format(@"Unknown instruction {0} has visited CodeGenerationStage<ContextType>.", instruction.GetType().FullName));
            throw new NotSupportedException();
        }

        #endregion // IInstructionVisitor<ContextType> Members
    }
}
