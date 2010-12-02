/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.Permutation
{
	public static class Numbers
	{

		public static IEnumerable<int> GetUpTo(int end)
		{
			for (int i = 0; i < end; i++)
				yield return i;
		}

		public static IEnumerable<int> SmallNumbers
		{
			get
			{
				yield return 0;
				yield return 1;
				yield return 2;
				yield return 4;
				yield return 6;
				yield return 7;
				yield return 8;
				yield return 10;
			}
		}

		public static IEnumerable<object[]> I1_I1
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1WithoutZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1Zero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					yield return new object[2] { a, (sbyte)0 };
			}
		}

		public static IEnumerable<object[]> I1_I1AboveZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b > 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1BelowZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b < 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> ISmall_I1
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (sbyte b in I1.Samples)
						yield return new object[2] { a, b };
			}
		}

	}
}
