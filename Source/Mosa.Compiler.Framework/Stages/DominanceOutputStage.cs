// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	public class DominanceOutputStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (!IsTraceable())
				return;

			OutputList();
			OutputDiagram();
		}

		private void OutputList()
		{
			var trace = CreateTraceLog("List");
			var sb = new StringBuilder();

			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				trace.Log("Head: " + headBlock);
				var dominance = new SimpleFastDominance(BasicBlocks, headBlock);

				for (int i = 0; i < BasicBlocks.Count; i++)
				{
					var block = BasicBlocks[i];

					sb.Clear();
					sb.Append("  Block ");
					sb.Append(block);
					sb.Append(" : ");

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
							trace.Log("\t" + block + " -> " + child);
						}
					}
				}
			}

			trace.Log("}");
		}

		//private void OutputDominanceBlock()
		//{
		//	var trace = CreateTraceLog("DominanceBlock");
		//	var sb = new StringBuilder();

		//	foreach (var headBlock in BasicBlocks.HeadBlocks)
		//	{
		//		trace.Log("Head: " + headBlock);
		//		var dominance = new SimpleFastDominance(BasicBlocks, headBlock);

		//		for (int i = 0; i < BasicBlocks.Count; i++)
		//		{
		//			var block = BasicBlocks[i];

		//			sb.Clear();
		//			sb.Append("  Block ");
		//			sb.Append(block);
		//			sb.Append(" : ");

		//			var children = dominance.GetDominators(block);

		//			if (children != null && children.Count != 0)
		//			{
		//				foreach (var child in children)
		//				{
		//					sb.Append(child);
		//					sb.Append(", ");
		//				}

		//				sb.Length -= 2;
		//			}

		//			trace.Log(sb.ToString());
		//		}

		//		trace.Log();
		//	}
		//}
	}
}
