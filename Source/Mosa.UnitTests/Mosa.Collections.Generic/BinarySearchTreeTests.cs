// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Collections.Generic;

namespace Mosa.UnitTests.Mosa.Collections.Generic
{
    public static class BinarySearchTreeTests
    {
		[MosaUnitTest]
		public static bool CreateValue()
		{
			BinarySearchTree<uint> BSTValue = new BinarySearchTree<uint>();

			return (BSTValue != null);
		}

		[MosaUnitTest]
		public static bool OperationsValue()
		{
			BinarySearchTree<uint> BSTValue = new BinarySearchTree<uint>();

			bool ResultAddition1;
			BSTValue.Add(3);
			BSTValue.Add(3);
			BSTValue.Add(3);
			BSTValue.Add(4);
			BSTValue.Add(2);
			BSTValue.Add(2);
			BSTValue.Add(1);
			ResultAddition1 = (BSTValue.GetActiveSize == 4) && (BSTValue.GetTotalSize == 4);
			ResultAddition1 = ResultAddition1 && (BSTValue.Find(4).CollisionCount == 1);
			ResultAddition1 = ResultAddition1 && (BSTValue.Find(3).CollisionCount == 3);
			ResultAddition1 = ResultAddition1 && (BSTValue.Find(2).CollisionCount == 2);
			ResultAddition1 = ResultAddition1 && (BSTValue.Find(1).CollisionCount == 1);

			bool ResultRemove;
			BSTValue.Remove(4);
			BSTValue.Remove(3);
			BSTValue.Remove(2);
			BSTValue.Remove(1);
			ResultRemove = (BSTValue.GetActiveSize == 2) && (BSTValue.GetTotalSize == 4);
			ResultRemove = ResultRemove && (BSTValue.Find(4).CollisionCount == 0);
			ResultRemove = ResultRemove && (BSTValue.Find(3).CollisionCount == 2);
			ResultRemove = ResultRemove && (BSTValue.Find(2).CollisionCount == 1);
			ResultRemove = ResultRemove && (BSTValue.Find(1).CollisionCount == 0);

			bool ResultAddition2;
			BSTValue.Add(1);
			ResultAddition2 = (BSTValue.GetActiveSize == 3) && (BSTValue.GetTotalSize == 4);
			ResultAddition2 = ResultAddition2 && (BSTValue.Find(4).CollisionCount == 0);
			ResultAddition2 = ResultAddition2 && (BSTValue.Find(3).CollisionCount == 2);
			ResultAddition2 = ResultAddition2 && (BSTValue.Find(2).CollisionCount == 1);
			ResultAddition2 = ResultAddition2 && (BSTValue.Find(1).CollisionCount == 1);

			bool ResultRemoveAll;
			BSTValue.RemoveAll(3);
			ResultRemoveAll = (BSTValue.GetActiveSize == 2) && (BSTValue.GetTotalSize == 4);
			ResultRemoveAll = ResultRemoveAll && (BSTValue.Find(4).CollisionCount == 0);
			ResultRemoveAll = ResultRemoveAll && (BSTValue.Find(3).CollisionCount == 0);
			ResultRemoveAll = ResultRemoveAll && (BSTValue.Find(2).CollisionCount == 1);
			ResultRemoveAll = ResultRemoveAll && (BSTValue.Find(1).CollisionCount == 1);

			bool ResultAddition3;
			BSTValue.Add(5);
			ResultAddition3 = (BSTValue.GetActiveSize == 3) && (BSTValue.GetTotalSize == 5);
			ResultAddition3 = ResultAddition3 && (BSTValue.Find(5).CollisionCount == 1);
			ResultAddition3 = ResultAddition3 && (BSTValue.Find(4).CollisionCount == 0);
			ResultAddition3 = ResultAddition3 && (BSTValue.Find(3).CollisionCount == 0);
			ResultAddition3 = ResultAddition3 && (BSTValue.Find(2).CollisionCount == 1);
			ResultAddition3 = ResultAddition3 && (BSTValue.Find(1).CollisionCount == 1);

			bool ResultClear;
			BSTValue.Clear();
			ResultClear = (BSTValue.GetActiveSize == 0 && BSTValue.GetTotalSize == 0 && BSTValue.GetRootNode == null);

			bool ResultFinal = ResultAddition1 && ResultRemove && ResultAddition2 && ResultRemoveAll && ResultAddition3 && ResultClear;
			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool CreateClass()
		{
			BinarySearchTree<TClass> BSTClass = new BinarySearchTree<TClass>();

			return (BSTClass != null);
		}

		[MosaUnitTest]
		public static bool OperationsClass()
		{
			BinarySearchTree<TClass> BSTClass = new BinarySearchTree<TClass>();

			TClass TClass1 = new TClass(1, 0xCAFEBABE);
			TClass TClass2 = new TClass(2, 0xDEADBABE);
			TClass TClass3 = new TClass(3, 0xDEADBEEF);

			bool ResultAddition1;
			BSTClass.Add(TClass2);
			BSTClass.Add(TClass1);
			BSTClass.Add(TClass3);
			ResultAddition1 = (BSTClass.GetActiveSize == 3) && (BSTClass.GetTotalSize == 3);
			ResultAddition1 = ResultAddition1 && (BSTClass.Find(TClass1).CollisionCount == 1);
			ResultAddition1 = ResultAddition1 && (BSTClass.Find(TClass2).CollisionCount == 1);
			ResultAddition1 = ResultAddition1 && (BSTClass.Find(TClass3).CollisionCount == 1);

			bool ResultAddition2;
			BSTClass.Add(TClass1);
			BSTClass.Add(TClass3);
			ResultAddition2 = (BSTClass.GetActiveSize == 3) && (BSTClass.GetTotalSize == 3);
			ResultAddition2 = ResultAddition2 && (BSTClass.Find(TClass1).CollisionCount == 2);
			ResultAddition2 = ResultAddition2 && (BSTClass.Find(TClass2).CollisionCount == 1);
			ResultAddition2 = ResultAddition2 && (BSTClass.Find(TClass3).CollisionCount == 2);

			bool ResultRemove;
			BSTClass.Remove(TClass2);
			ResultRemove = (BSTClass.GetActiveSize == 2) && (BSTClass.GetTotalSize == 3);
			ResultRemove = ResultRemove && (BSTClass.Find(TClass1).CollisionCount == 2);
			ResultRemove = ResultRemove && (BSTClass.Find(TClass2).CollisionCount == 0);
			ResultRemove = ResultRemove && (BSTClass.Find(TClass3).CollisionCount == 2);

			bool ResultClear;
			BSTClass.Clear();
			ResultClear = (BSTClass.GetActiveSize == 0 && BSTClass.GetTotalSize == 0 && BSTClass.GetRootNode == null);

			bool ResultFinal = ResultAddition1 && ResultAddition2 && ResultRemove && ResultClear;
			return ResultFinal;
		}
	}
}
