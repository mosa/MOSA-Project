// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Inline Stage
/// </summary>
public class InlineStage : BaseMethodCompilerStage
{
	private readonly Counter InlineCount = new Counter("InlineStage.MethodsWithInlinedCallSites");
	private readonly Counter InlinedCallSitesCount = new Counter("InlineStage.InlinedCallSites");

	private Dictionary<MosaMethod, KeyValuePair<MethodData, InlineMethodData>> cache;

	protected override void Initialize()
	{
		Register(InlineCount);
		Register(InlinedCallSitesCount);
	}

	protected override void Finish()
	{
		if (cache != null && cache.Count != 0)
		{
			cache = null;
		}
	}

	protected override void Run()
	{
		var trace = CreateTraceLog("Inlined");

		if (cache == null || cache.Count != 0)
		{
			cache = new Dictionary<MosaMethod, KeyValuePair<MethodData, InlineMethodData>>();
		}

		var callSiteCount = 0;

		// find all call sites
		var callSites = new List<InstructionNode>();

		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction != IRInstruction.CallStatic)
					continue;

				callSites.Add(node);
			}
		}

		foreach (var callSiteNode in callSites)
		{
			var invokedMethod = callSiteNode.Operand1.Method;

			// don't inline self
			if (invokedMethod == Method)
				continue;

			Debug.Assert(callSiteNode.Operand1.IsLabel);

			(MethodData Callee, InlineMethodData InlineMethodData) result = GetCalleeData(invokedMethod);

			//Debug.WriteLine($"{MethodScheduler.GetTimestamp()} - Inline: {(inlineMethodData.IsInlined ? "Inlined" : "NOT Inlined")} [{MethodData.Version}] {Method} -> [{inlineMethodData.Version}] {callee.Method}"); //DEBUGREMOVE

			if (!result.InlineMethodData.IsInlined)
				continue;

			Inline(callSiteNode, result.InlineMethodData.BasicBlocks);
			callSiteCount++;

			trace?.Log($" -> Inlined: [{result.Callee.Version}] {result.Callee.Method}");

			//Debug.WriteLine($" -> Inlined: [{callee.Version}] {callee.Method}");//DEBUGREMOVE
		}

		InlineCount.Set(1);
		InlinedCallSitesCount.Set(callSiteCount);
	}

	private (MethodData, InlineMethodData) GetCalleeData(MosaMethod invokedMethod)
	{
		if (cache.TryGetValue(invokedMethod, out KeyValuePair<MethodData, InlineMethodData> found))
		{
			return (found.Key, found.Value);
		}
		else
		{
			var callee = MethodCompiler.Compiler.GetMethodData(invokedMethod);
			var inlineMethodData = callee.GetInlineMethodDataForUseBy(Method);

			cache.Add(invokedMethod, new KeyValuePair<MethodData, InlineMethodData>(callee, inlineMethodData));

			return (callee, inlineMethodData);
		}
	}

	protected void Inline(InstructionNode callSiteNode, BasicBlocks blocks)
	{
		var mapBlocks = new Dictionary<BasicBlock, BasicBlock>(blocks.Count);
		var map = new Dictionary<Operand, Operand>();

		var nextBlock = Split(callSiteNode);

		// create basic blocks
		foreach (var block in blocks)
		{
			var newBlock = BasicBlocks.CreateBlock();
			mapBlocks.Add(block, newBlock);
		}

		// copy instructions
		foreach (var block in blocks)
		{
			var newBlock = mapBlocks[block];

			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				if (node.Instruction == IRInstruction.Prologue)
					continue;

				if (node.Instruction == IRInstruction.Epilogue)
				{
					newBlock.BeforeLast.Insert(new InstructionNode(IRInstruction.Jmp, nextBlock));
					continue;
				}

				if (node.Instruction == IRInstruction.SetReturn32
					|| node.Instruction == IRInstruction.SetReturn64
					|| node.Instruction == IRInstruction.SetReturnObject
					|| node.Instruction == IRInstruction.SetReturnManagedPointer
					|| node.Instruction == IRInstruction.SetReturnR4
					|| node.Instruction == IRInstruction.SetReturnR8
					|| node.Instruction == IRInstruction.SetReturnCompound)
				{
					if (callSiteNode.Result != null)
					{
						var newOperand = Map(node.Operand1, map, callSiteNode);
						var moveInstruction = GetMoveInstruction(node);

						var moveNode = new InstructionNode(moveInstruction, callSiteNode.Result, newOperand);

						newBlock.BeforeLast.Insert(moveNode);
					}

					continue;
				}

				var newNode = new InstructionNode(node.Instruction, node.OperandCount, node.ResultCount)
				{
					ConditionCode = node.ConditionCode,
					Label = callSiteNode.Label,
				};

				if (node.BranchTargets != null)
				{
					// copy targets
					foreach (var target in node.BranchTargets)
					{
						newNode.AddBranchTarget(mapBlocks[target]);
					}
				}

				// copy results
				for (var i = 0; i < node.ResultCount; i++)
				{
					var operand = node.GetResult(i);
					var newOperand = Map(operand, map, callSiteNode);
					newNode.SetResult(i, newOperand);
				}

				// copy operands
				for (var i = 0; i < node.OperandCount; i++)
				{
					var operand = node.GetOperand(i);
					var newOperand = Map(operand, map, callSiteNode);
					newNode.SetOperand(i, newOperand);
				}

				UpdateParameterInstruction(newNode);

				newBlock.BeforeLast.Insert(newNode);
			}
		}

		var prologue = mapBlocks[blocks.PrologueBlock];

		var callSiteOperands = callSiteNode.GetOperands();

		if (callSiteOperands.Count > 1)
		{
			var context = new Context(prologue);

			for (var i = 1; i < callSiteOperands.Count; i++)
			{
				var operand = callSiteOperands[i];

				if (!operand.IsVirtualRegister || operand.Low == null)
					continue;

				context.AppendInstruction(IRInstruction.GetLow32, operand.Low, operand);
				context.AppendInstruction(IRInstruction.GetHigh32, operand.High, operand);
			}
		}

		callSiteNode.SetInstruction(IRInstruction.Jmp, prologue);
	}

	private static BaseInstruction GetMoveInstruction(InstructionNode node)
	{
		var instruction = node.Instruction;

		if (instruction == IRInstruction.SetReturn32)
			return IRInstruction.Move32;
		else if (instruction == IRInstruction.SetReturn64)
			return IRInstruction.Move64;
		else if (instruction == IRInstruction.SetReturnObject)
			return IRInstruction.MoveObject;
		else if (instruction == IRInstruction.SetReturnManagedPointer)
			return IRInstruction.MoveManagedPointer;
		else if (instruction == IRInstruction.SetReturnR4)
			return IRInstruction.MoveR4;
		else if (instruction == IRInstruction.SetReturnR8)
			return IRInstruction.MoveR8;
		else if (instruction == IRInstruction.SetReturnCompound)
			return IRInstruction.MoveCompound;

		throw new CompilerException();
	}

	private static void UpdateParameterInstruction(InstructionNode newNode)
	{
		var instruction = newNode.Instruction;

		if (instruction == IRInstruction.LoadParamR4)
		{
			newNode.Instruction = IRInstruction.MoveR4;
		}
		else if (instruction == IRInstruction.LoadParamR8)
		{
			newNode.Instruction = IRInstruction.MoveR8;
		}
		else if (instruction == IRInstruction.LoadParam32
			|| instruction == IRInstruction.LoadParamSignExtend8x32
			|| instruction == IRInstruction.LoadParamSignExtend16x32
			|| instruction == IRInstruction.LoadParamZeroExtend8x32
			|| instruction == IRInstruction.LoadParamZeroExtend16x32)
		{
			newNode.Instruction = IRInstruction.Move32;
		}
		else if (instruction == IRInstruction.LoadParamObject)
		{
			newNode.Instruction = IRInstruction.MoveObject;
		}
		else if (instruction == IRInstruction.LoadParamManagedPointer)
		{
			newNode.Instruction = IRInstruction.MoveManagedPointer;
		}
		else if (instruction == IRInstruction.LoadParam64
			|| instruction == IRInstruction.LoadParamSignExtend8x64
			|| instruction == IRInstruction.LoadParamSignExtend16x64
			|| instruction == IRInstruction.LoadParamSignExtend32x64
			|| instruction == IRInstruction.LoadParamZeroExtend8x64
			|| instruction == IRInstruction.LoadParamZeroExtend16x64
			|| instruction == IRInstruction.LoadParamZeroExtend32x64)
		{
			newNode.Instruction = IRInstruction.Move64;
		}
		else if (instruction == IRInstruction.StoreParam8
			|| instruction == IRInstruction.StoreParam16
			|| instruction == IRInstruction.StoreParam32)
		{
			newNode.SetInstruction(IRInstruction.Move32, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IRInstruction.StoreParamObject)
		{
			newNode.SetInstruction(IRInstruction.MoveObject, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IRInstruction.StoreParamManagedPointer)
		{
			newNode.SetInstruction(IRInstruction.MoveManagedPointer, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IRInstruction.StoreParam64)
		{
			newNode.SetInstruction(IRInstruction.Move64, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IRInstruction.StoreParamR4)
		{
			newNode.SetInstruction(IRInstruction.MoveR4, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IRInstruction.StoreParamR8)
		{
			newNode.SetInstruction(IRInstruction.MoveR8, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IRInstruction.StoreParamCompound)
		{
			newNode.Instruction = IRInstruction.MoveCompound;
		}
		else if (instruction == IRInstruction.LoadParamCompound)
		{
			newNode.Instruction = IRInstruction.MoveCompound;
		}
	}

	private Operand Map(Operand operand, Dictionary<Operand, Operand> map, InstructionNode callSiteNode)
	{
		if (operand == null)
			return null;

		if (map.TryGetValue(operand, out Operand mappedOperand))
		{
			return mappedOperand;
		}

		if (operand.IsLabel)
		{
			mappedOperand = operand;
		}
		else if (operand.IsParameter)
		{
			mappedOperand = callSiteNode.GetOperand(operand.Index + 1);
		}
		else if (operand.IsLocalStack)
		{
			mappedOperand = MethodCompiler.LocalStack.Allocate(operand);
		}
		else if (operand.IsVirtualRegister)
		{
			mappedOperand = MethodCompiler.VirtualRegisters.Allocate(operand);
		}
		else if (operand.IsStaticField)
		{
			mappedOperand = operand;
		}
		else if (operand.IsCPURegister)
		{
			mappedOperand = operand;
		}
		else if (operand.IsConstant)
		{
			mappedOperand = operand;
		}

		Debug.Assert(mappedOperand != null);

		if (operand.HasParent && !mappedOperand.HasParent)
		{
			MethodCompiler.VirtualRegisters.SplitOperand(mappedOperand);

			if (operand.IsLow)
			{
				mappedOperand = mappedOperand.Low;
			}
			else if (operand.IsHigh)
			{
				mappedOperand = mappedOperand.High;
			}
		}

		map.Add(operand, mappedOperand);

		return mappedOperand;
	}
}
