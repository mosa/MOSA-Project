// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class SlotIndex : IComparable
	{
		public const int Increment = 2;

		private enum SlotType { Normal, HalfStepBack, HalfStepForward }

		public readonly InstructionNode Node;

		private readonly SlotType slotType;

		public int SlotNumber
		{
			get
			{
				int slot = Node.Offset;

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

		private SlotIndex(InstructionNode node, SlotType slotType)
		{
			Node = node;
			this.slotType = slotType;
		}

		public SlotIndex(InstructionNode node)
			: this(node, SlotType.Normal)
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

		public SlotIndex HalfStepForward
		{
			get
			{
				Debug.Assert(slotType == SlotType.Normal);
				return new SlotIndex(Node, SlotType.HalfStepForward);
			}
		}

		public SlotIndex HalfStepBack
		{
			get
			{
				Debug.Assert(slotType == SlotType.Normal);
				return new SlotIndex(Node, SlotType.HalfStepBack);
			}
		}

		public bool IsBlockStartInstruction
		{
			get
			{
				if (slotType != SlotType.Normal)
					return false;

				return Node.Instruction == IRInstruction.BlockStart;
			}
		}

		public bool IsBlockEndInstruction
		{
			get
			{
				if (slotType != SlotType.Normal)
					return false;

				return Node.Instruction == IRInstruction.BlockEnd;
			}
		}
	}
}
