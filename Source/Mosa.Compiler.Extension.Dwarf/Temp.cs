using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mosa.Compiler.Framework.Source
{

	/// <summary>
	/// Mapping of method address to source code location
	/// </summary>
	public class SourceRegion
	{
		/// <summary>
		/// Local address (Offset)
		/// </summary>
		public int Address { get; set; }

		/// <summary>
		/// How many bytes after Address belongs to this SourceRegion
		/// </summary>
		public int Length { get; set; }

		public int StartLine { get; set; }
		public int EndLine { get; set; }
		public int StartColumn { get; set; }
		public int EndColumn { get; set; }

		public string Filename { get; set; }
	}
}

namespace Mosa.Compiler.Framework.Source
{
	public static class SourceRegions
	{

		/// <summary>
		/// Returns mapping of method address to source code location.
		/// </summary>
		public static List<SourceRegion> GetSourceRegions(MethodData data)
		{
			var method = data.Method;
			var regions = new List<SourceRegion>(data.LabelRegions.Count + 1);

			// Add method header
			if (method.Code.Count > 0)
			{
				var firstInstruction = method.Code[0];

				regions.Add(new SourceRegion
				{
					Address = 0,
					Length = data.LabelRegions.Count > 0 ? data.LabelRegions[0].Start : 0,
					StartLine = firstInstruction.StartLine,
					EndLine = firstInstruction.EndLine,
					StartColumn = firstInstruction.StartColumn,
					EndColumn = firstInstruction.EndColumn,
					Filename = firstInstruction.Document
				});
			}

			var startLine = 0;
			var endLine = 0;
			var startColumn = 0;
			var endColumn = 0;
			var filename = "";

			foreach (var labelRegion in data.LabelRegions)
			{

				foreach (var instruction in method.Code)
				{

					// special case: the return label is always 0xFFFFF
					var searchForLabel = labelRegion.Label;
					if (labelRegion.Label == 0xFFFFF)
						searchForLabel = method.Code.Last().Offset;

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

					regions.Add(region);
				}
			}
			return regions;
		}

	}
}
