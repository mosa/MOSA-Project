// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Analysis;
using System.Text;

namespace Mosa.Tool.Explorer.Stages
{
	public class DominanceOutputStage : BaseMethodCompilerStage
	{
		private const int TraceLevel = 10;

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

				for (int i = 0; i < BasicBlocks.Count; i++)
				{
					var block = BasicBlocks[i];

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

					trace.Log(sb.ToString());
				}

				trace.Log();
			}
		}

		private void OutputDiagram()
		{
			var trace = CreateTraceLog("Diagram");
			var sb = new StringBuilder();

			trace.Log("digraph blocks {");

			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				var dominance = new SimpleFastDominance(BasicBlocks, headBlock);

				for (int i = 0; i < BasicBlocks.Count; i++)
				{
					var block = BasicBlocks[i];

					var children = dominance.GetChildren(block);
					if (children != null && children.Count != 0)
					{
						foreach (var child in children)
						{
							trace.Log($"\t{block} -> {child}");
						}
					}
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

				for (int i = 0; i < BasicBlocks.Count; i++)
				{
					var block = BasicBlocks[i];

					sb.Clear();
					sb.Append($"  Block {block} : ");

					var dom = dominance.GetImmediateDominator(block);

					sb.Append((dom != null) ? dom.ToString() : string.Empty);

					trace.Log(sb.ToString());
				}

				trace.Log();
			}
		}
	}
}
