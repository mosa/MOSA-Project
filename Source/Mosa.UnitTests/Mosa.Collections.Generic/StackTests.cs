// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Collections.Generic;

namespace Mosa.UnitTests.Mosa.Collections.Generic
{
	public static class StackTests
	{
		[MosaUnitTest]
		public static bool CreateValue()
		{
			Stack<uint> Calculator = new Stack<uint>();

			return (Calculator != null);
		}

		[MosaUnitTest]
		public static bool OperationsValue()
		{
			Stack<uint> Calculator = new Stack<uint>();

			bool ResultAddition;
			Calculator.Push(1);
			Calculator.Push(2);
			Calculator.Push(3);
			Calculator.Push(5);
			Calculator.Push(7);
			Calculator.Push(11);
			Calculator.Push(13);
			Calculator.Push(17);
			Calculator.Push(19);
			Calculator.Push(23);
			Calculator.Push(29);
			Calculator.Push(31);
			Calculator.Push(37);
			ResultAddition = (Calculator.Count == 13);

			bool ResultPop;
			ResultPop = (Calculator.Pop() == 37) && (Calculator.Pop() == 31) && (Calculator.Pop() == 29);

			bool ResultPeek;
			ResultPeek = (Calculator.Count == 10) && (Calculator.Peek() == 23);

			bool ResultFinal = ResultAddition && ResultPop && ResultPeek;
			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool CreateClass()
		{
			Stack<TClass> Calculator = new Stack<TClass>();

			return (Calculator != null);
		}

		[MosaUnitTest]
		public static bool OperationsClass()
		{
			Stack<TClass> Calculator = new Stack<TClass>();

			TClass Class1 = new TClass(1, 0xCAFEBABE);
			TClass Class2 = new TClass(2, 0xDEADBEEF);
			TClass Class3 = new TClass(3, 0xDEADCAFE);

			bool ResultAddition;
			Calculator.Push(Class3);
			Calculator.Push(Class2);
			Calculator.Push(Class1);
			ResultAddition = (Calculator.Count == 3);

			bool ResultPop;
			ResultPop = (Calculator.Pop().Magic == 0xCAFEBABE) && (Calculator.Pop().Magic == 0xDEADBEEF) && (Calculator.Pop().Magic == 0xDEADCAFE);

			bool ResultEmpty;
			ResultEmpty = (Calculator.Count == 0);

			bool ResultException = false;
			try
			{
				Calculator.Peek();
			}
			catch (CollectionsDataNotFoundException Except)
			{
				ResultException = true;
			}

			bool ResultFinal = ResultAddition && ResultPop && ResultEmpty && ResultException;
			return ResultFinal;
		}
	}
}
