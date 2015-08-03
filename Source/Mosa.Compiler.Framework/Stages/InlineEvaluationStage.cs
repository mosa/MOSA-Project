// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		public static int IRMaximumForInline = 8;
		public static string InlineMethodAttribute = "System.Runtime.CompilerServices.MethodImplAttribute";

		protected override void Run()
		{
			var method = MethodCompiler.Method;
			var compilerMethod = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(method);

			var trace = CreateTraceLog("Inline");

			var plugMethod = MethodCompiler.Compiler.PlugSystem.GetPlugMethod(MethodCompiler.Method);

			bool firstCompile = (compilerMethod.CompileCount == 0);

			compilerMethod.BasicBlocks = null;
			compilerMethod.IsCompiled = true;
			compilerMethod.HasProtectedRegions = HasProtectedRegions;
			compilerMethod.IsLinkerGenerated = method.IsLinkerGenerated;
			compilerMethod.IsCILDecoded = (!method.IsLinkerGenerated && method.Code.Count > 0);
			compilerMethod.HasLoops = false;
			compilerMethod.IsPlugged = (plugMethod != null);
			compilerMethod.IsVirtual = method.IsVirtual;

			// TODO
			compilerMethod.HasDoNotInlineAttribute = false;

			int totalIRCount = 0;
			int totalIROtherCount = 0;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction is BaseIRInstruction)
					{
						totalIRCount++;
					}
					else
					{
						totalIROtherCount++;
					}
				}

				if (block.PreviousBlocks.Count > 1)
				{
					compilerMethod.HasLoops = true;
				}
			}

			compilerMethod.IRInstructionCount = totalIRCount;
			compilerMethod.IROtherInstructionCount = totalIROtherCount;

			var methodAttribute = method.FindCustomAttribute(InlineMethodAttribute);

			if (methodAttribute != null)
			{
				compilerMethod.HasDoNotInlineAttribute = true;
			}

			compilerMethod.CanInline = CanInline(compilerMethod);

			if (compilerMethod.CanInline)
			{
				compilerMethod.BasicBlocks = CopyInstructions();
			}

			//foreach(var called in compilerMethod.CalledBy)
			//{
			//	MethodCompiler.Compiler.CompilationScheduler.Schedule(called);
			//}

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

		public bool CanInline(CompilerMethodData method)
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

			var returnType = method.Method.Signature.ReturnType;

			if (TypeLayout.IsCompoundType(returnType) && !returnType.IsUI8 && !returnType.IsR8)
				return false;

			return true;
		}

		protected BasicBlocks CopyInstructions()
		{
			var newBasicBlocks = new BasicBlocks();
			var mapBlocks = new Dictionary<BasicBlock, BasicBlock>(BasicBlocks.Count);
			var map = new Dictionary<Operand, Operand>();

			foreach (var block in BasicBlocks)
			{
				var newBlock = newBasicBlocks.CreateBlock(block.Label);
				mapBlocks.Add(block, newBlock);
			}

			var newPrologueBlock = newBasicBlocks.GetByLabel(BasicBlock.PrologueLabel);

			foreach (var operand in MethodCompiler.Parameters)
			{
				if (operand.Definitions.Count > 0)
				{
					var newOp = Map(operand, map);

					var newOperand = Operand.CreateVirtualRegister(operand.Type, -operand.Index);

					newPrologueBlock.BeforeLast.Insert(new InstructionNode(IRInstruction.Move, newOperand, newOp));

					// redirect map from parameter to virtual register going forward
					map.Remove(operand);
					map.Add(operand, newOperand);
				}
			}

			foreach (var block in BasicBlocks)
			{
				var newBlock = newBasicBlocks.GetByLabel(block.Label);

				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					var newNode = new InstructionNode(node.Instruction, node.OperandCount, node.ResultCount);
					newNode.Size = node.Size;
					newNode.ConditionCode = node.ConditionCode;

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

					newBlock.BeforeLast.Insert(newNode);
				}
			}

			var trace = CreateTraceLog("InlineMap");

			if (trace.Active)
			{
				foreach (var entry in map)
				{
					trace.Log(entry.Value.ToString() + " from: " + entry.Key.ToString());
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
				mappedOperand = Operand.CreateStackLocal(operand.Type, operand.Register, operand.Index);
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
				mappedOperand = Operand.CreateField(operand.Field);
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