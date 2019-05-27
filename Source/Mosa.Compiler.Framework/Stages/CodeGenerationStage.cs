// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Framework.Trace;
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
		private Counter GeneratedInstructionCount = new Counter("CodeGenerationStage.GeneratedInstructions");
		private Counter GeneratedBlockCount = new Counter("CodeGenerationStage.GeneratedBlocks");

		#region Data Members

		/// <summary>
		/// Holds the stream, where code is emitted to.
		/// </summary>
		protected MemoryStream codeStream;

		//protected Stream codeStream;

		#endregion Data Members

		#region Properties

		public BaseCodeEmitter CodeEmitter { get; protected set; }

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

		protected override void Initialize()
		{
			Register(GeneratedInstructionCount);
			Register(GeneratedBlockCount);
		}

		protected override void Run()
		{
			if (!EmitBinary)
				return;

			var symbol = MethodCompiler.Linker.DefineSymbol(MethodCompiler.Method.FullName, SectionKind.Text, 0, 0);

			codeStream = new MemoryStream();

			//codeStream = symbol.Stream;

			// Retrieve a stream to place the code into

			// HINT: We need seeking to resolve Labels.
			Debug.Assert(codeStream.CanSeek, "Can't seek codeReader output stream.");
			Debug.Assert(codeStream.CanWrite, "Can't write to codeReader output stream.");

			if (!codeStream.CanSeek || !codeStream.CanWrite)
				throw new NotSupportedException("Code stream2 doesn't support seeking or writing.");

			// Emit method prologue
			BeginGenerate();

			// Emit all instructions
			EmitInstructions();

			// Emit the method epilogue
			EndGenerate();

			symbol.SetData(codeStream);
		}

		protected override void Finish()
		{
			CodeEmitter = null;
			codeStream = null;
		}

		#region Methods

		/// <summary>
		/// Called to emit a list of instructions offered by the instruction provider.
		/// </summary>
		protected virtual void EmitInstructions()
		{
			var trace = CreateTraceLog();

			MethodCompiler.MethodData.LabelRegions.Clear();
			int labelCurrent = BasicBlock.ReservedLabel;
			int labelStart = 0;

			foreach (var block in BasicBlocks)
			{
				BlockStart(block);

				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.IsBlockStartInstruction)
					{
						trace?.Log($"Block #{block.Sequence} - Label L_{block.Label:X4}" + (block.IsHeadBlock ? " [Header]" : string.Empty));

						continue;
					}

					if (node.Instruction.IgnoreDuringCodeGeneration)
						continue;

					node.Offset = CodeEmitter.CurrentPosition;

					if (node.Instruction is BasePlatformInstruction baseInstruction)
					{
						if (node.Label != labelCurrent)
						{
							if (labelCurrent != BasicBlock.ReservedLabel)
							{
								MethodCompiler.MethodData.AddLabelRegion(labelCurrent, labelStart, CodeEmitter.CurrentPosition - labelStart);
							}

							labelCurrent = node.Label;
							labelStart = node.Offset;
						}

						baseInstruction.Emit(node, CodeEmitter);

						GeneratedInstructionCount++;

						trace?.Log($"{node.Offset} - /0x{node.Offset.ToString("X")} : {node}");
					}
					else
					{
						PostCompilerTraceEvent(CompilerEvent.Error, $"Missing Code Transformation: {node}");
					}
				}

				block.Last.Offset = CodeEmitter.CurrentPosition;

				BlockEnd(block);
				GeneratedBlockCount++;
			}

			MethodCompiler.MethodData.AddLabelRegion(labelCurrent, labelStart, CodeEmitter.CurrentPosition - labelStart);
		}

		/// <summary>
		/// Begins the generate.
		/// </summary>
		protected virtual void BeginGenerate()
		{
			CodeEmitter = new BaseCodeEmitter();
			CodeEmitter.Initialize(MethodCompiler.Method.FullName, MethodCompiler.Linker, codeStream);

			MethodCompiler.Labels = CodeEmitter.Labels;
		}

		/// <summary>
		/// Start of code generation for a block.
		/// </summary>
		/// <param name="block">The started block.</param>
		protected virtual void BlockStart(BasicBlock block)
		{
			CodeEmitter.Label(block.Label);
		}

		/// <summary>
		/// Completion of code generation for a block.
		/// </summary>
		/// <param name="block">The completed block.</param>
		protected virtual void BlockEnd(BasicBlock block)
		{
			// TODO: Adjust BaseCodeEmitter interface to mark the end of label sections, rather than create this special label:
			CodeEmitter.Label(block.Label + 0x0F000000);
		}

		/// <summary>
		/// Code generation completed.
		/// </summary>
		protected virtual void EndGenerate()
		{
			CodeEmitter.ResolvePatches();
		}

		#endregion Methods
	}
}
