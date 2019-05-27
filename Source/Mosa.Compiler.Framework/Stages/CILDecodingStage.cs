// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

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
		#region Data Members

		/// <summary>
		/// The instruction being decoded.
		/// </summary>
		private MosaInstruction instruction;

		/// <summary>
		/// The block instruction beings too
		/// </summary>
		private BasicBlock block;

		/// <summary>
		/// The counts
		/// </summary>
		private int[] counts;

		#endregion Data Members

		protected override void Setup()
		{
			instruction = null;
			block = null;
			counts = null;
		}

		protected override void Run()
		{
			if (!MethodCompiler.IsCILDecodeRequired)
				return;

			// No CIL decoding if this is a linker generated method
			Debug.Assert(!Method.IsCompilerGenerated);

			if (!Method.HasImplementation)
			{
				//if (DelegatePatcher.PatchDelegate(MethodCompiler))
				//	return;

				MethodCompiler.Stop();
				return;
			}

			if (CompilerOptions.EnableStatistics)
			{
				counts = new int[CILInstruction.MaxOpCodeValue];
			}

			MethodCompiler.SetLocalVariables(MethodCompiler.Method.LocalVariables);

			// Create the prologue block
			var prologue = CreateNewBlock(BasicBlock.PrologueLabel);
			BasicBlocks.AddHeadBlock(prologue);

			var jmpNode = new InstructionNode()
			{
				Label = BasicBlock.PrologueLabel,
				Block = prologue
			};
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
				if (!block.HasPreviousBlocks && !block.IsHeadBlock)
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
			if (counts == null)
				return;

			int total = 0;

			for (int op = 0; op < counts.Length; op++)
			{
				int count = counts[op];

				if (count == 0)
					continue;

				var cil = CILInstruction.Get((OpCode)op);

				//UpdateCounter("CILDecodingStage.OpCode." + cil.FullName, count);

				total += count;
			}

			//UpdateCounter("CILDecodingStage.CILInstructions", total);

			instruction = null;
			block = null;
			counts = null;
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

				++counts[(int)op];

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

				// Create and initialize the corresponding instruction
				var node = new InstructionNode()
				{
					Label = instruction.Offset,
					HasPrefix = prefix,
					Instruction = cil ?? throw new InvalidMetadataException()
				};
				block.BeforeLast.Insert(node);

				cil.Decode(node, this);

				prefix = (cil is PrefixInstruction);

				//instructionCount++;

				//const bool addjmp = false;

				var flow = node.Instruction.FlowControl;

				if (flow == FlowControl.Next || flow == FlowControl.Call || flow == FlowControl.ConditionalBranch || flow == FlowControl.Switch)
				{
					var nextInstruction = MethodCompiler.Method.Code[i + 1];

					if (BasicBlocks.GetByLabel(nextInstruction.Offset) != null)
					{
						var target = GetBlockByLabel(nextInstruction.Offset);

						var jmpNode = new InstructionNode(IRInstruction.Jmp, target)
						{
							Label = instruction.Offset
						};
						block.BeforeLast.Insert(jmpNode);
					}
				}
			}
		}

		private BasicBlock GetBlockByLabel(int label)
		{
			var block = BasicBlocks.GetByLabel(label) ?? CreateNewBlock(label, label);

			return block;
		}

		#endregion Internals

		#region IInstructionDecoder Members

		/// <summary>
		/// Gets the compiler, that is currently executing.
		/// </summary>
		/// <value></value>
		MethodCompiler IInstructionDecoder.MethodCompiler { get { return MethodCompiler; } }

		/// <summary>
		/// Gets the MosaMethod being compiled.
		/// </summary>
		/// <value></value>
		MosaMethod IInstructionDecoder.Method { get { return MethodCompiler.Method; } }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		TypeSystem IInstructionDecoder.TypeSystem { get { return TypeSystem; } }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		MethodScanner IInstructionDecoder.MethodScanner { get { return MethodCompiler.MethodScanner; } }

		/// <summary>
		/// Gets the Instruction being decoded.
		/// </summary>
		MosaInstruction IInstructionDecoder.Instruction { get { return instruction; } }

		/// <summary>
		/// Gets the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		BasicBlock IInstructionDecoder.GetBlock(int label)
		{
			return GetBlockByLabel(label);
		}

		/// <summary>
		/// Converts the virtual register to stack local.
		/// </summary>
		/// <param name="virtualRegister">The virtual register.</param>
		/// <returns></returns>
		Operand IInstructionDecoder.ConvertVirtualRegisterToStackLocal(Operand virtualRegister)
		{
			if (virtualRegister.IsStackLocal)
				return virtualRegister;

			int index = 0;

			foreach (var op in MethodCompiler.LocalVariables)
			{
				if (op == virtualRegister)
					break;

				index++;
			}

			var local = MethodCompiler.Method.LocalVariables[index];

			var stackLocal = MethodCompiler.AddStackLocal(local.Type, local.IsPinned);

			MethodCompiler.LocalVariables[index] = stackLocal;

			//ReplaceOperand(virtualRegister, stackLocal);
			foreach (var node in virtualRegister.Uses.ToArray())
			{
				for (int i = 0; i < node.OperandCount; i++)
				{
					var op = node.GetOperand(i);

					if (virtualRegister == op)
					{
						node.SetOperand(i, stackLocal);
					}
				}
			}

			foreach (var node in virtualRegister.Definitions.ToArray())
			{
				for (int i = 0; i < node.ResultCount; i++)
				{
					var op = node.GetResult(i);

					if (virtualRegister == op)
					{
						node.SetResult(i, stackLocal);
					}
				}
			}

			return stackLocal;
		}

		#endregion IInstructionDecoder Members
	}
}
