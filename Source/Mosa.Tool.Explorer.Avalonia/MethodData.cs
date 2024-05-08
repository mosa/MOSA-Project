// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Explorer.Avalonia;

public class MethodData
{
	//public MosaMethod Method;

	public int Version;

	public readonly List<string> OrderedStageNames = new List<string>();
	public readonly List<string> OrderedDebugStageNames = new List<string>();

	public readonly List<string> OrderedTransformStageNames = new List<string>();

	public readonly Dictionary<string, List<InstructionRecord>> InstructionLogs = new Dictionary<string, List<InstructionRecord>>();
	public readonly Dictionary<string, Dictionary<int, List<InstructionRecord>>> TransformLogs = new Dictionary<string, Dictionary<int, List<InstructionRecord>>>();

	public readonly Dictionary<string, List<string>> DebugLogs = new Dictionary<string, List<string>>();

	public List<string> Counters = new List<string>();
}
