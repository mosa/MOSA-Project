// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	public class GraphVizStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (!IsTraceable())
				return;

			var trace = CreateTraceLog();

			trace.Log("digraph blocks {");

			foreach (var block in BasicBlocks)
			{
				//trace.Log("\t" + block);

				foreach (var next in block.NextBlocks)
				{
					trace.Log("\t" + block + " -> " + next);
				}
			}

			trace.Log("}");
		}
	}
}
