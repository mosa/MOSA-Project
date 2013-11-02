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
	public sealed class SimMonitor
	{
		public delegate void OnSimStateUpdate(SimState state, bool forceUpdate);

		private HashSet<ulong> breakPoints = new HashSet<ulong>();

		public SimCPU CPU { get; private set; }

		public bool Stop { get; set; }

		public ulong BreakAtTick { get; set; }

		public bool BreakOnException = true;

		public bool DebugOutput { get; set; }

		public OnSimStateUpdate OnStateUpdate { get; set; }

		public object locker = new object();

		public SimMonitor(SimCPU cpu)
		{
			CPU = cpu;
			DebugOutput = false;
			Stop = false;
		}

		public bool Break
		{
			get
			{
				return Stop
					|| CPU.Tick == BreakAtTick
					|| (BreakOnException && CPU.LastException != null)
					|| breakPoints.Contains(CPU.CurrentInstructionPointer);
			}
		}

		public void AddBreakPoint(ulong address)
		{
			lock (locker)
			{
				breakPoints.Add(address);
			}
		}

		public void AddRemove(ulong address)
		{
			lock (locker)
			{
				breakPoints.Add(address);
			}
		}

		public void AddBreakPoint(string label)
		{
			ulong address = CPU.GetSymbol(label).Address;

			lock (locker)
			{
				AddBreakPoint(address);
			}
		}

		public void ClearBreakPoints()
		{
			lock (locker)
			{
				breakPoints.Clear();
			}
		}

		public void OnExecutionStepCompleted(bool forceUpdate)
		{
			if (OnStateUpdate == null)
				return;

			OnStateUpdate(CPU.GetState(), forceUpdate);
		}
	}
}