// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.UnitTests
{
	public static class CompilerBugTests
	{
		[MosaUnitTest]
		public static bool Test()
		{
			ProcessManager.ProcessList = new List<Process>();
			ProcessManager.ProcessList.Add(new Process { ProcessID = 0 });
			ProcessManager.ProcessList.Add(new Process { ProcessID = 1 });
			ProcessManager.ProcessList.Add(new Process { ProcessID = 2 });
			ProcessManager.ProcessList.Add(new Process { ProcessID = 3 });

			int targetProcessID = 2;
			var proc = ProcessManager.GetProcess(targetProcessID);

			return proc.ProcessID == 2;
		}

		[MosaUnitTest]
		public unsafe static bool TestSpan()
		{
			var alloc = stackalloc byte[12];

			var span1 = new Span<int>(alloc, 3);
			span1[2] = 42;

			var span2 = new Span<int>(alloc, 3);
			return span2[2] == 42;
		}
	}

	internal static class ProcessManager
	{
		public static List<Process> ProcessList;

		public static Process GetProcess(int processID)
		{
			lock (ProcessList)
				for (var i = 0; i < ProcessList.Count; i++)
					if (ProcessList[i].ProcessID == processID)
						return ProcessList[i];
			return null;
		}
	}

	internal class Process : IDisposable
	{
		public int ProcessID;

		public void Dispose()
		{
		}
	}
}
