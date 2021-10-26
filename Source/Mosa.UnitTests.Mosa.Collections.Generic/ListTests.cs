// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Collections.Generic;

namespace Mosa.UnitTests.Mosa.Collections.Generic
{
	public static class ListTests
	{
		[MosaUnitTest]
		public static bool CreateValue()
		{
			List<uint> Primes = new List<uint>();

			return (Primes != null);
		}

		[MosaUnitTest]
		public static bool OperationsValue()
		{
			List<uint> Primes = new List<uint>();

			bool ResultAddition1;
			Primes.AddLast(1);
			Primes.AddLast(1);
			Primes.AddLast(1);
			Primes.AddLast(2);
			Primes.AddLast(3);
			Primes.AddLast(3);
			Primes.AddLast(3);
			Primes.AddLast(5);
			Primes.AddLast(7);
			Primes.AddLast(11);
			Primes.AddLast(13);
			Primes.AddLast(17);
			Primes.AddLast(19);
			ResultAddition1 = (Primes.Count == 13);

			bool ResultRemoveAll1;
			Primes.RemoveAll(1);
			ResultRemoveAll1 = (Primes.Count == 10);

			bool ResultRemoveFirst;
			Primes.RemoveFirst(3);
			ResultRemoveFirst = (Primes.Count == 9);

			bool ResultRemoveLast;
			Primes.RemoveLast(3);
			ResultRemoveLast = (Primes.Count == 8);

			bool ResultRemoveAll2;
			Primes.RemoveAll(3);
			ResultRemoveAll2 = (Primes.Count == 7);

			bool ResultRemoveAll3;
			Primes.RemoveAll(7);
			ResultRemoveAll3 = (Primes.Count == 6);

			bool ResultAddAfter1;
			Primes.AddAfter(Primes.FindFirst(2), 3);
			ResultAddAfter1 = (Primes.Count == 7);

			bool ResultAddAfter2;
			Primes.AddAfter(Primes.FindFirst(5), 7);
			ResultAddAfter2 = (Primes.Count == 8);

			bool ResultAddAfter3;
			Primes.AddAfter(Primes.FindFirst(19), 23);
			ResultAddAfter3 = (Primes.Count == 9);

			bool ResultRemoveAll4;
			Primes.RemoveAll(2);
			ResultRemoveAll4 = (Primes.Count == 8);

			bool ResultRemoveAll5;
			Primes.RemoveAll(19);
			ResultRemoveAll5 = (Primes.Count == 7);

			bool ResultAddBefore1;
			Primes.AddBefore(Primes.FindFirst(3), 2);
			ResultAddBefore1 = (Primes.Count == 8);

			bool ResultAddBefore2;
			Primes.AddBefore(Primes.FindFirst(23), 19);
			ResultAddBefore2 = (Primes.Count == 9);

			bool ResultClear;
			Primes.Clear();
			ResultClear = (Primes.Count == 0);

			bool ResultFinal = ResultAddition1 && ResultRemoveAll1 && ResultRemoveFirst && ResultRemoveLast && ResultRemoveAll2 && ResultRemoveAll3;
			ResultFinal = ResultFinal && ResultAddAfter1 && ResultAddAfter2 && ResultAddAfter3 && ResultRemoveAll4 && ResultRemoveAll5;
			ResultFinal = ResultFinal && ResultAddBefore1 && ResultAddBefore2 && ResultClear;
			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool SortValue()
		{
			List<uint> Primes = new List<uint>();

			bool ResultAddition;
			Primes.Add(23);
			Primes.Add(13);
			Primes.Add(29);
			Primes.Add(2);
			Primes.Add(19);
			Primes.Add(5);
			Primes.Add(37);
			Primes.Add(17);
			Primes.Add(3);
			Primes.Add(31);
			Primes.Add(7);
			Primes.Add(11);
			ResultAddition = (Primes.Count == 12);

			bool ResultSort;
			Primes.SortWithBinarySearchTree();
			ResultSort = (Primes.Count == 12) && (Primes.GetFirstNode.Data == 2) && (Primes.GetLastNode.Data == 37);

			bool ResultFinal = ResultAddition && ResultSort;
			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool CreateClass()
		{
			List<TClass> Classes = new List<TClass>();

			return (Classes != null);
		}

		[MosaUnitTest]
		public static bool OperationsClass()
		{
			List<TClass> Classes = new List<TClass>();

			TClass Class1 = new TClass(1, 0xCAFEBABE);
			TClass Class2 = new TClass(2, 0xDEADBABE);
			TClass Class3 = new TClass(3, 0xDEADBEEF);

			bool ResultAddition;
			Classes.AddLast(Class1);
			Classes.AddLast(Class2);
			Classes.AddLast(Class3);
			Classes.AddLast(Class1);
			ResultAddition = (Classes.Count == 4);

			bool ResultRemoveAll;
			Classes.RemoveAll(Class1);
			ResultRemoveAll = (Classes.Count == 2);

			bool ResultAddBefore;
			Classes.AddBefore(Classes.FindFirst(Class2), Class1);
			ResultAddBefore = (Classes.Count == 3);

			bool ResultAddAfter;
			Classes.AddAfter(Classes.FindLast(Class3), Class1);
			ResultAddAfter = (Classes.Count == 4);

			bool ResultClear;
			Classes.Clear();
			ResultClear = (Classes.Count == 0);

			bool ResultFinal = ResultAddition && ResultRemoveAll && ResultAddBefore && ResultAddAfter && ResultClear;
			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool SortClass()
		{
			List<TClass> Classes = new List<TClass>();

			TClass Class1 = new TClass(1, 0xCAFEBABE);
			TClass Class2 = new TClass(2, 0xDEADBABE);
			TClass Class3 = new TClass(3, 0xDEADBEEF);

			bool ResultAddition;
			Classes.AddLast(Class3);
			Classes.AddLast(Class2);
			Classes.AddLast(Class1);
			ResultAddition = (Classes.Count == 3);

			bool ResultSort;
			Classes.SortWithBinarySearchTree();
			ResultSort = (Classes.Count == 3) && (Classes.GetFirstNode.Data.Magic == 0xCAFEBABE) && (Classes.GetLastNode.Data.Magic == 0xDEADBEEF);

			bool ResultFinal = ResultAddition && ResultSort;
			return ResultFinal;
		}
	}
}
