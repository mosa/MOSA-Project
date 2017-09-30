// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTest.Collection
{
	public static class GenericMethodTests
	{
		internal static List<object> CreateList()
		{
			var list = new List<object>();

			list.Add(1);
			list.Add("string");
			list.Add(new object());

			return list;
		}

		public static List<object> Get<T>(List<object> objects)
		{
			var list = new List<object>();

			foreach (var item in objects)
			{
				if (item is T)
				{
					list.Add(item);
				}
			}

			return list;
		}

		public static int MethodTestInt()
		{
			var list = CreateList();

			var ret = Get<int>(list);

			return ret.Count;
		}

		public static int MethodTestObject()
		{
			var list = CreateList();

			var ret = Get<object>(list);

			return ret.Count;
		}

		public static int MethodTestString()
		{
			var list = CreateList();

			var ret = Get<string>(list);

			return ret.Count;
		}
	}
}
