// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86;

/// <summary>
/// Unit Test Runner
/// </summary>
public static class UnitTestQueue
{
	private static Pointer Queue;
	private static Pointer QueueNext;
	private static Pointer QueueCurrent;
	private static uint Count;

	private static readonly uint QueueSize = 0x00100000;

	public static void Setup()
	{
		Queue = new Pointer(Address.UnitTestQueue);
		QueueNext = Queue;
		QueueCurrent = Queue;

		Count = 0;

		QueueNext.Store32(0);
	}

	public static bool QueueUnitTest(uint id, Pointer start, Pointer end)
	{
		var len = (uint)start.GetOffset(end);

		if (QueueNext + len + 32 > Queue + QueueSize)
		{
			if (Queue + len + 32 >= QueueCurrent)
				return false; // no space

			QueueNext.Store32(uint.MaxValue); // mark jump to front

			// cycle to front
			QueueNext = Queue;
		}

		QueueNext.Store32(len + 4);
		QueueNext += 4;

		QueueNext.Store32(id);
		QueueNext += 4;

		for (var i = start; i < end; i += 4)
		{
			uint value = i.Load32();
			QueueNext.Store32(value);
			QueueNext += 4;
		}

		QueueNext.Store32(0); // mark end
		++Count;

		return true;
	}

	public static void Process()
	{
		if (QueueNext == QueueCurrent)
			return;

		if (!UnitTestRunner.IsReady())
			return;

		var marker = QueueCurrent.Load32();

		if (marker == uint.MaxValue)
		{
			QueueCurrent = Queue;
		}

		var len = QueueCurrent.Load32();
		var id = QueueCurrent.Load32(4);
		var address = QueueCurrent.Load32(8);
		var type = QueueCurrent.Load32(12);
		var paramcnt = QueueCurrent.Load32(16);

		UnitTestRunner.SetUnitTestMethodAddress(address);
		UnitTestRunner.SetUnitTestResultType(type);
		UnitTestRunner.SetUnitTestMethodParameterCount(paramcnt);

		for (var index = 0u; index < paramcnt; index++)
		{
			var value = QueueCurrent.Load32(20 + index * 4);
			UnitTestRunner.SetUnitTestMethodParameter(index, value);
		}

		QueueCurrent = QueueCurrent + len + 4;
		--Count;

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
		Screen.Write(" - Cnt: ");
		Screen.Write(Count, 10, 4);

		UnitTestRunner.StartTest(id);
	}
}
