// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages.Diagnostic;

public class ControlFlowGraphStage : BaseMethodCompilerStage
{
	protected override void Run()
	{
		CreateDiagram();
	}

	protected void CreateDiagram()
	{
		var trace = CreateTraceLog("graphviz");

		trace.Log("digraph blocks {");

		foreach (var block in BasicBlocks)
		{
			foreach (var next in block.NextBlocks)
			{
				trace.Log($"\t{block} -> {next}");
			}
		}

		trace.Log("}");
	}
}
