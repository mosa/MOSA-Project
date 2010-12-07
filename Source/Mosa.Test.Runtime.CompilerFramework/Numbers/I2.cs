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

namespace Test.Mosa.Runtime.CompilerFramework.Numbers
{
	public class I2
	{
		private static IList<short> samples = null;
		public static IList<short> SampleData { get { if (samples == null) samples = GetSamples(); return samples; } }

		public static IEnumerable<short> Samples
		{
			get
			{
				foreach (short value in SampleData)
					yield return value;
			}
		}

		public static IList<short> GetSamples()
		{
			List<short> list = new List<short>();

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

			// Get negatives
			list.AddIfNew<short>(GetNegatives(list));

			list.Sort();

			return list;
		}

		protected static IList<short> GetNegatives(IList<short> list)
		{
			List<short> negs = new List<short>();

			foreach (short value in list)
			{
				if (value > 0)
					negs.AddIfNew<short>((short)-value);
			}

			return negs;
		}
	}
}
