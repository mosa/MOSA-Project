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
using Mosa.Compiler.Common;

namespace Mosa.TinyCPUSimulator
{
	public class SimCPU
	{
		private Dictionary<ulong, SimInstruction> InstructionCache { get; set; }

		private Dictionary<ulong, string> SourceInformation { get; set; }

		public BaseSimDevice[] PortDevices { get; private set; }

		public List<BaseSimDevice> SimDevices { get; private set; }

		public List<BaseSimDevice> SimMemoryDevices { get; private set; }

		public Dictionary<string, SimSymbol> Symbols { get; private set; }

		public ulong Tick { get; private set; }

		public SimMonitor Monitor { get; private set; }

		public bool IsLittleEndian { get; protected set; }

		public virtual ulong CurrentProgramCounter { get { return 0; } set { return; } }

		public virtual ulong LastProgramCounter { get; set; }

		public SimInstruction LastInstruction { get; private set; }

		public virtual ulong StackPointer { get { return 0; } set { return; } }

		public virtual ulong FramePointer { get { return 0; } set { return; } }

		public SimCPUException LastException { get; set; }

		public List<MemoryRegion> MemoryRegions { get; private set; }

		private uint[][] MemoryBlocks;

		internal static uint BlockSize = 1024 * 1024 / 4; // 1 MB
		internal static ulong MaxMemory = 1024L * 1024L * 1024L * 4L; // 4 GB

		public SimCPU()
		{
			MemoryBlocks = new uint[MaxMemory / BlockSize][];
			InstructionCache = new Dictionary<ulong, SimInstruction>();
			SourceInformation = new Dictionary<ulong, string>();
			SimDevices = new List<BaseSimDevice>();
			SimMemoryDevices = new List<BaseSimDevice>();
			PortDevices = new BaseSimDevice[65536];
			Symbols = new Dictionary<string, SimSymbol>();
			Monitor = new SimMonitor(this);
			MemoryRegions = new List<MemoryRegion>();

			Tick = 0;
			IsLittleEndian = true;
		}

		public void AddMemory(ulong address, ulong size, uint type)
		{
			MemoryRegions.Add(new MemoryRegion(address, size, type));
		}

		public bool IsValidMemoryReference(ulong address)
		{
			foreach (var region in MemoryRegions)
				if (region.Contains(address))
					return true;

			return false;
		}

		public virtual void Reset()
		{
			Tick = 0;

			foreach (var device in SimDevices)
			{
				device.Reset();
			}
		}

		private uint InternalRead32(ulong address)
		{
			Debug.Assert(address % 4 == 0);

			ulong index = address / (BlockSize * 4);

			var block = MemoryBlocks[index];

			if (block == null)
			{
				// for performance we only check if the memory access is valid when memory is allocated by the simulator
				if (!IsValidMemoryReference(address))
					throw new InvalidMemoryAccess(address);

				block = new uint[BlockSize];

				MemoryBlocks[index] = block;

				return 0;
			}

			uint offset = (uint)((address / 4) % BlockSize);

			return block[offset];
		}

		private void InternalWrite32(ulong address, uint value, uint mask)
		{
			Debug.Assert(address % 4 == 0);

			ulong index = address / (BlockSize * 4);
			var block = MemoryBlocks[index];

			if (block == null)
			{
				if (value == 0)
					return;

				// for performance we only check if the memory access is valid when memory is allocated by the simulator
				if (!IsValidMemoryReference(address))
					throw new InvalidMemoryAccess(address);

				block = new uint[BlockSize];

				MemoryBlocks[index] = block;
			}

			uint offset = (uint)((address / 4) % BlockSize);

			uint newvalue = (block[offset] & ~mask) | (value & mask);

			block[offset] = newvalue;

			Debug.Assert(InternalRead32(address) == newvalue); // very slow performance if assert enabled
		}

		protected virtual uint InternalRead32Ex(ulong address)
		{
			var offset = address % 4;

			if (offset == 0)
			{
				return InternalRead32(address);
			}
			else if (offset == 1)
			{
				uint a = InternalRead32(address - 1);
				uint b = InternalRead32(address + 3);

				return (a & 0x00FFFFFF) | (b & 0xFF000000);
			}
			else if (offset == 2)
			{
				uint a = InternalRead32(address - 2);
				uint b = InternalRead32(address + 2);

				return (a & 0x0000FFFF) | (b & 0xFFFF0000);
			}
			else if (offset == 3)
			{
				uint a = InternalRead32(address - 3);
				uint b = InternalRead32(address + 1);

				return (a & 0x000000FF) | (b & 0xFFFFFF00);
			}

			throw new InvalidProgramException();
		}

		protected virtual uint InternalRead16Ex(ulong address)
		{
			var offset = address % 4;

			uint a = InternalRead32(address - 1);

			if (offset == 0)
			{
				return (a & 0xFFFF0000) >> 16;
			}
			else if (offset == 1)
			{
				return (a & 0x00FFFF00) >> 8;
			}
			else if (offset == 2)
			{
				return (a & 0x0000FFFF);
			}
			else if (offset == 3)
			{
				uint b = InternalRead32(address + 1);

				return ((a & 0x000000FF) << 8) | ((b & 0xFF000000) >> 24);
			}

			throw new InvalidProgramException();
		}

		protected virtual ulong TranslateToPhysical(ulong address)
		{
			return address;
		}

		protected virtual void MemoryUpdate(ulong address, byte size)
		{
			foreach (var device in SimMemoryDevices)
			{
				device.MemoryWrite(address, size);
			}
		}

		public uint DirectRead32(ulong address)
		{
			uint value = InternalRead32Ex(address);

			if (Endian.NativeIsLittleEndian)
				value = Endian.Swap(value);

			//Debug.Assert(((uint)DirectRead16(address) | ((uint)DirectRead16(address + 2) << 16)) == value);

			return value;
		}

		public void DirectWrite32(ulong address, uint value)
		{
			uint offset = (uint)(address % 4);

			if (offset == 0)
			{
				InternalWrite32(address, value, 0xFFFFFFFF);
			}
			else if (offset == 1)
			{
				InternalWrite32(address - 1, value, 0x00FFFFFF);
				InternalWrite32(address + 3, value & 0xFF000000, 0xFF000000);
			}
			else if (offset == 2)
			{
				InternalWrite32(address - 2, value, 0x0000FFFF);
				InternalWrite32(address + 2, value & 0xFFFF0000, 0xFFFF0000);
			}
			else if (offset == 3)
			{
				InternalWrite32(address - 3, value, 0x000000FF);
				InternalWrite32(address + 1, value & 0xFFFFFF00, 0xFFFFFF00);
			}

			Debug.Assert(((uint)DirectRead16(address) | ((uint)DirectRead16(address + 2) << 16)) == value);
			Debug.Assert(DirectRead32(address) == value);	// very slow performance if assert enabled

			MemoryUpdate(address, 32);
		}

		public ushort DirectRead16(ulong address)
		{
			uint offset = (uint)(address % 4);

			uint value = 0;

			if (offset == 0)
			{
				uint a = InternalRead32(address);

				value = a & 0x0000FFFF;
			}
			else if (offset == 1)
			{
				uint a = InternalRead32(address - 1);

				value = (a >> 8) & 0x0000FFFF;
			}
			else if (offset == 2)
			{
				uint a = InternalRead32(address - 2);

				value = (a >> 16) & 0x0000FFFF;
			}
			else if (offset == 3)
			{
				uint a = InternalRead32(address - 3);
				uint b = InternalRead32(address + 1);

				value = ((a >> 24) & 0x000000FF) | ((b & 0xFF000000) >> 24);
			}

			return (ushort)value;
		}

		public void DirectWrite16(ulong address, ushort value)
		{
			uint offset = (uint)(address % 4);

			var val = (uint)value;

			if (offset == 0)
			{
				InternalWrite32(address, val, 0x0000FFFF);
			}
			else if (offset == 1)
			{
				InternalWrite32(address - 1, val << 8, 0x00FFFF00);
			}
			else if (offset == 2)
			{
				InternalWrite32(address - 2, val << 16, 0xFFFF0000);
			}
			else if (offset == 3)
			{
				InternalWrite32(address - 3, val << 24, 0x00000FF);
				InternalWrite32(address + 1, (val & 0x0000FF00) << 16, 0xFF000000);
			}

			Debug.Assert(DirectRead16(address) == val);	// very slow performance if assert enabled

			MemoryUpdate(address, 16);
		}

		public byte DirectRead8(ulong address)
		{
			uint offset = (uint)(address % 4);

			uint value = InternalRead32(address - offset);

			int shift = (3 - (int)offset) * 8;

			return (byte)((value >> shift) & 0xFF);
		}

		public void DirectWrite8(ulong address, byte value)
		{
			uint offset = (uint)(address % 4);

			int shift = (3 - (int)offset) * 8;

			InternalWrite32(address - offset, ((uint)value) << shift, (uint)0xFF << shift);

			Debug.Assert(DirectRead8(address) == value);	// very slow performance if assert enabled

			MemoryUpdate(address, 8);
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
			Symbols.Add(name, new SimSymbol(name, address, size));
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
			//Debug.Assert(!InstructionCache.ContainsKey(address), instruction.ToString());
			InstructionCache.Add(address, instruction);
		}

		public void AddSourceInformation(ulong address, string information)
		{
			SourceInformation.Add(address, information);
		}

		protected SimInstruction GetInstruction(ulong address)
		{
			SimInstruction instruction = null;

			InstructionCache.TryGetValue(address, out instruction);

			return instruction;
		}

		public string GetSourceInformation(ulong address)
		{
			string source = null;

			SourceInformation.TryGetValue(address, out source);

			return source;
		}

		public void AddDevice(BaseSimDevice device)
		{
			SimDevices.Add(device);
			device.Initialize();

			if (device.IsMemoryMonitor)
			{
				SimMemoryDevices.Add(device);
			}

			var ports = device.GetPortList();

			if (ports == null)
				return;

			foreach (var port in ports)
			{
				PortDevices[port] = device;
			}
		}

		private SimInstruction lastDecodedInstruction;
		private ulong lastDecodedProgramCounter = 0;

		public SimInstruction CurrentInstruction
		{
			get
			{
				if (CurrentProgramCounter == lastDecodedProgramCounter && lastDecodedProgramCounter != 0)
					return lastDecodedInstruction;

				lastDecodedInstruction = GetOpcode(CurrentProgramCounter);

				lastDecodedProgramCounter = CurrentProgramCounter;

				return lastDecodedInstruction;
			}
		}

		public SimInstruction GetOpcode(ulong address)
		{
			var instruction = GetInstruction(address);

			if (instruction == null)
			{
				instruction = DecodeOpcode(address);

				if (instruction == null)
				{
					return instruction;
					//throw new SimCPUException();
				}

				AddInstruction(address, instruction);
			}

			return instruction;
		}

		public virtual SimInstruction DecodeOpcode(ulong address)
		{
			return null;
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
			try
			{
				Tick++;
				LastException = null;
				LastProgramCounter = CurrentProgramCounter;

				if (CurrentInstruction == null)
					throw new NotSupportedException("Instruction is invalid!");

				LastInstruction = CurrentInstruction;

				ExecuteOpcode(LastInstruction);

				if (Monitor.DebugOutput)
				{
					Debug.Write(CompactDump());
					Debug.Write("  0x" + LastProgramCounter.ToString("X") + ": ");
					Debug.WriteLine(LastInstruction.ToString());
				}
			}
			catch (SimCPUException e)
			{
				LastException = e;
			}
			catch (NotSupportedException e)
			{
				LastException = new SimCPUException();
				Monitor.Stop = true;
			}
		}

		public void Execute()
		{
			try
			{
				Monitor.IsExecuting = true;

				if (Monitor.DebugOutput)
				{
					// Move to CPUx86
					//Debug.WriteLine("EIP        EAX        EBX        ECX        EDX        ESI        EDI        ESP        EBP        XMM#0      XMM#1      XMM#2      XMM#3      FLAGS");
					Debug.WriteLine("EIP        EAX        EBX        ECX        EDX        ESI        EDI        ESP        EBP        FLAGS");
				}

				for (; ; )
				{
					ExecuteInstruction();

					bool brk = Monitor.Break;

					Monitor.OnExecutionStepCompleted(brk);

					if (brk)
					{
						return;
					}
				}
			}
			finally
			{
				Monitor.IsExecuting = false;
			}
		}

		public virtual BaseSimState GetState()
		{
			return null;
		}

		public virtual void ExtendState(BaseSimState simState)
		{
		}
	}
}