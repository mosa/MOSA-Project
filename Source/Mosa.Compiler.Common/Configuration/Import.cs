// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common.Configuration;

public static class Import
{
	public static Settings RecursiveReader(List<Argument> map, string[] args)
	{
		var commandLineSettings = Reader.ParseArguments(args, map);

		var final = RecursiveReader(commandLineSettings);

		return final;
	}

	private static Settings RecursiveReader(Settings start)
	{
		var files = start.GetValueList("Settings");

		Settings children = null;

		if (files != null && files.Count > 0)
		{
			foreach (var file in files)
			{
				var fileSettings = Reader.Import(file);

				var childSettings = RecursiveReader(fileSettings);

				var merge = Settings.Merge(childSettings, fileSettings);

				children = merge;
			}
		}

		if (children == null)
			return start;

		var merge2 = Settings.Merge(start, children);

		return merge2;
	}
}
