// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework;
using Mosa.Compiler;

namespace Mosa.Compiler.Extensions.Dwarf
{

	public class SourceCodeLocation
	{
		public int Address;
		public int StartLine;
		public int StartColumn;
		public string Document;
	}

	public static class MethodDataExtensions
	{

		public static IEnumerable<SourceCodeLocation> DumpByInstructions(this MethodData methodData)
		{

			int StartLine = -1;
			int EndLine = 0;
			int EndColumn = 0;
			int StartColumn = 0;
			string Document = string.Empty;

			foreach (var region in methodData.LabelRegions)
			{
				int Address = -1;
				int Length = 0;

				foreach (var instruction in methodData.Method.Code)
				{
					if (instruction.Offset != region.Label)
						continue;

					Address = region.Start;
					Length = region.Length;

					StartLine = instruction.StartLine;
					EndLine = instruction.EndLine;
					StartColumn = instruction.StartColumn;
					EndColumn = instruction.EndColumn;
					Document = instruction.Document;

					break;
				}

				if (StartLine == -1)
					continue;

				if (Address == -1)
					continue;

				// trace.Log(String.Format("{0:X5}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
				// 	Label, Address, Length, StartLine, EndLine, StartColumn, EndColumn, node.ToString(), Document));

				yield return new SourceCodeLocation
				{
					Address = Address,
					StartLine = StartLine,
					StartColumn = StartColumn,
					Document = Document
				};
			}
		}

	}
}
