// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Unit Test Runner
	/// </summary>
	public static class UnitTestQueue
	{
		private static IntPtr queueNext;
		private static IntPtr queueCurrent;
		private static uint count = 0;

		private static uint TestQueueSize = 0x00100000;

		public static void Setup()
		{
			queueNext = new IntPtr(Address.UnitTestQueue);
			queueCurrent = new IntPtr(Address.UnitTestQueue);
			count = 0;

			Intrinsic.Store32(queueNext, 0);
		}

		public static bool QueueUnitTest(uint id, IntPtr start, IntPtr end)
		{
			uint len = (uint)start.GetOffset(end);

			if ((queueNext + (int)len + 32).GreaterThan(new IntPtr(Address.UnitTestQueue) + (int)TestQueueSize))
			{
				if (new IntPtr(Address.UnitTestQueue + len + 32).GreaterThanOrEqual(queueCurrent))
					return false; // no space

				Intrinsic.Store32(queueNext, uint.MaxValue); // mark jump to front

				// cycle to front
				queueNext = new IntPtr(Address.UnitTestQueue);
			}

			Intrinsic.Store32(queueNext, len + 4);
			queueNext += 4;

			Intrinsic.Store32(queueNext, id);
			queueNext += 4;

			for (var i = start; i.LessThan(end); i += 4)
			{
				uint value = Intrinsic.Load32(i);
				Intrinsic.Store32(queueNext, value);
				queueNext += 4;
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
				queueCurrent = new IntPtr(Address.UnitTestQueue);
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

			queueCurrent = queueCurrent + (int)len + 4;
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
