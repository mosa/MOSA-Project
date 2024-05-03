// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;
using Mosa.Compiler.Framework.Analysis;

<<<<<<<< HEAD:Source/Mosa.Tool.Explorer.Avalonia/Stages/DominanceOutputStage.cs
namespace Mosa.Tool.Explorer.Avalonia.Stages;
========
namespace Mosa.Compiler.Framework.Stages.Diagnostic;
>>>>>>>> 487-wip:Source/Mosa.Compiler.Framework/Stages/Diagnostic/DominanceAnalysisStage.cs

public class DominanceAnalysisStage : BaseMethodCompilerStage
{
	private const int TraceLevel = 5;

	protected override void Run()
	{
		if (!IsTraceable(TraceLevel))
			return;

		OutputList();
		OutputDiagram();
		OutputDominanceBlock();
	}

	private void OutputList()
	{
		var trace = CreateTraceLog("List");
		var sb = new StringBuilder();

		foreach (var headBlock in BasicBlocks.HeadBlocks)
		{
			trace.Log($"Head: {headBlock}");

			var dominance = new SimpleFastDominance(BasicBlocks, headBlock);
			foreach (var block in BasicBlocks)
			{
				sb.Clear();
				sb.Append($"  Block {block} : ");

				var children = dominance.GetChildren(block);
				if (children != null && children.Count != 0)
				{
					foreach (var child in children)
					{
						sb.Append(child);
						sb.Append(", ");
					}

					sb.Length -= 2;
				}

				trace.Log(sb);
			}

			trace.Log();
		}
	}

	private void OutputDiagram()
	{
<<<<<<<< HEAD:Source/Mosa.Tool.Explorer.Avalonia/Stages/DominanceOutputStage.cs
		var trace = CreateTraceLog("Diagram");
========
		var trace = CreateTraceLog("DominanceTree-graphviz");

>>>>>>>> 487-wip:Source/Mosa.Compiler.Framework/Stages/Diagnostic/DominanceAnalysisStage.cs
		trace.Log("digraph blocks {");

		foreach (var headBlock in BasicBlocks.HeadBlocks)
		{
			var dominance = new SimpleFastDominance(BasicBlocks, headBlock);

			foreach (var block in BasicBlocks)
			{
				var children = dominance.GetChildren(block);
				if (children == null || children.Count == 0)
					continue;

				foreach (var child in children)
					trace.Log($"\t{block} -> {child}");
			}
		}

		trace.Log("}");
	}

	private void OutputDominanceBlock()
	{
		var trace = CreateTraceLog("DominanceBlock");
		var sb = new StringBuilder();

		foreach (var headBlock in BasicBlocks.HeadBlocks)
		{
			trace.Log($"Head: {headBlock}");
			var dominance = new SimpleFastDominance(BasicBlocks, headBlock);

			for (var i = 0; i < BasicBlocks.Count; i++)
			{
				var block = BasicBlocks[i];

				sb.Clear();
				sb.Append($"  Block {block} : ");

				var dom = dominance.GetImmediateDominator(block);

				sb.Append((dom != null) ? dom.ToString() : string.Empty);

				trace.Log(sb);
			}

			trace.Log();
		}
	}
}
