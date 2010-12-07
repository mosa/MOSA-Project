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
	public class R8
	{
		private static IList<double> samples = null;
		public static IList<double> SampleData { get { if (samples == null) samples = GetSamples(); return samples; } }

		public static IEnumerable<double> Samples
		{
			get
			{
				foreach (double value in SampleData)
					yield return value;
			}
		}

		public static IList<double> GetSamples()
		{
			List<double> list = new List<double>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(double.MinValue);
			list.Add(double.MaxValue);
			list.Add(double.NaN);
			list.Add(double.PositiveInfinity);
			list.Add(double.NegativeInfinity);

			list.Add(1.00012);
			//list.Add(1.045);
			list.Add(1.1497);
			list.Add(1.2);
			//list.Add(1.5);
			//list.Add(1.67);
			//list.Add(1.78);
			//list.Add(2.1);
			//list.Add(2.11);
			//list.Add(2.198);
			//list.Add(2.3);
			list.Add(17.0002501);
			//list.Add(17.094002);
			//list.Add(17.1);
			//list.Add(17.12);
			//list.Add(17.4);
			//list.Add(17.59);
			//list.Add(17.6);
			//list.Add(17.875);
			//list.Add(17.99);
			//list.Add(21.2578);
			list.Add(23);
			//list.Add(123.001);
			//list.Add(123.023);
			//list.Add(123.1);
			//list.Add(123.235);
			//list.Add(123.283);
			//list.Add(123.34);
			//list.Add(123.41);
			list.Add(12321452132.561);

			// Get negatives
			list.AddIfNew<double>(GetNegatives(list));

			list.Sort();

			return list;
		}

		protected static IList<double> GetNegatives(IList<double> list)
		{
			List<double> negs = new List<double>();

			foreach (double value in list)
			{
				if (value > 0)
					negs.AddIfNew<double>((double)-value);
			}

			return negs;
		}
	}
}
