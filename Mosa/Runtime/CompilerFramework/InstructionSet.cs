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
	public class InstructionSet
	{
		#region Data Members

		/// <summary>
		/// 
		/// </summary>
		public InstructionData[] instructions;

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

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="InstructionSet"/> class.
		/// </summary>
		/// <param name="size">The size.</param>
		public InstructionSet(int size)
		{
			_size = size;
			_next = new int[size];
			_prev = new int[size];
			instructions = new InstructionData[size];
			Clear();
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_used = 0;

			// setup free list
			for (int i = 0; i < _size; i++) {
				_next[i] = i + 1;
				_prev[i] = i - 1;	// might not be necessary
			}

			_prev[0] = -1;
			_next[_size - 1] = -1;
		}

		/// <summary>
		/// Resizes the specified newsize.
		/// </summary>
		/// <param name="newsize">The newsize.</param>
		private void Resize(int newsize)
		{
			int[] newNext = new int[newsize];
			int[] newPrev = new int[newsize];
			InstructionData[] newInstructions = new InstructionData[newsize];

			_next.CopyTo(newNext, 0);
			_prev.CopyTo(newPrev, 0);
			instructions.CopyTo(newInstructions, 0);

			_next = newNext;
			_prev = newPrev;
			instructions = newInstructions;
			_size = newsize;
		}

		/// <summary>
		/// Nexts the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public int Next(int index)
		{
			Debug.Assert(index < _size);
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
			Debug.Assert(index < _size);
			Debug.Assert(index >= 0);

			if (_used == 0)
				return -1;

			return _prev[index];
		}

		/// <summary>
		/// Gets the free.
		/// </summary>
		/// <returns></returns>
		private int GetFree()
		{
			// TODO: Resize array
			Debug.Assert(_used != _size, "array too small");

			int free = _free;

			_free = _next[free];
			//_prev[_free] = -1;
			instructions[free].Ignore = true;
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

			Debug.Assert(index > 0);

			int free = GetFree();

			// setup new node's prev/next
			_next[free] = _next[free];
			_prev[free] = index;

			_next[index] = free;

			if (_next[free] >= 0)
				_prev[_next[free]] = free;

			return -1;
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
			else
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
		/// Slices the instruction flow.
		/// </summary>
		/// <param name="index">The index.</param>
		public void SliceAfter(int index)
		{
			_prev[_next[index]] = -1;
			_next[index] = -1;
		}

		#endregion // Methods

		#region Utility Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="prefix"></param>
		public void SetPrefix(int index, CIL.PrefixInstruction prefix)
		{
			IsIndexValid(index);
			instructions[index].Prefix = prefix;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="offset"></param>
		public void SetOffset(int index, int offset)
		{
			IsIndexValid(index);
			instructions[index].Offset = offset;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void IsIndexValid(int index)
		{
			Debug.Assert(index < _size);
			Debug.Assert(index >= 0);
		}
		#endregion
	}
}
