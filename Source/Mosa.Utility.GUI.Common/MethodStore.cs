// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Utility.GUI.Common
{
	public class MethodStore
	{
		private readonly Dictionary<MosaMethod, MethodData> methodDataStore = new Dictionary<MosaMethod, MethodData>();

		public void Clear()
		{
			methodDataStore.Clear();
		}

		public MethodData GetMethodData(MosaMethod method, bool create)
		{
			lock (methodDataStore)
			{
				if (!methodDataStore.TryGetValue(method, out MethodData methodData))
				{
					if (create)
					{
						methodData = new MethodData();
						methodDataStore.Add(method, methodData);
					}
				}
				return methodData;
			}
		}

		public void SetInstructionTraceInformation(MosaMethod method, string stage, List<string> lines)
		{
			var methodData = GetMethodData(method, true);

			lock (methodData)
			{
				methodData.OrderedStageNames.AddIfNew(stage);
				methodData.InstructionLogs.Remove(stage);
				methodData.InstructionLogs.Add(stage, lines);
			}
		}

		public void SetDebugStageInformation(MosaMethod method, string stage, List<string> lines)
		{
			var methodData = GetMethodData(method, true);

			lock (methodData)
			{
				methodData.OrderedDebugStageNames.AddIfNew(stage);
				methodData.DebugLogs.Remove(stage);
				methodData.DebugLogs.Add(stage, lines);
			}
		}

		public void SetMethodCounterInformation(MosaMethod method, List<string> lines)
		{
			var methodData = GetMethodData(method, true);

			lock (methodData)
			{
				methodData.CounterData = lines;
			}
		}

		public string GetStageInstructions(List<string> lines, string blockLabel, bool strip, bool pad)
		{
			var result = new StringBuilder();

			if (lines == null)
				return string.Empty;

			if (string.IsNullOrWhiteSpace(blockLabel))
			{
				foreach (var l in lines)
				{
					string line = l;

					if (line.Contains("IR.BlockStart") || line.Contains("IR.BlockEnd"))
						continue;

					if (strip)
						line = StripBracketContents(line);

					if (pad)
						line = PadInstruction(line);

					line = Simplify(line);

					result.Append(line);
					result.Append('\n');
				}

				return result.ToString();
			}

			bool inBlock = false;

			foreach (var l in lines)
			{
				string line = l;

				if ((!inBlock) && line.StartsWith("Block #") && line.EndsWith(blockLabel))
				{
					inBlock = true;
				}

				if (inBlock)
				{
					if (line.Contains("IR.BlockStart") || line.Contains("IR.BlockEnd"))
						continue;

					if (strip)
						line = StripBracketContents(line);

					if (pad)
						line = PadInstruction(line);

					line = Simplify(line);

					result.Append(line);
					result.Append("\n");

					if (line.StartsWith("  Next:"))
					{
						return result.ToString();
					}
				}
			}

			return result.ToString();
		}

		private string StripBracketContents(string s)
		{
			if (string.IsNullOrEmpty(s) || s.Length < 5)
				return s;

			if (!char.IsDigit(s[0]))
				return s;

			int at = 0;

			while (true)
			{
				int open = s.IndexOf(" [", at);

				if (open < 0)
					return s;

				int close = s.IndexOf(']', open);

				if (close < 0)
					return s;

				var part = s.Substring(open + 2, close - open - 2);

				if (part == "NULL" || char.IsSymbol(part[0]) || char.IsPunctuation(part[0]))
				{
					at = close;
					continue;
				}

				s = s.Remove(open, close - open + 1);

				at = open;
			}
		}

		private string PadInstruction(string s)
		{
			const int padding = 30;

			if (string.IsNullOrEmpty(s) || s.Length < 5)
				return s;

			if (!char.IsDigit(s[0]))
				return s;

			int first = s.IndexOf(':');

			if (first < 0 || first > 15)
				return s;

			int second = s.IndexOf(' ', first + 2);

			if (second < 0)
				return s;

			if (second > padding)
				return s;

			s = s.Insert(second, new string(' ', padding - second));

			return s;
		}

		private string Simplify(string s)
		{
			return s.Replace("const=", string.Empty);
		}
	}
}
