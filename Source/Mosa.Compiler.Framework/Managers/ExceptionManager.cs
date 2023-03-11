// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Exception Manager
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformManager" />
public class ExceptionManager : BaseTransformManager
{
	public readonly List<BasicBlock> LeaveTargets = new List<BasicBlock>();

	public readonly Dictionary<BasicBlock, Operand> ExceptionVirtualRegisters = new Dictionary<BasicBlock, Operand>();
	public readonly Dictionary<BasicBlock, Operand> LeaveTargetVirtualRegisters = new Dictionary<BasicBlock, Operand>();

	public MosaMethod ExceptionHandler { get; private set; }

	public Operand ExceptionHandlerMethod { get; private set; }

	public override void Initialize(Compiler compiler)
	{
		ExceptionHandler = compiler.PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");
		ExceptionHandlerMethod = Operand.CreateSymbolFromMethod(ExceptionHandler, compiler.TypeSystem);
	}

	public override void Setup(MethodCompiler methodCompiler)
	{
		CollectLeaveTargets(methodCompiler.BasicBlocks);
	}

	public override void Finish()
	{
		LeaveTargets.Clear();
	}

	private void CollectLeaveTargets(BasicBlocks basicBlocks)
	{
		LeaveTargets.Clear();

		foreach (var block in basicBlocks)
		{
			var node = block.BeforeLast;

			while (node.IsEmptyOrNop || node.Instruction == IRInstruction.Flow)
			{
				node = node.Previous;
			}

			if (node.Instruction == IRInstruction.ExceptionEnd || node.Instruction == IRInstruction.TryEnd)
			{
				LeaveTargets.AddIfNew(node.BranchTargets[0]);
			}
		}
	}
}
