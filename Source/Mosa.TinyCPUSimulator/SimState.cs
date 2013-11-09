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
	public class SimState
	{
		public ulong Tick { get; private set; }

		public ulong NextIP { get; private set; }

		public ulong IP { get; private set; }

		public SimInstruction Instruction { get; private set; }

		public Dictionary<string, object> Values { get; private set; }

		//public Dictionary<ulong, KeyValuePair<byte, byte>> MemoryDelta { get; private set; }

		public SimCPUException CPUException { get; private set; }

		public SimState(ulong tick, ulong ip, SimInstruction instruction, SimCPUException exception, ulong nextIP)
		{
			Tick = tick;
			IP = ip;
			NextIP = nextIP;
			Instruction = instruction;
			CPUException = exception;

			Values = new Dictionary<string, object>();
			//MemoryDelta = new Dictionary<ulong, KeyValuePair<byte, byte>>();
		}

		public void StoreValue(string name, object value)
		{
			Values.Add(name, value);
		}

		//public void StoreMemoryDelta(Dictionary<ulong, KeyValuePair<byte, byte>> memoryDelta)
		//{
		//	foreach (var value in memoryDelta)
		//	{
		//		MemoryDelta.Add(value.Key, value.Value);
		//	}
		//}

		public override string ToString()
		{
			return "[" + Tick.ToString("D5") + "] " + IP.ToString("X8") + ": " + (Instruction == null ? "<None>" : Instruction.ToString());
		}
	}
}