// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class SlotIndex : IComparable
	{
		// FUTURE:
		//		Convert SlotIndex into struct with internal integer value
		//		InstructionNode is removed (another data structure with the allocator maps index values to slots)
		//		The slot type is embedded into the internal value
		//		Index is the internal value with the slot type masked out
		//		See Slot.cs for future replacement
		// RATIONAL:
		//		1. No object creation
		//		2. Smaller memory footprint
		//		3. Fits into a single register
		//		4. Much faster to use

		public const int Increment = 2;

		private enum SlotType { On, Before, After }

		public readonly InstructionNode Node;

		private readonly SlotType slotType;

		public int SlotNumber
		{
			get
			{
				int slot = Node.Offset;

				if (slotType == SlotType.After)
					slot++;
				else if (slotType == SlotType.Before)
					slot--;

				return slot;
			}
		}

		public bool IsOnSlot { get { return slotType != SlotType.On; } }

		public bool IsAfterSlot { get { return slotType == SlotType.After; } }

		public bool IsBeforeSlot { get { return slotType == SlotType.Before; } }

		private SlotIndex(InstructionNode node, SlotType slotType)
		{
			Node = node;
			this.slotType = slotType;
		}

		public SlotIndex(InstructionNode node)
			: this(node, SlotType.On)
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
			bool ns1 = ReferenceEquals(null, s1);
			bool ns2 = ReferenceEquals(null, s2);

			if (ns1 && ns2)
				return true;
			else if (ns1 ^ ns2)
				return false;

			// FUTURE: compare slot type too!

			return s1.SlotNumber == s2.SlotNumber;
		}

		public static bool operator !=(SlotIndex s1, SlotIndex s2)
		{
			bool ns1 = ReferenceEquals(null, s1);
			bool ns2 = ReferenceEquals(null, s2);

			if (ns1 && ns2)
				return false;
			else if (ns1 ^ ns2)
				return true;

			return s1.SlotNumber != s2.SlotNumber;
		}

		public override bool Equals(object obj)
		{
			var o = obj as SlotIndex;

			if (o == null)
				return false;

			return o.Node == Node;
		}

		public override int GetHashCode()
		{
			return SlotNumber;
		}

		int IComparable.CompareTo(Object obj)
		{
			var slotIndex = obj as SlotIndex;

			return SlotNumber - slotIndex.SlotNumber;
		}

		public override string ToString()
		{
			return SlotNumber.ToString();
		}

		public SlotIndex GetSlotAfter()
		{
			Debug.Assert(slotType == SlotType.On);
			return new SlotIndex(Node, SlotType.After);
		}

		public SlotIndex GetSlotBefore()
		{
			Debug.Assert(slotType == SlotType.On);
			return new SlotIndex(Node, SlotType.Before);
		}

		public bool IsBlockStartInstruction
		{
			get
			{
				if (slotType != SlotType.On)
					return false;

				return Node.Instruction == IRInstruction.BlockStart;
			}
		}

		public bool IsBlockEndInstruction
		{
			get
			{
				if (slotType != SlotType.On)
					return false;

				return Node.Instruction == IRInstruction.BlockEnd;
			}
		}
	}
}
