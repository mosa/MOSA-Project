/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Class maintenances an array of sorted instruction
	/// </summary>
	public sealed class InstructionSet
	{
		#region Data Members

		/// <summary>
		/// 
		/// </summary>
		public InstructionData[] Data;

		/// <summary>
		/// 
		/// </summary>
		private int size;
		/// <summary>
		/// 
		/// </summary>
		private int[] next;
		/// <summary>
		/// 
		/// </summary>
		private int[] prev;
		/// <summary>
		/// 
		/// </summary>
		private int used;
		/// <summary>
		/// 
		/// </summary>
		private int free;

		#endregion // Data Members

		#region Properties

		/// <summary>
		/// Gets the size.
		/// </summary>
		public int Size { get { return size; } }

		/// <summary>
		/// Gets the amount of used indices.
		/// </summary>
		public int Used { get { return used; } }

		/// <summary>
		/// Gets an array that maps each index to its corresponding next index.
		/// </summary>
		public int[] NextArray { get { return next; } }

		/// <summary>
		/// Gets an array that maps each index to its corresponding previous index.
		/// </summary>
		public int[] PrevArray { get { return prev; } }

		#endregion

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="InstructionSet"/> class.
		/// </summary>
		/// <param name="size">The size.</param>
		public InstructionSet(int size)
		{
			this.size = size;
			this.next = new int[size];
			this.prev = new int[size + 2];
			Data = new InstructionData[size];
			Clear();
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			used = 0;
			free = 0;

			// setup free list
			for (int i = 0; i < size; i++)
			{
				next[i] = prev[i + 2] = i + 1;
			}

			prev[0] = -1;
			prev[1] = 0;

			next[size - 1] = -1;
		}

		/// <summary>
		/// Resizes the instruction set.
		/// </summary>
		/// <param name="newsize">The new size.</param>
		public void Resize(int newsize)
		{
			int[] newNext = new int[newsize];
			int[] newPrev = new int[newsize + 2];
			InstructionData[] newInstructions = new InstructionData[newsize];

			next.CopyTo(newNext, 0);
			prev.CopyTo(newPrev, 0);

			for (int i = size; i < newsize; ++i)
			{
				newNext[i] = i + 1;
				newPrev[i] = i - 1;
			}
			newNext[newsize - 1] = -1;
			newPrev[size] = -1;
			Data.CopyTo(newInstructions, 0);

			free = size;
			next = newNext;
			prev = newPrev;
			size = newsize;
			Data = newInstructions;
		}

		/// <summary>
		/// Gets the next index after the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The next index, or -1.</returns>
		public int Next(int index)
		{
			Debug.Assert(index < size);
			Debug.Assert(index >= 0);

			if (used == 0)
				return -1;

			return next[index];
		}

		/// <summary>
		/// Gets the previous index before the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The previous index, or -1.</returns>
		public int Previous(int index)
		{
			Debug.Assert(index < size);
			Debug.Assert(index >= 0);

			if (used == 0)
				return -1;

			return prev[index];
		}

		/// <summary>
		/// Gets the next free index.
		/// </summary>
		/// <returns></returns>
		public int GetFree()
		{
			//	if (_used + 1 == _size)
			if (free == -1)
				Resize(size * 2);

			int beforeFree = free;

			free = next[beforeFree];
			//_prev[_free] = -1;
			Data[beforeFree].Instruction = null;
			used++;

			return beforeFree;
		}

		private void AddFree(int index)
		{
			next[index] = free;
			prev[index] = -1;
			//_prev[_free] = index;
			free = index;
			used--;
		}

		/// <summary>
		/// Creates the root.
		/// </summary>
		/// <returns></returns>
		public int CreateRoot()
		{
			int free = GetFree();

			next[free] = -1;
			prev[free] = -1;

			return free;
		}

		/// <summary>
		/// Inserts a node after a specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The inserted index.</returns>
		public int InsertAfter(int index)
		{
			if (index == -1)
				return CreateRoot();

			Debug.Assert(index >= 0);

			int free = GetFree();

			// setup new node's prev/next
			next[free] = next[index];
			prev[free] = index;

			next[index] = free;

			if (next[free] >= 0)
				prev[next[free]] = free;

			return free;
		}

		/// <summary>
		/// Inserts a node before a specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The inserted index.</returns>
		public int InsertBefore(int index)
		{
			if (index == -1)
				return CreateRoot();

			int free = GetFree();

			// setup new node's prev/next
			next[free] = index;
			prev[free] = prev[index];

			if (prev[index] >= 0)
				next[prev[index]] = free;

			prev[index] = free;

			return free;
		}

		/// <summary>
		/// Removes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void Remove(int index)
		{
			if (next[index] < 0)
			{
				if (prev[index] >= 0)
					next[prev[index]] = -1;
				AddFree(index);
				return;
			}
			if (prev[index] < 0)
			{
				prev[next[index]] = -1;
				AddFree(index);
				return;
			}

			next[prev[index]] = next[index];
			prev[next[index]] = prev[index];

			AddFree(index);
		}


		/// <summary>
		/// Slices the instruction flow before the current instruction.
		/// </summary>
		/// <param name="index">The index.</param>
		public void SliceBefore(int index)
		{
			if (prev[index] == -1)
				return;

			next[prev[index]] = -1;
			prev[index] = -1;
		}

		/// <summary>
		/// Slices the instruction flow after the current instruction.
		/// </summary>
		/// <param name="index">The index.</param>
		public void SliceAfter(int index)
		{
			if (next[index] == -1)
				return;

			prev[next[index]] = -1;
			next[index] = -1;
		}

		#endregion // Methods

	}
}
