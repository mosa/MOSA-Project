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
	public static class R8
	{
		private static IList<double> series = null;

		public static IEnumerable<double> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (double value in series)
					yield return value;
			}
		}

		public static IList<double> GetSeries()
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
			list.Add(1.1497);
			list.Add(1.2);
			list.Add(17.0002501);
			list.Add(23);
			list.Add(12321452132.561);

			// Get negatives
			list.AddIfNew<double>(GetNegatives(list));

			list.Sort();

			return list;
		}

		private static IList<double> GetNegatives(IList<double> list)
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
