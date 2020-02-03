// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
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

		private readonly Counter InlineCount = new Counter("InlineMethodEvaluationStage.Inline");
		private readonly Counter ReversedInlineCount = new Counter("InlineMethodEvaluationStage.ReversedInline");

		public const int MaximumCompileCount = 10;

		private bool InlineExplicitOnly;
		private int InlineAggressiveMaximum;
		private int InlineMaximum;

		protected override void Initialize()
		{
			Register(InlineCount);
			Register(ReversedInlineCount);

			// cache for performance
			InlineExplicitOnly = CompilerSettings.InlineExplicitOnly;
			InlineAggressiveMaximum = CompilerSettings.InlineAggressiveMaximum;
			InlineMaximum = CompilerSettings.InlineMaximum;
		}

		protected override void Run()
		{
			var trace = CreateTraceLog();

			MethodData.HasAddressOfInstruction = false;
			MethodData.HasLoops = false;

			//MethodData.IsDevirtualized = Method.IsVirtual && !TypeLayout.IsMethodOverridden(Method);

			trace?.Log($"DoNotInline: {MethodData.DoNotInline}");
			trace?.Log($"IsVirtual: {Method.IsVirtual}");
			trace?.Log($"IsDevirtualized: {MethodData.IsDevirtualized}");
			trace?.Log($"HasProtectedRegions: {MethodData.HasProtectedRegions}");
			trace?.Log($"HasDoNotInlineAttribute: {MethodData.HasDoNotInlineAttribute}");
			trace?.Log($"HasAggressiveInliningAttribute: {MethodData.HasAggressiveInliningAttribute}");
			trace?.Log($"AggressiveInlineRequested: {MethodData.AggressiveInlineRequested}");
			trace?.Log($"IsMethodImplementationReplaced (Plugged): {MethodData.IsMethodImplementationReplaced}");
			trace?.Log($"CompileCount: {MethodData.Version}");

			if (StaticCanNotInline(MethodData))
			{
				SetInlinedBasicBlocks(null);

				trace?.Log($"** Statically Evaluated");
				trace?.Log($"Inlined: {MethodData.Inlined}");

				//Debug.WriteLine($">Inlined: No"); //DEBUGREMOVE

				return;
			}

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
							|| node.Instruction == IRInstruction.LoadParam32
							|| node.Instruction == IRInstruction.LoadParam64
							|| node.Instruction == IRInstruction.LoadParamR4
							|| node.Instruction == IRInstruction.LoadParamR4
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

			bool inline = CanInline(MethodData);

			if (inline)
			{
				var inlineBlocks = CopyInstructions();
				SetInlinedBasicBlocks(inlineBlocks);
			}
			else
			{
				SetInlinedBasicBlocks(null);
			}

			trace?.Log($"IRInstructionCount: {MethodData.IRInstructionCount}");
			trace?.Log($"IRStackParameterInstructionCount: {MethodData.IRStackParameterInstructionCount}");
			trace?.Log($"InlinedIRMaximum: {InlineMaximum}");
			trace?.Log($"InlineExplicitOnly: {InlineExplicitOnly}");
			trace?.Log($"NonIRInstructionCount: {MethodData.NonIRInstructionCount}");
			trace?.Log($"HasAddressOfInstruction: {MethodData.HasAddressOfInstruction}");
			trace?.Log($"HasLoops: {MethodData.HasLoops}");
			trace?.Log($"** Dynamically Evaluated");
			trace?.Log($"Inlined: {MethodData.Inlined}");

			InlineCount.Set(inline);
			ReversedInlineCount.Set(MethodData.Version >= MaximumCompileCount);

			//Debug.WriteLine($">Inlined: {(inline ? "Yes" : "No")}"); //DEBUGREMOVE
		}

		private void SetInlinedBasicBlocks(BasicBlocks inlineBlocks)
		{
			MethodCompiler.IsMethodInlined = inlineBlocks != null;

			var previousInlineMethodData = MethodData.SwapInlineMethodData(inlineBlocks);

			ScheduleReferenceMethods(previousInlineMethodData);
		}

		private void ScheduleReferenceMethods(InlineMethodData previous)
		{
			var current = MethodData.GetInlineMethodData();

			// If previous was not inlined and current is not inline, do nothing
			if (!current.IsInlined && !previous.IsInlined)
			{
				foreach (var method in previous.References)
				{
					MethodData.GetInlineMethodDataForUseBy(method);
				}

				return;
			}

			// If previous or current is inline, schedule all references from previous
			MethodScheduler.AddToRecompileQueue(previous.References);
		}

		private bool StaticCanNotInline(MethodData methodData)
		{
			if (InlineExplicitOnly && !methodData.HasAggressiveInliningAttribute)
				return true;

			if (methodData.HasDoNotInlineAttribute)
				return true;

			if (methodData.IsMethodImplementationReplaced)
				return true;

			if (methodData.HasProtectedRegions)
				return true;

			if (methodData.HasMethodPointerReferenced)
				return true;

			var method = methodData.Method;

			if (method.IsVirtual && !methodData.IsDevirtualized)
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
			if (!MosaTypeLayout.CanFitInRegister(returnType) && !returnType.IsUI8 && !returnType.IsR8)
				return true;

			// FUTURE: Don't hardcode namepsace
			if (((method.MethodAttributes & MosaMethodAttributes.Public) == MosaMethodAttributes.Public) && method.DeclaringType.BaseType != null && method.DeclaringType.BaseType.Namespace == "Mosa.UnitTests")
				return true;

			return false;
		}

		private bool CanInline(MethodData methodData)
		{
			if (StaticCanNotInline(methodData))
				return false;

			// current implementation limitation - can't include methods with AddressOf instruction
			if (methodData.HasAddressOfInstruction)
				return false;

			if (methodData.NonIRInstructionCount > 0)
				return false;

			if (MethodData.Version >= MaximumCompileCount)
				return false;   // too many compiles - cyclic loop suspected

			// methods with aggressive inline attribute will double the allow IR instruction count
			int max = methodData.HasAggressiveInliningAttribute || MethodData.AggressiveInlineRequested ? InlineAggressiveMaximum : InlineMaximum;

			if ((methodData.IRInstructionCount - methodData.IRStackParameterInstructionCount) > max)
				return false;

			return true;
		}

		protected BasicBlocks CopyInstructions()
		{
			var newBasicBlocks = new BasicBlocks();
			var mapBlocks = new Dictionary<BasicBlock, BasicBlock>(BasicBlocks.Count);
			var map = new Dictionary<Operand, Operand>();
			var staticCalls = new List<MosaMethod>();

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

					var moveInstruction = !MosaTypeLayout.CanFitInRegister(newOperand.Type)
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
					if (node.IsEmptyOrNop)
						continue;

					if (node.Instruction == IRInstruction.CallStatic)
					{
						staticCalls.AddIfNew(node.Operand1.Method);
					}

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

			var trace = CreateTraceLog("InlineMap", 9);

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
