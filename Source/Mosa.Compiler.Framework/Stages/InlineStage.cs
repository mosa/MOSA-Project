// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Inline Stage
	/// </summary>
	public class InlineStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (HasProtectedRegions)
				return;

			if (MethodCompiler.Method.IsLinkerGenerated && MethodCompiler.Method.Name == TypeInitializerSchedulerStage.TypeInitializerName)
				return;

			MethodData.CompileCount++;
			MethodData.Calls.Clear();

			var nodes = new List<InstructionNode>();

			// find all call sites
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction != IRInstruction.CallStatic)
						continue;

					if (!node.Operand1.IsSymbol)
						continue;

					var invokedMethod = node.Operand1.Method;

					if (invokedMethod == null)
						continue;

					nodes.Add(node);

					var invoked = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(invokedMethod);

					MethodData.Calls.AddIfNew(invokedMethod);

					invoked.AddCalledBy(MethodCompiler.Method);
				}
			}

			if (nodes.Count == 0)
				return;

			var trace = CreateTraceLog("Inlined");

			foreach (var node in nodes)
			{
				var invokedMethod = node.Operand1.Method;

				var invoked = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(invokedMethod);

				if (!invoked.CanInline)
					continue;

				// don't inline self
				if (invoked.Method == MethodCompiler.Method)
					continue;

				var blocks = invoked.BasicBlocks;

				if (blocks == null)
					continue;

				if (trace.Active)
					trace.Log(invoked.Method.FullName);

				Inline(node, blocks);
			}

			UpdateCounter("InlineStage.InlinedMethodCount", 1);
			UpdateCounter("InlineStage.InlinedCallSiteCount", nodes.Count);

			//UpdateCounter("InlineStage.Compiled", MethodData.CompileCount == 0 ? 1 : 0);
			//UpdateCounter("InlineStage.Recompiled", MethodData.CompileCount > 1 ? 1 : 0);
		}

		protected void Inline(InstructionNode callNode, BasicBlocks blocks)
		{
			var mapBlocks = new Dictionary<BasicBlock, BasicBlock>(blocks.Count);
			var map = new Dictionary<Operand, Operand>();

			var nextBlock = Split(callNode);

			// create basic blocks
			foreach (var block in blocks)
			{
				var newBlock = CreateNewBlock();
				mapBlocks.Add(block, newBlock);
			}

			// copy instructions
			foreach (var block in blocks)
			{
				var newBlock = mapBlocks[block];

				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
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

					if (node.Instruction == IRInstruction.SetReturn)
					{
						if (callNode.Result != null)
						{
							var newOp = Map(node.Operand1, map, callNode);
							var moveInsturction = GetMoveInstruction(callNode.Result.Type);

							var moveNode = new InstructionNode(moveInsturction, callNode.Result, newOp);

							newBlock.BeforeLast.Insert(moveNode);
						}
						continue;
					}

					var newNode = new InstructionNode(node.Instruction, node.OperandCount, node.ResultCount)
					{
						Size = node.Size,
						ConditionCode = node.ConditionCode
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
					for (int i = 0; i < node.ResultCount; i++)
					{
						var op = node.GetResult(i);

						var newOp = Map(op, map, callNode);

						newNode.SetResult(i, newOp);
					}

					// copy operands
					for (int i = 0; i < node.OperandCount; i++)
					{
						var op = node.GetOperand(i);

						var newOp = Map(op, map, callNode);

						newNode.SetOperand(i, newOp);
					}

					// copy other
					if (node.MosaType != null)
						newNode.MosaType = node.MosaType;
					if (node.MosaField != null)
						newNode.MosaField = node.MosaField;
					if (node.InvokeMethod != null)
						newNode.InvokeMethod = node.InvokeMethod;

					UpdateParameterInstructions(newNode);

					newBlock.BeforeLast.Insert(newNode);
				}
			}

			callNode.SetInstruction(IRInstruction.Jmp, mapBlocks[blocks.PrologueBlock]);
		}

		private static void UpdateParameterInstructions(InstructionNode newNode)
		{
			if (newNode.Instruction == IRInstruction.LoadParameterFloatR4)
			{
				newNode.Instruction = IRInstruction.MoveFloatR4;
			}
			else if (newNode.Instruction == IRInstruction.LoadParameterFloatR8)
			{
				newNode.Instruction = IRInstruction.MoveFloatR8;
			}
			else if (newNode.Instruction == IRInstruction.LoadParameterInteger)
			{
				newNode.Instruction = IRInstruction.MoveInteger;
			}
			else if (newNode.Instruction == IRInstruction.LoadParameterSignExtended)
			{
				newNode.Instruction = IRInstruction.MoveSignExtended;
			}
			else if (newNode.Instruction == IRInstruction.LoadParameterZeroExtended)
			{
				newNode.Instruction = IRInstruction.MoveZeroExtended;
			}
			else if (newNode.Instruction == IRInstruction.StoreParameterInteger8
				|| newNode.Instruction == IRInstruction.StoreParameterInteger16
				|| newNode.Instruction == IRInstruction.StoreParameterInteger32
				|| newNode.Instruction == IRInstruction.StoreParameterInteger64)
			{
				newNode.Instruction = IRInstruction.MoveInteger;
				newNode.Result = newNode.Operand1;
				newNode.ResultCount = 1;
				newNode.Operand1 = newNode.Operand2;
				newNode.Operand2 = null;
				newNode.OperandCount = 1;
			}
			else if (newNode.Instruction == IRInstruction.StoreParameterFloatR4)
			{
				newNode.Instruction = IRInstruction.MoveFloatR4;
				newNode.Result = newNode.Operand1;
				newNode.ResultCount = 1;
				newNode.Operand1 = newNode.Operand2;
				newNode.Operand2 = null;
				newNode.OperandCount = 1;
			}
			else if (newNode.Instruction == IRInstruction.StoreParameterFloatR8)
			{
				newNode.Instruction = IRInstruction.MoveFloatR8;
				newNode.Result = newNode.Operand1;
				newNode.ResultCount = 1;
				newNode.Operand1 = newNode.Operand2;
				newNode.Operand2 = null;
				newNode.OperandCount = 1;
			}
			else if (newNode.Instruction == IRInstruction.StoreParameterCompound)
			{
				newNode.Instruction = IRInstruction.MoveCompound;
			}
			else if (newNode.Instruction == IRInstruction.LoadParameterCompound)
			{
				newNode.Instruction = IRInstruction.MoveCompound;
			}
		}

		private Operand Map(Operand operand, Dictionary<Operand, Operand> map, InstructionNode callNode)
		{
			if (operand == null)
				return null;

			if (map.TryGetValue(operand, out Operand mappedOperand))
			{
				return mappedOperand;
			}

			if (operand.IsSymbol)
			{
				if (operand.StringData != null)
				{
					mappedOperand = Operand.CreateStringSymbol(operand.Name, operand.StringData, operand.Type.TypeSystem);
				}
				else if (operand.Method != null)
				{
					mappedOperand = Operand.CreateSymbolFromMethod(operand.Method, operand.Type.TypeSystem);
				}
				else if (operand.Name != null)
				{
					mappedOperand = Operand.CreateSymbol(operand.Type, operand.Name);
				}
			}
			else if (operand.IsParameter)
			{
				mappedOperand = callNode.GetOperand(operand.Index + 1);
			}
			else if (operand.IsStackLocal)
			{
				mappedOperand = MethodCompiler.AddStackLocal(operand.Type, operand.IsPinned);
			}
			else if (operand.IsVirtualRegister)
			{
				mappedOperand = AllocateVirtualRegister(operand.Type);
			}
			else if (operand.IsStaticField)
			{
				mappedOperand = Operand.CreateStaticField(operand.Field, TypeSystem);
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

			map.Add(operand, mappedOperand);

			return mappedOperand;
		}
	}
}
