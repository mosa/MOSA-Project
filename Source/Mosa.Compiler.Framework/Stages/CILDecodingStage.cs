// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages;

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

	#endregion Data Members

	protected override void Setup()
	{
		instruction = null;
		block = null;
	}

	protected override void Run()
	{
		if (!MethodCompiler.IsCILStream)
			return;

		// No CIL decoding if this is a linker generated method
		Debug.Assert(!MethodData.IsCompilerGenerated);

		MethodCompiler.SetLocalVariables(Method.LocalVariables);

		var prologueBlock = BasicBlocks.CreatePrologueBlock();
		var startBlock = BasicBlocks.CreateStartBlock();

		var prologue = new Context(prologueBlock.First);
		prologue.AppendInstruction(IRInstruction.Prologue);
		prologue.AppendInstruction(IRInstruction.Jmp, startBlock);

		DecodeInstructionTargets();

		DecodeProtectedRegionTargets();

		DecodeInstructions();

		foreach (var block in BasicBlocks)
		{
			if (!block.HasPreviousBlocks && !block.IsHeadBlock)
			{
				// block was targeted (probably by an leave instruction within a protected region)
				BasicBlocks.AddHeadBlock(block);
			}
		}

		InitializePromotedLocalVariablesToVirtualRegisters();

		InsertExceptionStartInstructions();

		InsertFlowOrJumpInstructions();

		// This makes it easier to review --- it's not necessary
		OrderByLabel();
	}

	public void OrderByLabel()
	{
		var blocks = new SortedList<int, BasicBlock>(BasicBlocks.Count);

		foreach (var block in BasicBlocks)
		{
			blocks.Add(block.Label, block);
		}

		var blocks2 = new List<BasicBlock>(BasicBlocks.Count);

		foreach (var item in blocks)
		{
			if (item.Value.IsPrologue)
				blocks2.Insert(0, item.Value);
			else
				blocks2.Add(item.Value);
		}

		BasicBlocks.ReorderBlocks(blocks2);
	}

	protected override void Finish()
	{
		instruction = null;
		block = null;
	}

	#region Internals

	private void DecodeProtectedRegionTargets()
	{
		foreach (var handler in Method.ExceptionHandlers)
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
		var branched = false;

		var code = Method.Code;

		for (var i = 0; i < code.Count; i++)
		{
			instruction = code[i];

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
		var prefix = false;

		var code = Method.Code;

		for (var i = 0; i < code.Count; i++)
		{
			instruction = code[i];

			block = BasicBlocks.GetByLabel(instruction.Offset) ?? block;

			var op = (OpCode)instruction.OpCode;

			var cil = CILInstruction.Get(op);

			// Create and initialize the corresponding instruction
			var node = new InstructionNode
			{
				Label = instruction.Offset,

				//HasPrefix = prefix,
				Instruction = cil ?? throw new InvalidMetadataException()
			};
			block.BeforeLast.Insert(node);

			cil.Decode(node, this);

			prefix = cil is PrefixInstruction;

			//instructionCount++;

			var flow = node.Instruction.FlowControl;

			if (flow is FlowControl.Next or FlowControl.Call or FlowControl.ConditionalBranch or FlowControl.Switch)
			{
				var nextInstruction = code[i + 1];

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
		var block = BasicBlocks.GetByLabel(label) ?? BasicBlocks.CreateBlock(label, label);

		return block;
	}

	#endregion Internals

	private void InitializePromotedLocalVariablesToVirtualRegisters()
	{
		if (Method.LocalVariables.Count == 0)
			return;

		var prologue = new Context(BasicBlocks.PrologueBlock.First.NextNonEmpty);

		foreach (var variable in MethodCompiler.LocalVariables)
		{
			if (!variable.IsVirtualRegister)
				continue;

			if (variable.IsValueType)
			{
				var type = MosaTypeLayout.GetUnderlyingType(variable.Type);

				if (type.IsR4)
				{
					prologue.AppendInstruction(IRInstruction.MoveR4, variable, CreateConstantR4(0.0f));
				}
				else if (type.IsR8)
				{
					prologue.AppendInstruction(IRInstruction.MoveR8, variable, CreateConstantR8(0.0d));
				}
				else if (type.IsUI8 || (Is64BitPlatform && (type.IsU || type.IsI)))
				{
					prologue.AppendInstruction(IRInstruction.Move64, variable, Constant64_0);
				}
				else
				{
					prologue.AppendInstruction(IRInstruction.Move32, variable, Constant32_0);
				}
			}
			else if (variable.IsReferenceType)
			{
				prologue.AppendInstruction(IRInstruction.MoveObject, variable, Operand.GetNull(variable.Type));
			}
			else if (variable.IsInteger64)
			{
				prologue.AppendInstruction(IRInstruction.Move64, variable, Constant64_0);
			}
			else if (variable.IsR4)
			{
				prologue.AppendInstruction(IRInstruction.MoveR4, variable, CreateConstantR4(0.0f));
			}
			else if (variable.IsR8)
			{
				prologue.AppendInstruction(IRInstruction.MoveR8, variable, CreateConstantR8(0.0d));
			}
			else
			{
				prologue.AppendInstruction(IRInstruction.Move32, variable, Constant32_0);
			}
		}
	}

	private void InsertExceptionStartInstructions()
	{
		foreach (var clause in Method.ExceptionHandlers)
		{
			if (clause.ExceptionHandlerType == ExceptionHandlerType.Exception)
			{
				var handler = BasicBlocks.GetByLabel(clause.HandlerStart);

				var exceptionObject = AllocateVirtualRegister(clause.Type);

				var context = new Context(handler);

				context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
			}

			if (clause.ExceptionHandlerType == ExceptionHandlerType.Filter)
			{
				{
					var handler = BasicBlocks.GetByLabel(clause.HandlerStart);

					var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

					var context = new Context(handler);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}

				{
					var handler = BasicBlocks.GetByLabel(clause.FilterStart.Value);

					var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

					var context = new Context(handler);

					context.AppendInstruction(IRInstruction.FilterStart, exceptionObject);
				}
			}
		}
	}

	private bool IsSourceAndTargetWithinSameTryOrException(InstructionNode node)
	{
		var leaveLabel = TraverseBackToNativeBlock(node.Block).Label;
		var targetLabel = TraverseBackToNativeBlock(node.BranchTargets[0]).Label;

		foreach (var handler in Method.ExceptionHandlers)
		{
			var one = handler.IsLabelWithinTry(leaveLabel);
			var two = handler.IsLabelWithinTry(targetLabel);

			if (one && !two)
				return false;

			if (!one && two)
				return false;

			if (one && two)
				return true;

			one = handler.IsLabelWithinHandler(leaveLabel);
			two = handler.IsLabelWithinHandler(targetLabel);

			if (one && !two)
				return false;

			if (!one && two)
				return false;

			if (one && two)
				return true;
		}

		// very odd
		return true;
	}

	private void InsertFlowOrJumpInstructions()
	{
		foreach (var block in BasicBlocks)
		{
			var label = TraverseBackToNativeBlock(block).Label;

			for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (!(node.Instruction is LeaveInstruction))
					continue;   // FUTURE: Could this be a break instruction instead?

				var target = node.BranchTargets[0];

				if (IsSourceAndTargetWithinSameTryOrException(node))
				{
					// Leave instruction can be converted into a simple jump instruction
					node.SetInstruction(IRInstruction.Jmp, target);
					BasicBlocks.RemoveHeaderBlock(target);
					continue;
				}

				var entry = FindImmediateExceptionHandler(label);

				if (!entry.IsLabelWithinTry(label))
					break;

				var flowNode = new InstructionNode(IRInstruction.Flow, target);

				node.Insert(flowNode);

				if (target.IsHeadBlock)
				{
					BasicBlocks.RemoveHeaderBlock(target);
				}

				break;
			}
		}
	}

	#region IInstructionDecoder Members

	/// <summary>
	/// Gets the compiler, that is currently executing.
	/// </summary>
	/// <value></value>
	MethodCompiler IInstructionDecoder.MethodCompiler => MethodCompiler;

	/// <summary>
	/// Gets the MosaMethod being compiled.
	/// </summary>
	/// <value></value>
	MosaMethod IInstructionDecoder.Method => MethodCompiler.Method;

	/// <summary>
	/// Gets the type system.
	/// </summary>
	TypeSystem IInstructionDecoder.TypeSystem => TypeSystem;

	/// <summary>
	/// Gets the type system.
	/// </summary>
	MethodScanner IInstructionDecoder.MethodScanner => MethodCompiler.MethodScanner;

	/// <summary>
	/// Gets the Instruction being decoded.
	/// </summary>
	MosaInstruction IInstructionDecoder.Instruction => instruction;

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

		var index = 0;

		foreach (var op in MethodCompiler.LocalVariables)
		{
			if (op == virtualRegister)
				break;

			index++;
		}

		var local = Method.LocalVariables[index];

		var stackLocal = MethodCompiler.AddStackLocal(local.Type, local.IsPinned);

		MethodCompiler.LocalVariables[index] = stackLocal;

		//ReplaceOperand(virtualRegister, stackLocal);
		foreach (var node in virtualRegister.Uses.ToArray())
		{
			for (var i = 0; i < node.OperandCount; i++)
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
			for (var i = 0; i < node.ResultCount; i++)
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
