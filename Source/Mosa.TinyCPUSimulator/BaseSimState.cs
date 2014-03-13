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
	public abstract class BaseSimState
	{
		public ulong Tick { get; private set; }

		public ulong NextIP { get; private set; }

		public ulong IP { get; private set; }

		public SimInstruction Instruction { get; private set; }

		public Dictionary<string, object> Values { get; private set; }

		public SimCPUException CPUException { get; private set; }

		public abstract int NativeRegisterSize { get; }

		public abstract string[] RegisterList { get; }

		public abstract string[] FlagList { get; }

		public double TotalElapsedSeconds { get; set; }

		protected BaseSimState(SimCPU simCPU)
		{
			Tick = simCPU.Tick;
			IP = simCPU.LastProgramCounter;
			NextIP = simCPU.CurrentProgramCounter;
			Instruction = simCPU.LastInstruction;
			CPUException = simCPU.LastException;

			Values = new Dictionary<string, object>();
		}

		public abstract object GetRegister(string name);

		public void StoreValue(string name, object value)
		{
			Values.Add(name, value);
		}

		public virtual void ExtendState(SimCPU simCPU)
		{ }

		public override string ToString()
		{
			return "[" + Tick.ToString("D5") + "] " + IP.ToString("X8") + ": " + (Instruction == null ? "<None>" : Instruction.ToString());
		}
	}
}