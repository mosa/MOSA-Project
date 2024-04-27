// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;
using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

/// <summary>
/// Logs all instructions.
/// </summary>
public static class InstructionTrace
{
	public static TraceLog Run(string stage, MosaMethod method, BasicBlocks basicBlocks, int version, string section, int step)
	{
		var traceLog = new TraceLog(TraceType.MethodInstructions, method, stage, section, version, step);

		traceLog?.Log($"M\t{method.FullName}\t{version}\t{stage}\t{step}");

		var sb = new StringBuilder();

		foreach (var block in basicBlocks)
		{
			traceLog?.Log($"S\t{block}\t{block.Sequence}\t{(block.IsHeadBlock ? "Header" : string.Empty)}\t{block.PreviousBlocks.Count}\t{GetTabBlocks(block.PreviousBlocks)}");

			for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				sb.Clear();

				sb.Append($"I\t{node.Label:X5}\t{(node.IsMarked ? "*" : string.Empty)}\t{node.Instruction}\t");
				sb.Append($"{(node.ConditionCode != ConditionCode.Undefined ? node.ConditionCode.GetConditionString() : string.Empty)}\t");

				sb.Append($"{node.ResultCount}\t");
				sb.Append($"{node.OperandCount}\t");
				sb.Append($"{node.BranchTargetsCount}\t");
				sb.Append($"{node.PhiBlockCount}\t");

				foreach (var operand in node.Results)
				{
					sb.Append($"{operand}\t");
				}

				foreach (var operand in node.Operands)
				{
					sb.Append($"{operand}\t");
				}

				sb.Append($"{GetTabBlocks(node.BranchTargets)}");
				sb.Append($"{GetTabBlocks(node.PhiBlocks)}");

				sb.Length--;

				traceLog?.Log(sb.ToString());
			}

			traceLog?.Log($"E\t{block}\t{block.Sequence}\t{block.NextBlocks.Count}\t{GetTabBlocks(block.NextBlocks)}");
		}

		return traceLog;
	}

	private static string GetTabBlocks(IList<BasicBlock> blocks)
	{
		if (blocks == null || blocks.Count == 0)
			return string.Empty;

		var sb = new StringBuilder();

		foreach (var next in blocks)
		{
			sb.AppendFormat($"{next}\t");
		}

		return sb.ToString();
	}
}
