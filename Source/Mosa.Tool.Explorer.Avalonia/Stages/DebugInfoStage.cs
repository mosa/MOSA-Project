// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Tool.Explorer.Avalonia.Stages;

public class DebugInfoStage : BaseMethodCompilerStage
{
	protected override void Run()
	{
		DumpByInstructions();
		DumpRegions();
		DumpSourceInfo();
	}

	protected void DumpByInstructions()
	{
		var trace = CreateTraceLog("Instructions");
		if (trace == null)
			return;

		trace.Log("Label\tAddress\tLength\tStartLine\tEndLine\tStartColumn\tEndColumn\tInstruction\tDocument");

		foreach (var block in BasicBlocks)
		{
			for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				if (node.Instruction.IgnoreDuringCodeGeneration)
					continue;

				var label = node.Label;
				var startLine = -1;
				var endLine = 0;
				var endColumn = 0;
				var startColumn = 0;
				var document = string.Empty;

				//var offset = node.Offset;

				var address = -1;
				var length = 0;

				foreach (var instruction in Method.Code)
				{
					if (instruction.Offset != label)
						continue;

					startLine = instruction.StartLine;
					endLine = instruction.EndLine;
					startColumn = instruction.StartColumn;
					endColumn = instruction.EndColumn;
					document = instruction.Document;

					break;
				}

				if (startLine == -1)
					continue;

				foreach (var region in MethodData.LabelRegions)
				{
					if (region.Label != label)
						continue;

					address = region.Start;
					length = region.Length;
					break;
				}

				if (address == -1)
					continue;

				trace.Log($"{label:X5}\t{address}\t{length}\t{startLine}\t{endLine}\t{startColumn}\t{endColumn}\t{node}\t{document}");
			}
		}
	}

	protected void DumpRegions()
	{
		var trace = CreateTraceLog("Regions");

		trace.Log("Label\tAddress\tLength");

		foreach (var region in MethodData.LabelRegions)
		{
			trace.Log($"{region.Label:X5}\t{region.Start}\t{region.Length}");
		}
	}

	protected void DumpSourceInfo()
	{
		var trace = CreateTraceLog("Source");

		trace.Log("Label\tStartLine\tEndLine\tStartColumn\tEndColumn\tDocument");

		foreach (var instruction in Method.Code)
		{
			trace.Log($"{instruction.Offset:X5}\t{instruction.StartLine}\t{instruction.EndLine}\t{instruction.StartColumn}\t{instruction.EndColumn}\t{instruction.Document}");
		}
	}
}
