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
	public class BaseSetup<T> where T : SimCPU, new()
	{
		protected readonly T CPU;
		protected uint Address = 0x00000;

		public BaseSetup()
		{
			CPU = new T();
			CPU.CurrentProgramCounter = Address;
		}

		public void Add(SimInstruction instruction)
		{
			CPU.AddInstruction(Address, instruction);
			Address = Address + instruction.OpcodeSize;
		}

		public void Add(ulong address, SimInstruction instruction)
		{
			CPU.AddInstruction(address, instruction);
		}

		private Dictionary<SimRegister, SimOperand> registerOperands = new Dictionary<SimRegister, SimOperand>();

		private object myLock = new object();

		protected SimOperand CreateOperand(SimRegister register)
		{
			SimOperand operand = null;

			lock (myLock)
			{
				if (!registerOperands.TryGetValue(register, out operand))
				{
					operand = new SimOperand(register);
					registerOperands.Add(register, operand);
				}
			}
			return operand;
		}

		protected static SimOperand CreateImmediate(uint value)
		{
			return SimOperand.CreateImmediate(value);
		}

		protected static SimOperand CreateImmediate(int value)
		{
			return SimOperand.CreateImmediate((uint)value);
		}

		protected static SimOperand CreateImmediate(ushort value)
		{
			return SimOperand.CreateImmediate(value);
		}

		protected static SimOperand CreateImmediate(byte value)
		{
			return SimOperand.CreateImmediate(value);
		}

		protected static SimOperand CreateImmediate(ulong value, int size)
		{
			return SimOperand.CreateImmediate(value, size);
		}

		public static SimOperand CreateMemoryAddressOperand(int size, ulong immediate)
		{
			return SimOperand.CreateMemoryAddress(size, immediate);
		}

		public static SimOperand CreateMemoryAddressOperand(int size, SimRegister baseRegister, SimRegister index, int scale, int displacement)
		{
			return SimOperand.CreateMemoryAddress(size, baseRegister, index, scale, displacement);
		}

		public static SimOperand CreateLabel(int size, string label)
		{
			return SimOperand.CreateLabel(size, label);
		}

		public static SimOperand CreateMemoryAddressLabel(int size, string label)
		{
			return SimOperand.CreateMemoryAddressLabel(size, label);
		}

		public void Add(BaseOpcode opcode, byte size, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimOperand operand1, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, operand1, opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimOperand operand1, SimOperand operand2, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, operand1, operand2, opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimOperand operand1, SimOperand operand2, SimOperand operand3, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, operand1, operand2, operand3, opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimOperand operand1, SimOperand operand2, SimOperand operand3, SimOperand operand4, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, operand1, operand2, operand3, operand4, opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimRegister register1, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, CreateOperand(register1), opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimRegister register1, SimRegister register2, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, CreateOperand(register1), CreateOperand(register2), opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimRegister register1, SimRegister register2, SimRegister register3, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, CreateOperand(register1), CreateOperand(register2), CreateOperand(register3), opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimRegister register1, SimRegister register2, SimRegister register3, SimRegister register4, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, CreateOperand(register1), CreateOperand(register2), CreateOperand(register3), CreateOperand(register4), opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimRegister register1, SimOperand operand2, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, CreateOperand(register1), operand2, opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimOperand operand1, SimRegister register2, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, operand1, CreateOperand(register2), opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimRegister register1, uint immediate2, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, CreateOperand(register1), CreateImmediate(immediate2), opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte size, SimRegister register1, int immediate2, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, size, CreateOperand(register1), CreateImmediate(immediate2), opcodeSize));
		}
	}
}
