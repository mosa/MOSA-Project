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

		protected static bool IsZero(ulong v, int size)
		{
			if (size == 32) return IsZero((uint)v);
			else if (size == 16) return IsZero((ushort)v);
			else if (size == 8) return IsZero((byte)v);
			else if (size == 64) return IsZero((ulong)v);

			throw new CPUException();
		}

		protected static bool IsZero(ulong v)
		{
			return (v == 0);
		}

		protected static bool IsZero(uint v)
		{
			return (v == 0);
		}

		protected static bool IsZero(ushort v)
		{
			return (v == 0);
		}

		protected static bool IsZero(byte v)
		{
			return (v == 0);
		}

		protected static bool IsParity(ulong v)
		{
			return ((v & 0xF) % 2) == 1;
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

		// The CARRY flag and OVERFLOW flag in binary arithmetic
		// http://teaching.idallen.com/dat2343/10f/notes/040_overflow.txt

		protected static bool IsOverflow(ulong r, ulong v1, ulong v2, int size)
		{
			if (size == 32) return IsOverflow((uint)r, (uint)v1, (uint)v2);
			else if (size == 16) return IsOverflow((ushort)r, (ushort)v1, (ushort)v2);
			else if (size == 8) return IsOverflow((byte)r, (byte)v1, (byte)v2);

			throw new CPUException();
		}

		protected static bool IsOverflow(bool br, bool b1, bool b2)
		{
			if (b1 && b2 && !br) return true;
			if (!b1 && !b2 && br) return true;
			return false;
		}

		protected static bool IsOverflow(byte r, byte v1, byte v2)
		{
			bool b1 = (((v1 >> 8) & 0x1) == 1);
			bool b2 = (((v2 >> 8) & 0x1) == 1);
			bool br = (((r >> 8) & 0x1) == 1);

			return IsOverflow(b1, b2, br);
		}

		protected static bool IsOverflow(ushort r, ushort v1, ushort v2)
		{
			bool b1 = (((v1 >> 15) & 0x1) == 1);
			bool b2 = (((v2 >> 15) & 0x1) == 1);
			bool br = (((r >> 15) & 0x1) == 1);

			return IsOverflow(b1, b2, br);
		}

		protected static bool IsOverflow(uint r, uint v1, uint v2)
		{
			bool b1 = (((v1 >> 31) & 0x1) == 1);
			bool b2 = (((v2 >> 31) & 0x1) == 1);
			bool br = (((r >> 31) & 0x1) == 1);

			return IsOverflow(b1, b2, br);
		}

		protected uint GetAddress(CPUx86 cpu, SimOperand operand)
		{
			Debug.Assert(operand.IsMemory);

			// Memory = Register + (Base * Scale) + Displacement
			int address = (int)(((operand.Index) as GeneralPurposeRegister).Value * operand.Scale) + operand.Displacement;

			if (operand != null)
			{
				address = address + (int)((operand.Register) as GeneralPurposeRegister).Value;
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
					//if (operand.IsRelativeToCurrentInstructionPointer)
					//	return (uint)((uint)cpu.CurrentInstructionPointer - (int)address);
					//else
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

				if (operand.IsMemory)
				{
					Write(cpu, address, value, size);
				}
				else
				{
					throw new CPUException();
				}
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