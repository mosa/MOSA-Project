/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	// TODO: Use data structures which are faster at finding intersetions, and add & evicting intervals.
	public class LiveIntervalUnion
	{
		protected List<LiveInterval> liveIntervals = new List<LiveInterval>();
		protected Register register;

		public IList<LiveInterval> LiveIntervals { get { return liveIntervals.AsReadOnly(); } }

		public Register Register { get { return register; } }

		public bool IsFloatingPoint { get { return register.IsFloatingPoint; } }

		public bool IsInteger { get { return register.IsInteger; } }

		public LiveIntervalUnion(Register register)
		{
			this.register = register;
		}

		public void Add(LiveInterval liveInterval)
		{
			liveIntervals.Add(liveInterval);
		}

		public void Evict(LiveInterval liveInterval)
		{
			liveIntervals.Remove(liveInterval);
		}

		public void Evict(List<LiveInterval> liveIntervals)
		{
			foreach (var interval in liveIntervals)
			{
				Evict(interval);
			}
		}

		public bool Intersects(LiveInterval liveInterval)
		{
			foreach (var interval in liveIntervals)
			{
				if (interval.Intersects(liveInterval))
				{
					return true;
				}
			}

			return false;
		}

		public List<LiveInterval> GetIntersections(LiveInterval liveInterval)
		{
			List<LiveInterval> intersections = null;

			foreach (var interval in liveIntervals)
			{
				if (interval.Intersects(liveInterval))
				{
					if (intersections == null)
						intersections = new List<LiveInterval>();

					intersections.Add(interval);
				}
			}

			return intersections;
		}
	}
}