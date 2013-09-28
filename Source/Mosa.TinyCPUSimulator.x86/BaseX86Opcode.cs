/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.x86
{
	public class BaseX86Opcode : BaseOpcode
	{
		public override void Execute(SimCPU cpu, SimInstruction instruction)
		{
			var x86 = cpu as CPUx86;

			x86.EIP.Value = (uint)(x86.EIP.Value + instruction.OpcodeSize);

			Execute(x86, instruction);
		}

		public virtual void Execute(CPUx86 cpu, SimInstruction instruction)
		{
		}

		protected static bool IsSign(ulong v, int size)
		{
			if (size == 32) return IsSign((uint)v);
			else if (size == 16) return IsSign((ushort)v);
			else if (size == 8) return IsSign((byte)v);

			throw new CPUException();
		}

		protected static bool IsSign(uint v)
		{
			return (((v >> 31) & 0x1) == 1);
		}

		protected static bool IsSign(ushort v)
		{
			return (((v >> 15) & 0x1) == 1);
		}

		protected static bool IsSign(byte v)
		{
			return (((v >> 7) & 0x1) == 1);
		}

		protected static bool IsAdjustAfterAdd(ulong v1, ulong v2)
		{
			return (((v1 & 0xF) + (v2 & 0xF)) > 16);
		}

		protected static bool IsAdjustAfterSub(ulong v1, ulong v2)
		{
			return ((v1 & 0xF) < (v2 & 0xF));
		}

		protected void UpdateFlags(CPUx86 cpu, int size, long s, ulong u, bool zeroFlag, bool parityParity, bool signFlag, bool carryFlag, bool overFlowFlag)
		{
			if (size == 32)
				UpdateFlags32(cpu, s, u, zeroFlag, parityParity, signFlag, carryFlag, overFlowFlag);
			else if (size == 16)
				UpdateFlags16(cpu, s, u, zeroFlag, parityParity, signFlag, carryFlag, overFlowFlag);
			else
				UpdateFlags8(cpu, s, u, zeroFlag, parityParity, signFlag, carryFlag, overFlowFlag);
		}

		protected void UpdateFlags8(CPUx86 cpu, long s, ulong u, bool zeroFlag, bool parityParity, bool signFlag, bool carryFlag, bool overFlowFlag)
		{
			if (zeroFlag)
				cpu.FLAGS.Zero = ((u & 0xFF) == 0);
			if (overFlowFlag)
				cpu.FLAGS.Overflow = (s < byte.MinValue || s > byte.MaxValue);
			if (carryFlag)
				cpu.FLAGS.Carry = ((u >> 8) != 0);
			if (signFlag)
				cpu.FLAGS.Sign = (((u >> 7) & 0x01) != 0);
			if (parityParity)
				cpu.FLAGS.Parity = ((u & 0xFF) % 2) == 1;
		}

		protected void UpdateFlags16(CPUx86 cpu, long s, ulong u, bool zeroFlag, bool parityParity, bool signFlag, bool carryFlag, bool overFlowFlag)
		{
			if (zeroFlag)
				cpu.FLAGS.Zero = ((u & 0xFFFF) == 0);
			if (overFlowFlag)
				cpu.FLAGS.Overflow = (s < short.MinValue || s > short.MaxValue);
			if (carryFlag)
				cpu.FLAGS.Carry = ((u >> 16) != 0);
			if (signFlag)
				cpu.FLAGS.Sign = (((u >> 15) & 0x01) != 0);
			if (parityParity)
				cpu.FLAGS.Parity = ((u & 0xFF) % 2) == 1;
		}

		protected void UpdateFlags32(CPUx86 cpu, long s, ulong u, bool zeroFlag, bool parityParity, bool signFlag, bool carryFlag, bool overFlowFlag)
		{
			if (zeroFlag)
				cpu.FLAGS.Zero = ((u & 0xFFFFFFFF) == 0);
			if (overFlowFlag)
				cpu.FLAGS.Overflow = (s < int.MinValue || s > int.MaxValue);
			if (carryFlag)
				cpu.FLAGS.Carry = ((u >> 32) != 0);
			if (signFlag)
				cpu.FLAGS.Sign = (((u >> 31) & 0x01) != 0);
			if (parityParity)
				cpu.FLAGS.Parity = ((u & 0xFF) % 2) == 1;
		}

		protected uint GetAddress(CPUx86 cpu, SimOperand operand)
		{
			Debug.Assert(operand.IsMemory);

			int address = 0;

			if (operand.Index != null)
			{
				// Memory = Register + (Base * Scale) + Displacement
				address = (int)(((operand.Index) as GeneralPurposeRegister).Value * operand.Scale);
			}

			if (operand.Register != null)
			{
				address = address + (int)((operand.Register) as GeneralPurposeRegister).Value + operand.Displacement;
			}
			else
			{
				Debug.Assert(true);
			}

			return (uint)address;
		}

		protected uint LoadValue(CPUx86 cpu, SimOperand operand)
		{
			if (operand.IsImmediate)
			{
				return (uint)operand.Immediate;
			}

			if (operand.IsRegister)
			{
				return ((operand.Register) as Register32Bit).Value;
			}

			if (operand.IsLabel)
			{
				uint address = (uint)cpu.GetLabel(operand.Label);

				if (operand.IsMemory)
					return Read(cpu, address, operand.Size);
				else
				{
					return address;
				}
			}

			if (operand.IsMemory)
			{
				uint address = GetAddress(cpu, operand);

				return Read(cpu, address, operand.Size);
			}

			throw new CPUException();
		}

		protected void StoreValue(CPUx86 cpu, SimOperand operand, uint value, int size)
		{
			Debug.Assert(!operand.IsImmediate);

			if (operand.IsRegister)
			{
				((operand.Register) as Register32Bit).Value = value;
			}

			if (operand.IsLabel)
			{
				uint address = (uint)cpu.GetLabel(operand.Label);

				Write(cpu, address, value, size);
			}

			if (operand.IsMemory)
			{
				uint address = GetAddress(cpu, operand);

				Write(cpu, address, value, size);
			}
		}

		protected double LoadFloatValue(CPUx86 cpu, SimOperand operand)
		{
			if (operand.IsRegister)
			{
				return ((operand.Register) as RegisterFloatingPoint).Value;
			}

			if (operand.IsMemory)
			{
				uint address = GetAddress(cpu, operand);

				return ReadFloat(cpu, address, operand.Size);
			}

			throw new CPUException();
		}

		protected void StoreFloatValue(CPUx86 cpu, SimOperand operand, double value, int size)
		{
			Debug.Assert(!operand.IsImmediate);

			if (operand.IsRegister)
			{
				((operand.Register) as RegisterFloatingPoint).Value = value;
			}

			if (operand.IsMemory)
			{
				uint address = GetAddress(cpu, operand);

				WriteFloat(cpu, address, value, size);
			}
		}

		protected uint Read(CPUx86 cpu, uint address, int size)
		{
			if (size == 32) return cpu.Read32(address);
			else if (size == 16) return cpu.Read16(address);
			else if (size == 8) return cpu.Read8(address);

			throw new CPUException();
		}

		protected void Write(CPUx86 cpu, uint address, uint value, int size)
		{
			if (size == 32) cpu.Write32(address, (uint)value);
			else if (size == 16) cpu.Write16(address, (ushort)value);
			else if (size == 8) cpu.Write8(address, (byte)value);
		}

		protected double ReadFloat(CPUx86 cpu, uint address, int size)
		{
			if (size == 64)
			{
				byte[] b = new byte[8];

				b[0] = cpu.Read8(address + 0);
				b[1] = cpu.Read8(address + 1);
				b[2] = cpu.Read8(address + 2);
				b[3] = cpu.Read8(address + 3);
				b[4] = cpu.Read8(address + 4);
				b[5] = cpu.Read8(address + 5);
				b[6] = cpu.Read8(address + 6);
				b[7] = cpu.Read8(address + 7);

				return BitConverter.ToDouble(b, 0);
			}
			else if (size == 32)
			{
				byte[] b = new byte[4];

				b[0] = cpu.Read8(address + 0);
				b[1] = cpu.Read8(address + 1);
				b[2] = cpu.Read8(address + 2);
				b[3] = cpu.Read8(address + 3);

				return BitConverter.ToSingle(b, 0);
			}

			throw new CPUException();
		}

		protected void WriteFloat(CPUx86 cpu, uint address, double value, int size)
		{
			if (size == 64)
			{
				byte[] b = BitConverter.GetBytes(value);

				cpu.Write8(address + 0, b[0]);
				cpu.Write8(address + 1, b[1]);
				cpu.Write8(address + 2, b[2]);
				cpu.Write8(address + 3, b[3]);
				cpu.Write8(address + 4, b[4]);
				cpu.Write8(address + 5, b[5]);
				cpu.Write8(address + 6, b[6]);
				cpu.Write8(address + 7, b[7]);
			}
			else if (size == 32)
			{
				byte[] b = BitConverter.GetBytes((float)value);

				cpu.Write8(address + 0, b[0]);
				cpu.Write8(address + 1, b[1]);
				cpu.Write8(address + 2, b[2]);
				cpu.Write8(address + 3, b[3]);
			}
		}
	}
}