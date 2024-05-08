// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Tool.Explorer.Avalonia;

public class MethodStore
{
	private readonly Dictionary<MosaMethod, MethodData> methodDataStore = new Dictionary<MosaMethod, MethodData>();

	public void Clear()
	{
		lock (methodDataStore)
			methodDataStore.Clear();
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
		var records = new List<InstructionRecord>();

		foreach (var line in lines)
			records.Add(new InstructionRecord(line));

		lock (methodData)
		{
			ClearMethodDataOnNewVersion(version, methodData);

			methodData.OrderedStageNames.AddIfNew(stage);
			methodData.InstructionLogs.Remove(stage);
			methodData.InstructionLogs.Add(stage, records);
		}
	}

	public void SetTransformTraceInformation(MosaMethod method, string stage, List<string> lines, int version, int step)
	{
		var methodData = GetMethodData(method, true);
		var records = new List<InstructionRecord>();

		foreach (var line in lines)
			records.Add(new InstructionRecord(line));

		lock (methodData)
		{
			ClearMethodDataOnNewVersion(version, methodData);

			methodData.OrderedTransformStageNames.AddIfNew(stage);

			if (!methodData.TransformLogs.TryGetValue(stage, out var dictionary))
			{
				dictionary = new Dictionary<int, List<InstructionRecord>>();
				methodData.TransformLogs.Add(stage, dictionary);
			}

			dictionary.Add(step, records);
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
}
