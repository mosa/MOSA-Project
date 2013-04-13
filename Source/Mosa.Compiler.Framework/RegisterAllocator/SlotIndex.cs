/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class SlotIndex : IComparable
	{
		private InstructionSet instructionSet;

		private int index;

		public SlotIndex(InstructionSet instructionSet, int index)
		{
			Debug.Assert(index >= 0);

			this.instructionSet = instructionSet;
			this.index = index;
		}

		public SlotIndex(Context context)
		{
			Debug.Assert(context.Index >= 0);

			this.instructionSet = context.InstructionSet;
			this.index = context.Index;
		}

		//public int Index { get { return index; } }

		public int SlotNumber { get { return instructionSet.Data[index].SlotNumber; } }

		public static int operator -(SlotIndex s1, SlotIndex s2)
		{
			return s1.SlotNumber - s2.SlotNumber;
		}

		public static bool operator >=(SlotIndex s1, SlotIndex s2)
		{
			return s1.SlotNumber >= s2.SlotNumber;
		}

		public static bool operator <=(SlotIndex s1, SlotIndex s2)
		{
			return s1.SlotNumber <= s2.SlotNumber;
		}

		public static bool operator >(SlotIndex s1, SlotIndex s2)
		{
			return s1.SlotNumber > s2.SlotNumber;
		}

		public static bool operator <(SlotIndex s1, SlotIndex s2)
		{
			return s1.SlotNumber < s2.SlotNumber;
		}

		public static bool operator ==(SlotIndex s1, SlotIndex s2)
		{
			bool ns1 = object.ReferenceEquals(null, s1);
			bool ns2 = object.ReferenceEquals(null, s2);

			if (ns1 && ns2)
				return true;
			else if ((ns1 && !ns2) || (!ns1 && ns2))
				return false;

			return s1.SlotNumber == s2.SlotNumber;
		}

		public static bool operator !=(SlotIndex s1, SlotIndex s2)
		{
			bool ns1 = object.ReferenceEquals(null, s1);
			bool ns2 = object.ReferenceEquals(null, s2);

			if (ns1 && ns2)
				return false;
			else if ((ns1 && !ns2) || (!ns1 && ns2))
				return true;

			return s1.SlotNumber != s2.SlotNumber;
		}

		public override bool Equals(object s)
		{
			if (!(s is SlotIndex))
				return false;

			return ((s as SlotIndex).index == index);
		}

		public override int GetHashCode()
		{
			return SlotNumber;
		}

		int IComparable.CompareTo(Object o)
		{
			SlotIndex slotIndex = o as SlotIndex;

			return (SlotNumber - slotIndex.SlotNumber);
		}

		public override string ToString()
		{
			return SlotNumber.ToString();
		}

		public SlotIndex Next
		{
			get
			{
				int next = instructionSet.Next(index);

				while (instructionSet.Data[next].Instruction == null)
				{
					next = instructionSet.Next(next);
				}

				return new SlotIndex(instructionSet, next);
			}
		}

		public SlotIndex Previous
		{
			get
			{
				int previous = instructionSet.Previous(index);

				while (instructionSet.Data[previous].Instruction == null)
				{
					previous = instructionSet.Previous(previous);
				}

				return new SlotIndex(instructionSet, previous);
			}
		}

		public bool IsBlockStartInstruction
		{
			get
			{
				return instructionSet.Data[index].Instruction is IR.BlockStart;
			}
		}

		public bool IsBlockEndInstruction
		{
			get
			{
				return instructionSet.Data[index].Instruction is IR.BlockEnd;
			}
		}
	}
}