/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class SlotIndex : IComparable 
	{

		private InstructionSet instructionSet;

		private int index;

		public SlotIndex(InstructionSet instructionSet, int index)
		{
			this.instructionSet = instructionSet;
			this.index = index;
		}

		public SlotIndex(Context context)
		{
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

			return (index - slotIndex.index);
		}

		public override string ToString()
		{
			return SlotNumber.ToString();
		}

	}
}