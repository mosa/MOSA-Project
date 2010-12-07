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

namespace Mosa.Test.Runtime.CompilerFramework.Numbers
{
	public class I4
	{
		private static IList<int> samples = null;
		public static IList<int> SampleData { get { if (samples == null) samples = GetSamples(); return samples; } }

		public static IEnumerable<int> Samples
		{
			get
			{
				foreach (int value in SampleData)
					yield return value;
			}
		}

		public static IList<int> GetSamples()
		{
			List<int> list = new List<int>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(sbyte.MinValue);
			list.Add(sbyte.MaxValue);
			list.Add(sbyte.MinValue + 1);
			list.Add(sbyte.MaxValue - 1);
			list.Add(byte.MaxValue);
			list.Add(byte.MinValue);
			list.Add(short.MinValue);
			list.Add(short.MaxValue);
			list.Add(short.MinValue + 1);
			list.Add(short.MaxValue - 1);
			list.Add(int.MinValue);
			list.Add(int.MaxValue);
			list.Add(int.MinValue + 1);
			list.Add(int.MaxValue - 1);

			// Get negatives
			list.AddIfNew<int>(GetNegatives(list));

			list.Sort();

			return list;
		}

		protected static IList<int> GetNegatives(IList<int> list)
		{
			List<int> negs = new List<int>();

			foreach (int value in list)
			{
				if (value > 0)
					negs.AddIfNew<int>((int)-value);
			}

			return negs;
		}
	}
}
