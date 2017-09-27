// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Unit Test Runner
	/// </summary>
	public static class UnitTestQueue
	{
		private static uint queueNext = Address.UnitTestQueue;
		private static uint queueCurrent = Address.UnitTestQueue;
		private static uint count = 0;
		private static uint tick = 0;

		private static uint TestQueueSize = 0x00001000;

		public static void Setup()
		{
			queueNext = Address.UnitTestQueue;
			queueCurrent = Address.UnitTestQueue;
			count = 0;

			Intrinsic.Store32(queueNext, 0);
		}

		public static bool QueueUnitTest(uint id, uint start, uint end)
		{
			uint len = end - start;
			if (queueNext + len + 32 >= Address.UnitTestQueue + TestQueueSize)
			{
				if (Address.UnitTestQueue + len + 32 >= queueCurrent)
					return false; // no space

				Intrinsic.Store32(queueNext, uint.MaxValue); // mark jump to front

				// cycle to front
				queueNext = Address.UnitTestQueue;
			}

			Intrinsic.Store32(queueNext, len + 4);
			queueNext = queueNext + 4;

			Intrinsic.Store32(queueNext, (uint)id);
			queueNext = queueNext + 4;

			for (uint i = start; i < end; i = i + 4)
			{
				uint value = Intrinsic.Load32(i);
				Intrinsic.Store32(queueNext, value);
				queueNext = queueNext + 4;
			}

			Intrinsic.Store32(queueNext, 0); // mark end
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

			uint marker = Intrinsic.Load32(queueCurrent);

			if (marker == uint.MaxValue)
			{
				queueCurrent = Address.UnitTestQueue;
			}

			uint len = Intrinsic.Load32(queueCurrent);
			uint id = Intrinsic.Load32(queueCurrent, 4);
			uint address = Intrinsic.Load32(queueCurrent, 8);
			uint type = Intrinsic.Load32(queueCurrent, 12);
			uint paramcnt = Intrinsic.Load32(queueCurrent, 16);

			UnitTestRunner.SetUnitTestMethodAddress(address);
			UnitTestRunner.SetUnitTestResultType(type);
			UnitTestRunner.SetUnitTestMethodParameterCount(paramcnt);

			for (uint index = 0; index < paramcnt; index++)
			{
				uint value = Intrinsic.Load32(queueCurrent, 20 + (index * 4));
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
