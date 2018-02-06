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

			var callSites = new List<InstructionNode>();

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

					callSites.Add(node);

					var invoked = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(invokedMethod);

					MethodData.Calls.AddIfNew(invokedMethod);

					invoked.AddCalledBy(MethodCompiler.Method);
				}
			}

			if (callSites.Count == 0)
				return;

			var trace = CreateTraceLog("Inlined");

			foreach (var callSiteNode in callSites)
			{
				var invokedMethod = callSiteNode.Operand1.Method;

				var callee = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(invokedMethod);

				if (!callee.CanInline)
					continue;

				// don't inline self
				if (callee.Method == MethodCompiler.Method)
					continue;

				var blocks = callee.BasicBlocks;

				if (blocks == null)
					continue;

				if (trace.Active)
					trace.Log(callee.Method.FullName);

				Inline(callSiteNode, blocks);
			}

			UpdateCounter("InlineStage.InlinedMethodCount", 1);
			UpdateCounter("InlineStage.InlinedCallSiteCount", callSites.Count);

			//UpdateCounter("InlineStage.Compiled", MethodData.CompileCount == 0 ? 1 : 0);
			//UpdateCounter("InlineStage.Recompiled", MethodData.CompileCount > 1 ? 1 : 0);
		}

		protected void Inline(InstructionNode callSiteNode, BasicBlocks blocks)
		{
			var mapBlocks = new Dictionary<BasicBlock, BasicBlock>(blocks.Count);
			var map = new Dictionary<Operand, Operand>();

			var nextBlock = Split(callSiteNode);

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
						if (callSiteNode.Result != null)
						{
							var newOp = Map(node.Operand1, map, callSiteNode);
							var moveInsturction = GetMoveInstruction(callSiteNode.Result.Type);

							var moveNode = new InstructionNode(moveInsturction, callSiteNode.Result, newOp);

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

						var newOp = Map(op, map, callSiteNode);

						newNode.SetResult(i, newOp);
					}

					// copy operands
					for (int i = 0; i < node.OperandCount; i++)
					{
						var op = node.GetOperand(i);

						var newOp = Map(op, map, callSiteNode);

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

			callSiteNode.SetInstruction(IRInstruction.Jmp, mapBlocks[blocks.PrologueBlock]);
		}

		private static void UpdateParameterInstructions(InstructionNode newNode)
		{
			var instruction = newNode.Instruction;
			if (instruction == IRInstruction.LoadParameterFloatR4)
			{
				instruction = IRInstruction.MoveFloatR4;
			}
			else if (instruction == IRInstruction.LoadParameterFloatR8)
			{
				instruction = IRInstruction.MoveFloatR8;
			}
			else if (instruction == IRInstruction.LoadParameterInteger32
				|| instruction == IRInstruction.LoadParameterInteger64
				|| instruction == IRInstruction.LoadParameterSignExtended8x32
				|| instruction == IRInstruction.LoadParameterSignExtended16x32
				|| instruction == IRInstruction.LoadParameterSignExtended8x64
				|| instruction == IRInstruction.LoadParameterSignExtended16x64
				|| instruction == IRInstruction.LoadParameterSignExtended32x64
				|| instruction == IRInstruction.LoadParameterZeroExtended8x32
				|| instruction == IRInstruction.LoadParameterZeroExtended16x32
				|| instruction == IRInstruction.LoadParameterZeroExtended8x64
				|| instruction == IRInstruction.LoadParameterZeroExtended16x64
				|| instruction == IRInstruction.LoadParameterZeroExtended32x64)
			{
				instruction = IRInstruction.MoveInteger;
			}
			else if (instruction == IRInstruction.StoreParameterInteger8
				|| instruction == IRInstruction.StoreParameterInteger16
				|| instruction == IRInstruction.StoreParameterInteger32
				|| instruction == IRInstruction.StoreParameterInteger64)
			{
				newNode.SetInstruction(IRInstruction.MoveInteger, newNode.Operand1, newNode.Operand2);
			}
			else if (instruction == IRInstruction.StoreParameterFloatR4)
			{
				newNode.SetInstruction(IRInstruction.MoveFloatR4, newNode.Operand1, newNode.Operand2);
			}
			else if (instruction == IRInstruction.StoreParameterFloatR8)
			{
				newNode.SetInstruction(IRInstruction.MoveFloatR8, newNode.Operand1, newNode.Operand2);
			}
			else if (instruction == IRInstruction.StoreParameterCompound)
			{
				instruction = IRInstruction.MoveCompound;
			}
			else if (instruction == IRInstruction.LoadParameterCompound)
			{
				instruction = IRInstruction.MoveCompound;
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
				mappedOperand = callSiteNode.GetOperand(operand.Index + 1);
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
