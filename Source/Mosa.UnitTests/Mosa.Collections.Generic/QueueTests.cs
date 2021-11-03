// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Collections.Generic;

namespace Mosa.UnitTests.Mosa.Collections.Generic
{
	public static class QueueTests
	{
		[MosaUnitTest]
		public static bool CreateValue()
		{
			Queue<uint> WaitingLine = new Queue<uint>();

			return (WaitingLine != null);
		}

		[MosaUnitTest]
		public static bool OperationsValue()
		{
			Queue<uint> WaitingLine = new Queue<uint>();

			bool ResultAddition;
			WaitingLine.Enqueue(1);
			WaitingLine.Enqueue(2);
			WaitingLine.Enqueue(3);
			WaitingLine.Enqueue(5);
			WaitingLine.Enqueue(7);
			WaitingLine.Enqueue(11);
			WaitingLine.Enqueue(13);
			WaitingLine.Enqueue(17);
			WaitingLine.Enqueue(19);
			WaitingLine.Enqueue(23);
			WaitingLine.Enqueue(29);
			WaitingLine.Enqueue(31);
			WaitingLine.Enqueue(37);
			ResultAddition = (WaitingLine.Count == 13);

			bool ResultDequeue;
			ResultDequeue = (WaitingLine.Dequeue() == 1) && (WaitingLine.Dequeue() == 2) && (WaitingLine.Dequeue() == 3);

			bool ResultCount;
			ResultCount = (WaitingLine.Count == 10);

			bool ResultPeek;
			ResultPeek = (WaitingLine.Peek() == 5);

			bool ResultReverse;
			Queue<uint> Reversed = WaitingLine.Reverse();
			ResultReverse = (Reversed.GetFirstNode.Data == 37) && (Reversed.GetLastNode.Data == 5);

			bool ResultFinal;
			ResultFinal = ResultAddition && ResultDequeue && ResultCount && ResultPeek && ResultReverse;

			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool OperationsClass()
		{
			Queue<TClass> WaitingLine = new Queue<TClass>();

			TClass Class1 = new TClass(1, 0xCAFEBABE);
			TClass Class2 = new TClass(2, 0xDEADBEEF);
			TClass Class3 = new TClass(3, 0xDEADCAFE);

			bool ResultAddition;
			WaitingLine.Enqueue(Class1);
			WaitingLine.Enqueue(Class2);
			WaitingLine.Enqueue(Class3);
			ResultAddition = (WaitingLine.Count == 3);

			bool ResultDequeue;
			ResultDequeue = (WaitingLine.Dequeue().Magic == 0xCAFEBABE);
			ResultDequeue = ResultDequeue && (WaitingLine.Dequeue().Magic == 0xDEADBEEF);
			ResultDequeue = ResultDequeue && (WaitingLine.Dequeue().Magic == 0xDEADCAFE);

			bool ResultCount;
			ResultCount = (WaitingLine.Count == 0);

			bool ResultException = false;
			try
			{
				WaitingLine.Peek();
			}
			catch (CollectionsDataNotFoundException Except)
			{
				ResultException = true;
			}

			bool ResultFinal = ResultAddition && ResultDequeue && ResultCount && ResultException;
			return ResultFinal;
		}
	}
}
