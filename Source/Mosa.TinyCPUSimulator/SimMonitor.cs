/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.TinyCPUSimulator
{
	public class SimMonitor
	{
		protected HashSet<ulong> breakPoints = new HashSet<ulong>();

		public SimCPU CPU { get; private set; }

		public long BreakAtTick = -1;

		public SimMonitor(SimCPU cpu)
		{
			this.CPU = cpu;
		}

		public bool Break
		{
			get
			{
				return CPU.Tick == BreakAtTick || breakPoints.Contains(CPU.CurrentInstructionPointer);
			}
		}

		public void AddBreakPoint(ulong address)
		{
			breakPoints.Add(address);
		}

		public void AddRemove(ulong address)
		{
			breakPoints.Add(address);
		}
	}
}