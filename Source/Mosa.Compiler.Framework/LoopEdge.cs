/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{

	/// <summary>
	/// Loop
	/// </summary>
	public struct LoopEdge
	{
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock tail;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock head;

		/// <summary>
		/// Current Block
		/// </summary>
		public BasicBlock Tail { get { return tail; } }
		/// <summary>
		/// Successor Block
		/// </summary>
		public BasicBlock Head { get { return head; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="LoopEdge"/> class.
		/// </summary>
		/// <param name="head">From block.</param>
		/// <param name="tail">To block.</param>
		public LoopEdge(BasicBlock head, BasicBlock tail)
		{
			this.head = head;
			this.tail = tail;
		}
	}
}

