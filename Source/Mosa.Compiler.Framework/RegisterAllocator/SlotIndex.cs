/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class SlotIndex : IComparable
	{
		private enum SlotType { Normal, HalfStepBack, HalfStepForward };

		public const int Increment = 2;

		public readonly InstructionNode Index;

		private SlotType slotType;

		public int SlotNumber
		{
			get
			{
				int slot = Index.SlotNumber;

				if (slotType == SlotType.HalfStepForward)
					slot++;
				else if (slotType == SlotType.HalfStepBack)
					slot--;

				return slot;
			}
		}

		public bool IsOnHalfStep { get { return slotType != SlotType.Normal; } }

		public bool IsOnHalfStepForward { get { return slotType == SlotType.HalfStepForward; } }

		public bool IsOnHalfStepBack { get { return slotType == SlotType.HalfStepBack; } }

		private SlotIndex(InstructionNode index, SlotType slotType)
		{
			this.Index = index;
			this.slotType = slotType;
		}

		public SlotIndex(InstructionNode index)
			: this(index, SlotType.Normal)
		{
		}

		public SlotIndex(Context context)
			: this(context.Node, SlotType.Normal)
		{
		}

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

			return ((s as SlotIndex).Index == Index);
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

		public SlotIndex HalfStepForward
		{
			get
			{
				Debug.Assert(slotType == SlotType.Normal);
				return new SlotIndex(Index, SlotType.HalfStepForward);
			}
		}

		public SlotIndex HalfStepBack
		{
			get
			{
				Debug.Assert(slotType == SlotType.Normal);
				return new SlotIndex(Index, SlotType.HalfStepBack);
			}
		}

		public bool IsBlockStartInstruction
		{
			get
			{
				if (slotType != SlotType.Normal)
					return false;

				return Index.Instruction == IRInstruction.BlockStart;
			}
		}

		public bool IsBlockEndInstruction
		{
			get
			{
				if (slotType != SlotType.Normal)
					return false;

				return Index.Instruction == IRInstruction.BlockEnd;
			}
		}
	}
}
