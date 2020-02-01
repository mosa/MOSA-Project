// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.DebugData
{
	public static class Source
	{
		public static SourceLocation Find(DebugSource debugSource, ulong address)
		{
			var method = debugSource.GetMethod(address);

			if (method == null)
				return null;

			var sourceLabel = debugSource.GetSourceLabel(method.ID, (int)(address - method.Address));

			if (sourceLabel == null)
				return null;

			var sourceInfo = debugSource.GetSourcePreviousClosest(method.ID, sourceLabel.Label);

			if (sourceInfo == null)
				return null;

			var sourceFileInfo = debugSource.GetSourceFile(sourceInfo.SourceFileID);

			var sourceLocation = new SourceLocation()
			{
				Address = method.Address + (ulong)sourceLabel.StartOffset,
				Length = sourceLabel.Length,
				Label = sourceLabel.Label,
				SourceLabel = sourceInfo.Label,
				MethodFullName = method.FullName,
				StartLine = sourceInfo.StartLine,
				StartColumn = sourceInfo.StartColumn,
				EndLine = sourceInfo.EndLine,
				EndColumn = sourceInfo.EndColumn,
				SourceFilename = sourceFileInfo.Filename,
				MethodID = method.ID,
				SourceFileID = sourceInfo.SourceFileID,
			};

			return sourceLocation;
		}
	}
}

//SourceLabelInfo:
//	MethodID
//	Label<-            | need this
//	Start              |
//	Length             V
//                     V
//                     V
//SourceInfo:          V
//	MethodID           |
//	Offset [/Label] <- | to get this
//	[...]
//	SourceFileID
