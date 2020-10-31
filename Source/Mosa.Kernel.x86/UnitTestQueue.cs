// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Unit Test Runner
	/// </summary>
	public static class UnitTestQueue
	{
		private static Pointer queueNext;
		private static Pointer queueCurrent;
		private static uint count = 0;

		private static readonly uint TestQueueSize = 0x00100000;

		public static void Setup()
		{
			queueNext = new Pointer(Address.UnitTestQueue);
			queueCurrent = new Pointer(Address.UnitTestQueue);
			count = 0;

			queueNext.Store32(0);
		}

		public static bool QueueUnitTest(uint id, Pointer start, Pointer end)
		{
			uint len = (uint)start.GetOffset(end);

			if ((queueNext + len + 32) > (new Pointer(Address.UnitTestQueue) + TestQueueSize))
			{
				if (new Pointer(Address.UnitTestQueue + len + 32) >= queueCurrent)
					return false; // no space

				queueNext.Store32(uint.MaxValue); // mark jump to front

				// cycle to front
				queueNext = new Pointer(Address.UnitTestQueue);
			}

			queueNext.Store32(len + 4);
			queueNext += 4;

			queueNext.Store32(id);
			queueNext += 4;

			for (var i = start; i < end; i += 4)
			{
				uint value = i.Load32();
				queueNext.Store32(value);
				queueNext += 4;
			}

			queueNext.Store32(0); // mark end
			++count;

			return true;
		}

		public static void ProcessQueue()
		{
			if (queueNext == queueCurrent)
				return;

			if (!UnitTestRunner.IsReady())
			{
				return;
			}

			uint marker = queueCurrent.Load32();

			if (marker == uint.MaxValue)
			{
				queueCurrent = new Pointer(Address.UnitTestQueue);
			}

			uint len = queueCurrent.Load32();
			uint id = queueCurrent.Load32(4);
			uint address = queueCurrent.Load32(8);
			uint type = queueCurrent.Load32(12);
			uint paramcnt = queueCurrent.Load32(16);

			UnitTestRunner.SetUnitTestMethodAddress(address);
			UnitTestRunner.SetUnitTestResultType(type);
			UnitTestRunner.SetUnitTestMethodParameterCount(paramcnt);

			for (uint index = 0; index < paramcnt; index++)
			{
				uint value = queueCurrent.Load32(20 + (index * 4));
				UnitTestRunner.SetUnitTestMethodParameter(index, value);
			}

			queueCurrent = queueCurrent + len + 4;
			--count;

			Screen.Goto(17, 0);
			Screen.ClearRow();
			Screen.Write("[Unit Test]");
			Screen.NextLine();
			Screen.ClearRow();
			Screen.Write("ID: ");
			Screen.Write(id, 10, 5);
			Screen.Write(" Address: ");
			Screen.Write(address, 16, 8);
			Screen.Write(" Param: ");
			Screen.Write(paramcnt, 10, 2);
			Screen.Write(" Len: ");
			Screen.Write(len, 10, 4);
			Screen.Write(" - Cnt: ");
			Screen.Write(count, 10, 4);

			UnitTestRunner.StartTest(id);
		}
	}
}
