// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Tool.Explorer
{
	public class MethodData
	{
		public int Version;
		public List<string> OrderedStageNames = new List<string>();
		public List<string> OrderedDebugStageNames = new List<string>();
		public Dictionary<string, List<string>> InstructionLogs = new Dictionary<string, List<string>>();
		public Dictionary<string, List<string>> DebugLogs = new Dictionary<string, List<string>>();
		public List<string> CounterData = new List<string>();
	}
}
