/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class IRCleanup : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			// FUTURE: Consolidate empty blocks, etc.

			// Remove Nops
			foreach (var block in BasicBlocks)
			{
				for (var ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (ctx.Instruction == IRInstruction.Nop)
					{
						ctx.Remove();
						continue;
					}
				}
			}
		}
	}
}