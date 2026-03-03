// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Explorer.Common;

public static class TransformListBuilder
{
	public static List<TransformEntry> BuildTransformList(List<string> debugLines)
	{
		var list = new List<TransformEntry>();

		if (debugLines == null)
			return list;

		//if (debugLines.Contains("*** Pass"))
		//	return list;

		list.Add(new TransformEntry { ID = 0, Name = "***Start***" });

		var pass = 0;
		TransformEntry entry = null;

		foreach (var line in debugLines)
		{
			if (string.IsNullOrEmpty(line))
				continue;

			if (line.StartsWith("*** Pass"))
			{
				pass = Convert.ToInt32(line[10..]);
				continue;
			}

			if (line.StartsWith("Merge Blocking: ") || line.StartsWith("Removed Unreachable Block:"))
				continue;

			var parts = line.Split('\t');
			if (parts.Length != 2)
				continue;

			var part1 = parts[1][1..].Trim();

			if (parts[0].StartsWith("L_") && entry != null)
			{
				entry.Block = parts[0].TrimEnd();
				entry.Before = part1;
				continue;
			}

			if (parts[0].StartsWith(' ') && entry != null)
			{
				entry.After = part1;
				continue;
			}

			entry = new TransformEntry
			{
				ID = Convert.ToInt32(parts[0].Trim()),
				Name = part1,
				Pass = pass
			};

			list.Add(entry);
		}

		return list;
	}
}
