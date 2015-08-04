// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// The simple dead code removal stage remove useless instructions
	/// and NOP instructions prior to the register allocation stage.
	/// </summary>
	public sealed class SimpleDeadCodeRemovalStage : BaseTransformationStage
	{
		protected override void Run()
		{
			var trace = CreateTraceLog();

			bool changed = true;
			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
					{
						if (node.IsEmpty)
							continue;

						// Remove Nop instructions
						if (node.Instruction == X86.Nop)
						{
							node.Empty();
							continue;
						}

						// Remove useless instructions
						if (node.ResultCount == 1 && node.Result.Uses.Count == 0 && node.Result.IsVirtualRegister)
						{
							// Check is split child, if so check is parent in use (IR.Return for example)
							if (node.Result.IsSplitChild && node.Result.SplitParent.Uses.Count != 0)
								continue;

							if (trace.Active) trace.Log("REMOVED:\t" + node.ToString());

							node.Empty();
							changed = true;
						}
					}
				}
			}
		}
	}
}
