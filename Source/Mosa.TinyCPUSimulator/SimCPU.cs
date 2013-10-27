/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.TinyCPUSimulator
{
	public class SimCPU
	{
		public List<RAMBank> RAMBanks { get; private set; }

		public Dictionary<ulong, SimInstruction> InstructionCache { get; private set; }

		public BaseSimDevice[] PortDevices { get; private set; }

		public List<BaseSimDevice> SimDevices { get; private set; }

		public Dictionary<string, SimSymbol> Symbols { get; private set; }

		public ulong Tick { get; private set; }

		public SimMonitor Monitor { get; set; }

		public bool IsLittleEndian { get; protected set; }

		public virtual ulong CurrentInstructionPointer { get { return 0; } set { return; } }
		public virtual ulong PreviousInstructionPointer { get; set; }

		public virtual ulong StackPointer { get { return 0; } set { return; } }

		public virtual ulong FramePointer { get { return 0; } set { return; } }

		public virtual long PreviousFrameOffset { get { return -4; } }

		public Dictionary<ulong, KeyValuePair<byte, byte>> MemoryDelta { get; private set; }

		public SimCPUException LastException { get; set; }

		public SimCPU()
		{
			RAMBanks = new List<RAMBank>();
			InstructionCache = new Dictionary<ulong, SimInstruction>();
			SimDevices = new List<BaseSimDevice>();
			PortDevices = new BaseSimDevice[65536];
			Symbols = new Dictionary<string, SimSymbol>();
			MemoryDelta = new Dictionary<ulong, KeyValuePair<byte, byte>>();

			Tick = 0;
			IsLittleEndian = true;
		}

		public virtual void Reset()
		{
			foreach (var device in SimDevices)
			{
				device.Reset();
			}
		}

		private RAMBank Find(ulong address)
		{
			foreach (var r in RAMBanks)
				if (r.Contains(address))
					return r;

			throw new InvalidMemoryAccess(address);
		}

		private byte InternalRead8(ulong address)
		{
			RAMBank ram = Find(address);

			if (ram != null)
				return ram.Read8(address);

			return 0;
		}

		private void InternalWrite8(ulong address, byte value)
		{
			RAMBank ram = Find(address);

			MemoryDelta.Add(address, new KeyValuePair<byte, byte>(ram.Read8(address), value));

			if (ram != null)
				ram.Write8(address, value);

			return;
		}

		protected virtual ulong TranslateToPhysical(ulong address)
		{
			return address;
		}

		protected virtual void MemoryUpdate(ulong address, byte size)
		{
			foreach (var device in SimDevices)
			{
				device.MemoryWrite(address, size);
			}
		}

		public void AddMemory(ulong address, ulong size, uint type)
		{
			RAMBanks.Add(new RAMBank(address, size, type));
		}

		public void AddMemory(RAMBank bank)
		{
			RAMBanks.Add(bank);
		}

		public void DirectWrite8(ulong address, byte value)
		{
			InternalWrite8(address, value);

			MemoryUpdate(address, 8);
		}

		public void DirectWrite16(ulong address, ushort value)
		{
			if (IsLittleEndian)
			{
				InternalWrite8(address + 0, (byte)(value & 0xFF));
				InternalWrite8(address + 1, (byte)(value >> 8 & 0xFF));
			}
			else
			{
				InternalWrite8(address + 1, (byte)(value & 0xFF));
				InternalWrite8(address + 0, (byte)(value >> 8 & 0xFF));
			}

			MemoryUpdate(address, 16);
		}

		public void DirectWrite32(ulong address, uint value)
		{
			if (IsLittleEndian)
			{
				InternalWrite8(address + 0, (byte)(value & 0xFF));
				InternalWrite8(address + 1, (byte)(value >> 8 & 0xFF));
				InternalWrite8(address + 2, (byte)(value >> 16 & 0xFF));
				InternalWrite8(address + 3, (byte)(value >> 24 & 0xFF));
			}
			else
			{
				InternalWrite8(address + 3, (byte)(value & 0xFF));
				InternalWrite8(address + 2, (byte)(value >> 8 & 0xFF));
				InternalWrite8(address + 1, (byte)(value >> 16 & 0xFF));
				InternalWrite8(address + 0, (byte)(value >> 24 & 0xFF));
			}

			MemoryUpdate(address, 32);
		}

		public byte DirectRead8(ulong address)
		{
			return InternalRead8(address);
		}

		public ushort DirectRead16(ulong address)
		{
			if (IsLittleEndian)
				return (ushort)(DirectRead8(address + 0) | (DirectRead8(address + 1) << 8));
			else
				return (ushort)(DirectRead8(address + 1) | (DirectRead8(address + 0) << 8));
		}

		public uint DirectRead32(ulong address)
		{
			if (IsLittleEndian)
				return (uint)(DirectRead8(address + 0) | (DirectRead8(address + 1) << 8) | (DirectRead8(address + 2) << 16) | (DirectRead8(address + 3) << 24));
			else
				return (uint)(DirectRead8(address + 3) | (DirectRead8(address + 2) << 8) | (DirectRead8(address + 1) << 16) | (DirectRead8(address + 0) << 24));
		}

		public void Write8(ulong address, byte value)
		{
			DirectWrite8(TranslateToPhysical(address), value);
		}

		public void Write16(ulong address, ushort value)
		{
			DirectWrite16(TranslateToPhysical(address), value);
		}

		public void Write32(ulong address, uint value)
		{
			DirectWrite32(TranslateToPhysical(address), value);
		}

		public byte Read8(ulong address)
		{
			return DirectRead8(TranslateToPhysical(address));
		}

		public ushort Read16(ulong address)
		{
			return DirectRead16(TranslateToPhysical(address));
		}

		public uint Read32(ulong address)
		{
			return DirectRead32(TranslateToPhysical(address));
		}

		public void SetSymbol(string name, ulong address, ulong size)
		{
			if (Symbols.ContainsKey(name))
				return; // HACK for generics which duplicate methods!

			Symbols.Add(name, new SimSymbol(name, address, size));

			//Debug.WriteLine("0x" + address.ToString("X") + ": " + label);
		}

		public SimSymbol GetSymbol(string name)
		{
			SimSymbol symbol = null;

			if (!Symbols.TryGetValue(name, out symbol))
			{
				throw new SimCPUException();
			}

			return symbol;
		}

		public SimSymbol FindSymbol(ulong address)
		{
			foreach (var entry in Symbols)
			{
				if (address >= entry.Value.Address && address < entry.Value.EndAddress)
					return entry.Value;
			}

			return null;
		}

		public void AddInstruction(ulong address, SimInstruction instruction)
		{
			InstructionCache.Add(address, instruction);
		}

		protected SimInstruction GetInstruction(ulong address)
		{
			SimInstruction instruction = null;

			InstructionCache.TryGetValue(address, out instruction);

			return instruction;
		}

		public void AddDevice(BaseSimDevice device)
		{
			SimDevices.Add(device);
			device.Initialize();

			var ports = device.GetPortList();

			if (ports == null)
				return;

			foreach (var port in ports)
			{
				PortDevices[port] = device;
			}
		}

		public virtual SimInstruction DecodeOpcode(ulong address)
		{
			var instruction = GetInstruction(address);

			// if instruction is null --- a binary decode would be necessary

			return instruction;
		}

		protected virtual void ExecuteOpcode(SimInstruction instruction)
		{
			instruction.Opcode.Execute(this, instruction);
		}

		public virtual string CompactDump()
		{
			return string.Empty;
		}

		protected void ExecuteInstruction()
		{
			MemoryDelta.Clear();

			try
			{
				//if (Monitor.DebugOutput)
				//	Debug.Write("0x" + CurrentInstructionPointer.ToString("X") + ": ");

				Tick++;
				LastException = null;
				PreviousInstructionPointer = CurrentInstructionPointer;

				var instruction = DecodeOpcode(CurrentInstructionPointer);

				var PreviousInstruction = instruction;

				ExecuteOpcode(instruction);

				if (Monitor.DebugOutput)
				{
					Debug.Write(CompactDump());
					Debug.Write("  0x" + PreviousInstructionPointer.ToString("X") + ": ");
					Debug.WriteLine(PreviousInstruction.ToString());
				}
			}
			catch (SimCPUException e)
			{
				LastException = e;
			}
		}

		public void Execute()
		{
			//foreach (var pair in InstructionCache)
			//{
			//	Debug.WriteLine("0x" + pair.Key.ToString("X") + ": " + pair.Value);
			//}
			//Debug.WriteLine(string.Empty);

			if (Monitor.DebugOutput)
				Debug.WriteLine("EIP        EAX        EBX        ECX        EDX        ESI        EDI        ESP        EBP        FLAGS");

			for (; ; )
			{
				ExecuteInstruction();

				if (Monitor != null && Monitor.Break)
					return;
			}
		}

		public virtual SimState GetState()
		{
			SimState simState = new SimState(Tick, CurrentInstructionPointer, PreviousInstructionPointer, LastException, DecodeOpcode(CurrentInstructionPointer));

			simState.StoreMemoryDelta(MemoryDelta);

			return simState;
		}
	}
}