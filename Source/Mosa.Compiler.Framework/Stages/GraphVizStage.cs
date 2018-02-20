// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	public class GraphVizStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			var trace = CreateTraceLog();

			const char q = '"';

			trace.Log("digraph blocks {");

			foreach (var block in BasicBlocks)
			{
				trace.Log("\t" + block);

				foreach (var next in block.NextBlocks)
				{
					trace.Log("\t" + block + " -> " + next);
				}
			}

			trace.Log("}");
		}
	}
}
