// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages.Diagnostic;

public class GraphVizStage : BaseMethodCompilerStage
{
	private const int TraceLevel = 6;

	protected override void Run()
	{
		if (!IsTraceable(TraceLevel))
			return;

		var trace = CreateTraceLog();

		trace.Log("digraph blocks {");

		foreach (var block in BasicBlocks)
		{
			//trace.Log("\t" + block);

			foreach (var next in block.NextBlocks)
			{
				trace.Log($"\t{block} -> {next}");
			}
		}

		trace.Log("}");
	}
}
