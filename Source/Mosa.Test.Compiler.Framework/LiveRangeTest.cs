/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using MbUnit.Framework;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;

namespace Mosa.Test.Compiler.Framework
{

	[TestFixture]
	public class LiveRangeTest
	{
		[Test]
		public void Inside1()
		{
			Interval l1_2 = new Interval(1, 2);

			Assert.IsTrue(l1_2.IsInside(1));
			Assert.IsFalse(l1_2.IsInside(2));
			Assert.IsFalse(l1_2.IsInside(0));
			Assert.IsFalse(l1_2.IsInside(3));
		}

		[Test]
		public void Intersect1()
		{
			Interval l1_2 = new Interval(1, 2);
			Interval l2_3 = new Interval(2, 3);
			
			Assert.IsFalse(l1_2.Intersects(l2_3));
			Assert.IsFalse(l2_3.Intersects(l1_2));

			Interval l1_3 = new Interval(1, 3);

			Assert.IsTrue(l1_2.Intersects(l1_3));
			Assert.IsTrue(l1_3.Intersects(l1_2));

			Interval l1_100 = new Interval(1, 100);
			Interval l50_60 = new Interval(50, 60);

			Assert.IsTrue(l1_100.Intersects(l50_60));
			//Assert.IsTrue(l50_60.Overlaps(l1_100));
		}

		[Test]
		public void RangeMerge1()
		{
			List<Interval> ranges = new List<Interval>();

			Interval.AddRangeToList(ranges, new Interval(1, 2));
			Assert.Count(1, ranges);

			Interval.AddRangeToList(ranges, new Interval(2, 3));
			Assert.Count(1, ranges);

			Interval.AddRangeToList(ranges, new Interval(3, 4));
			Assert.Count(1, ranges);
		}

		[Test]
		public void RangeMerge2()
		{
			List<Interval> ranges = new List<Interval>();

			Interval.AddRangeToList(ranges, new Interval(2, 3));
			Assert.Count(1, ranges);

			Interval.AddRangeToList(ranges, new Interval(1, 2));
			Assert.Count(1, ranges);

			Interval.AddRangeToList(ranges, new Interval(3, 4));
			Assert.Count(1, ranges);
		}

		[Test]
		public void RangeMerge3()
		{
			List<Interval> ranges = new List<Interval>();

			Interval.AddRangeToList(ranges, new Interval(1, 2));
			Assert.Count(1, ranges);

			Interval.AddRangeToList(ranges, new Interval(4, 5));
			Assert.Count(2, ranges);

			Interval.AddRangeToList(ranges, new Interval(3, 3));
			Assert.Count(3, ranges);

			Interval.AddRangeToList(ranges, new Interval(2, 3));
			Assert.Count(2, ranges);

			Interval.AddRangeToList(ranges, new Interval(3, 4));
			Assert.Count(1, ranges);
		}

	}
}
