// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.TinyCPUSimulator
{
	public sealed class SimMonitor
	{
		public delegate void OnSimStateUpdate(BaseSimState state, bool forceUpdate);

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

		public bool IsExecuting { get { return isExecuting; } internal set { isExecuting = value; } }

		public OnSimStateUpdate OnStateUpdate { get; set; }

		private object locker = new object();

		private volatile bool isExecuting;

		public SimMonitor(SimCPU cpu)
		{
			CPU = cpu;
			DebugOutput = false;

			//DebugOutput = true;
			Stop = false;
			StepOverBreakPoint = 0;
		}

		public bool Break
		{
			get
			{
				lock (locker)
				{
					var lastFlowType = CPU.LastInstruction != null ? CPU.LastInstruction.Opcode.FlowType : OpcodeFlowType.Normal;
					var currentFlowType = CPU.CurrentInstruction != null ? CPU.CurrentInstruction.Opcode.FlowType : OpcodeFlowType.Normal;

					return Stop
						|| CPU.Tick == BreakAtTick
						|| (BreakOnException && CPU.LastException != null)
						|| (StepOverBreakPoint == CPU.CurrentProgramCounter)

						|| (BreakAfterJump && lastFlowType == OpcodeFlowType.Jump)
						|| (BreakAfterCall && lastFlowType == OpcodeFlowType.Call)
						|| (BreakAfterReturn && lastFlowType == OpcodeFlowType.Return)
						|| (BreakAfterBranch && lastFlowType == OpcodeFlowType.Branch)

						|| (BreakOnJump && currentFlowType == OpcodeFlowType.Jump)
						|| (BreakOnCall && currentFlowType == OpcodeFlowType.Call)
						|| (BreakOnReturn && currentFlowType == OpcodeFlowType.Return)
						|| (BreakOnBranch && currentFlowType == OpcodeFlowType.Branch)

						|| breakPoints.Contains(CPU.CurrentProgramCounter);
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
				breakPoints.Remove(address);
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
