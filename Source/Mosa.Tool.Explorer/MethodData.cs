// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Explorer;

public class MethodData
{
	//public MosaMethod Method;

	public int Version;

	public List<string> OrderedStageNames = new List<string>();
	public List<string> OrderedDebugStageNames = new List<string>();

	public List<string> OrderedTransformStageNames = new List<string>();

	public Dictionary<string, List<string>> InstructionLogs = new Dictionary<string, List<string>>();
	public Dictionary<string, List<string>> DebugLogs = new Dictionary<string, List<string>>();

	public List<string> MethodCounters = new List<string>();

	public Dictionary<string, Dictionary<int, List<string>>> TransformLogs = new Dictionary<string, Dictionary<int, List<string>>>();
}
