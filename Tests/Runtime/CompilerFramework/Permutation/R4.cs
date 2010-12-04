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
	public class R4
	{
		private static IList<float> samples = null;
		public static IList<float> SampleData { get { if (samples == null) samples = GetSamples(); return samples; } }

		public static IEnumerable<float> Samples
		{
			get
			{
				foreach (float value in SampleData)
					yield return value;
			}
		}

		public static IList<float> GetSamples()
		{
			List<float> list = new List<float>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(float.MinValue);
			list.Add(float.MaxValue);
			list.Add(float.NaN);
			list.Add(float.PositiveInfinity);
			list.Add(float.NegativeInfinity);

			list.Add(1.00012f);
			//list.Add(1.045);
			list.Add(1.1497f);
			list.Add(1.2f);
			//list.Add(1.5f);
			//list.Add(1.67f);
			//list.Add(1.78f);
			//list.Add(2.1f);
			//list.Add(2.11f);
			//list.Add(2.198f);
			//list.Add(2.3f);
			list.Add(17.0002501f);
			//list.Add(17.094002f);
			//list.Add(17.1f);
			//list.Add(17.12f);
			//list.Add(17.4f);
			//list.Add(17.59f);
			//list.Add(17.6f);
			//list.Add(17.875f);
			//list.Add(17.99f);
			//list.Add(21.2578f);
			list.Add(23f);
			//list.Add(123.001f);
			//list.Add(123.023f);
			//list.Add(123.1f);
			//list.Add(123.235f);
			//list.Add(123.283f);
			//list.Add(123.34f);
			//list.Add(123.41f);
			list.Add(12321452132.561f);

			// Get negatives
			list.AddIfNew<float>(GetNegatives(list));

			list.Sort();

			return list;
		}

		protected static IList<float> GetNegatives(IList<float> list)
		{
			List<float> negs = new List<float>();

			foreach (float value in list)
			{
				if (value > 0)
					negs.AddIfNew<float>((float)-value);
			}

			return negs;
		}
	}
}
