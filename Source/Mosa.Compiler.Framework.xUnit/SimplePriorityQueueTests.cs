/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit
{
	public class SimplePriorityQueueTests
	{
		[Fact]
		public void EnqueueDequeueTest()
		{
			var queue = new SimpleKeyPriorityQueue<int>();
			queue.Enqueue(21, 21);
			queue.Enqueue(100, 100);
			queue.Enqueue(3, 3);

			Assert.Equal(100, queue.Dequeue());
			Assert.Equal(21, queue.Dequeue());
			Assert.Equal(3, queue.Dequeue());
		}

		[Fact]
		public void IsEmptyTest()
		{
			var queue = new SimpleKeyPriorityQueue<int>();

			Assert.True(queue.IsEmpty);
		}

		[Fact]
		public void IsNotEmptyTest()
		{
			var queue = new SimpleKeyPriorityQueue<int>();
			queue.Enqueue(1, 1);

			Assert.False(queue.IsEmpty);
		}
	}
}