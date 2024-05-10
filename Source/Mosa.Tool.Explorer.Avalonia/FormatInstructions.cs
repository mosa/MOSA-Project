// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Tool.Explorer.Avalonia;

public static class FormatInstructions
{
	private const int Padding = 34;

	public static string Format(List<InstructionRecord> records, string blockLabel, bool strip, bool removeNop, bool lineBetweenBlocks)
	{
		var sb = new StringBuilder();
		var blocks = new StringBuilder();

		if (records == null || records.Count == 0)
			return string.Empty;

		var allLines = string.IsNullOrWhiteSpace(blockLabel);
		var inBlock = allLines;

		foreach (var record in records)
		{
			switch (record.Type)
			{
				case "M":
				{
					sb.AppendLine($"{record.MethodName} [v{record.Version}] @ {record.Stage}:");
					sb.AppendLine();
					break;
				}
				case "S":
				{
					inBlock = record.BlockLabel == blockLabel;

					if (!inBlock && !allLines)
						continue;

					sb.Append($"{record.BlockLabel}:");

					blocks.Clear();

					// Previous Branch Targets
					if (record.PreviousBlockCount != 0)
					{
						for (var i = 0; i < record.PreviousBlockCount; i++)
						{
							if (i != 0)
								blocks.Append(' ');

							var op = record.GetPreviousBlocks(i);

							blocks.Append(op);
						}

						sb.Append("".PadRight(record.PreviousBlockCount == 1 ? Padding : Padding - 8));

						sb.Append(blocks);
					}

					sb.AppendLine();
					break;
				}
				case "I":
				{
					if (!inBlock && !allLines)
						continue;

					if (record.Instruction is "IR.BlockStart" or "IR.BlockEnd")
						break;

					if (removeNop && record.Instruction == "IR.Nop")
						continue;

					sb.Append("      ");

					var instruction = record.Instruction;
					var condition = !string.IsNullOrEmpty(record.Condition) ? " [" + record.Condition + "]" : string.Empty;

					var both = $"{instruction}{condition}";

					blocks.Clear();

					// Branch Targets
					if (record.BranchTargetCount != 0)
					{
						if (condition.Length != 0)
							blocks.Append(' ');

						for (var i = 0; i < record.BranchTargetCount; i++)
						{
							if (i != 0)
								blocks.Append(' ');

							var op = record.GetBranchTarget(i);

							blocks.Append(op);
						}
					}

					var count = Padding + 2 - both.Length - blocks.Length;
					if (count < 0)
						count = 1;

					var padding = string.Empty.PadRight(count);

					sb.Append($"{record.Label[..5]}:{record.Mark.PadLeft(1)}{both}{padding}{blocks} ");

					// Result
					for (var i = 0; i < record.ResultCount; i++)
					{
						if (i != 0)
							sb.Append(", ");

						var op = record.GetResult(i);

						op = Simplify(op);

						if (strip)
							op = StripBracketData(op);

						sb.Append(op);
					}

					if (record.ResultCount != 0 && record.OperandCount != 0)
						sb.Append(" <= ");

					// Operands
					for (var i = 0; i < record.OperandCount; i++)
					{
						if (i != 0)
							sb.Append(", ");

						var op = record.GetOperand(i);

						op = Simplify(op);

						if (strip)
							op = StripBracketData(op);

						sb.Append(op);
					}

					// Phi Blocks
					if (record.PhiBlockCount != 0)
					{
						sb.Append(" (");

						for (var i = 0; i < record.PhiBlockCount; i++)
						{
							if (i != 0)
								sb.Append(", ");

							var op = record.GetPhilBlock(i);

							sb.Append(op);
						}

						sb.Append(") ");
					}

					while (sb.Length > 0 && sb[^1] == ' ')
						sb.Length--;

					sb.AppendLine();
					break;
				}
				case "E":
				{
					inBlock = false;

					if (!inBlock && !allLines)
						continue;

					if (lineBetweenBlocks)
						sb.AppendLine();

					break;
				}
			}
		}

		return sb.ToString();
	}

	private static string StripBracketData(string s)
	{
		var i = s.IndexOf('[');
		return i <= 0 ? s : s[..(i - 1)].Trim();
	}

	private static string Simplify(string s) => s.Replace("const=", string.Empty);
}
