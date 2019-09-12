// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage simplified virtual register usages by dead code elimation and constant and value propagation
	/// </summary>
	public class FastSimplification : BaseMethodCompilerStage
	{
		private Counter InstructionsRemovedCount = new Counter("FastSimplification.IRInstructionRemoved");
		private Counter PropagateConstantCount = new Counter("FastSimplification.PropagateConstant");
		private Counter PropagateMoveCount = new Counter("FastSimplification.PropagateMoveCount");
		private Counter DeadCodeEliminationCount = new Counter("FastSimplification.DeadCodeElimination");

		private TraceLog trace;

		protected override void Initialize()
		{
			Register(InstructionsRemovedCount);
			Register(PropagateConstantCount);
			Register(PropagateMoveCount);
			Register(DeadCodeEliminationCount);
		}

		protected override void Finish()
		{
			trace = null;
		}

		protected override void Run()
		{
			if (HasProtectedRegions)
				return;

			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			if (BasicBlocks.PrologueBlock == null)
				return;

			trace = CreateTraceLog(5);

			var change = true;
			while (change)
			{
				change = false;

				foreach (var virtualRegister in MethodCompiler.VirtualRegisters)
				{
					if (!ValidateSSAForm(virtualRegister))
						continue;

					var node = virtualRegister.Definitions[0];

					if ((node.Instruction == IRInstruction.Move32
						|| node.Instruction == IRInstruction.Move64)
						&& node.Operand1.IsResolvedConstant)
					{
						// Propagate Constant

						var source = node.Operand1;

						// for each statement T that uses operand, substituted c in statement T
						foreach (var useNode in virtualRegister.Uses.ToArray())
						{
							for (int i = 0; i < useNode.OperandCount; i++)
							{
								var operand = useNode.GetOperand(i);

								if (operand == virtualRegister)
								{
									trace?.Log("*** PropagateConstant");
									trace?.Log($"BEFORE:\t{useNode}");

									useNode.SetOperand(i, source);
									trace?.Log($"AFTER: \t{useNode}");
								}
							}
						}

						change = true;
						PropagateConstantCount.Increment();
					}
					else if ((node.Instruction == IRInstruction.Move32
						|| node.Instruction == IRInstruction.Move64
						|| node.Instruction == IRInstruction.MoveR4
						|| node.Instruction == IRInstruction.MoveR8)
						&& node.Operand1.IsVirtualRegister
						&& ValidateSSAForm(node.Operand1))
					{
						//  Propagate Move Value

						var source = node.Operand1;

						foreach (var useNode in virtualRegister.Uses.ToArray())
						{
							for (int i = 0; i < useNode.OperandCount; i++)
							{
								var operand = useNode.GetOperand(i);

								if (virtualRegister == operand)
								{
									trace?.Log("*** PropagateMove");
									trace?.Log($"BEFORE:\t{useNode}");
									useNode.SetOperand(i, source);
									trace?.Log($"AFTER: \t{useNode}");
								}
							}
						}

						change = true;
					}

					if (BuiltInOptimizations.IsDeadCode(node))
					{
						// Remove Dead Code

						trace?.Log("*** DeadCode");
						trace?.Log($"REMOVED:\t{node}");

						node.SetInstruction(IRInstruction.Nop);
						DeadCodeEliminationCount.Increment();
						InstructionsRemovedCount.Increment();
						continue;
					}
				}
			}
		}
	}
}
