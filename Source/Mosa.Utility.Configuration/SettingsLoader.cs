// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Utility.Configuration
{
	public static class SettingsLoader
	{
		public static Settings RecursiveReader(string[] args)
		{
			var commandLineSettings = Reader.ParseArguments(args, CommandLineArguments.Map);

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
}
