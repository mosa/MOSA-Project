// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Inline Evaluation Stage
/// </summary>
public class InlineEvaluationStage : BaseMethodCompilerStage
{
	public static string InlineMethodAttribute = "System.Runtime.CompilerServices.MethodImplAttribute";

	private readonly Counter InlineCount = new Counter("InlineMethodEvaluationStage.Inline");
	private readonly Counter ReversedInlineCount = new Counter("InlineMethodEvaluationStage.ReversedInline");

	public const int MaximumCompileCount = 10;

	private bool InlineExplicit;
	private bool InlineMethods;
	private int InlineAggressiveMaximum;
	private int InlineMaximum;

	protected override void Initialize()
	{
		Register(InlineCount);
		Register(ReversedInlineCount);

		// cache for performance
		InlineMethods = CompilerSettings.InlineMethods;
		InlineExplicit = CompilerSettings.InlineExplicit;
		InlineAggressiveMaximum = CompilerSettings.InlineAggressiveMaximum;
		InlineMaximum = CompilerSettings.InlineMaximum;
	}

	protected override void Run()
	{
		var trace = CreateTraceLog();

		MethodData.HasAddressOfInstruction = false;
		MethodData.HasLoops = false;
		MethodData.IsSelfReferenced = false;

		//MethodData.IsDevirtualized = Method.IsVirtual && !TypeLayout.IsMethodOverridden(Method);

		trace?.Log($"DoNotInline: {MethodData.DoNotInline}");
		trace?.Log($"IsVirtual: {Method.IsVirtual}");
		trace?.Log($"IsDevirtualized: {MethodData.IsDevirtualized}");
		trace?.Log($"HasProtectedRegions: {MethodData.HasProtectedRegions}");
		trace?.Log($"HasDoNotInlineAttribute: {MethodData.HasDoNotInlineAttribute}");
		trace?.Log($"HasAggressiveInliningAttribute: {MethodData.HasAggressiveInliningAttribute}");
		trace?.Log($"AggressiveInlineRequested: {MethodData.AggressiveInlineRequested}");
		trace?.Log($"IsMethodImplementationReplaced (Plugged): {MethodData.IsMethodImplementationReplaced}");
		trace?.Log($"IsReferenced: {MethodData.IsReferenced}");
		trace?.Log($"CompileCount: {MethodData.Version}");

		if (StaticCanNotInline(MethodData))
		{
			SetInlinedBasicBlocks(null);

			trace?.Log($"** Statically Evaluated");
			trace?.Log($"Inlined: {MethodData.Inlined}");

			//Debug.WriteLine($">Inlined: No"); //DEBUGREMOVE

			return;
		}

		var totalIRCount = 0;
		var totalNonIRCount = 0;
		var totalStackParameterInstruction = 0;

		if (!Method.IsNoInlining)
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction.IsIRInstruction)
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

					if (node.Instruction == IRInstruction.CallStatic && node.Operand1.Method == Method)
					{
						MethodData.IsSelfReferenced = true;
					}

					if (node.Instruction == IRInstruction.SetReturn32

						|| node.Instruction == IRInstruction.SetReturn64
						|| node.Instruction == IRInstruction.SetReturnR4
						|| node.Instruction == IRInstruction.SetReturnR8
						|| node.Instruction == IRInstruction.SetReturnObject
						|| node.Block.IsEpilogue
						|| node.Block.IsPrologue
						|| node.Instruction.IsParameterLoad
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

		var inline = CanInline(MethodData);

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
		trace?.Log($"InlineAggressiveMaximum: {InlineAggressiveMaximum}");
		trace?.Log($"InlineMaximum: {InlineMaximum}");
		trace?.Log($"InlineExplicitOnly: {InlineExplicit}");
		trace?.Log($"NonIRInstructionCount: {MethodData.NonIRInstructionCount}");
		trace?.Log($"HasAddressOfInstruction: {MethodData.HasAddressOfInstruction}");
		trace?.Log($"HasLoops: {MethodData.HasLoops}");
		trace?.Log($"IsSelfReferenced: {MethodData.IsSelfReferenced}");
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
		if (!InlineMethods && !methodData.HasAggressiveInliningAttribute)
			return true;

		if (methodData.HasDoNotInlineAttribute)
			return true;

		if (methodData.IsMethodImplementationReplaced)
			return true;

		if (methodData.HasProtectedRegions)
			return true;

		if (methodData.IsReferenced)
			return true;

		if (methodData.IsSelfReferenced)
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
		if (!(returnType.IsVoid
			|| returnType.IsUI8
			|| returnType.IsR8
			|| MosaTypeLayout.IsUnderlyingPrimitive(returnType)
			|| TypeLayout.GetTypeLayoutSize(returnType) <= 8))
			return true;

		// FUTURE: Don't hardcode namepsace
		if ((method.MethodAttributes & MosaMethodAttributes.Public) == MosaMethodAttributes.Public && method.DeclaringType.BaseType != null && method.DeclaringType.BaseType.Namespace == "Mosa.UnitTests")
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
		var max = methodData.HasAggressiveInliningAttribute || MethodData.AggressiveInlineRequested ? InlineAggressiveMaximum : InlineMaximum;

		if (methodData.IRInstructionCount - methodData.IRStackParameterInstructionCount > max)
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

		var newPrologueBlock = newBasicBlocks.PrologueBlock;

		foreach (var operand in MethodCompiler.Parameters)
		{
			if (operand.Definitions.Count > 0)
			{
				var newOp = Map(operand, map);

				var newOperand = Operand.CreateVirtualRegister(operand, -operand.Index);

				var moveInstruction = operand.IsPrimitive
					? MethodCompiler.GetMoveInstruction(newOperand.Primitive)
					: IRInstruction.MoveCompound;

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
					ConditionCode = node.ConditionCode,
					InvokeMethod = node.InvokeMethod,
					MosaType = node.MosaType,

					//Label = callSiteNode.Label,
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
					var op = node.GetResult(i);
					var newOp = Map(op, map);

					newNode.SetResult(i, newOp);
				}

				// copy operands
				for (var i = 0; i < node.OperandCount; i++)
				{
					var op = node.GetOperand(i);
					var newOp = Map(op, map);

					newNode.SetOperand(i, newOp);
				}

				// copy other
				if (node.MosaType != null)
					newNode.MosaType = node.MosaType;

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

	private static Operand Map(Operand operand, Dictionary<Operand, Operand> map)
	{
		if (operand == null)
			return null;

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
			mappedOperand = operand;
		}
		else if (operand.IsLocalStack)
		{
			mappedOperand = Operand.CreateStackLocal(operand.Primitive, operand.Index, operand.IsPinned, operand.Type);
		}
		else if (operand.IsVirtualRegister)
		{
			if (operand.Uses.Count != 0 || operand.Definitions.Count != 0)
			{
				mappedOperand = Operand.CreateVirtualRegister(operand, operand.Index);
			}
		}
		else if (operand.IsStaticField)
		{
			mappedOperand = operand;
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
