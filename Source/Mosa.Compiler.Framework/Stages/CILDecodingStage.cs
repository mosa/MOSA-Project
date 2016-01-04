// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Represents the CIL decoding compilation stage.
	/// </summary>
	/// <remarks>
	/// The CIL decoding stage takes a stream of bytes and decodes the
	/// instructions represented into an MSIL based intermediate
	/// representation. The instructions are grouped into basic Blocks
	/// for easier local optimizations in later compiler stages.
	/// </remarks>
	public sealed class CILDecodingStage : BaseMethodCompilerStage, IInstructionDecoder
	{
		#region Data members

		/// <summary>
		/// The instruction being decoded.
		/// </summary>
		private MosaInstruction instruction;

		/// <summary>
		/// The block instruction beings too
		/// </summary>
		private BasicBlock block;

		#endregion Data members

		protected override void Run()
		{
			// No CIL decoding if this is a linker generated method
			if (MethodCompiler.Method.IsLinkerGenerated)
				return;

			var plugMethod = MethodCompiler.Compiler.PlugSystem.GetPlugMethod(MethodCompiler.Method);

			if (plugMethod != null)
			{
				var plugSymbol = Operand.CreateSymbolFromMethod(TypeSystem, plugMethod);
				var context = CreateNewBlockContext(-1);
				context.AppendInstruction(IRInstruction.Jmp, null, plugSymbol);
				BasicBlocks.AddHeadBlock(context.Block);
				return;
			}

			if (MethodCompiler.Method.Code.Count == 0)
			{
				if (DelegatePatcher.PatchDelegate(MethodCompiler))
					return;

				MethodCompiler.Stop();
				return;
			}

			MethodCompiler.SetLocalVariables(MethodCompiler.Method.LocalVariables);

			// Create the prologue block
			var prologue = CreateNewBlock(BasicBlock.PrologueLabel);
			BasicBlocks.AddHeadBlock(prologue);

			var jmpNode = new InstructionNode();
			jmpNode.Label = BasicBlock.PrologueLabel;
			jmpNode.Block = prologue;
			prologue.First.Insert(jmpNode);

			// Create starting block
			var startBlock = CreateNewBlock(0);

			jmpNode.SetInstruction(IRInstruction.Jmp, startBlock);

			DecodeInstructionTargets();

			DecodeProtectedRegionTargets();

			/* Decode the instructions */
			DecodeInstructions();

			foreach (var block in BasicBlocks)
			{
				if (!block.HasPreviousBlocks && !BasicBlocks.HeadBlocks.Contains(block))
				{
					// block was targeted (probably by an leave instruction within a protected region)
					BasicBlocks.AddHeadBlock(block);
				}
			}

			// This makes it easier to review --- it's not necessary
			BasicBlocks.OrderByLabel();
		}

		protected override void Finish()
		{
			UpdateCounter("CILDecoding.Instructions", instructionCount);
		}

		#region Internals

		private void DecodeProtectedRegionTargets()
		{
			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				if (handler.TryStart != 0)
				{
					var block = GetBlockByLabel(handler.TryStart);
				}
				if (handler.TryEnd != 0)
				{
					var block = GetBlockByLabel(handler.TryEnd);
				}
				if (handler.HandlerStart != 0)
				{
					var block = GetBlockByLabel(handler.HandlerStart);
					BasicBlocks.AddHeadBlock(block);
					BasicBlocks.AddHandlerHeadBlock(block);
				}
				if (handler.FilterStart != null)
				{
					var block = GetBlockByLabel(handler.FilterStart.Value);
					BasicBlocks.AddHeadBlock(block);
					BasicBlocks.AddHandlerHeadBlock(block);
				}
			}
		}

		private void DecodeInstructionTargets()
		{
			bool branched = false;

			for (int i = 0; i < MethodCompiler.Method.Code.Count; i++)
			{
				instruction = MethodCompiler.Method.Code[i];

				if (branched)
				{
					GetBlockByLabel(instruction.Offset);
					branched = false;
				}

				var op = (OpCode)instruction.OpCode;

				var cil = CILInstruction.Get(op);

				branched = cil.DecodeTargets(this);
			}
		}

		/// <summary>
		/// Decodes the instruction stream of the reader and populates the compiler.
		/// </summary>
		/// <exception cref="InvalidMetadataException"></exception>
		private void DecodeInstructions()
		{
			block = null;

			// Prefix instruction
			bool prefix = false;

			for (int i = 0; i < MethodCompiler.Method.Code.Count; i++)
			{
				instruction = MethodCompiler.Method.Code[i];

				block = BasicBlocks.GetByLabel(instruction.Offset) ?? block;

				var op = (OpCode)instruction.OpCode;

				var cil = CILInstruction.Get(op);

				if (cil == null)
				{
					throw new InvalidMetadataException();
				}

				// Create and initialize the corresponding instruction
				var node = new InstructionNode();
				node.Label = instruction.Offset;
				node.HasPrefix = prefix;
				node.Instruction = cil;

				block.BeforeLast.Insert(node);

				cil.Decode(node, this);

				prefix = (cil is PrefixInstruction);
				instructionCount++;

				bool addjmp = false;

				var flow = node.Instruction.FlowControl;

				if (flow == FlowControl.Next || flow == FlowControl.Call || flow == FlowControl.ConditionalBranch || flow == FlowControl.Switch)
				{
					var nextInstruction = MethodCompiler.Method.Code[i + 1];

					if (BasicBlocks.GetByLabel(nextInstruction.Offset) != null)
					{
						var target = GetBlockByLabel(nextInstruction.Offset);

						var jmpNode = new InstructionNode(IRInstruction.Jmp, target);
						jmpNode.Label = instruction.Offset;
						block.BeforeLast.Insert(jmpNode);
					}
				}
			}
		}

		private BasicBlock GetBlockByLabel(int label)
		{
			var block = BasicBlocks.GetByLabel(label);

			if (block == null)
			{
				block = CreateNewBlock(label);
			}

			return block;
		}

		#endregion Internals

		#region IInstructionDecoder Members

		/// <summary>
		/// Gets the compiler, that is currently executing.
		/// </summary>
		/// <value></value>
		BaseMethodCompiler IInstructionDecoder.Compiler { get { return MethodCompiler; } }

		/// <summary>
		/// Gets the MosaMethod being compiled.
		/// </summary>
		/// <value></value>
		MosaMethod IInstructionDecoder.Method { get { return MethodCompiler.Method; } }

		/// <summary>
		/// Gets the Instruction being decoded.
		/// </summary>
		MosaInstruction IInstructionDecoder.Instruction { get { return instruction; } }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>
		/// The type system.
		/// </value>
		TypeSystem IInstructionDecoder.TypeSystem { get { return TypeSystem; } }

		/// <summary>
		/// Gets the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		BasicBlock IInstructionDecoder.GetBlock(int label)
		{
			return GetBlockByLabel(label);
		}

		#endregion IInstructionDecoder Members
	}
}
