// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Source
{
	public static class SourceRegions
	{
		public static List<SourceRegion> GetSourceRegions(MosaMethod method, MethodData data)
		{
			var regions = new List<SourceRegion>(data.LabelRegions.Count);

			foreach (var labelRegion in data.LabelRegions)
			{
				foreach (var instruction in method.Code)
				{
					if (instruction.Offset != labelRegion.Label)
						continue;

					var region = new SourceRegion()
					{
						Address = labelRegion.Start,
						Length = labelRegion.Length,
						StartLine = instruction.StartLine,
						EndLine = instruction.EndLine,
						StartColumn = instruction.StartColumn,
						EndColumn = instruction.EndColumn,
						Source = instruction.Document
					};

					regions.Add(region);

					break;
				}
			}

			return regions;
		}
	}
}
