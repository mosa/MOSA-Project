// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Exception Manager
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformManager" />
public class ExceptionManager : BaseTransformManager
{
	public readonly List<BasicBlock> LeaveTargets = new();

	public readonly Dictionary<BasicBlock, Operand> ExceptionVirtualRegisters = new();
	public readonly Dictionary<BasicBlock, Operand> LeaveTargetVirtualRegisters = new();

	public MosaMethod ExceptionHandler { get; private set; }

	public Operand ExceptionHandlerMethod { get; private set; }

	public override void Initialize(Compiler compiler)
	{
		ExceptionHandler = compiler.PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");
		ExceptionHandlerMethod = Operand.CreateLabel(ExceptionHandler, compiler.Architecture.Is32BitPlatform);
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

			while (node.IsEmptyOrNop || node.Instruction == IR.Flow)
			{
				node = node.Previous;
			}

			if (node.Instruction == IR.ExceptionEnd || node.Instruction == IR.TryEnd)
			{
				LeaveTargets.AddIfNew(node.BranchTargets[0]);
			}
		}
	}
}
