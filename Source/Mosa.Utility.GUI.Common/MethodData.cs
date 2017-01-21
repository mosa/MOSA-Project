// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Utility.GUI.Common
{
	public class MethodData
	{
		public List<string> OrderedStageNames = new List<string>();
		public List<string> OrderedDebugStageNames = new List<string>();
		public Dictionary<string, List<string>> InstructionLogs = new Dictionary<string, List<string>>();
		public Dictionary<string, List<string>> DebugLogs = new Dictionary<string, List<string>>();
		public List<string> CounterData = new List<string>();
	}
}
