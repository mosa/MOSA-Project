// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
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

		private readonly Counter InlinedMethodsCount = new Counter("InlineMethodEvaluationStage.InlinedMethods");
		private readonly Counter ReversedInlinedMethodsCount = new Counter("InlineMethodEvaluationStage.ReversedInlinedMethods");

		public const int MaximumCompileCount = 10;

		protected override void Initialize()
		{
			Register(InlinedMethodsCount);
			Register(ReversedInlinedMethodsCount);
		}

		protected override void Run()
		{
			var trace = CreateTraceLog();

			MethodData.IsCompiled = false;
			MethodData.BasicBlocks = null;
			MethodData.HasProtectedRegions = HasProtectedRegions;
			MethodData.IsLinkerGenerated = Method.IsCompilerGenerated;
			MethodData.IsMethodImplementationReplaced = MethodCompiler.IsMethodPlugged;
			MethodData.HasDoNotInlineAttribute = Method.IsNoInlining;
			MethodData.HasAggressiveInliningAttribute = Method.IsAggressiveInlining;
			MethodData.HasAddressOfInstruction = false;
			MethodData.HasLoops = false;
			MethodData.IsVirtual = Method.IsVirtual;
			MethodData.IsDevirtualized = Method.IsVirtual && !TypeLayout.IsMethodOverridden(Method);

			trace?.Log($"DoNotInline: {MethodData.DoNotInline}");
			trace?.Log($"IsVirtual: {MethodData.IsVirtual}");
			trace?.Log($"IsDevirtualized: {MethodData.IsDevirtualized}");
			trace?.Log($"HasProtectedRegions: {MethodData.HasProtectedRegions}");
			trace?.Log($"HasDoNotInlineAttribute: {MethodData.HasDoNotInlineAttribute}");
			trace?.Log($"HasAggressiveInliningAttribute: {MethodData.HasAggressiveInliningAttribute}");
			trace?.Log($"IsMethodImplementationReplaced (Plugged): {MethodData.IsMethodImplementationReplaced}");
			trace?.Log($"CompileCount: {MethodData.CompileCount}");

			if (StaticCanNotInline(MethodData, Method))
			{
				trace?.Log($"** Staticly Evaluated");
				trace?.Log($"Inlined: {MethodData.Inlined}");
				return;
			}

			var currentInlineStatus = MethodData.Inlined;

			int totalIRCount = 0;
			int totalNonIRCount = 0;
			int totalStackParameterInstruction = 0;

			if (!Method.IsNoInlining)
			{
				foreach (var block in BasicBlocks)
				{
					for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
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
							|| node.Instruction == IRInstruction.LoadParamCompound
							|| node.Instruction == IRInstruction.LoadParamInt32
							|| node.Instruction == IRInstruction.LoadParamInt64
							|| node.Instruction == IRInstruction.LoadParamFloatR4
							|| node.Instruction == IRInstruction.LoadParamFloatR4
							|| node.Instruction == IRInstruction.LoadParamSignExtend16x32
							|| node.Instruction == IRInstruction.LoadParamSignExtend16x64
							|| node.Instruction == IRInstruction.LoadParamSignExtend32x64
							|| node.Instruction == IRInstruction.LoadParamSignExtend8x32
							|| node.Instruction == IRInstruction.LoadParamSignExtend8x64
							|| node.Instruction == IRInstruction.LoadParamZeroExtend16x32
							|| node.Instruction == IRInstruction.LoadParamZeroExtend16x64
							|| node.Instruction == IRInstruction.LoadParamZeroExtend32x64
							|| node.Instruction == IRInstruction.LoadParamZeroExtend8x32
							|| node.Instruction == IRInstruction.LoadParamZeroExtend8x64

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
			}

			MethodData.IRInstructionCount = totalIRCount;
			MethodData.NonIRInstructionCount = totalNonIRCount;
			MethodData.IRStackParameterInstructionCount = totalStackParameterInstruction;

			bool inline = CanInline(MethodData, Method);

			MethodData.Inlined = inline;
			MethodCompiler.IsMethodInlined = inline;

			bool triggerReschedules = inline || (currentInlineStatus && !inline);

			if (inline)
			{
				MethodData.BasicBlocks = CopyInstructions();
			}

			if (triggerReschedules)
			{
				MethodScheduler.AddToInlineQueueByCallee(MethodData);
			}

			trace?.Log($"IRInstructionCount: {MethodData.IRInstructionCount}");
			trace?.Log($"IRStackParameterInstructionCount: {MethodData.IRStackParameterInstructionCount}");
			trace?.Log($"InlinedIRMaximum: {CompilerOptions.InlinedIRMaximum}");
			trace?.Log($"NonIRInstructionCount: {MethodData.NonIRInstructionCount}");
			trace?.Log($"HasAddressOfInstruction: {MethodData.HasAddressOfInstruction}");
			trace?.Log($"HasLoops: {MethodData.HasLoops}");
			trace?.Log($"** Dynamically Evaluated");
			trace?.Log($"Inlined: {MethodData.Inlined}");

			InlinedMethodsCount.Set(inline);
			ReversedInlinedMethodsCount.Set(MethodData.CompileCount >= MaximumCompileCount);
		}

		public bool StaticCanNotInline(MethodData methodData, MosaMethod method)
		{
			if (methodData.HasDoNotInlineAttribute)
				return true;

			if (methodData.IsMethodImplementationReplaced)
				return true;

			if (methodData.HasProtectedRegions)
				return true;

			if (methodData.IsVirtual && !methodData.IsDevirtualized)
				return true;

			if (methodData.DoNotInline)
				return true;

			if (method.DeclaringType.IsValueType
				&& method.IsVirtual
				&& !method.IsConstructor
				&& !method.IsStatic)
				return true;

			var returnType = methodData.Method.Signature.ReturnType;

			// FIXME: Add rational
			if (MosaTypeLayout.IsStoredOnStack(returnType) && !returnType.IsUI8 && !returnType.IsR8)
				return true;

			return false;
		}

		public bool CanInline(MethodData methodData, MosaMethod method)
		{
			if (StaticCanNotInline(methodData, method))
				return false;

			// current implementation limitation - can't include methods with addressOf instruction
			if (methodData.HasAddressOfInstruction)
				return false;

			if (methodData.NonIRInstructionCount > 0)
				return false;

			if (MethodData.CompileCount >= MaximumCompileCount)
				return false;   // too many compiles - cyclic loop suspected

			// methods with aggressive inline attribute will double the allow IR instruction count
			int max = methodData.HasAggressiveInliningAttribute ? (CompilerOptions.InlinedIRMaximum * 2) : CompilerOptions.InlinedIRMaximum;

			if ((methodData.IRInstructionCount - methodData.IRStackParameterInstructionCount) > max)
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

				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
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

					newBlock.BeforeLast.Insert(newNode);
				}
			}

			var trace = CreateTraceLog("InlineMap");

			if (trace != null)
			{
				foreach (var entry in map)
				{
					trace.Log($"{entry.Value} from: {entry.Key}");
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

			map.Add(operand, mappedOperand);

			return mappedOperand;
		}
	}
}
