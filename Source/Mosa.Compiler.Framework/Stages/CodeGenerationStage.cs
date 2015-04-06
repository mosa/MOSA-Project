/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Trace;
using System;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Base class for code generation stages.
	/// </summary>
	public class CodeGenerationStage : BaseMethodCompilerStage
	{
		#region Data members

		/// <summary>
		/// Holds the stream, where code is emitted to.
		/// </summary>
		protected Stream codeStream;

		/// <summary>
		///
		/// </summary>
		protected BaseCodeEmitter codeEmitter;

		private int generatedInstructionCount = 0;
		private int generatedBlockCount = 0;

		#endregion Data members

		#region Properties

		public BaseCodeEmitter CodeEmitter { get { return codeEmitter; } }

		public bool EmitBinary { get; set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeGenerationStage"/> class.
		/// </summary>
		public CodeGenerationStage()
		{
			EmitBinary = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeGenerationStage" /> class.
		/// </summary>
		/// <param name="emitBinary">if set to <c>true</c> [emit binary].</param>
		public CodeGenerationStage(bool emitBinary)
		{
			EmitBinary = emitBinary;
		}

		#endregion Construction

		protected override void Run()
		{
			if (!EmitBinary)
				return;

			var symbol = MethodCompiler.Linker.CreateSymbol(MethodCompiler.Method.FullName, SectionKind.Text, 0, 0);
			codeStream = symbol.Stream;

			// Retrieve a stream to place the code into

			// HINT: We need seeking to resolve labels.
			Debug.Assert(codeStream.CanSeek, @"Can't seek codeReader output stream2.");
			Debug.Assert(codeStream.CanWrite, @"Can't write to codeReader output stream2.");

			if (!codeStream.CanSeek || !codeStream.CanWrite)
				throw new NotSupportedException(@"Code stream2 doesn't support seeking or writing.");

			// Emit method prologue
			BeginGenerate();

			// Emit all instructions
			EmitInstructions();

			// Emit the method epilogue
			EndGenerate();

			UpdateCounter("CodeGeneration.GeneratedInstructions", generatedInstructionCount);
			UpdateCounter("CodeGeneration.GeneratedBlocks", generatedBlockCount);
		}

		#region Methods

		/// <summary>
		/// Called to emit a list of instructions offered by the instruction provider.
		/// </summary>
		protected virtual void EmitInstructions()
		{
			var trace = CreateTraceLog();

			foreach (var block in BasicBlocks)
			{
				BlockStart(block);

				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					node.SlotNumber = codeEmitter.CurrentPosition;

					if (node.IsBlockStartInstruction)
					{
						if (trace.Active)
						{
							trace.Log(String.Format("Block #{0} - Label L_{1:X4}", block.Sequence, block.Label)
								   + (BasicBlocks.IsHeaderBlock(block) ? " [Header]" : string.Empty));
						}
					}

					if (node.Instruction.IgnoreDuringCodeGeneration)
						continue;

					if (node.Instruction is BasePlatformInstruction)
					{
						EmitInstruction(node, codeEmitter);
						generatedInstructionCount++;

						if (trace.Active) trace.Log(node.SlotNumber.ToString() + " - /0x" + node.SlotNumber.ToString("X") + " : " + node.ToString());
					}
					else
					{
						NewCompilerTraceEvent(CompilerEvent.Error, "Missing Code Transformation: " + node.ToString());
					}
				}

				BlockEnd(block);
				generatedBlockCount++;
			}
		}

		/// <summary>
		/// Emits the instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="codeEmitter">The code emitter.</param>
		protected virtual void EmitInstruction(InstructionNode node, BaseCodeEmitter codeEmitter)
		{
			(node.Instruction as BasePlatformInstruction).Emit(node, codeEmitter);
		}

		/// <summary>
		/// Begins the generate.
		/// </summary>
		protected virtual void BeginGenerate()
		{
			codeEmitter = Architecture.GetCodeEmitter();
			codeEmitter.Initialize(MethodCompiler.Method.FullName, MethodCompiler.Linker, codeStream, TypeSystem);
		}

		/// <summary>
		/// Start of code generation for a block.
		/// </summary>
		/// <param name="block">The started block.</param>
		protected virtual void BlockStart(BasicBlock block)
		{
			codeEmitter.Label(block.Label);
		}

		/// <summary>
		/// Completion of code generation for a block.
		/// </summary>
		/// <param name="block">The completed block.</param>
		protected virtual void BlockEnd(BasicBlock block)
		{
			// TODO: Adjust BaseCodeEmitter interface to mark the end of label sections, rather than create this special label:
			codeEmitter.Label(block.Label + 0x0F000000);
		}

		/// <summary>
		/// Code generation completed.
		/// </summary>
		protected virtual void EndGenerate()
		{
			codeEmitter.ResolvePatches();
		}

		#endregion Methods
	}
}
