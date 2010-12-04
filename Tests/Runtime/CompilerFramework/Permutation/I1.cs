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
	public class I1
	{
		private static IList<sbyte> samples = null;
		public static IList<sbyte> SampleData { get { if (samples == null) samples = GetSamples(); return samples; } }

		public static IEnumerable<sbyte> Samples
		{
			get
			{
				foreach (sbyte value in SampleData)
					yield return value;
			}
		}

		public static IList<sbyte> GetSamples()
		{
			List<sbyte> list = new List<sbyte>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(sbyte.MinValue);
			list.Add(sbyte.MaxValue);
			list.Add(sbyte.MinValue + 1);
			list.Add(sbyte.MaxValue - 1);

			// Get negatives
			list.AddIfNew<sbyte>(GetNegatives(list));

			list.Sort();

			return list;
		}

		protected static IList<sbyte> GetNegatives(IList<sbyte> list)
		{
			List<sbyte> negs = new List<sbyte>();

			foreach (sbyte value in list)
			{
				if (value > 0)
					negs.AddIfNew<sbyte>((sbyte)-value);
			}

			return negs;
		}
	}
}
