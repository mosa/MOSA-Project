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
	public sealed class CodeGenerationStage : BaseMethodCompilerStage
	{
		private Counter GeneratedInstructionCount = new Counter("CodeGenerationStage.GeneratedInstructions");
		private Counter GeneratedBlockCount = new Counter("CodeGenerationStage.GeneratedBlocks");

		#region Data Members

		/// <summary>
		/// Holds the stream, where code is emitted to.
		/// </summary>
		private MemoryStream codeStream;

		//protected Stream codeStream;

		#endregion Data Members

		#region Properties

		private BaseCodeEmitter CodeEmitter;

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

			var symbol = Linker.DefineSymbol(Method.FullName, SectionKind.Text, 0, 0);

			codeStream = new MemoryStream();

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
			MethodData.HasCode = true;
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
		private void EmitInstructions()
		{
			var trace = CreateTraceLog(9);

			MethodData.LabelRegions.Clear();
			int labelCurrent = BasicBlock.ReservedLabel;
			int labelStart = 0;

			foreach (var block in BasicBlocks)
			{
				BlockStart(block);

				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
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
								MethodData.AddLabelRegion(labelCurrent, labelStart, CodeEmitter.CurrentPosition - labelStart);
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
						PostEvent(CompilerEvent.Error, $"Missing Code Transformation: {node} in {Method}");
					}
				}

				block.Last.Offset = CodeEmitter.CurrentPosition;

				BlockEnd(block);
				GeneratedBlockCount++;
			}

			MethodData.AddLabelRegion(labelCurrent, labelStart, CodeEmitter.CurrentPosition - labelStart);
		}

		/// <summary>
		/// Begins the generate.
		/// </summary>
		private void BeginGenerate()
		{
			CodeEmitter = new BaseCodeEmitter();
			CodeEmitter.Initialize(Method.FullName, Linker, codeStream);

			MethodCompiler.Labels = CodeEmitter.Labels;
		}

		/// <summary>
		/// Start of code generation for a block.
		/// </summary>
		/// <param name="block">The started block.</param>
		private void BlockStart(BasicBlock block)
		{
			CodeEmitter.Label(block.Label);
		}

		/// <summary>
		/// Completion of code generation for a block.
		/// </summary>
		/// <param name="block">The completed block.</param>
		private void BlockEnd(BasicBlock block)
		{
			// TODO: Adjust BaseCodeEmitter interface to mark the end of label sections, rather than create this special label:
			CodeEmitter.Label(block.Label + 0x0F000000);
		}

		/// <summary>
		/// Code generation completed.
		/// </summary>
		private void EndGenerate()
		{
			CodeEmitter.ResolvePatches();
		}

		#endregion Methods
	}
}
