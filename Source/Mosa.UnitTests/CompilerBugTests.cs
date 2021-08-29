// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
		public unsafe static bool TestMethodGeneric()
		{
			_ = new Span<byte>();
			return true;
		}

		[MosaUnitTest]
		public unsafe static bool TestSpan1()
		{
			var myArray = new int[3];
			fixed (int* ptr = &myArray[0])
			{
				var span1 = new Span<int>(ptr, 3);
				span1[1] = 42;

				var span2 = new Span<int>(ptr, 3);
				return span2[1] == 42;
			}
		}

		[MosaUnitTest]
		public unsafe static bool TestSpan2()
		{
			var myArray = new int[3];
			myArray[1] = 42;
			fixed (int* ptr = &myArray[0])
			{
				var span = new Span<int>(ptr, 3);
				return span[1] == 42;
			}
		}

		[MosaUnitTest]
		public unsafe static bool TestSpan3()
		{
			var myArray = new int[3];
			fixed (int* ptr = &myArray[0])
			{
				var span = new Span<int>(ptr, 3);
				TestSpan3Inner(span);
				return span[1] == 42;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal unsafe static void TestSpan3Inner(Span<int> span)
		{
			span[1] = 42;
		}

		[MosaUnitTest]
		public unsafe static bool TestReadOnlySpan1()
		{
			var myArray = new int[3];
			fixed (int* ptr = &myArray[0])
			{
				var span1 = new Span<int>(ptr, 3);
				span1[1] = 42;

				var span2 = new ReadOnlySpan<int>(ptr, 3);
				return span2[1] == 42;
			}
		}

		[MosaUnitTest]
		public unsafe static bool TestReadOnlySpan2()
		{
			var myArray = new int[3];
			myArray[1] = 42;
			fixed (int* ptr = &myArray[0])
			{
				var span = new ReadOnlySpan<int>(ptr, 3);
				return span[1] == 42;
			}
		}

		public unsafe struct TestRefStoreStruct
		{
			public int a;
		}

		[MosaUnitTest]
		public unsafe static bool TestRefStore()
		{
			var value = new TestRefStoreStruct();
			var ptr = 42;
			TestRefStoreInner(ref value, ref ptr);
			return value.a == ptr;
		}

		internal unsafe static void TestRefStoreInner(ref TestRefStoreStruct value, ref int ptr)
		{
			value.a = ptr;
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
