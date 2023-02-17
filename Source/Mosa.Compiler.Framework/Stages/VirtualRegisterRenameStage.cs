// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Virtual Register Rename Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
public sealed class VirtualRegisterRenameStage : BaseCodeTransformationStage
{
	protected override void PopulateVisitationDictionary()
	{
		// Nothing to do
	}

	protected override void Run()
	{
		var vr = new List<Operand>();

		foreach (var block in BasicBlocks)
		{
			//for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				foreach (var op in node.Results)
				{
					if (op.IsVirtualRegister)
					{
						vr.AddIfNew(op);
					}
				}

				foreach (var op in node.Operands)
				{
					if (op.IsVirtualRegister)
					{
						vr.AddIfNew(op);
					}
				}


			}
		}

		foreach (var v in MethodCompiler.VirtualRegisters)
		{
			vr.AddIfNew(v);
		}

		var index = 0;

		foreach (var v in vr)
		{
			MethodCompiler.VirtualRegisters.ReOrdered(v, ++index);
		}
	}
}
