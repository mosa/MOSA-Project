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

		public string GetStageInstructions(List<string> lines, string blockLabel)
		{
			var result = new StringBuilder();

			if (lines == null)
				return string.Empty;

			if (string.IsNullOrWhiteSpace(blockLabel))
			{
				foreach (var line in lines)
				{
					if (line.Contains("IR.BlockStart") || line.Contains("IR.BlockEnd"))
						continue;

					result.Append(line);
					result.Append("\n");
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
	}
}
