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
		public void InsideTests1()
		{
			LiveRange l1_2 = new LiveRange(1, 2);

			Assert.IsTrue(l1_2.IsInside(1));
			Assert.IsFalse(l1_2.IsInside(2));
			Assert.IsFalse(l1_2.IsInside(0));
			Assert.IsFalse(l1_2.IsInside(3));
		}

		[Test]
		public void OverlapTests1()
		{
			LiveRange l1_2 = new LiveRange(1, 2);
			LiveRange l2_3 = new LiveRange(2, 3);
			
			Assert.IsFalse(l1_2.Overlaps(l2_3));
			Assert.IsFalse(l2_3.Overlaps(l1_2));

			LiveRange l1_3 = new LiveRange(1, 3);

			Assert.IsTrue(l1_2.Overlaps(l1_3));
			Assert.IsTrue(l1_3.Overlaps(l1_2));

			LiveRange l1_100 = new LiveRange(1, 100);
			LiveRange l50_60 = new LiveRange(50, 60);

			Assert.IsTrue(l1_100.Overlaps(l50_60));
			//Assert.IsTrue(l50_60.Overlaps(l1_100));
		}

		[Test]
		public void RangeTests1()
		{
			List<LiveRange> ranges = new List<LiveRange>();

			LiveRange.AddRangeToList(ranges, new LiveRange(1, 2));
			Assert.Count(1, ranges);

			LiveRange.AddRangeToList(ranges, new LiveRange(2, 3));
			Assert.Count(2, ranges);

			LiveRange.AddRangeToList(ranges, new LiveRange(1, 3));
			Assert.Count(1, ranges);
		}

		[Test]
		public void RangeTests2()
		{
			List<LiveRange> ranges = new List<LiveRange>();

			LiveRange.AddRangeToList(ranges, new LiveRange(2, 3));
			Assert.Count(1, ranges);

			LiveRange.AddRangeToList(ranges, new LiveRange(1, 2));
			Assert.Count(2, ranges);

			LiveRange.AddRangeToList(ranges, new LiveRange(1, 3));
			Assert.Count(1, ranges);
		}

		[Test]
		public void RangeTests3()
		{
			List<LiveRange> ranges = new List<LiveRange>();

			LiveRange.AddRangeToList(ranges, new LiveRange(1, 3));
			Assert.Count(1, ranges);

			LiveRange.AddRangeToList(ranges, new LiveRange(2, 3));
			Assert.Count(1, ranges);

			LiveRange.AddRangeToList(ranges, new LiveRange(1, 2));
			Assert.Count(1, ranges);
		}

	}
}
