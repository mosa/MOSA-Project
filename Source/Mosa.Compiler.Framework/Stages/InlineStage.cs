// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Inline Stage
/// </summary>
public class InlineStage : BaseMethodCompilerStage
{
	private readonly Counter InlineCount = new("Inline.MethodsWithInlinedCallSites");
	private readonly Counter InlinedCallSitesCount = new("Inline.InlinedCallSites");

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
		var callSites = new List<Node>();

		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction != IR.CallStatic)
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

	protected void Inline(Node callSiteNode, BasicBlocks blocks)
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

				if (node.Instruction == IR.Prologue)
					continue;

				if (node.Instruction == IR.Epilogue)
				{
					newBlock.BeforeLast.Insert(new Node(IR.Jmp, nextBlock));
					continue;
				}

				if (node.Instruction == IR.SetReturn32
					|| node.Instruction == IR.SetReturn64
					|| node.Instruction == IR.SetReturnObject
					|| node.Instruction == IR.SetReturnManagedPointer
					|| node.Instruction == IR.SetReturnR4
					|| node.Instruction == IR.SetReturnR8
					|| node.Instruction == IR.SetReturnCompound)
				{
					if (callSiteNode.Result != null)
					{
						var newOperand = Map(node.Operand1, map, callSiteNode);
						var moveInstruction = GetMoveInstruction(node);

						var moveNode = new Node(moveInstruction, callSiteNode.Result, newOperand);

						newBlock.BeforeLast.Insert(moveNode);
					}

					continue;
				}

				var newNode = new Node(node.Instruction, node.OperandCount, node.ResultCount)
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

				context.AppendInstruction(IR.GetLow32, operand.Low, operand);
				context.AppendInstruction(IR.GetHigh32, operand.High, operand);
			}
		}

		callSiteNode.SetInstruction(IR.Jmp, prologue);
	}

	private static BaseInstruction GetMoveInstruction(Node node)
	{
		var instruction = node.Instruction;

		if (instruction == IR.SetReturn32)
			return IR.Move32;
		else if (instruction == IR.SetReturn64)
			return IR.Move64;
		else if (instruction == IR.SetReturnObject)
			return IR.MoveObject;
		else if (instruction == IR.SetReturnManagedPointer)
			return IR.MoveManagedPointer;
		else if (instruction == IR.SetReturnR4)
			return IR.MoveR4;
		else if (instruction == IR.SetReturnR8)
			return IR.MoveR8;
		else if (instruction == IR.SetReturnCompound)
			return IR.MoveCompound;

		throw new CompilerException();
	}

	private static void UpdateParameterInstruction(Node newNode)
	{
		var instruction = newNode.Instruction;

		if (instruction == IR.LoadParamR4)
		{
			newNode.Instruction = IR.MoveR4;
		}
		else if (instruction == IR.LoadParamR8)
		{
			newNode.Instruction = IR.MoveR8;
		}
		else if (instruction == IR.LoadParam32)
		{
			newNode.Instruction = IR.Move32;
		}
		else if (instruction == IR.LoadParamSignExtend8x32)
		{
			newNode.Instruction = IR.SignExtend8x32;
		}
		else if (instruction == IR.LoadParamSignExtend16x32)
		{
			newNode.Instruction = IR.SignExtend16x32;
		}
		else if (instruction == IR.ZeroExtend8x32)
		{
			newNode.Instruction = IR.ZeroExtend8x32;
		}
		else if (instruction == IR.LoadParamZeroExtend16x32)
		{
			newNode.Instruction = IR.ZeroExtend16x32;
		}
		else if (instruction == IR.LoadParamZeroExtend8x32)
		{
			newNode.Instruction = IR.ZeroExtend8x32;
		}
		else if (instruction == IR.LoadParamObject)
		{
			newNode.Instruction = IR.MoveObject;
		}
		else if (instruction == IR.LoadParamManagedPointer)
		{
			newNode.Instruction = IR.MoveManagedPointer;
		}
		else if (instruction == IR.LoadParam64)
		{
			newNode.Instruction = IR.Move64;
		}
		else if (instruction == IR.LoadParamSignExtend8x64)
		{
			newNode.Instruction = IR.SignExtend8x64;
		}
		else if (instruction == IR.LoadParamSignExtend16x64)
		{
			newNode.Instruction = IR.SignExtend16x64;
		}
		else if (instruction == IR.LoadParamSignExtend32x64)
		{
			newNode.Instruction = IR.SignExtend32x64;
		}
		else if (instruction == IR.LoadParamZeroExtend8x64)
		{
			newNode.Instruction = IR.ZeroExtend8x64;
		}
		else if (instruction == IR.LoadParamZeroExtend16x64)
		{
			newNode.Instruction = IR.ZeroExtend16x64;
		}
		else if (instruction == IR.LoadParamZeroExtend32x64)
		{
			newNode.Instruction = IR.ZeroExtend32x64;
		}
		else if (instruction == IR.StoreParam32)
		{
			newNode.SetInstruction(IR.Move32, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IR.StoreParam8)
		{
			newNode.SetInstruction(IR.ZeroExtend8x32, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IR.StoreParam16)
		{
			newNode.SetInstruction(IR.ZeroExtend16x32, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IR.StoreParamObject)
		{
			newNode.SetInstruction(IR.MoveObject, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IR.StoreParamManagedPointer)
		{
			newNode.SetInstruction(IR.MoveManagedPointer, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IR.StoreParam64)
		{
			newNode.SetInstruction(IR.Move64, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IR.StoreParamR4)
		{
			newNode.SetInstruction(IR.MoveR4, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IR.StoreParamR8)
		{
			newNode.SetInstruction(IR.MoveR8, newNode.Operand1, newNode.Operand2);
		}
		else if (instruction == IR.StoreParamCompound)
		{
			newNode.Instruction = IR.MoveCompound;
		}
		else if (instruction == IR.LoadParamCompound)
		{
			newNode.Instruction = IR.MoveCompound;
		}
	}

	private Operand Map(Operand operand, Dictionary<Operand, Operand> map, Node callSiteNode)
	{
		if (operand == null)
			return null;

		// Create parent first
		if (operand.HasParent)
		{
			Map(operand.Parent, map, callSiteNode);
		}

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
		else if (operand.IsPhysicalRegister)
		{
			mappedOperand = MethodCompiler.PhysicalRegisters.Allocate(operand);
		}
		else if (operand.IsStaticField)
		{
			mappedOperand = operand;
		}
		else if (operand.IsConstant)
		{
			mappedOperand = operand;
		}

		map.Add(operand, mappedOperand);

		if (operand.IsParent)
		{
			Debug.Assert(operand.Low != null);
			Debug.Assert(operand.High != null);

			MethodCompiler.VirtualRegisters.SplitOperand(mappedOperand);

			map.AddIfNew(operand.Low, mappedOperand.Low);
			map.AddIfNew(operand.High, mappedOperand.High);
		}

		return mappedOperand;
	}
}
