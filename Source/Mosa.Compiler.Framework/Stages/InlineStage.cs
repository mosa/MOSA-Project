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

			UpdateCounter("InlineStage.InlinedMethods", 1);
			UpdateCounter("InlineStage.InlinedCallSites", callSites.Count);
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

					if (node.Instruction == IRInstruction.SetReturn32
						|| node.Instruction == IRInstruction.SetReturn64
						|| node.Instruction == IRInstruction.SetReturnR4
						|| node.Instruction == IRInstruction.SetReturnR8
						|| node.Instruction == IRInstruction.SetReturnCompound)
					{
						if (callSiteNode.Result != null)
						{
							var newOperand = Map(node.Operand1, map, callSiteNode);

							BaseInstruction moveInstruction = null;

							if (node.Instruction == IRInstruction.SetReturn32)
								moveInstruction = IRInstruction.MoveInt32;
							else if (node.Instruction == IRInstruction.SetReturn64)
								moveInstruction = IRInstruction.MoveInt64;
							else if (node.Instruction == IRInstruction.SetReturnR4)
								moveInstruction = IRInstruction.MoveFloatR4;
							else if (node.Instruction == IRInstruction.SetReturnR8)
								moveInstruction = IRInstruction.MoveFloatR8;
							else if (node.Instruction == IRInstruction.SetReturnCompound)
								moveInstruction = IRInstruction.MoveCompound;

							Debug.Assert(moveInstruction != null);

							var moveNode = new InstructionNode(moveInstruction, callSiteNode.Result, newOperand);

							newBlock.BeforeLast.Insert(moveNode);
						}

						continue;
					}

					var newNode = new InstructionNode(node.Instruction, node.OperandCount, node.ResultCount)
					{
						ConditionCode = node.ConditionCode,
						Label = callSiteNode.Label
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

			var prologue = mapBlocks[blocks.PrologueBlock];

			var callSiteOperands = callSiteNode.GetOperands();

			if (callSiteOperands.Count > 1)
			{
				var context = new Context(prologue);

				for (int i = 1; i < callSiteOperands.Count; i++)
				{
					var operand = callSiteOperands[i];

					if (!operand.IsVirtualRegister || operand.Low == null)
						continue;

					context.AppendInstruction(IRInstruction.GetLow64, operand.Low, operand);
					context.AppendInstruction(IRInstruction.GetHigh64, operand.High, operand);
				}
			}

			callSiteNode.SetInstruction(IRInstruction.Jmp, prologue);

			//MethodCompiler.Stop();
		}

		private static void UpdateParameterInstructions(InstructionNode newNode)
		{
			var instruction = newNode.Instruction;

			if (instruction == IRInstruction.LoadParamFloatR4)
			{
				newNode.Instruction = IRInstruction.MoveFloatR4;
			}
			else if (instruction == IRInstruction.LoadParamFloatR8)
			{
				newNode.Instruction = IRInstruction.MoveFloatR8;
			}
			else if (instruction == IRInstruction.LoadParamInt32
				|| instruction == IRInstruction.LoadParamSignExtend8x32
				|| instruction == IRInstruction.LoadParamSignExtend16x32
				|| instruction == IRInstruction.LoadParamZeroExtend8x32
				|| instruction == IRInstruction.LoadParamZeroExtend16x32)
			{
				newNode.Instruction = IRInstruction.MoveInt32;
			}
			else if (instruction == IRInstruction.LoadParamInt64
				|| instruction == IRInstruction.LoadParamSignExtend8x64
				|| instruction == IRInstruction.LoadParamSignExtend16x64
				|| instruction == IRInstruction.LoadParamSignExtend32x64
				|| instruction == IRInstruction.LoadParamZeroExtend8x64
				|| instruction == IRInstruction.LoadParamZeroExtend16x64
				|| instruction == IRInstruction.LoadParamZeroExtend32x64)
			{
				newNode.Instruction = IRInstruction.MoveInt64;
			}
			else if (instruction == IRInstruction.StoreParamInt8
				|| instruction == IRInstruction.StoreParamInt16
				|| instruction == IRInstruction.StoreParamInt32)
			{
				newNode.SetInstruction(IRInstruction.MoveInt32, newNode.Operand1, newNode.Operand2);
			}
			else if (instruction == IRInstruction.StoreParamInt64)
			{
				newNode.SetInstruction(IRInstruction.MoveInt64, newNode.Operand1, newNode.Operand2);
			}
			else if (instruction == IRInstruction.StoreParamFloatR4)
			{
				newNode.SetInstruction(IRInstruction.MoveFloatR4, newNode.Operand1, newNode.Operand2);
			}
			else if (instruction == IRInstruction.StoreParamFloatR8)
			{
				newNode.SetInstruction(IRInstruction.MoveFloatR8, newNode.Operand1, newNode.Operand2);
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

			if (operand.HasLongParent)
			{
				MethodCompiler.SplitLongOperand(mappedOperand);

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
}
