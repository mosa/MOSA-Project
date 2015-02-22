/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class InlineEvaluationStage : BaseMethodCompilerStage
	{
		public static int IRMaximumForInline = 24;

		protected override void Run()
		{
			var method = MethodCompiler.Method;
			var compilerMethod = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(method);

			var trace = CreateTraceLog("Inline");

			compilerMethod.IsCompiled = true;
			compilerMethod.HasProtectedRegions = HasProtectedRegions;
			compilerMethod.IsLinkerGenerated = method.IsLinkerGenerated;
			compilerMethod.IsCILDecoded = (!method.IsLinkerGenerated && method.Code.Count > 0);
			compilerMethod.IsPlugged = !HasCode;
			compilerMethod.HasLoops = false;

			// TODO
			compilerMethod.HasDoNotInlineAttribute = false;

			int totalIRCount = 0;
			int totalIROtherCount = 0;

			foreach (var block in BasicBlocks)
			{
				int blockIRCount = 0;

				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction is BaseIRInstruction)
					{
						blockIRCount++;
					}
					else
					{
						totalIROtherCount++;
					}

					if (node.InvokeMethod != null)
					{
						Debug.Assert(node.Instruction == IRInstruction.Call || node.Instruction == IRInstruction.IntrinsicMethodCall);

						var invoked = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(node.InvokeMethod);

						compilerMethod.IsMethodInvoked = true;
						compilerMethod.AddCall(node.InvokeMethod);

						invoked.AddCalledBy(MethodCompiler.Method);
					}
				}

				if (!block.IsPrologue && !block.IsEpilogue)
				{
					totalIRCount = totalIRCount + blockIRCount;
				}

				if (block.PreviousBlocks.Count > 1)
				{
					compilerMethod.HasLoops = true;
				}
			}

			compilerMethod.IRInstructionCount = totalIRCount;
			compilerMethod.IROtherInstructionCount = totalIROtherCount;
			compilerMethod.IsVirtual = method.IsVirtual;

			compilerMethod.CanInline = CanInline(compilerMethod);

			if (compilerMethod.CanInline)
			{
				compilerMethod.BasicBlocks = CopyInstructions();
			}

			trace.Log("CanInline: " + compilerMethod.CanInline.ToString());
			trace.Log("IsVirtual: " + compilerMethod.IsVirtual.ToString());
			trace.Log("HasLoops: " + compilerMethod.HasLoops.ToString());
			trace.Log("HasProtectedRegions: " + compilerMethod.HasProtectedRegions.ToString());
			trace.Log("IRInstructionCount: " + compilerMethod.IRInstructionCount.ToString());
			trace.Log("IROtherInstructionCount: " + compilerMethod.IROtherInstructionCount.ToString());

			UpdateCounter("InlineMethodEvaluationStage.MethodCount", 1);
			if (compilerMethod.CanInline)
			{
				UpdateCounter("InlineMethodEvaluationStage.CanInline", 1);
			}
		}

		public static bool CanInline(CompilerMethodData method)
		{
			if (method.HasDoNotInlineAttribute)
				return false;

			if (method.IsPlugged)
				return false;

			if (method.HasProtectedRegions)
				return false;

			//if (method.HasLoops)
			//	return false;

			if (method.IsVirtual)
				return false;

			if (method.IROtherInstructionCount > 0)
				return false;

			if (method.IRInstructionCount > IRMaximumForInline)
				return false;

			return true;
		}

		protected BasicBlocks CopyInstructions()
		{
			var newBasicBlocks = new BasicBlocks();
			var map = new Dictionary<Operand, Operand>();

			foreach (var block in BasicBlocks)
			{
				newBasicBlocks.CreateBlock(block.Label);
			}

			foreach (var block in BasicBlocks)
			{
				var newBlock = newBasicBlocks.GetByLabel(block.Label);

				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					var newNode = new InstructionNode(node.Instruction, node.OperandCount, node.ResultCount);

					if (node.BranchTargets != null)
					{
						// copy targets
						foreach (var target in node.BranchTargets)
						{
							newNode.AddBranchTarget(target);
						}
					}

					// copy results
					for (int i = 0; i < node.ResultCount; i++)
					{
						var op = node.GetResult(i);

						var newOp = Map(op, map);

						newNode.SetResult(i, newOp);
					}

					// copy operands
					for (int i = 0; i < node.OperandCount; i++)
					{
						var op = node.GetOperand(i);

						var newOp = Map(op, map);

						newNode.SetOperand(i, newOp);
					}

					// copy other
					if (node.MosaType != null)
						newNode.MosaType = node.MosaType;
					if (node.MosaField != null)
						newNode.MosaField = node.MosaField;
					if (node.InvokeMethod != null)
						newNode.InvokeMethod = node.InvokeMethod;

					newBlock.Last.Previous.Insert(newNode);
				}
			}

			var trace = CreateTraceLog("InlineMap");

			if (trace.Active)
			{
				foreach (var entry in map)
				{
					//if (entry.Value != null)
					trace.Log(entry.Value.ToString());
				}
			}

			return newBasicBlocks;
		}

		private static Operand Map(Operand operand, Dictionary<Operand, Operand> map)
		{
			if (operand == null)
				return null;

			Operand mappedOperand;

			if (map.TryGetValue(operand, out mappedOperand))
			{
				return mappedOperand;
			}

			if (operand.IsSymbol)
			{
				if (operand.StringData != null)
				{
					mappedOperand = Operand.CreateStringSymbol(operand.Type.TypeSystem, operand.Name, operand.StringData);
				}
				else if (operand.Method != null)
				{
					mappedOperand = Operand.CreateSymbolFromMethod(operand.Type.TypeSystem, operand.Method);
				}
				else if (operand.Name != null)
				{
					mappedOperand = Operand.CreateManagedSymbol(operand.Type, operand.Name);
				}
			}
			else if (operand.IsParameter)
			{
				mappedOperand = operand;
			}
			else if (operand.IsStackLocal)
			{
				if (operand.Uses.Count != 0)
				{
					mappedOperand = Operand.CreateStackLocal(operand.Type, operand.Register, operand.Index);
				}
			}
			else if (operand.IsVirtualRegister)
			{
				if (operand.Uses.Count != 0 || operand.Definitions.Count != 0)
				{
					mappedOperand = Operand.CreateVirtualRegister(operand.Type, operand.Index, operand.Name);
				}
			}
			else if (operand.IsField)
			{
				mappedOperand = operand;
			}
			else if (operand.IsConstant)
			{
				mappedOperand = operand;
			}
			else if (operand.IsCPURegister)
			{
				mappedOperand = operand;
			}

			Debug.Assert(mappedOperand != null);

			map.Add(operand, mappedOperand);

			return mappedOperand;
		}
	}
}
