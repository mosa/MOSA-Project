// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Unit Test Runner
	/// </summary>
	public static class UnitTestQueue
	{
		private static uint queueNext = Address.UnitTestQueueStart;
		private static uint queueCurrent = Address.UnitTestQueueStart;
		private static uint count = 0;
		private static uint tick = 0;

		public static void Setup()
		{
			queueNext = Address.UnitTestQueueStart;
			queueCurrent = Address.UnitTestQueueStart;
			count = 0;

			Native.Set32(queueNext, 0);
		}

		public static bool QueueUnitTest(int id, uint start, uint end)
		{
			//Screen.Row = 12;
			//Screen.Column = 0;

			//Screen.Write("Queue:  ");
			//Screen.Write(" Tick: ");
			//Screen.Write(tick++, 16, 4);
			//Screen.Write(" Q.Next: ");
			//Screen.Write(queueNext, 16, 8);
			//Screen.Write(" Q.Current: ");
			//Screen.Write(queueCurrent, 16, 8);
			//Screen.Write(" Cnt: ");
			//Screen.Write(count, 16, 4);

			uint len = end - start;

			if (queueNext + len + 32 >= Address.UnitTestQueueEnd)
			{
				if (Address.UnitTestQueueStart + len + 32 >= queueCurrent)
					return false; // no space

				Native.Set32(queueNext, uint.MaxValue); // mark jump to front

				// cycle to front
				queueNext = Address.UnitTestQueueStart;

				//Screen.Write(" Cycling");
			}

			//else
			//{
			//	Screen.Write("        ");
			//}

			Native.Set32(queueNext, len + 4);
			queueNext = queueNext + 4;

			Native.Set32(queueNext, (uint)id);
			queueNext = queueNext + 4;

			for (uint i = start; i < end; i = i + 4)
			{
				uint value = Native.Get32(i);
				Native.Set32(queueNext, value);
				queueNext = queueNext + 4;
			}

			Native.Set32(queueNext, 0); // mark end
			count++;

			//Screen.NextLine();
			//Screen.Write("After:  ");
			//Screen.Write(" Tick: ");
			//Screen.Write(tick++, 16, 4);
			//Screen.Write(" Q.Next: ");
			//Screen.Write(queueNext, 16, 8);
			//Screen.Write(" Q.Current: ");
			//Screen.Write(queueCurrent, 16, 8);
			//Screen.Write(" Cnt: ");
			//Screen.Write(count, 16, 4);

			return true;
		}

		public static void ProcessQueue()
		{
			//Screen.Row = 16;
			//Screen.Column = 0;

			if (queueNext == queueCurrent)
				return;

			//Screen.Write("Process:");
			//Screen.Write(" Tick: ");
			//Screen.Write(tick++, 16, 4);
			//Screen.Write(" Q.Next: ");
			//Screen.Write(queueNext, 16, 8);
			//Screen.Write(" Q.Current: ");
			//Screen.Write(queueCurrent, 16, 8);
			//Screen.Write(" Cnt: ");
			//Screen.Write(count, 16, 4);

			if (!UnitTestRunner.IsReady())
			{
				//Screen.Write(" Not Ready ");
				return;
			}

			uint marker = Native.Get32(queueCurrent);

			if (marker == uint.MaxValue)
			{
				queueCurrent = Address.UnitTestQueueStart;

				//Screen.Write(" Executing!!");
			}

			//else
			//{
			//	Screen.Write(" Executing !");
			//}

			uint len = Native.Get32(queueCurrent);
			uint id = Native.Get32(queueCurrent + 4);
			uint address = Native.Get32(queueCurrent + 8);
			uint type = Native.Get32(queueCurrent + 12);
			uint paramcnt = Native.Get32(queueCurrent + 16);

			UnitTestRunner.SetUnitTestMethodAddress(address);
			UnitTestRunner.SetUnitTestResultType(type);
			UnitTestRunner.SetUnitTestMethodParameterCount(paramcnt);

			for (uint index = 0; index < paramcnt; index++)
			{
				uint value = Native.Get32(queueCurrent + 20 + (index * 4));
				UnitTestRunner.SetUnitTestMethodParameter(index, value);
			}

			queueCurrent = queueCurrent + len + 4;

			//Screen.NextLine();
			//Screen.Write("  After:");
			//Screen.Write(" Tick: ");
			//Screen.Write(tick++, 16, 4);
			//Screen.Write(" Q.Next: ");
			//Screen.Write(queueNext, 16, 8);
			//Screen.Write(" Q.Current: ");
			//Screen.Write(queueCurrent, 16, 8);
			//Screen.Write(" Cnt: ");
			//Screen.Write(queueCurrent, 16, 8);
			//Screen.NextLine();
			//Screen.NextLine();

			Screen.Row = 16;
			Screen.Column = 0;
			Screen.Write("   Test:");
			Screen.Write(" ID: ");
			Screen.Write(id, 10, 7);
			Screen.Write(" Address: ");
			Screen.Write(address, 16, 8);
			Screen.Write(" Param: ");
			Screen.Write(paramcnt, 16, 2);
			Screen.Write(" Len: ");
			Screen.Write(len, 16, 8);

			UnitTestRunner.StartTest((int)id);
		}
	}
}
