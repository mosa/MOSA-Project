/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using MbUnit.Framework;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.MbUnit
{
	[TestFixture]
	public class SimplePriorityQueueTests
	{
		[Test]
		public void EnqueueDequeueTest()
		{
			var queue = new SimpleKeyPriorityQueue<int>();
			queue.Enqueue(21, 21);
			queue.Enqueue(100, 100);
			queue.Enqueue(3, 3);

			Assert.AreEqual(100, queue.Dequeue());
			Assert.AreEqual(21, queue.Dequeue());
			Assert.AreEqual(3, queue.Dequeue());
		}

		[Test]
		public void IsEmptyTest()
		{
			var queue = new SimpleKeyPriorityQueue<int>();

			Assert.IsTrue(queue.IsEmpty);
		}

		[Test]
		public void IsNotEmptyTest()
		{
			var queue = new SimpleKeyPriorityQueue<int>();
			queue.Enqueue(1, 1);

			Assert.IsFalse(queue.IsEmpty);
		}
	}
}