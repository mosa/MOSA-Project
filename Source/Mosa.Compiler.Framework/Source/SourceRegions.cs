// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.Compiler.Framework.Source
{
	public static class SourceRegions
	{
		/// <summary>
		/// Returns mapping of method address to source code location.
		/// </summary>
		public static List<SourceRegion> GetSourceRegions(MethodData methodData)
		{
			if (methodData.IsMethodImplementationReplaced && methodData.Symbol.IsExternalSymbol)
			{
				var regions = new List<SourceRegion>(1);

				var headerRegion = new SourceRegion
				{
					Address = (int)methodData.Symbol.VirtualAddress,
					Length = (int)methodData.Symbol.Size,
					StartLine = 0,
					EndLine = 0,
					StartColumn = 0,
					EndColumn = 0,
					Filename = string.Empty
				};

				regions.Add(headerRegion);

				return regions;
			}

			if (methodData.Method.HasImplementation
				&& methodData.HasCode
				&& !methodData.IsMethodImplementationReplaced
				&& !methodData.IsLinkerGenerated)
			{
				// Add method header
				var regions = new List<SourceRegion>(methodData.LabelRegions.Count + 1);

				var firstInstruction = methodData.Method.Code[0];

				var headerRegion = new SourceRegion
				{
					Address = 0,
					Length = methodData.LabelRegions.Count > 0 ? methodData.LabelRegions[0].Start : 0,
					StartLine = firstInstruction.StartLine,
					EndLine = firstInstruction.EndLine,
					StartColumn = firstInstruction.StartColumn,
					EndColumn = firstInstruction.EndColumn,
					Filename = firstInstruction.Document
				};

				if (headerRegion.IsValid)
				{
					regions.Add(headerRegion);
				}

				var startLine = 0;
				var endLine = 0;
				var startColumn = 0;
				var endColumn = 0;
				var filename = string.Empty;

				foreach (var labelRegion in methodData.LabelRegions)
				{
					foreach (var instruction in methodData.Method.Code)
					{
						// special case: the return label is always 0xFFFFF
						var searchForLabel = labelRegion.Label;

						if (labelRegion.Label == 0xFFFFF)
						{
							searchForLabel = methodData.Method.Code.Last().Offset;
						}

						if (instruction.StartLine > 0)
						{
							startLine = instruction.StartLine;
							endLine = instruction.EndLine;
							startColumn = instruction.StartColumn;
							endColumn = instruction.EndColumn;
							filename = instruction.Document;
						}

						if (instruction.Offset != searchForLabel)
							continue;

						var region = new SourceRegion()
						{
							Address = labelRegion.Start,
							Length = labelRegion.Length,
							StartLine = startLine,
							EndLine = endLine,
							StartColumn = startColumn,
							EndColumn = endColumn,
							Filename = filename
						};

						if (region.IsValid)
						{
							regions.Add(region);
						}
					}
				}

				return regions;
			}

			return null;
		}
	}
}
