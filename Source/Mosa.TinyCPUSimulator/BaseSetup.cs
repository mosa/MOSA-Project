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
		protected readonly SimMonitor Monitor;
		protected uint Address = 0x00000;

		public BaseSetup()
		{
			CPU = new T();
			Monitor = new SimMonitor(CPU);
			CPU.Monitor = Monitor;
			CPU.CurrentInstructionPointer = Address;
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

		protected SimOperand CreateOperand(SimRegister register)
		{
			SimOperand operand = null;

			if (!registerOperands.TryGetValue(register, out operand))
			{
				operand = new SimOperand(register);
				registerOperands.Add(register, operand);
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

		public void Add(BaseOpcode opcode, byte opcodeSize)
		{
			Add(new SimInstruction(opcode, opcodeSize));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimOperand operand1)
		{
			Add(new SimInstruction(opcode, opcodeSize, operand1));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimOperand operand1, SimOperand operand2)
		{
			Add(new SimInstruction(opcode, opcodeSize, operand1, operand2));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimOperand operand1, SimOperand operand2, SimOperand operand3)
		{
			Add(new SimInstruction(opcode, opcodeSize, operand1, operand2, operand3));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimOperand operand1, SimOperand operand2, SimOperand operand3, SimOperand operand4)
		{
			Add(new SimInstruction(opcode, opcodeSize, operand1, operand2, operand3, operand4));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimRegister register1)
		{
			Add(new SimInstruction(opcode, opcodeSize, CreateOperand(register1)));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimRegister register1, SimRegister register2)
		{
			Add(new SimInstruction(opcode, opcodeSize, CreateOperand(register1), CreateOperand(register2)));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimRegister register1, SimRegister register2, SimRegister register3)
		{
			Add(new SimInstruction(opcode, opcodeSize, CreateOperand(register1), CreateOperand(register2), CreateOperand(register3)));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimRegister register1, SimRegister register2, SimRegister register3, SimRegister register4)
		{
			Add(new SimInstruction(opcode, opcodeSize, CreateOperand(register1), CreateOperand(register2), CreateOperand(register3), CreateOperand(register4)));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimRegister register1, SimOperand operand2)
		{
			Add(new SimInstruction(opcode, opcodeSize, CreateOperand(register1), operand2));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimOperand operand1, SimRegister register2)
		{
			Add(new SimInstruction(opcode, opcodeSize, operand1, CreateOperand(register2)));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimRegister register1, uint immediate2)
		{
			Add(new SimInstruction(opcode, opcodeSize, CreateOperand(register1), CreateImmediate(immediate2)));
		}

		public void Add(BaseOpcode opcode, byte opcodeSize, SimRegister register1, int immediate2)
		{
			Add(new SimInstruction(opcode, opcodeSize, CreateOperand(register1), CreateImmediate(immediate2)));
		}
	}
}