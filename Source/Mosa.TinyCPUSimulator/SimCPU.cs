/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.TinyCPUSimulator
{
	public class SimCPU
	{
		public Dictionary<ulong, SimInstruction> InstructionCache { get; private set; }

		public BaseSimDevice[] PortDevices { get; private set; }

		public List<BaseSimDevice> SimDevices { get; private set; }

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

		internal static uint BlockSize = 1024 * 1024; // 1 MB
		internal static ulong MaxMemory = 1024L * 1024L * 1024L * 4L; // 4 GB

		public SimCPU()
		{
			MemoryBlocks = new uint[MaxMemory / BlockSize][];
			InstructionCache = new Dictionary<ulong, SimInstruction>();
			SimDevices = new List<BaseSimDevice>();
			PortDevices = new BaseSimDevice[65536];
			Symbols = new Dictionary<string, SimSymbol>();
			Monitor = new SimMonitor(this);
			MemoryRegions = new List<MemoryRegion>();
			//MemoryDelta = new Dictionary<ulong, KeyValuePair<byte, byte>>();

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
			
			ulong index = address / BlockSize / 4;

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

			ulong index = address / BlockSize / 4;
			var block = MemoryBlocks[index];

			if (block == null)
			{
				if (value == 0)
					return;

				// for performance we only check if the memory access is valid when memory is allocated by the simulator
				if (!IsValidMemoryReference(address))
					throw new InvalidMemoryAccess(address);

				block = new uint[BlockSize / 4];

				MemoryBlocks[index] = block;
			}

			uint offset = (uint)((address / 4) % BlockSize);

			block[offset] = (block[offset] & ~mask) | (value & mask);
		}

		protected virtual ulong TranslateToPhysical(ulong address)
		{
			return address;
		}

		protected virtual void MemoryUpdate(ulong address, byte size)
		{
			//FIXME: make faster
			foreach (var device in SimDevices)
			{
				device.MemoryWrite(address, size);
			}
		}

		public uint EndianSwap(uint value)
		{
			if (IsLittleEndian)
				return value;

			return ((value & 0x000000FF) << 24) | ((value & 0x0000FF00) << 8) |
				((value & 0x00FF0000) >> 8) | ((value & 0xFF000000) >> 24);
		}

		public ushort EndianSwap(ushort value)
		{
			if (IsLittleEndian)
				return value;

			return (ushort)(((value & 0xFF) << 8) | ((value & 0xFF00) >> 8));
		}

		public uint DirectRead32(ulong address)
		{
			uint offset = (uint)(address % 4);

			uint value = 0;

			if (offset == 0)
			{
				value = InternalRead32(address);
			}
			else if (offset == 1)
			{
				value =
					((InternalRead32(address - 1) & 0xFFFFFF00) << 8) |
					((InternalRead32(address + 3) & 0xFF000000) >> 24);
			}
			else if (offset == 2)
			{
				value =
					((InternalRead32(address - 2) & 0xFFFF0000) << 16) |
					((InternalRead32(address + 2) & 0xFFFF0000) >> 16);
			}
			else if (offset == 3)
			{
				value =
					((InternalRead32(address - 3) & 0xFF000000) << 24) |
					((InternalRead32(address + 1) & 0xFFFFFF00) >> 8);
			}

			return EndianSwap(value);
		}

		public ushort DirectRead16(ulong address)
		{
			uint offset = (uint)(address % 4);

			ushort value = 0;

			if (offset == 0)
			{
				value = (ushort)((InternalRead32(address) & 0xFFFF0000) >> 16);
			}
			else if (offset == 1)
			{
				value = (ushort)((InternalRead32(address - 1) & 0x00FFFF00) >> 8);
			}
			else if (offset == 2)
			{
				value = (ushort)(InternalRead32(address - 2) & 0x0000FFFF);
			}
			else if (offset == 3)
			{
				value = (ushort)((InternalRead32(address - 3) & 0x000000FF) | ((InternalRead32(address + 1) & 0xFF000000) >> 24));
			}

			return value;
		}

		public byte DirectRead8(ulong address)
		{
			uint offset = (uint)(address % 4);

			uint value = InternalRead32(address - offset);

			int shift = (3 - (int)offset) * 8;

			return (byte)((value >> shift) & 0xFF);
		}

		public void DirectWrite32(ulong address, uint value)
		{
			uint offset = (uint)(address % 4);

			value = EndianSwap(value);

			if (offset == 0)
			{
				InternalWrite32(address, value, 0xFFFFFFFF);
			}
			else if (offset == 1)
			{
				InternalWrite32(address - 1, value >> 8, 0x00FFFFFF);
				InternalWrite32(address + 3, value << 24, 0xFF000000);
			}
			else if (offset == 2)
			{
				InternalWrite32(address - 2, value >> 16, 0x0000FFFF);
				InternalWrite32(address + 2, value << 16, 0xFFFF0000);
			}
			else if (offset == 3)
			{
				InternalWrite32(address - 3, value >> 24, 0x000000FF);
				InternalWrite32(address + 1, value << 8, 0xFFFFFF00);
			}

			MemoryUpdate(address, 32);
		}

		public void DirectWrite16(ulong address, ushort value)
		{
			uint offset = (uint)(address % 4);

			value = EndianSwap(value);

			if (offset == 0)
			{
				InternalWrite32(address - 0, ((uint)value << 16), 0xFFFF0000);
			}
			else if (offset == 1)
			{
				InternalWrite32(address - 1, ((uint)value << 8), 0x00FFFF00);
			}
			else if (offset == 2)
			{
				InternalWrite32(address - 2, ((uint)value), 0x0000FFFF);
			}
			else if (offset == 3)
			{
				InternalWrite32(address - 3, ((uint)value >> 8), 0x000000FF);
				InternalWrite32(address + 1, ((uint)value << 24), 0xFF000000);
			}

			MemoryUpdate(address, 16);
		}

		public void DirectWrite8(ulong address, byte value)
		{
			uint offset = (uint)(address % 4);

			int shift = (3 - (int)offset) * 8;

			InternalWrite32(address - offset, ((uint)value) << shift, (uint)0xFF << shift);

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

		private SimInstruction lastDecodedInstruction;
		private ulong lastDecodedProgramCounter = 0;

		public SimInstruction CurrentInstruction
		{
			get
			{
				if (CurrentProgramCounter == lastDecodedProgramCounter && lastDecodedProgramCounter != 0)
					return lastDecodedInstruction;

				lastDecodedInstruction = DecodeOpcode(CurrentProgramCounter);

				// if lastDecodedInstruction is null --- a binary decode would be necessary

				lastDecodedProgramCounter = CurrentProgramCounter;

				return lastDecodedInstruction;
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
			try
			{
				Tick++;
				LastException = null;
				LastProgramCounter = CurrentProgramCounter;

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