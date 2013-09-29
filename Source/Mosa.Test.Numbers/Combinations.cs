/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Collections.Generic;

namespace Mosa.Test.Numbers
{
	public static class Combinations
	{
		public static IEnumerable<object[]> I4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					yield return new object[1] { i1 };
			}
		}

		public static IEnumerable<object[]> I4I4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					foreach (var i2 in Mosa.Test.Numbers.I4.Series)
						yield return new object[2] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> SmallI4I4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.I4.Series)
						yield return new object[2] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> SmallI4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					yield return new object[1] { i1 };
			}
		}
	}
}