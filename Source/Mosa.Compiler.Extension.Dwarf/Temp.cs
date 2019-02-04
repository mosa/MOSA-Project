using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Source
{
	public class SourceRegion
	{
		public int Address { get; set; }
		public int Length { get; set; }

		public int StartLine { get; set; }
		public int EndLine { get; set; }
		public int StartColumn { get; set; }
		public int EndColumn { get; set; }

		public string Source { get; set; }
	}
}


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
