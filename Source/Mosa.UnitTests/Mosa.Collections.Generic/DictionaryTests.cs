// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Collections.Generic;

namespace Mosa.UnitTests.Mosa.Collections.Generic
{
	public static class DictionaryTests
	{
		[MosaUnitTest]
		public static bool CreateValue()
		{
			Dictionary<uint, uint> MagicWords = new Dictionary<uint, uint>();

			return (MagicWords != null);
		}

		[MosaUnitTest]
		public static bool OperationsValue()
		{
			Dictionary<uint, uint> MagicWords = new Dictionary<uint, uint>();

			bool ResultAddition1;
			MagicWords.Add(1, 0xCAFEBABE);
			MagicWords.Add(2, 0xDEADBEEF);
			MagicWords.Add(3, 0xDEADCAFE);
			ResultAddition1 = (MagicWords.Count == 3);
			ResultAddition1 = ResultAddition1 && (MagicWords[1] == 0xCAFEBABE);
			ResultAddition1 = ResultAddition1 && (MagicWords[2] == 0xDEADBEEF);
			ResultAddition1 = ResultAddition1 && (MagicWords[3] == 0xDEADCAFE);

			bool ResultException1 = false;
			try
			{
				uint Dummy = MagicWords[4];
			}
			catch (CollectionsDataNotFoundException Except)
			{
				ResultException1 = true;
			}

			bool ResultException2 = false;
			try
			{
				MagicWords.Add(3, 0xCAFEBEEF);
			}
			catch (CollectionsDataExistsException Except)
			{
				ResultException2 = true;
			}

			bool ResultAddition2;
			MagicWords.Add(4, 0xCAFEBEEF);
			ResultAddition2 = (MagicWords.Count == 4) && (MagicWords[4] == 0xCAFEBEEF);

			bool ResultClear;
			MagicWords.Clear();
			ResultClear = (MagicWords.Count == 0) && (MagicWords.GetFirstNode == null) && (MagicWords.GetLastNode == null);

			bool ResultFinal = ResultAddition1 && ResultException1 && ResultException2 && ResultAddition2 && ResultClear;
			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool SortValue()
		{
			Dictionary<uint, uint> MagicWords = new Dictionary<uint, uint>();

			bool ResultAddition;
			MagicWords.Add(3, 0xDEADCAFE);
			MagicWords.Add(2, 0xDEADBEEF);
			MagicWords.Add(1, 0xCAFEBABE);
			ResultAddition = (MagicWords.Count == 3);

			bool ResultSort;
			MagicWords.SortWithBinarySearchTree();
			ResultSort = (MagicWords.Count == 3) && (MagicWords.GetFirstNode.Value == 0xCAFEBABE) && (MagicWords.GetLastNode.Value == 0xDEADCAFE);

			bool ResultFinal = ResultAddition && ResultSort;
			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool CreateClass()
		{
			Dictionary<uint, TClass> MagicWords = new Dictionary<uint, TClass>();

			return (MagicWords != null);
		}

		[MosaUnitTest]
		public static bool OperationsClass()
		{
			Dictionary<uint, TClass> MagicWords = new Dictionary<uint, TClass>();

			TClass TClass1 = new TClass(1, 0xCAFEBABE);
			TClass TClass2 = new TClass(2, 0xDEADBEEF);
			TClass TClass3 = new TClass(3, 0xDEADCAFE);
			TClass TClass4 = new TClass(4, 0xCAFEBEEF);

			bool ResultAddition1;
			MagicWords.Add(1, TClass1);
			MagicWords.Add(2, TClass2);
			MagicWords.Add(3, TClass3);
			ResultAddition1 = (MagicWords.Count == 3);
			ResultAddition1 = ResultAddition1 && (MagicWords[1].Magic == 0xCAFEBABE);
			ResultAddition1 = ResultAddition1 && (MagicWords[2].Magic == 0xDEADBEEF);
			ResultAddition1 = ResultAddition1 && (MagicWords[3].Magic == 0xDEADCAFE);

			bool ResultException1 = false;
			try
			{
				uint Dummy = MagicWords[4].Magic;
			}
			catch (CollectionsDataNotFoundException Except)
			{
				ResultException1 = true;
			}

			bool ResultException2 = false;
			try
			{
				MagicWords.Add(3, TClass4);
			}
			catch (CollectionsDataExistsException Except)
			{
				ResultException2 = true;
			}

			bool ResultAddition2;
			MagicWords.Add(4, TClass4);
			ResultAddition2 = (MagicWords.Count == 4);
			ResultAddition2 = ResultAddition2 && (MagicWords[4].Magic == 0xCAFEBEEF);

			bool ResultClear;
			MagicWords.Clear();
			ResultClear = (MagicWords.Count == 0) && (MagicWords.GetFirstNode == null) && (MagicWords.GetLastNode == null);

			bool ResultFinal = ResultAddition1 && ResultException1 && ResultException2 && ResultAddition2 && ResultClear;
			return ResultFinal;
		}

		[MosaUnitTest]
		public static bool SortClass()
		{
			Dictionary<uint, TClass> MagicWords = new Dictionary<uint, TClass>();

			TClass Class1 = new TClass(1, 0xCAFEBABE);
			TClass Class2 = new TClass(2, 0xDEADBEEF);
			TClass Class3 = new TClass(3, 0xDEADCAFE);

			bool ResultAddition;
			MagicWords.Add(3, Class3);
			MagicWords.Add(2, Class2);
			MagicWords.Add(1, Class1);
			ResultAddition = (MagicWords.Count == 3);

			bool ResultSort;
			MagicWords.SortWithBinarySearchTree();
			ResultSort = (MagicWords.Count == 3) && (MagicWords.GetFirstNode.Value.Magic == 0xCAFEBABE) && (MagicWords.GetLastNode.Value.Magic == 0xDEADCAFE);

			bool ResultFinal = ResultAddition && ResultSort;
			return ResultFinal;
		}
	}
}
