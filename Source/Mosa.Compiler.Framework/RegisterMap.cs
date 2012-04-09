/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// </summary>
	public class RegisterMap
	{
		#region Data members

		protected RegisterBitmap[] map;

		protected InstructionSet instructionSet;

		#endregion

		RegisterMap(InstructionSet instructionSet)
		{
			this.instructionSet = instructionSet;
			map = new RegisterBitmap[instructionSet.Size];
		}

		protected void CheckAndResize()
		{
			if (instructionSet.Size <= map.Length)
				return;

			RegisterBitmap[] newmap = new RegisterBitmap[instructionSet.Size];
			map.CopyTo(newmap, 0);
			map = newmap;
		}

		public void Set(int index, Register register)
		{
			map[index].Set(register);
		}

		public void Clear(int index, Register register)
		{
			map[index].Clear(register);
		}

		public void Not(int index)
		{
			map[index].Not();
		}

		public void And(int dest, int src)
		{
			map[dest].And(map[src]);
		}

		public void Or(int dest, int src)
		{
			map[dest].And(map[src]);
		}

		public void ClearAll(int index)
		{
			map[index].ClearAll();
		}

		public void SetAll(int index)
		{
			map[index].SetAll();
		}

	}
}
