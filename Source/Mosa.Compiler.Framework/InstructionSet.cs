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
		private int _size;
		/// <summary>
		/// 
		/// </summary>
		private int[] _next;
		/// <summary>
		/// 
		/// </summary>
		private int[] _prev;
		/// <summary>
		/// 
		/// </summary>
		private int _used;
		/// <summary>
		/// 
		/// </summary>
		private int _free;

		#endregion // Data Members

		#region Properties

		/// <summary>
		/// Gets the size.
		/// </summary>
		public int Size
		{
			get
			{
				return _size;
			}
		}

		/// <summary>
		/// Gets the amount of used indices.
		/// </summary>
		public int Used
		{
			get
			{
				return _used;
			}
		}

		/// <summary>
		/// Gets an array that maps each index to its corresponding next index.
		/// </summary>
		public int[] NextArray
		{
			get
			{
				return _next;
			}
		}

		/// <summary>
		/// Gets an array that maps each index to its corresponding previous index.
		/// </summary>
		public int[] PrevArray
		{
			get
			{
				return _prev;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="InstructionSet"/> class.
		/// </summary>
		/// <param name="size">The size.</param>
		public InstructionSet(int size)
		{
			_size = size;
			_next = new int[size];
			_prev = new int[size + 2];
			Data = new InstructionData[size];
			Clear();
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_used = 0;
			_free = 0;

			// setup free list
			for (int i = 0; i < _size; i++)
			{
				_next[i] = _prev[i + 2] = i + 1;
			}

			_prev[0] = -1;
			_prev[1] = 0;

			_next[_size - 1] = -1;
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

			_next.CopyTo(newNext, 0);
			_prev.CopyTo(newPrev, 0);

			for (int i = _size; i < newsize; ++i)
			{
				newNext[i] = i + 1;
				newPrev[i] = i - 1;
			}
			newNext[newsize - 1] = -1;
			newPrev[_size] = -1;
			Data.CopyTo(newInstructions, 0);

			_free = _size;
			_next = newNext;
			_prev = newPrev;
			_size = newsize;
			Data = newInstructions;
		}

		/// <summary>
		/// Gets the next index after the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The next index, or -1.</returns>
		public int Next(int index)
		{
			Debug.Assert(index < _size);
			Debug.Assert(index >= 0);

			if (_used == 0)
				return -1;

			return _next[index];
		}

		/// <summary>
		/// Gets the previous index before the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The previous index, or -1.</returns>
		public int Previous(int index)
		{
			Debug.Assert(index < _size);
			Debug.Assert(index >= 0);

			if (_used == 0)
				return -1;

			return _prev[index];
		}

		/// <summary>
		/// Gets the next free index.
		/// </summary>
		/// <returns></returns>
		public int GetFree()
		{
			//	if (_used + 1 == _size)
			if (_free == -1)
				Resize(_size * 2);

			int free = _free;

			_free = _next[free];
			//_prev[_free] = -1;
			Data[free].Ignore = true;
			_used++;

			return free;
		}

		private void AddFree(int index)
		{
			_next[index] = _free;
			_prev[index] = -1;
			//_prev[_free] = index;
			_free = index;
			_used--;
		}

		/// <summary>
		/// Creates the root.
		/// </summary>
		/// <returns></returns>
		public int CreateRoot()
		{
			int free = GetFree();

			_next[free] = -1;
			_prev[free] = -1;

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
			_next[free] = _next[index];
			_prev[free] = index;

			_next[index] = free;

			if (_next[free] >= 0)
				_prev[_next[free]] = free;

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
			_next[free] = index;
			_prev[free] = _prev[index];

			if (_prev[index] >= 0)
				_next[_prev[index]] = free;

			_prev[index] = free;

			return free;
		}

		/// <summary>
		/// Removes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void Remove(int index)
		{
			if (_next[index] < 0)
			{
				if (_prev[index] >= 0)
					_next[_prev[index]] = -1;
				AddFree(index);
				return;
			}
			if (_prev[index] < 0)
			{
				_prev[_next[index]] = -1;
				AddFree(index);
				return;
			}

			_next[_prev[index]] = _next[index];
			_prev[_next[index]] = _prev[index];

			AddFree(index);
		}


		/// <summary>
		/// Slices the instruction flow before the current instruction.
		/// </summary>
		/// <param name="index">The index.</param>
		public void SliceBefore(int index)
		{
			if (_prev[index] == -1)
				return;

			_next[_prev[index]] = -1;
			_prev[index] = -1;
		}

		/// <summary>
		/// Slices the instruction flow after the current instruction.
		/// </summary>
		/// <param name="index">The index.</param>
		public void SliceAfter(int index)
		{
			if (_next[index] == -1)
				return;

			_prev[_next[index]] = -1;
			_next[index] = -1;
		}

		#endregion // Methods

	}
}
