/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class VirtualRegister
	{
		private List<LiveInterval> liveIntervals = new List<LiveInterval>(1);
		private List<int> usePositions = new List<int>();

		public Operand VirtualRegisterOperand { get; private set; }

		public Register PhysicalRegister { get; private set; }

		public bool IsPhysicalRegister { get { return VirtualRegisterOperand == null; } }

		public List<LiveInterval> LiveIntervals { get { return liveIntervals; } }

		public int Count { get { return liveIntervals.Count; } }

		public List<int> UsePositions { get { return usePositions; } }

		public LiveInterval LastRange { get { return liveIntervals.Count == 0 ? null : liveIntervals[liveIntervals.Count - 1]; } }

		public LiveInterval FirstRange { get { return liveIntervals.Count == 0 ? null : liveIntervals[0]; } }

		public int SpillSlot { get; set; }

		public bool IsFloatingPoint { get { return VirtualRegisterOperand.IsFloatingPoint; } }

		public VirtualRegister(Operand virtualRegister)
		{
			this.VirtualRegisterOperand = virtualRegister;
		}

		public VirtualRegister(Register physicalRegister)
		{
			this.PhysicalRegister = physicalRegister;
		}

		public void AddUsePosition(int position)
		{
			Debug.Assert(!usePositions.Contains(position));
			usePositions.Add(position);
		}

		public void AddRange(int start, int end)
		{
			if (liveIntervals.Count == 0)
			{
				liveIntervals.Add(new LiveInterval(this, start, end));
				return;
			}

			for (int i = 0; i < liveIntervals.Count; i++)
			{
				var liveRange = liveIntervals[i];

				if (liveRange.IsSame(start, end))
					return;

				if (liveRange.IsAdjacent(start, end))
				{
					liveRange.Merge(start, end);

					if (i + 1 < liveIntervals.Count)
					{
						var nextLiveRange = liveIntervals[i + 1];
						if (liveRange.IsAdjacent(nextLiveRange))
						{
							liveRange.Merge(nextLiveRange);
							liveIntervals.RemoveAt(i + 1);
						}
					}

					return;
				}

				if (liveRange.Start > end)
				{
					// new range is before the current range (so insert before)
					liveIntervals.Insert(i, new LiveInterval(this, start, end));
					return;
				}
			}

			// new range is after the last range
			liveIntervals.Add(new LiveInterval(this, start, end));
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (IsPhysicalRegister)
				return PhysicalRegister.ToString();
			else
				return VirtualRegisterOperand.ToString();
		}
	}
}