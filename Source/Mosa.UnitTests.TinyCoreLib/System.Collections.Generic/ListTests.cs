// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTests.TinyCoreLib;

public static class ListTests
{
	// == Constructor tests

	[MosaUnitTest]
	public static int Test_Constructor_Default()
	{
		var list = new List<int>();
		return list.Count;
	}

	[MosaUnitTest]
	public static bool Test_Constructor_WithCapacity()
	{
		var list = new List<int>(10);
		return list.Count == 0 && list.Capacity >= 10;
	}

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_Constructor_FromCollection(int val1, int val2, int val3, int val4, int val5)
	{
		var source = new int[] { val1, val2, val3, val4, val5 };
		var list = new List<int>(source);
		return list.Count == 5 && list[0] == val1 && list[4] == val5;
	}

	//  == Add tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Add_SingleItem(int value)
	{
		var list = new List<int>();
		list.Add(value);
		return list.Count == 1 && list[0] == value;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Add_MultipleItems(int val1, int val2, int val3)
	{
		var list = new List<int>();
		list.Add(val1);
		list.Add(val2);
		list.Add(val3);
		return list.Count == 3 && list[0] == val1 && list[1] == val2 && list[2] == val3;
	}

	[MosaUnitTest]
	public static bool Test_Add_NullItem()
	{
		var list = new List<string?>();
		list.Add(null);
		return list.Count == 1 && list[0] == null;
	}

	//  == AddRange tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_AddRange_ValidCollection(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2 };
		var toAdd = new int[] { val3, val4, val5 };
		list.AddRange(toAdd);
		return list.Count == 5 && list[0] == val1 && list[4] == val5;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static int Test_AddRange_EmptyCollection(int val1, int val2)
	{
		var list = new List<int> { val1, val2 };
		list.AddRange(new int[] { });
		return list.Count;
	}

	//  == Insert tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_Insert_AtBeginning(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val2, val3, val4 };
		list.Insert(0, val1);
		return list.Count == 4 && list[0] == val1 && list[1] == val2;
	}

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static bool Test_Insert_AtMiddle(int val1, int val2, int val3, int val4)
	{
		var list = new List<int> { val1, val2, val4 };
		list.Insert(2, val3);
		return list.Count == 4 && list[2] == val3;
	}

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static bool Test_Insert_AtEnd(int val1, int val2, int val3, int val4)
	{
		var list = new List<int> { val1, val2, val3 };
		list.Insert(3, val4);
		return list.Count == 4 && list[3] == val4;
	}

	//  == InsertRange tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_InsertRange_ValidCollection(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val5 };
		var toInsert = new int[] { val2, val3, val4 };
		list.InsertRange(1, toInsert);
		return list.Count == 5 && list[1] == val2 && list[2] == val3 && list[3] == val4;
	}

	//  == Remove tests

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static bool Test_Remove_ExistingItem(int val1, int val2, int val3, int val4)
	{
		var list = new List<int> { val1, val2, val3, val4 };
		bool removed = list.Remove(val3);
		return removed && list.Count == 3 && !list.Contains(val3);
	}

	[MosaUnitTest]
	public static bool Test_Remove_NonExistingItem()
	{
		var list = new List<int> { 1, 2, 3 };
		bool removed = list.Remove(99);
		return !removed && list.Count == 3;
	}

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static bool Test_RemoveAt_ValidIndex(int val1, int val2, int val3, int val4)
	{
		var list = new List<int> { val1, val2, val3, val4 };
		list.RemoveAt(2);
		return list.Count == 3 && list[2] == val4;
	}

	//  == RemoveAll tests

	[MosaUnitTest]
	public static bool Test_RemoveAll_WithPredicate()
	{
		var list = new List<int> { 1, 2, 3, 4, 5, 6 };
		int removed = list.RemoveAll(x => x % 2 == 0);
		return removed == 3 && list.Count == 3 && list.Contains(1) && list.Contains(3) && list.Contains(5);
	}

	//  == RemoveRange tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_RemoveRange_ValidRange(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		list.RemoveRange(1, 3);
		return list.Count == 2 && list[0] == val1 && list[1] == val5;
	}

	//  == IndexOf tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_IndexOf_ExistingItem(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		int index = list.IndexOf(val3);
		return index;
	}

	[MosaUnitTest]
	public static int Test_IndexOf_NonExistingItem()
	{
		var list = new List<int> { 1, 2, 3 };
		int index = list.IndexOf(99);
		return index;
	}

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static int Test_IndexOf_WithStartIndex(int val1, int val2, int val3, int val4)
	{
		var list = new List<int> { val1, val2, val3, val2, val4 };
		int index = list.IndexOf(val2, 2);
		return index;
	}

	//  == LastIndexOf tests

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static int Test_LastIndexOf_ExistingItem(int val1, int val2, int val3, int val4)
	{
		var list = new List<int> { val1, val2, val3, val2, val4 };
		int index = list.LastIndexOf(val2);
		return index;
	}

	[MosaUnitTest]
	public static int Test_LastIndexOf_NonExistingItem()
	{
		var list = new List<int> { 1, 2, 3 };
		int index = list.LastIndexOf(99);
		return index;
	}

	//  == Contains tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_Contains_ExistingItem(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		return list.Contains(val3);
	}

	[MosaUnitTest]
	public static bool Test_Contains_NonExistingItem()
	{
		var list = new List<int> { 1, 2, 3 };
		return !list.Contains(99);
	}

	//  == Clear tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_Clear_NonEmptyList(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		list.Clear();
		return list.Count;
	}

	//  == Capacity tests

	[MosaUnitTest]
	public static bool Test_Capacity_SetValue()
	{
		var list = new List<int>();
		list.Capacity = 50;
		return list.Capacity >= 50;
	}

	//  == Indexer tests

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static bool Test_Indexer_Set(int val1, int val2, int val3, int val4)
	{
		var list = new List<int> { val1, val2, val3 };
		list[1] = val4;
		return list[1] == val4;
	}

	//  == CopyTo tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_CopyTo_ValidArray(int val1, int val2, int val3)
	{
		var list = new List<int> { val1, val2, val3 };
		var array = new int[4];
		list.CopyTo(array, 1);
		return array[0] == 0 && array[1] == val1 && array[2] == val2 && array[3] == val3;
	}

	//  == ToArray tests

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static bool Test_ToArray_ValidList(int val1, int val2, int val3, int val4)
	{
		var list = new List<int> { val1, val2, val3, val4 };
		var array = list.ToArray();
		return array.Length == 4 && array[0] == val1 && array[1] == val2 && array[2] == val3 && array[3] == val4;
	}

	//  == GetRange tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_GetRange_ValidRange(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		var range = list.GetRange(1, 3);
		return range.Count == 3 && range[0] == val2 && range[1] == val3 && range[2] == val4;
	}

	//  == Reverse tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_Reverse_EntireList(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		list.Reverse();
		return list[0] == val5 && list[1] == val4 && list[3] == val2 && list[4] == val1;
	}

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_Reverse_WithRange(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		list.Reverse(1, 3);
		return list[0] == val1 && list[1] == val4 && list[3] == val2 && list[4] == val5;
	}

	//  == Sort tests

	[MosaUnitTest]
	public static bool Test_Sort_DefaultComparer()
	{
		var list = new List<int> { 5, 2, 8, 1, 9 };
		list.Sort();
		return list[0] == 1 && list[1] == 2 && list[2] == 5 && list[3] == 8 && list[4] == 9;
	}

	[MosaUnitTest]
	public static bool Test_Sort_WithComparer()
	{
		var list = new List<int> { 1, 2, 3, 4, 5 };
		list.Sort(Comparer<int>.Create((a, b) => b.CompareTo(a))); // Descending
		return list[0] == 5 && list[1] == 4 && list[2] == 3 && list[3] == 2 && list[4] == 1;
	}

	//  == Find tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_Find_ExistingItem(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		var result = list.Find(x => x > val3);
		return result;
	}

	[MosaUnitTest]
	public static int Test_Find_NonExistingItem()
	{
		var list = new List<int> { 1, 2, 3 };
		var result = list.Find(x => x > 10);
		return result;
	}

	//  == FindAll tests

	[MosaUnitTest]
	public static bool Test_FindAll_WithMatches()
	{
		var list = new List<int> { 1, 2, 3, 4, 5, 6 };
		var result = list.FindAll(x => x % 2 == 0);
		return result.Count == 3 && result.Contains(2) && result.Contains(4) && result.Contains(6);
	}

	[MosaUnitTest]
	public static int Test_FindAll_NoMatches()
	{
		var list = new List<int> { 1, 3, 5 };
		var result = list.FindAll(x => x % 2 == 0);
		return result.Count;
	}

	//  == FindIndex tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_FindIndex_ExistingItem(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		var index = list.FindIndex(x => x > val3);
		return index;
	}

	[MosaUnitTest]
	public static int Test_FindIndex_NonExistingItem()
	{
		var list = new List<int> { 1, 2, 3 };
		var index = list.FindIndex(x => x > 10);
		return index;
	}

	//  == FindLast tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_FindLast_ExistingItem(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		var result = list.FindLast(x => x < val4);
		return result;
	}

	//  == FindLastIndex tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_FindLastIndex_ExistingItem(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		var index = list.FindLastIndex(x => x < val4);
		return index;
	}

	//  == Exists tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_Exists_WithMatch(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		var exists = list.Exists(x => x == val3);
		return exists;
	}

	[MosaUnitTest]
	public static bool Test_Exists_NoMatch()
	{
		var list = new List<int> { 1, 2, 3 };
		var exists = list.Exists(x => x > 10);
		return !exists;
	}

	//  == TrueForAll tests

	[MosaUnitTest]
	public static bool Test_TrueForAll_AllMatch()
	{
		var list = new List<int> { 2, 4, 6, 8 };
		var result = list.TrueForAll(x => x % 2 == 0);
		return result;
	}

	[MosaUnitTest]
	public static bool Test_TrueForAll_NotAllMatch()
	{
		var list = new List<int> { 1, 2, 3, 4 };
		var result = list.TrueForAll(x => x % 2 == 0);
		return !result;
	}

	//  == ForEach tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_ForEach_ExecutesAction(int val1, int val2, int val3)
	{
		var list = new List<int> { val1, val2, val3 };
		var sum = 0;
		list.ForEach(x => sum += x);
		return sum;
	}

	//  == ConvertAll tests

	[MosaUnitTest]
	public static bool Test_ConvertAll_ValidConverter()
	{
		var list = new List<int> { 1, 2, 3 };
		var result = list.ConvertAll(x => x.ToString());
		return result.Count == 3 && result[0] == "1" && result[1] == "2" && result[2] == "3";
	}

	//  == Enumerator tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_Enumerator_IteratesList(int val1, int val2, int val3)
	{
		var list = new List<int> { val1, val2, val3 };
		var sum = 0;
		foreach (var item in list)
		{
			sum += item;
		}
		return sum;
	}

	//  == BinarySearch tests

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_BinarySearch_ExistingItem(int val1, int val2, int val3, int val4, int val5)
	{
		var list = new List<int> { val1, val2, val3, val4, val5 };
		var index = list.BinarySearch(val3);
		return index;
	}

	[MosaUnitTest]
	public static int Test_BinarySearch_NonExistingItem()
	{
		var list = new List<int> { 1, 2, 4, 5 };
		var index = list.BinarySearch(3);
		return index;
	}

	//  == TrimExcess tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_TrimExcess_ReducesCapacity(int val1, int val2, int val3)
	{
		var list = new List<int>(100);
		list.Add(val1);
		list.Add(val2);
		list.Add(val3);
		var oldCapacity = list.Capacity;
		list.TrimExcess();
		return list.Capacity <= oldCapacity && list.Count == 3;
	}

	//  == AsReadOnly tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_AsReadOnly_ReturnsReadOnlyCollection(int val1, int val2, int val3)
	{
		var list = new List<int> { val1, val2, val3 };
		var readOnly = list.AsReadOnly();
		return readOnly.Count == 3 && readOnly[0] == val1 && readOnly[1] == val2 && readOnly[2] == val3;
	}
}
