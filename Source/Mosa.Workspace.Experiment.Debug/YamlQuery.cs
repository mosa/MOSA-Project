// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace Mosa.Workspace.Experiment.Debug
{
	public struct YamlQuery
	{
		private static readonly YamlQuery Empty = new YamlQuery(null as object);

		private object current;

		public YamlQuery(string text)
		{
			var yaml = new YamlStream();

			using (var input = new StringReader(text))
			{
				yaml.Load(input);
			}

			current = yaml.Documents[0].RootNode;
		}

		public YamlQuery(YamlStream stream)
		{
			current = stream.Documents[0].RootNode;
		}

		public YamlQuery(object yaml)
		{
			current = yaml;
		}

		public static YamlQuery Query(object yaml)
		{
			return new YamlQuery(yaml);
		}

		public YamlQuery On(string key)
		{
			if (current == null)
				return Empty;

			var node = current as YamlMappingNode;

			if (node == null)
				return Empty;

			if (!node.Children.ContainsKey(key))
				return Empty;

			return new YamlQuery(node[key]);
		}

		public bool IsNull { get { return current == null; } }

		public string AsString { get { return ((YamlScalarNode)current).Value as String; } }

		public long AsInteger { get { return Int64.Parse(AsString); } }

		public bool IsTrue { get { return CheckTrue(AsString); } }

		public bool IsFalse { get { return CheckFalse(AsString); } }

		public bool IsValueTrueOrFalse { get { return IsTrue || IsFalse; } }

		public bool IsInteger { get { return Int64.TryParse(AsString, out _); } }

		public void Update(ref string value)
		{
			if (IsNull)
				return;

			value = AsString;
		}

		public void Update(ref bool value)
		{
			if (IsNull)
				return;

			if (IsTrue)
				value = true;
			else if (IsFalse)
				value = false;
		}

		public void Update(ref long value)
		{
			if (IsNull)
				return;

			if (!IsInteger)
				return;

			value = AsInteger;
		}

		private static bool CheckTrue(string s)
		{
			if (string.IsNullOrEmpty(s)) return false;

			var lower = s.ToLower();

			if (lower == "true" || lower == "yes" || lower == "y") return true;

			return false;
		}

		private static bool CheckFalse(string s)
		{
			if (string.IsNullOrEmpty(s)) return false;

			var lower = s.ToLower();

			if (lower == "false" || lower == "no" || lower == "n") return true;

			return false;
		}

		public bool TryParseString(Action<string> setValue)
		{
			if (IsNull)
				return false;

			setValue(AsString);

			return true;
		}

		public bool TryParseBool(Action<bool> setValue)
		{
			if (IsNull)
				return false;

			if (IsTrue)
			{
				setValue(true);
				return true;
			}
			else if (IsFalse)
			{
				setValue(false);
				return true;
			}

			return false;
		}

		public bool TryParseInt32(Action<int> setValue)
		{
			if (IsNull)
				return false;

			if (!IsInteger)
				return false;

			setValue((int)AsInteger);

			return true;
		}

		public bool TryParseInt32(Action<long> setValue)
		{
			if (IsNull)
				return false;

			if (!IsInteger)
				return false;

			setValue(AsInteger);

			return true;
		}
	}
}
