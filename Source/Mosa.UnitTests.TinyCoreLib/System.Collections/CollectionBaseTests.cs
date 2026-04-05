// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.UnitTests.TinyCoreLib;

public static class CollectionBaseTests
{
	[MosaUnitTest]
	public static int Test_CollectionBase_Add()
	{
		var collection = new TestCollection
		{
			"Item1",
			"Item2"
		};
		return collection.Count;
	}

	[MosaUnitTest]
	public static int Test_CollectionBase_Remove()
	{
		var collection = new TestCollection
		{
			"Item1",
			"Item2"
		};
		collection.Remove("Item1");
		return collection.Count;
	}

	[MosaUnitTest]
	public static int Test_CollectionBase_Clear()
	{
		var collection = new TestCollection
		{
			"Item1",
			"Item2"
		};
		collection.Clear();
		return collection.Count;
	}

	[MosaUnitTest]
	public static bool Test_CollectionBase_Count()
	{
		var collection = new TestCollection();
		var assert1 = collection.Count == 0;
		collection.Add("Item1");
		return assert1 && collection.Count == 1;
	}

	[MosaUnitTest]
	public static bool Test_CollectionBase_Indexer()
	{
		var collection = new TestCollection
		{
			"Item1",
			"Item2"
		};
		return collection[0] == "Item1" && collection[1] == "Item2";
	}

	[MosaUnitTest]
	public static bool Test_CollectionBase_Contains()
	{
		var collection = new TestCollection
		{
			"Item1",
			"Item2"
		};
		return collection.Contains("Item1") && !collection.Contains("Item3");
	}

	// Helper class for CollectionBase testing
	private class TestCollection : CollectionBase
	{
		public void Add(string item)
		{
			List.Add(item);
		}

		public void Remove(string item)
		{
			List.Remove(item);
		}

		public bool Contains(string item)
		{
			return List.Contains(item);
		}

		public string this[int index]
		{
			get { return (string)List[index]; }
			set { List[index] = value; }
		}
	}
}
