// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages.Diagnostic;

<<<<<<<< HEAD:Source/Mosa.Compiler.Framework/Stages/Diagnostic/GraphVizStage.cs
public class GraphVizStage : BaseMethodCompilerStage
========
public class ControlFlowGraphStage : BaseMethodCompilerStage
>>>>>>>> 487-wip:Source/Mosa.Compiler.Framework/Stages/Diagnostic/ControlFlowGraphStage.cs
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
