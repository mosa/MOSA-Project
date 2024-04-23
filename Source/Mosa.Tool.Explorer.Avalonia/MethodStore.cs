// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Tool.Explorer.Avalonia;

public class MethodStore
{
	private readonly Dictionary<MosaMethod, MethodData> methodDataStore = new Dictionary<MosaMethod, MethodData>();

	public void Clear()
	{
		lock (methodDataStore)
		{
			methodDataStore.Clear();
		}
	}

	private static void ClearMethodDataOnNewVersion(int version, MethodData methodData)
	{
		if (methodData.Version == version)
			return;

		methodData.InstructionLogs.Clear();
		methodData.OrderedDebugStageNames.Clear();
		methodData.OrderedStageNames.Clear();
		methodData.OrderedTransformStageNames.Clear();
		methodData.Counters.Clear();
		methodData.DebugLogs.Clear();
		methodData.TransformLogs.Clear();
		methodData.Version = version;
	}

	public MethodData GetMethodData(MosaMethod method, bool create)
	{
		lock (methodDataStore)
		{
			if (methodDataStore.TryGetValue(method, out MethodData methodData) || !create)
				return methodData;

			methodData = new MethodData();
			methodDataStore.Add(method, methodData);
			return methodData;
		}
	}

	public void SetInstructionTraceInformation(MosaMethod method, string stage, List<string> lines, int version)
	{
		var methodData = GetMethodData(method, true);

		lock (methodData)
		{
			ClearMethodDataOnNewVersion(version, methodData);

			methodData.OrderedStageNames.AddIfNew(stage);
			methodData.InstructionLogs.Remove(stage);
			methodData.InstructionLogs.Add(stage, lines);
		}
	}

	public void SetTransformTraceInformation(MosaMethod method, string stage, List<string> lines, int version, int step)
	{
		var methodData = GetMethodData(method, true);

		lock (methodData)
		{
			ClearMethodDataOnNewVersion(version, methodData);

			methodData.OrderedTransformStageNames.AddIfNew(stage);

			if (!methodData.TransformLogs.TryGetValue(stage, out var directionary))
			{
				directionary = new Dictionary<int, List<string>>();
				methodData.TransformLogs.Add(stage, directionary);
			}

			directionary.Add(step, lines);
		}
	}

	public void SetDebugStageInformation(MosaMethod method, string stage, List<string> lines, int version)
	{
		var methodData = GetMethodData(method, true);

		lock (methodData)
		{
			ClearMethodDataOnNewVersion(version, methodData);

			methodData.OrderedDebugStageNames.AddIfNew(stage);
			methodData.DebugLogs.Remove(stage);
			methodData.DebugLogs.Add(stage, lines);
		}
	}

	public void SetMethodCounterInformation(MosaMethod method, List<string> lines, int version)
	{
		var methodData = GetMethodData(method, true);

		lock (methodData)
		{
			ClearMethodDataOnNewVersion(version, methodData);

			methodData.Counters = lines;
		}
	}

	public string GetStageInstructions(List<string> lines, string blockLabel, bool strip, bool pad, bool removeNop)
	{
		var result = new StringBuilder();

		if (lines == null)
			return string.Empty;

		if (string.IsNullOrWhiteSpace(blockLabel))
		{
			foreach (var l in lines)
			{
				var line = l;

				if (line.Contains("IR.BlockStart") || line.Contains("IR.BlockEnd"))
					continue;

				if (removeNop && line.Contains("IR.Nop"))
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

		var inBlock = false;

		foreach (var l in lines)
		{
			var line = l;

			if ((!inBlock) && line.StartsWith("Block #") && line.EndsWith(blockLabel))
				inBlock = true;

			if (!inBlock)
				continue;

			if (line.Contains("IR.BlockStart") || line.Contains("IR.BlockEnd"))
				continue;

			if (strip)
				line = StripBracketContents(line);

			if (pad)
				line = PadInstruction(line);

			line = Simplify(line);

			result.Append(line);
			result.Append('\n');

			if (line.StartsWith("  Next:"))
				return result.ToString();
		}

		return result.ToString();
	}

	private static string StripBracketContents(string s)
	{
		if (string.IsNullOrEmpty(s) || s.Length < 5)
			return s;

		if (!char.IsDigit(s[0]))
			return s;

		var at = 0;
		while (true)
		{
			var open = s.IndexOf(" [", at, StringComparison.Ordinal);
			if (open < 0)
				return s;

			var close = s.IndexOf(']', open);
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

	private static string PadInstruction(string s)
	{
		const int padding = 30;

		if (string.IsNullOrEmpty(s) || s.Length < 5)
			return s;

		if (!char.IsDigit(s[0]))
			return s;

		var first = s.IndexOf(':');
		if (first is < 0 or > 15)
			return s;

		var second = s.IndexOf(' ', first + 2);

		switch (second)
		{
			case < 0: return s;
			case > padding: return s;
			default:
				{
					s = s.Insert(second, new string(' ', padding - second));
					return s;
				}
		}
	}

	private static string Simplify(string s) => s.Replace("const=", string.Empty);
}
