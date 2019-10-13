// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class LiveRange
	{
		// Live ranges includes:
		// 1. Use > start && Use <= end
		// 2. Def >= start && Def < end

		// Notes:
		// Uses can not be at the start of a live range
		// Uses can end at the end of a live range
		// Defs can be at the start of a live range
		// Defs can not end at the end of a live range

		private readonly VirtualRegister VirtualRegister;

		private readonly int StartIndex;
		private readonly int EndIndex;

		private readonly int FirstUseIndex;
		private readonly int LastUseIndex;

		private readonly int FirstDefIndex;
		private readonly int LastDefIndex;

		public readonly SlotIndex Start;
		public readonly SlotIndex End;

		public readonly bool IsEmpty;

		public readonly SlotIndex First;
		public readonly SlotIndex Last;

		public readonly SlotIndex FirstUse;
		public readonly SlotIndex FirstDef;

		public readonly SlotIndex LastUse;
		public readonly SlotIndex LastDef;

		public readonly int UseCount;

		public readonly int DefCount;

		public readonly bool IsDefFirst;

		public LiveRange(SlotIndex start, SlotIndex end, VirtualRegister virtualRegister, int startIndex = 0, int endIndex = Int32.MaxValue)
		{
			VirtualRegister = virtualRegister;
			Start = start;
			End = end;

			if (virtualRegister.IsPhysicalRegister)
			{
				return;
			}

			int lastUseIndex = -1;
			int firstUseIndex = -1;
			int lastDefIndex = -1;
			int firstDefIndex = -1;
			int useCount = 0;
			int defCount = 0;

			for (int i = startIndex; i < virtualRegister.UsePositions.Count && i <= endIndex; i++)
			{
				var use = virtualRegister.UsePositions[i];

				if (use > end)
					break;

				if (use > start) // && (use <= end)
				{
					if (firstUseIndex < 0)
					{
						firstUseIndex = i;
					}
					useCount++;
					lastUseIndex = i;
				}
			}

			for (int i = startIndex; i < virtualRegister.DefPositions.Count && i <= endIndex; i++)
			{
				var def = virtualRegister.DefPositions[i];

				if (def >= end)
					break;

				if (def >= start) // && (def < end)
				{
					if (firstDefIndex < 0)
					{
						firstDefIndex = i;
					}
					defCount++;
					lastDefIndex = i;
				}
			}

			StartIndex = LowestGreaterThanZero(firstUseIndex, firstDefIndex);
			EndIndex = Math.Max(lastUseIndex, lastDefIndex);

			FirstUseIndex = firstUseIndex;
			LastUseIndex = lastUseIndex;

			FirstDefIndex = firstDefIndex;
			LastDefIndex = lastDefIndex;

			UseCount = useCount;
			DefCount = defCount;

			IsEmpty = useCount + defCount == 0;

			FirstUse = useCount == 0 ? SlotIndex.NullSlot : VirtualRegister.UsePositions[firstUseIndex];
			FirstDef = defCount == 0 ? SlotIndex.NullSlot : VirtualRegister.DefPositions[firstDefIndex];
			LastUse = useCount == 0 ? SlotIndex.NullSlot : VirtualRegister.UsePositions[lastUseIndex];
			LastDef = defCount == 0 ? SlotIndex.NullSlot : VirtualRegister.DefPositions[lastDefIndex];

			First = (useCount == 0 || firstDefIndex < firstUseIndex) ? FirstDef : FirstUse;
			Last = (useCount == 0 || lastDefIndex > lastUseIndex) ? LastDef : LastUse;

			IsDefFirst = defCount != 0 && (useCount == 0 || FirstDefIndex < FirstUseIndex);

			//Debug.Assert(InternalValidation());
		}

		private bool InternalValidation()
		{
			foreach (var use in UsePositions)
			{
				if (!ContainUse(use))
					return false;

				if (UseCount == 0)
					return false;
			}

			foreach (var def in DefPositions)
			{
				if (!ContainDef(def))
					return false;

				if (DefCount == 0)
					return false;
			}

			if (FirstUse.IsNotNull && !ContainUse(FirstUse))
				return false;

			if (LastUse.IsNotNull && !ContainUse(LastUse))
				return false;

			if (FirstDef.IsNotNull && !ContainDef(FirstDef))
				return false;

			if (LastDef.IsNotNull && !ContainDef(LastDef))
				return false;

			return true;
		}

		private static int LowestGreaterThanZero(int a, int b)
		{
			if (a < 0)
				return b;

			if (b < 0)
				return a;

			if (a > b)
				return b;
			else
				return a;
		}

		public bool ContainUse(SlotIndex at)
		{
			if (!at.IsOnSlot)
				return false;

			if (UseCount == 0 || at < FirstUse || at > LastUse)
				return false;

			for (int i = FirstUseIndex; i <= LastUseIndex; i++)
			{
				var use = VirtualRegister.UsePositions[i];

				if (at == use)
					return true;

				if (use > at)   // list is sorted, so fast out
					return false;
			}

			return false;
		}

		public bool ContainDef(SlotIndex at)
		{
			if (!at.IsOnSlot)
				return false;

			if (DefCount == 0 || at < FirstDef || at > LastDef)
				return false;

			for (int i = FirstDefIndex; i <= LastDefIndex; i++)
			{
				var def = VirtualRegister.DefPositions[i];

				if (at == def)
					return true;

				if (def > at)   // list is sorted, so fast out
					return false;
			}

			return false;
		}

		public SlotIndex GetNextUsePosition(SlotIndex at)
		{
			if (UseCount == 0 || at < Start || at > LastUse) // || at > End
				return SlotIndex.NullSlot;

			for (int i = FirstUseIndex; i <= LastUseIndex; i++)
			{
				var use = VirtualRegister.UsePositions[i];

				if (use > at)
					return use;
			}

			return SlotIndex.NullSlot;
		}

		public SlotIndex GetNextDefPosition(SlotIndex at)
		{
			if (DefCount == 0 || at < Start || at > LastDef) // || at > End
				return SlotIndex.NullSlot;

			for (int i = FirstDefIndex; i <= LastDefIndex; i++)
			{
				var def = VirtualRegister.DefPositions[i];

				if (def > at)
					return def;
			}

			return SlotIndex.NullSlot;
		}

		public SlotIndex GetPreviousUsePosition(SlotIndex at)
		{
			if (UseCount == 0 || at < Start || at < FirstUse) // || at > End
				return SlotIndex.NullSlot;

			for (int i = LastUseIndex; i >= FirstUseIndex; i--)
			{
				var use = VirtualRegister.UsePositions[i];

				if (use < at)
					return use;
			}

			return SlotIndex.NullSlot;
		}

		public SlotIndex GetPreviousDefPosition(SlotIndex at)
		{
			if (DefCount == 0 || at < Start || at < FirstDef)  // || at > End
				return SlotIndex.NullSlot;

			for (int i = LastDefIndex; i >= FirstDefIndex; i--)
			{
				var def = VirtualRegister.DefPositions[i];

				if (def < at)
					return def;
			}

			return SlotIndex.NullSlot;
		}

		public bool CanSplitAt(SlotIndex at)
		{
			if (at <= Start || at >= End)
				return false;

			return !ContainUse(at);
		}

		public List<LiveRange> SplitAt(SlotIndex at)
		{
//			if (!CanSplitAt(at))
//				throw new CompilerException($"Can not split at {at}");

			Debug.Assert(CanSplitAt(at));

			// FUTURE: Optimize below --

			return new List<LiveRange>(2)
			{
				new LiveRange(Start, at, VirtualRegister, StartIndex, EndIndex),
				new LiveRange(at, End, VirtualRegister, StartIndex, EndIndex)
			};
		}

		public bool CanSplitAt(SlotIndex low, SlotIndex high)
		{
			//return CanSplitAt(low) && CanSplitAt(high); // slightly slower version

			if (low <= Start || low >= End)
				return false;

			if (high <= Start || high >= End)
				return false;

			if (low >= high)
				return false;

			if (ContainUse(low))
				return false;

			if (ContainUse(high))
				return false;

			return true;
		}

		public List<LiveRange> SplitAt(SlotIndex low, SlotIndex high)
		{
			if (low == high)
			{
				return SplitAt(low);
			}

			Debug.Assert(CanSplitAt(low, high));

			// FUTURE: Optimize below --

			return new List<LiveRange>(3)
			{
				new LiveRange(Start, low,  VirtualRegister, StartIndex, EndIndex),
				new LiveRange(low, high,  VirtualRegister, StartIndex, EndIndex),
				new LiveRange(high, End,  VirtualRegister, StartIndex, EndIndex)
			};
		}

		public IEnumerable<SlotIndex> UsePositions
		{
			get
			{
				if (UseCount == 0)
					yield break;

				for (int i = FirstUseIndex; i <= LastUseIndex; i++)
				{
					yield return VirtualRegister.UsePositions[i];
				}
			}
		}

		public IEnumerable<SlotIndex> DefPositions
		{
			get
			{
				if (DefCount == 0)
					yield break;

				for (int i = FirstDefIndex; i <= LastDefIndex; i++)
				{
					yield return VirtualRegister.DefPositions[i];
				}
			}
		}

		public override string ToString()
		{
			return $"({Start} to {End})";
		}
	}
}
