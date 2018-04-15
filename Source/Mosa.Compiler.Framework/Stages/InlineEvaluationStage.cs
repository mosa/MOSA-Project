// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Inline Evaluation Stage
	/// </summary>
	public class InlineEvaluationStage : BaseMethodCompilerStage
	{
		public static string InlineMethodAttribute = "System.Runtime.CompilerServices.MethodImplAttribute";

		public const int MaximumCompileCount = 10;

		protected override void Run()
		{
			var method = MethodCompiler.Method;

			var trace = CreateTraceLog("Inline");

			bool firstCompile = (MethodData.CompileCount == 0);

			MethodData.BasicBlocks = null;
			MethodData.IsCompiled = true;
			MethodData.HasProtectedRegions = HasProtectedRegions;
			MethodData.IsLinkerGenerated = method.IsLinkerGenerated;
			MethodData.IsCILDecoded = (!method.IsLinkerGenerated && method.Code.Count > 0);
			MethodData.HasLoops = false;
			MethodData.IsPlugged = IsPlugged;
			MethodData.HasDoNotInlineAttribute = false;
			MethodData.HasAddressOfInstruction = false;
			MethodData.IsVirtual = method.IsVirtual;
			MethodData.IsDevirtualized = (method.IsVirtual && !TypeLayout.IsMethodOverridden(method));

			int totalIRCount = 0;
			int totalNonIRCount = 0;
			int totalStackParameterInstruction = 0;

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
						totalNonIRCount++;
					}

					if (node.Instruction == IRInstruction.AddressOf)
					{
						MethodData.HasAddressOfInstruction = true;
					}

					if (node.Instruction == IRInstruction.SetReturn32
						|| node.Instruction == IRInstruction.SetReturn64
						|| node.Instruction == IRInstruction.SetReturnR4
						|| node.Instruction == IRInstruction.SetReturnR8
						|| node.Instruction == IRInstruction.LoadParameterCompound
						|| node.Instruction == IRInstruction.LoadParameterInteger32
						|| node.Instruction == IRInstruction.LoadParameterInteger64
						|| node.Instruction == IRInstruction.LoadParameterFloatR4
						|| node.Instruction == IRInstruction.LoadParameterFloatR4
						|| node.Instruction == IRInstruction.LoadParameterSignExtended16x32
						|| node.Instruction == IRInstruction.LoadParameterSignExtended16x64
						|| node.Instruction == IRInstruction.LoadParameterSignExtended32x64
						|| node.Instruction == IRInstruction.LoadParameterSignExtended8x32
						|| node.Instruction == IRInstruction.LoadParameterSignExtended8x64
						|| node.Instruction == IRInstruction.LoadParameterZeroExtended16x32
						|| node.Instruction == IRInstruction.LoadParameterZeroExtended16x64
						|| node.Instruction == IRInstruction.LoadParameterZeroExtended32x64
						|| node.Instruction == IRInstruction.LoadParameterZeroExtended8x32
						|| node.Instruction == IRInstruction.LoadParameterZeroExtended8x64
						//|| node.Instruction == IRInstruction.Epilogue
						//|| node.Instruction == IRInstruction.Prologue
						|| node.Block.IsEpilogue
						|| node.Block.IsPrologue
						)
					{
						totalStackParameterInstruction++;
					}
				}

				if (block.PreviousBlocks.Count > 1)
				{
					MethodData.HasLoops = true;
				}
			}

			MethodData.IRInstructionCount = totalIRCount;
			MethodData.NonIRInstructionCount = totalNonIRCount;
			MethodData.IRStackParameterInstructionCount = totalStackParameterInstruction;

			MethodData.HasDoNotInlineAttribute = MethodCompiler.Method.IsNoInlining;

			//if (!compilerMethod.HasDoNotInlineAttribute)
			//{
			//	// check attribute
			//	// BUG: does not find the attribute
			//	var methodAttribute = method.FindCustomAttribute(InlineMethodAttribute);

			//	//TODO: check for specific attribute: System.Runtime.CompilerServices.MethodImplOptions.NoInlining
			//	if (methodAttribute != null)
			//	{
			//		compilerMethod.HasDoNotInlineAttribute = true;
			//	}
			//}

			MethodData.CanInline = CanInline(MethodData);

			if (MethodData.CanInline)
			{
				MethodData.BasicBlocks = CopyInstructions();

				if (MethodData.CompileCount < MaximumCompileCount)
				{
					MethodCompiler.Compiler.CompilationScheduler.AddToInlineQueue(MethodData);
				}
			}

			//lock (compilerMethod)
			//{
			//	foreach (var called in compilerMethod.CalledBy)
			//	{
			//		var calledMethod = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(called);

			//		if (calledMethod.CompileCount < MaximumCompileCount)
			//		{
			//			MethodCompiler.Compiler.CompilationScheduler.Schedule(called);
			//		}
			//	}
			//}

			trace.Log("CanInline: " + MethodData.CanInline.ToString());
			trace.Log("IsVirtual: " + MethodData.IsVirtual.ToString());
			trace.Log("IsDevirtualized: " + MethodData.IsDevirtualized.ToString());
			trace.Log("HasLoops: " + MethodData.HasLoops.ToString());
			trace.Log("HasProtectedRegions: " + MethodData.HasProtectedRegions.ToString());
			trace.Log("IRInstructionCount: " + MethodData.IRInstructionCount.ToString());
			trace.Log("IRStackParameterInstructionCount: " + MethodData.IRStackParameterInstructionCount.ToString());
			trace.Log("InlinedIRMaximum: " + CompilerOptions.InlinedIRMaximum.ToString());
			trace.Log("NonIRInstructionCount: " + MethodData.NonIRInstructionCount.ToString());
			trace.Log("HasDoNotInlineAttribute: " + MethodData.HasDoNotInlineAttribute.ToString());
			trace.Log("IsPlugged: " + MethodData.IsPlugged.ToString());
			trace.Log("HasAddressOfInstruction: " + MethodData.HasAddressOfInstruction.ToString());

			UpdateCounter("InlineMethodEvaluationStage.MethodCount", 1);
			if (MethodData.CanInline)
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

			if (method.IsVirtual && !method.IsDevirtualized)
				return false;

			// current implementation limitation - can't include methods with addressOf instruction
			if (method.HasAddressOfInstruction)
				return false;

			if (method.NonIRInstructionCount > 0)
				return false;

			if ((method.IRInstructionCount - method.IRStackParameterInstructionCount) > CompilerOptions.InlinedIRMaximum)
				//if (method.IRInstructionCount > CompilerOptions.InlinedIRMaximum)
				return false;

			var returnType = method.Method.Signature.ReturnType;

			// FIXME: Add rational
			if (MosaTypeLayout.IsStoredOnStack(returnType) && !returnType.IsUI8 && !returnType.IsR8)
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

					var moveInstruction = MosaTypeLayout.IsStoredOnStack(newOperand.Type)
						? IRInstruction.MoveCompound
						: GetMoveInstruction(newOperand.Type);

					var moveNode = new InstructionNode(moveInstruction, newOperand, newOp);

					newPrologueBlock.BeforeLast.Insert(moveNode);

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

					var newNode = new InstructionNode(node.Instruction, node.OperandCount, node.ResultCount)
					{
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
					trace.Log(entry.Value + " from: " + entry.Key);
				}
			}

			return newBasicBlocks;
		}

		private Operand Map(Operand operand, Dictionary<Operand, Operand> map)
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
				mappedOperand = operand;
			}
			else if (operand.IsStackLocal)
			{
				mappedOperand = Operand.CreateStackLocal(operand.Type, operand.Index, operand.IsPinned);
			}
			else if (operand.IsVirtualRegister)
			{
				if (operand.Uses.Count != 0 || operand.Definitions.Count != 0)
				{
					mappedOperand = Operand.CreateVirtualRegister(operand.Type, operand.Index);
				}
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
