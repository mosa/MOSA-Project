﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Base class for code generation stages.
	/// </summary>
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

			trace.Log("Label\tAddress\tLength\tStartLine\tEndLine\tStartColumn\tEndColumn\tInstruction\tDocument");

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction.IgnoreDuringCodeGeneration)
						continue;

					int Label = node.Label;
					int StartLine = -1;
					int EndLine = 0;
					int EndColumn = 0;
					int StartColumn = 0;
					string Document = string.Empty;

					//int Offset = node.Offset;

					int Address = -1;
					int Length = 0;

					foreach (var instruction in Method.Code)
					{
						if (instruction.Offset != Label)
							continue;

						StartLine = instruction.StartLine;
						EndLine = instruction.EndLine;
						StartColumn = instruction.StartColumn;
						EndColumn = instruction.EndColumn;
						Document = instruction.Document;

						break;
					}

					if (StartLine == -1)
						continue;

					foreach (var region in MethodData.LabelRegions)
					{
						if (region.Label != Label)
							continue;

						Address = region.Start;
						Length = region.Length;
						break;
					}

					if (Address == -1)
						continue;

					trace.Log(String.Format("{0:X5}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
						Label, Address, Length, StartLine, EndLine, StartColumn, EndColumn, node.ToString(), Document));
				}
			}
		}

		protected void DumpRegions()
		{
			var trace = CreateTraceLog("Regions");

			trace.Log("Label\tAddress\tLength");

			foreach (var region in MethodData.LabelRegions)
			{
				trace.Log(String.Format("{0:X5}\t{1}\t{2}",
					region.Label, region.Start, region.Length));
			}
		}

		protected void DumpSourceInfo()
		{
			var trace = CreateTraceLog("Source");

			trace.Log("Label\tStartLine\tEndLine\tStartColumn\tEndColumn\tDocument");

			foreach (var instruction in Method.Code)
			{
				trace.Log(String.Format("{0:X5}\t{1}\t{2}\t{3}\t{4}\t{5}",
					instruction.Offset, instruction.StartLine, instruction.EndLine, instruction.StartColumn, instruction.EndColumn, instruction.Document));
			}
		}
	}
}
