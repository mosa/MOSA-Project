// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Common.Configuration
{
	public static class Reader
	{
		private static char[] Whitespace = new char[] { '\t', ' ' };

		public static Settings Import(string filename)
		{
			var lines = File.ReadAllLines(filename);
			return Import(lines);
		}

		public static Settings Import(IList<string> lines)
		{
			var settings = new Settings();

			string lastProperty = string.Empty;
			int lastLevel = 0;

			foreach (var l in lines)
			{
				if (l == null)
					continue;

				var line = TrimOutComment(l).TrimEnd(Whitespace);

				var level = DetermineLevel(line);

				var data = line.Substring(level).Trim();

				if (data.Length == 0)
					continue;

				if (data.StartsWith("\'") || data.StartsWith("-"))
				{
					string item = string.Empty;

					if (data.StartsWith("\'"))
						item = data.Trim('\'');
					else if (data.StartsWith("-"))
						item = data.Substring(1).Trim(Whitespace);

					var parent = GetSubPropertyName(lastProperty, level + lastLevel);
					var property = settings.CreateProperty(parent);

					property.List.Add(item);

					lastLevel = property.Level + level - 1;
					continue;
				}
				else
				{
					var name = ParsePropertyName(data);
					var value = ParsePropertyValue(data);

					var parent = GetSubPropertyName(lastProperty, level);

					var fullname = parent.Length == 0 ? name : $"{parent}.{name}";

					lastProperty = fullname;

					var property = settings.CreateProperty(fullname);
					property.Value = value;

					lastLevel = property.Level + level;
					continue;
				}
			}

			return settings;
		}

		public static Settings ParseArguments(string[] args, List<Argument> arguments)
		{
			var settings = new Settings();
			var defaultMap = GetDefaultArgumentMap(arguments);

			for (var at = 0; at < args.Length; at++)
			{
				var arg = args[at];

				if (arg == "-s" || arg == "-p" || arg == "-setting" || arg == "-property")
				{
					var parts = args[++at].Split('=');

					var property = settings.CreateProperty(parts[0]);

					property.Value = parts[1];

					continue;
				}

				var map = GetArgumentMap(arg, arguments);

				if (map == null || map.Count == 0)
				{
					if (defaultMap == null)
						continue;

					var property = settings.CreateProperty(defaultMap.Setting);

					if (defaultMap.IsList)
					{
						property.List.Add(arg);
					}
					else
					{
						property.Value = arg;
					}

					continue;
				}
				else
				{
					bool increment = false;

					foreach (var entry in map)
					{
						var property = settings.CreateProperty(entry.Setting);

						if (entry.Value != null)
						{
							property.Value = entry.Value;
						}
						else if (entry.IsList)
						{
							increment = true;
							property.List.Add(args[at + 1]);
						}
						else
						{
							increment = true;
							property.Value = args[at + 1];
						}
					}

					if (increment)
						at++;
				}
			}

			return settings;
		}

		#region Internal Methods

		private static string ParsePropertyName(string data)
		{
			var pos = data.IndexOf(':');

			if (pos == 0)
				return null;

			var name = data.Substring(0, pos).Trim(Whitespace);

			return name;
		}

		private static string ParsePropertyValue(string data)
		{
			var pos = data.IndexOf(':');

			if (pos == 0)
				return null;

			var value = data.Substring(pos + 1).Trim(Whitespace);

			return value;
		}

		private static string TrimOutComment(string line)
		{
			int pos = line.IndexOf('#');

			if (pos < 0)
				return line;

			return line.Substring(0, pos);
		}

		private static string GetSubPropertyName(string name, int level)
		{
			if (level == 0)
				return string.Empty;

			int count = 0;

			for (int i = 0; i < name.Length; i++)
			{
				var c = name[i];

				if (c == '.')
				{
					count++;

					if (count == level)
					{
						return name.Substring(0, i);
					}
				}
			}

			return name;
		}

		private static int DetermineLevel(string line)
		{
			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];

				if (c != '\t')
					return i;
			}

			return 0;
		}

		#endregion Internal Methods

		private static List<Argument> GetArgumentMap(string arg, List<Argument> arguments)
		{
			var map = new List<Argument>();

			foreach (var entry in arguments)
			{
				if (entry.Name == arg)
				{
					map.Add(entry);
				}
			}
			return map;
		}

		private static Argument GetDefaultArgumentMap(List<Argument> map)
		{
			foreach (var entry in map)
			{
				if (entry.Value == null && entry.Name == null)
				{
					return entry;
				}
			}

			return null;
		}
	}
}
