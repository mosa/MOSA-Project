/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class IntersectionResult
	{
		public static IntersectionResult FreeToInfinity = new IntersectionResult();

		public LiveInterval LiveInterval { get; private set; }
		public SlotIndex EndOfFree { get; private set; }

		public bool IsFree { get { return LiveInterval == null; } }
		public bool IsFreeToInfinity { get { return LiveInterval == null && EndOfFree == null; } }

		public IntersectionResult(LiveInterval liveInterval)
		{
			this.LiveInterval = liveInterval;
		}

		public IntersectionResult(SlotIndex endOfFree)
		{
			this.EndOfFree = endOfFree;
		}

		private IntersectionResult()
		{
		}

		public override string ToString()
		{
			if (!IsFree) return "[" + LiveInterval.ToString() + "]";
			else
				if (IsFreeToInfinity)
					return "[free to infinitiy]";
				else
					return "[free to " + EndOfFree.ToString() + "]";
		}
	}
}