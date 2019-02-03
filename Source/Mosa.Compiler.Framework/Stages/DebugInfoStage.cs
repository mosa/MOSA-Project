// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			var trace = CreateTraceLog();

			trace.Log("Label\tAddressStart\tAddressLength\tStartLinevEndLine\tStartColumn\tStartColumn\tDocument");

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

					int AddressStart = -1;
					int AddressLength = 0;

					foreach (var instruction in Method.Code)
					{
						if (instruction.Offset != Label)
							continue;

						StartLine = instruction.StartLine;
						EndLine = instruction.EndLine;
						EndColumn = instruction.EndColumn;
						StartColumn = instruction.StartColumn;
						Document = instruction.Document;

						break;
					}

					if (StartLine == -1)
						continue;

					foreach (var region in MethodData.LabelRegions)
					{
						if (region.Label != Label)
							continue;

						AddressStart = region.Start;
						AddressLength = region.Length;
						break;
					}

					if (AddressStart == -1)
						continue;

					trace.Log(
							String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
							Label, AddressStart, AddressLength, StartLine, EndLine, StartColumn, StartColumn, Document));
				}
			}
		}
	}
}
