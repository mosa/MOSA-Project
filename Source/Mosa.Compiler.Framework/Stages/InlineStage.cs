// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Inline Stage
	/// </summary>
	public class InlineStage : BaseMethodCompilerStage
	{
		private readonly Counter InlineCount = new Counter("InlineStage.MethodsWithInlinedCallSites");
		private readonly Counter InlinedCallSitesCount = new Counter("InlineStage.InlinedCallSites");

		protected override void Initialize()
		{
			Register(InlineCount);
			Register(InlinedCallSitesCount);
		}

		protected override void Run()
		{
			var trace = CreateTraceLog("Inlined");

			int callSiteCount = 0;

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

				Debug.Assert(callSiteNode.Operand1.IsSymbol);

				var callee = MethodCompiler.Compiler.GetMethodData(invokedMethod);

				var inlineMethodData = callee.GetInlineMethodDataForUseBy(Method);

				//Debug.WriteLine($"{MethodScheduler.GetTimestamp()} - Inline: {(inlineMethodData.IsInlined ? "Inlined" : "NOT Inlined")} [{MethodData.Version}] {Method} -> [{inlineMethodData.Version}] {callee.Method}"); //DEBUGREMOVE

				if (!inlineMethodData.IsInlined)
					continue;

				Inline(callSiteNode, inlineMethodData.BasicBlocks);
				callSiteCount++;

				trace?.Log($" -> Inlined: [{callee.Version}] {callee.Method}");

				//Debug.WriteLine($" -> Inlined: [{callee.Version}] {callee.Method}");//DEBUGREMOVE
			}

			InlineCount.Set(1);
			InlinedCallSitesCount.Set(callSiteCount);
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
						|| node.Instruction == IRInstruction.SetReturnR4
						|| node.Instruction == IRInstruction.SetReturnR8
						|| node.Instruction == IRInstruction.SetReturnCompound)
					{
						if (callSiteNode.Result != null)
						{
							var newOperand = Map(node.Operand1, map, callSiteNode);

							BaseInstruction moveInstruction = null;

							if (node.Instruction == IRInstruction.SetReturn32)
								moveInstruction = IRInstruction.Move32;
							else if (node.Instruction == IRInstruction.SetReturn64)
								moveInstruction = IRInstruction.Move64;
							else if (node.Instruction == IRInstruction.SetReturnR4)
								moveInstruction = IRInstruction.MoveR4;
							else if (node.Instruction == IRInstruction.SetReturnR8)
								moveInstruction = IRInstruction.MoveR8;
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
		}

		private static void UpdateParameterInstructions(InstructionNode newNode)
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

			if (operand.IsSymbol)
			{
				if (operand.IsString)
				{
					// FUTURE: explore operand re-use
					mappedOperand = Operand.CreateStringSymbol(operand.Name, operand.StringData, operand.Type.TypeSystem);
				}
				else if (operand.Method != null)
				{
					// FUTURE: explore operand re-use
					mappedOperand = Operand.CreateSymbolFromMethod(operand.Method, operand.Type.TypeSystem);
				}
				else if (operand.Name != null)
				{
					// FUTURE: explore operand re-use
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
				// FUTURE: explore operand re-use
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
