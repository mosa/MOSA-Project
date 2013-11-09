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

		public bool BreakOnJump = false;

		public bool BreakOnCall = false;

		public bool BreakOnReturn = false;

		public bool BreakOnBranch = false;

		public bool BreakAfterJump = false;

		public bool BreakAfterCall = false;

		public bool BreakAfterReturn = false;

		public bool BreakAfterBranch = false;

		public bool DebugOutput { get; set; }

		public ulong StepOverBreakPoint { get; set; }

		public OnSimStateUpdate OnStateUpdate { get; set; }

		public object locker = new object();

		public SimMonitor(SimCPU cpu)
		{
			CPU = cpu;
			DebugOutput = false;
			Stop = false;
			StepOverBreakPoint = 0;
		}

		public bool Break
		{
			get
			{
				lock (locker)
				{
					var lastFlowType = CPU.LastInstruction.Opcode.FlowType;
					var currentFlowType = CPU.CurrentInstruction.Opcode.FlowType;

					return Stop
						|| CPU.Tick == BreakAtTick
						|| (BreakOnException && CPU.LastException != null)
						|| (StepOverBreakPoint == CPU.CurrentInstructionPointer)
						
						|| (BreakAfterJump && lastFlowType == OpcodeFlowType.Jump)
						|| (BreakAfterCall && lastFlowType == OpcodeFlowType.Call)
						|| (BreakAfterReturn && lastFlowType == OpcodeFlowType.Return)
						|| (BreakAfterBranch && lastFlowType == OpcodeFlowType.Branch)

						|| (BreakOnJump && currentFlowType == OpcodeFlowType.Jump)
						|| (BreakOnCall && currentFlowType == OpcodeFlowType.Call)
						|| (BreakOnReturn && currentFlowType == OpcodeFlowType.Return)
						|| (BreakOnBranch && currentFlowType == OpcodeFlowType.Branch)

						|| breakPoints.Contains(CPU.CurrentInstructionPointer);
				}
			}
		}

		public void AddBreakPoint(ulong address)
		{
			lock (locker)
			{
				breakPoints.Add(address);
			}
		}

		public void RemoveBreakPoint(ulong address)
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

		public void BreakFromCurrentTick(uint steps)
		{
			lock (locker)
			{
				BreakAtTick = CPU.Tick + steps;
			}
		}
	}
}