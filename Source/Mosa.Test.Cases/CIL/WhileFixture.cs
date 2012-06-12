/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phillip Webster (mincus) <phillmwebster@gmail.com>
 */

using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class WhileFixture : TestCompilerAdapter
	{

		public WhileFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		#region WhileIncI4 test
		// Tests basic increment loop

		[Row(0, 20)]
		[Row(-20, 0)]
		[Row(-100, 100)]
		[Test]
		public void WhileIncI4(int start, int limit)
		{
			Assert.AreEqual<int>(limit - start, Run<int>("Mosa.Test.Collection", "WhileTests", "WhileIncI4", start, limit));
		}

		#endregion

		#region WhileDecI4 test
		// Tests basic decrement loop.

		[Row(20, 0)]
		[Row(0, -20)]
		[Row(100, -100)]
		[Test]
		public void WhileDecI4(int start, int limit)
		{
			Assert.AreEqual<int>(start - limit, Run<int>("Mosa.Test.Collection", "WhileTests", "WhileDecI4", start, limit));
		}

		#endregion

		#region WhileFalse() test
		// Tests while(false)
		// Ensures "unreachable code" is never reached.

		[Row()]
		[Test]
		public void WhileFalse()
		{
			Assert.IsFalse(Run<bool>("Mosa.Test.Collection", "WhileTests", "WhileFalse"));
		}

		#endregion

		#region WhileContinueBreak() test

		// Tests while(true)
		// Tests break;
		// Tests continue;
		// Tests "unreachable code" after a continue or break is never reached.

		[Row()]
		[Test]
		public void WhileContinueBreak()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "WhileTests", "WhileContinueBreak"));
		}

		#endregion

		#region WhileOverflowIncI1 test

		// Tests overflowing a variable during an increment loop.

		[Row(254, 1)]
		[Row(byte.MaxValue, byte.MinValue)]
		[Test]
		public void WhileOverflowIncI1(byte start, byte limit)
		{
			int expect = (256 + (int)limit) - start;
			Assert.AreEqual<int>(expect, Run<int>("Mosa.Test.Collection", "WhileTests", "WhileOverflowIncI1", start, limit));
		}

		#endregion

		#region WhileOverflowDecI1 test

		// Tests overflowing a variable in decrement loops.

		[Row(1, 254)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void WhileOverflowDecI1(byte start, byte limit)
		{
			int expect = (256 + (int)start) - limit;
			Assert.AreEqual<int>(expect, Run<int>("Mosa.Test.Collection", "WhileTests", "WhileOverflowDecI1", start, limit));
		}

		#endregion

		#region WhileNestedEqualsI4 test

		// Tests nested looping (basic increment loop internally)
		// Tests == operator on external loop

		[Row(2, 3, 0, 20)]
		[Row(0, 1, 100, 200)]
		[Row(1, 0, -100, 100)]
		[Row(int.MaxValue, int.MinValue, -2, 3)]
		[Test]
		public void WhileNestedEqualsI4(int initialStatus, int wantedStatus, int start, int limit)
		{
			int count = limit - start;
			int expect = (int)((count * count) - ((count / 2.0f) * count) + (count / 2.0f));
			Assert.AreEqual<int>(expect, Run<int>("Mosa.Test.Collection", "WhileTests", "WhileNestedEqualsI4", initialStatus, wantedStatus, start, limit));
		}

		#endregion
	}
}
