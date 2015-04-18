/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.Stages
{
	public class GraphVizStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			var trace = CreateTraceLog();

			char q = '"';

			trace.Log("digraph blocks {");

			foreach (var block in BasicBlocks)
			{
				//var sb = new StringBuilder();

				//for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				//{
				//	if (node.IsEmpty)
				//		continue;

				//	sb.Append(node.ToString());
				//	sb.Append(@"\n");
				//}
				//sb.Length = sb.Length - 1;
				//sb.Replace('"', ' ');
				//sb.Replace('[', ' ');
				//sb.Replace(']', ' ');

				//trace.Log("\t" + block.ToString() + " [label=" + q + sb.ToString() + q + "];");

				trace.Log("\t" + block.ToString());

				foreach (var next in block.NextBlocks)
				{
					trace.Log("\t" + block.ToString() + " -> " + next.ToString());
				}
			}

			trace.Log("}");
		}
	}
}
