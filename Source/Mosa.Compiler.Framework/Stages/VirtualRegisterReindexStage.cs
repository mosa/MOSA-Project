// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Virtual Register Reindex Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
public sealed class VirtualRegisterReindexStage : BaseMethodCompilerStage
{
	protected override void Run()
	{
		MethodCompiler.VirtualRegisters.RemoveUnused();
		OrderVirtualRegisters();
		OrderLocalStack();
	}

	private void OrderLocalStack()
	{
		var sl = new List<Operand>();

		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				foreach (var op in node.Operands)
				{
					if (op.IsLocalStack)
					{
						sl.AddIfNew(op);
					}
				}
			}
		}

		foreach (var s in MethodCompiler.LocalStack)
		{
			sl.AddIfNew(s.HasParent ? s.Parent : s);
		}

		var index = 0;

		foreach (var s in sl)
		{
			if (!s.HasParent)
			{
				MethodCompiler.LocalStack.Reorder(s, ++index);
				s.Reindex(index);

				if (s.Low != null)
				{
					s.Low.Reindex(index);
					s.High.Reindex(index);
				}
			}
		}
	}

	private void OrderVirtualRegisters()
	{
		var at = 0;
		var max = MethodCompiler.VirtualRegisters.Count - 1;

		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				foreach (var op in node.Results)
				{
					if (op.IsVirtualRegister && op.Index - 1 >= at)
					{
						MethodCompiler.VirtualRegisters.SwapPosition(MethodCompiler.VirtualRegisters[at++], op);
					}
				}

				foreach (var op in node.Operands)
				{
					if (op.IsVirtualRegister && op.Index - 1 >= at)
					{
						MethodCompiler.VirtualRegisters.SwapPosition(MethodCompiler.VirtualRegisters[at++], op);
					}
				}

				if (at == max)
					return;
			}
		}
	}
}
