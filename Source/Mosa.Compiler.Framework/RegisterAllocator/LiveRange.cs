/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.RegisterAllocator
{

	public sealed class LiveInterval
	{
		private List<Interval> liveRanges = new List<Interval>(1);
		private List<int> usePositions = new List<int>();

		public Operand VirtualRegister { get; private set; }
		public Register PhysicalRegister { get; private set; }
		public int Sequence { get; private set; }
		public List<Interval> LiveRanges { get { return liveRanges; } }
		public int Count { get { return liveRanges.Count; } }
		public List<int> UsePositions { get { return usePositions; } }

		public bool IsPhysicalRegister { get { return VirtualRegister == null; } }
		public Interval LastRange { get { return liveRanges.Count == 0 ? null : liveRanges[liveRanges.Count - 1]; } }
		public Interval FirstRange { get { return liveRanges.Count == 0 ? null : liveRanges[0]; } }

		public int SpillSlot { get; set; }

		public LiveInterval(Operand virtualRegister, int sequence)
		{
			this.VirtualRegister = virtualRegister;
			this.Sequence = sequence;
		}

		public LiveInterval(Register physicalRegister, int sequence)
		{
			this.PhysicalRegister = physicalRegister;
			this.Sequence = sequence;
		}

		public void AddRange(int start, int end)
		{
			Interval.AddRangeToList(liveRanges, start, end);
		}

		public void AddUsePosition(int position)
		{
			Debug.Assert(!usePositions.Contains(position));
			usePositions.Add(position);
		}

		//public bool Contains(int position)
		//{
		//	foreach (var range in liveRanges)
		//	{
		//		// TODO: early exit optimization
		//		if (range.IsInside(position))
		//		{
		//			return true;
		//		}
		//	}

		//	return false;
		//}

	}
}

