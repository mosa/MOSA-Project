/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
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
	public abstract class CodeGenerationStage : ICodeGenerationStage, IMethodCompilerStage
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

		#endregion // Data members

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
			_compiler = compiler;

			// Retrieve a stream to place the code into
			using (_codeStream = _compiler.RequestCodeStream()) {
				// HINT: We need seeking to resolve labels.
				Debug.Assert(_codeStream.CanSeek, @"Can't seek code output stream.");
				Debug.Assert(_codeStream.CanWrite, @"Can't write to code output stream.");

				if (!_codeStream.CanSeek || !_codeStream.CanWrite)
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
		/// Adds the stage to the pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add to.</param>
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
			foreach (BasicBlock block in _compiler.BasicBlocks) {
				BlockStart(block);

				for (Context ctx = new Context(_compiler.InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
					if (ctx.Instruction != null)
						if (!ctx.Ignore) {
							IPlatformInstruction instruction = ctx.Instruction as IPlatformInstruction;
							if (instruction != null)
								instruction.Emit(ctx, _codeStream);
						}

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

	}
}
