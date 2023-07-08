// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
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
		InlineMethods = MosaSettings.InlineMethods;
		InlineExplicit = MosaSettings.InlineExplicit;
		InlineAggressiveMaximum = MosaSettings.InlineAggressiveMaximum;
		InlineMaximum = MosaSettings.InlineMaximum;
	}

	protected override void Run()
	{
		var trace = CreateTraceLog();

		MethodData.HasAddressOfInstruction = false;
		MethodData.HasLoops = false;
		MethodData.IsSelfReferenced = false;
		MethodData.HasEpilogue = false;
		MethodData.HasReturnValue = false;
		MethodData.IsUnitTest = IsUnitTest(Method);

		trace?.Log($"DoNotInline: {MethodData.DoNotInline}");
		trace?.Log($"IsVirtual: {Method.IsVirtual}");
		trace?.Log($"IsDevirtualized: {MethodData.IsDevirtualized}");
		trace?.Log($"HasProtectedRegions: {MethodData.HasProtectedRegions}");
		trace?.Log($"HasDoNotInlineAttribute: {MethodData.HasDoNotInlineAttribute}");
		trace?.Log($"HasAggressiveInliningAttribute: {MethodData.HasAggressiveInliningAttribute}");
		trace?.Log($"AggressiveInlineRequested: {MethodData.AggressiveInlineRequested}");
		trace?.Log($"IsMethodImplementationReplaced (Plugged): {MethodData.IsMethodImplementationReplaced}");
		trace?.Log($"IsReferenced: {MethodData.IsReferenced}");
		trace?.Log($"IsUnitTest: {MethodData.IsUnitTest}");
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
					if (node.IsEmptyOrNop)
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
					else if (node.Instruction == IRInstruction.CallStatic && node.Operand1.Method == Method)
					{
						MethodData.IsSelfReferenced = true;
					}
					else if (node.Instruction == IRInstruction.Epilogue)
					{
						MethodData.HasEpilogue = true;
					}
					else if (node.Instruction.IsReturn)
					{
						MethodData.HasReturnValue = true;
						totalStackParameterInstruction++;
					}
					else if (node.Instruction.IsParameterLoad)
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

		SetInlinedBasicBlocks(inline ? BasicBlocks : null);

		trace?.Log($"IRInstructionCount: {MethodData.IRInstructionCount}");
		trace?.Log($"IRStackParameterInstructionCount: {MethodData.IRStackParameterInstructionCount}");
		trace?.Log($"InlineAggressiveMaximum: {InlineAggressiveMaximum}");
		trace?.Log($"InlineMaximum: {InlineMaximum}");
		trace?.Log($"InlineExplicitOnly: {InlineExplicit}");
		trace?.Log($"NonIRInstructionCount: {MethodData.NonIRInstructionCount}");
		trace?.Log($"HasAddressOfInstruction: {MethodData.HasAddressOfInstruction}");
		trace?.Log($"HasLoops: {MethodData.HasLoops}");
		trace?.Log($"HasEpilogue: {MethodData.HasEpilogue}");
		trace?.Log($"HasReturnValue: {MethodData.HasReturnValue}");
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

		if (methodData.IsUnitTest)
			return false;

		return false;
	}

	public static bool IsUnitTest(MosaMethod method)
	{
		return (method.MethodAttributes & MosaMethodAttributes.Public) == MosaMethodAttributes.Public
			&& method.DeclaringType.BaseType != null
			&& method.DeclaringType.BaseType.Namespace == "Mosa.UnitTests";
	}

	private bool CanInline(MethodData methodData)
	{
		if (StaticCanNotInline(methodData))
			return false;

		// current implementation limitation - can't include methods with AddressOf instruction
		if (methodData.HasAddressOfInstruction)
			return false;

		if (!MethodData.HasEpilogue && !methodData.Method.Signature.ReturnType.IsVoid)
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
}
