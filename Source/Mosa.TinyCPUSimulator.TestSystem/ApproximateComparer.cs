
// Source:
// http://www.ademiller.com/blogs/tech/2008/12/dealing-with-rounding-errors-in-numerical-unit-tests/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public class ApproximateComparer : IComparer<double>
	{
		public double MarginOfError { get; private set; }

		public ApproximateComparer(double marginOfError)
		{
			if ((marginOfError <= 0) || (marginOfError >= 1.0))
				throw new ArgumentException("...");

			MarginOfError = marginOfError;
		}

		public int Compare(double x, double y)  // x = expected, y = actual
		{
			if (x != 0)
			{
				double margin = Math.Abs((x - y) / x);

				if (margin <= MarginOfError)
					return 0;
			}

			return new Comparer(CultureInfo.CurrentUICulture).Compare(x, y);
		}
	}
}