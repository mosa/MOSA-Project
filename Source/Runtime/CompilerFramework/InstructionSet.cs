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
using System.Diagnostics;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Class maintenances an array of sorted instruction
	/// </summary>
	public sealed class InstructionSet
	{
		#region Data Members

		private const int MINSETUP = 16;

		/// <summary>
		/// 
		/// </summary>
		public InstructionData[] Data;
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
		/// <summary>
		/// 
		/// </summary>
		private int _low;
		/// <summary>
		/// 
		/// </summary>
		private int _allocated;

		#endregion // Data Members

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public int Size { get { return _allocated; } }

		/// <summary>
		/// 
		/// </summary>
		public int Used { get { return _used; } }

		#endregion

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="InstructionSet"/> class.
		/// </summary>
		/// <param name="size">The size.</param>
		public InstructionSet(int size)
		{
			_low = MINSETUP;
			_allocated = Math.Max(size, _low);

			_next = new int[_allocated];
			_prev = new int[_allocated];

			Data = new InstructionData[_allocated];

			Clear();
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_used = 0;
			_free = 0;
			_low = MINSETUP;

			// setup free list
			for (int i = 0; i < _low; i++) {
				_next[i] = i + 1;
				_prev[i] = i - 1;	// might not be necessary
			}

			_prev[0] = -1;
			_next[_low - 1] = -1;
		}

		/// <summary>
		/// Expands this instance.
		/// </summary>
		public int Expand()
		{
			if (_low == _allocated)
				Resize(_allocated * 2);

			return _low++;
		}

		/// <summary>
		/// Resizes the specified newsize.
		/// </summary>
		/// <param name="newsize">The newsize.</param>
		public void Resize(int newsize)
		{
			_allocated = newsize;

			int[] newNext = new int[_allocated];
			int[] newPrev = new int[_allocated];
			InstructionData[] newInstructions = new InstructionData[_allocated];

			_next.CopyTo(newNext, 0);
			_prev.CopyTo(newPrev, 0);

			Data.CopyTo(newInstructions, 0);
			_next = newNext;
			_prev = newPrev;
			Data = newInstructions;
		}

		/// <summary>
		/// Nexts the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public int Next(int index)
		{
			Debug.Assert(index < _low);
			Debug.Assert(index >= 0);

			if (_used == 0)
				return -1;

			return _next[index];
		}

		/// <summary>
		/// Previouses the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public int Previous(int index)
		{
			Debug.Assert(index < _low);
			Debug.Assert(index >= 0);

			if (_used == 0)
				return -1;

			return _prev[index];
		}

		/// <summary>
		/// Gets the free.
		/// </summary>
		/// <returns></returns>
		public int GetFree()
		{
			int free;

			if (_free == -1)
				free = Expand();
			else {
				free = _free;
				_free = _next[free];
			}

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
		/// Inserts the after.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
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
		/// Inserts the after.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public int InsertBefore(int index)
		{
			if (index == -1)
				return CreateRoot();

			// FIXME: Asserts without a message or a comment are not useful to figure out.
			// I don't see a reason this should not be valid??
			//Debug.Assert(index > 0);

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
			if (_next[index] < 0) {
				if (_prev[index] >= 0)
					_next[_prev[index]] = -1;
				AddFree(index);
				return;
			}
			if (_prev[index] < 0) {
				_prev[_next[index]] = -1;
				AddFree(index);
				return;
			}

			_next[_prev[index]] = _next[index];
			_prev[_next[index]] = _prev[index];

			AddFree(index);
		}


		/// <summary>
		/// Slices the instruction flow before the current instruction
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
		/// Slices the instruction flow after the current instruction
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
