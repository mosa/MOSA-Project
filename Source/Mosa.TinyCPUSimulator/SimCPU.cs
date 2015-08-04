// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.TinyCPUSimulator
{
	public abstract class SimCPU
	{
		private Dictionary<ulong, SimInstruction> InstructionCache { get; set; }

		private Dictionary<ulong, string> SourceInformation { get; set; }

		public BaseSimDevice[] PortDevices { get; private set; }

		public List<BaseSimDevice> SimDevices { get; private set; }

		public List<BaseSimDevice> SimMemoryDevices { get; private set; }

		public Dictionary<string, SimSymbol> Symbols { get; private set; }

		public ulong Tick { get; set; }

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

		private uint InternalRead32Ex(ulong address)
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

		private void InternalWrite32Ex(ulong address, uint value, uint mask)
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

			// very slow performance if assert enabled
			//Debug.Assert(InternalRead32(address) == newvalue);
		}

		protected uint InternalRead32(ulong address)
		{
			var offset = address % 4;

			if (offset == 0)
			{
				return InternalRead32Ex(address);
			}
			else if (offset == 1)
			{
				uint a = InternalRead32Ex(address - 1);
				uint b = InternalRead32Ex(address + 3);

				return ((a & 0x00FFFFFF) << 8) | ((b & 0xFF000000) >> 24);
			}
			else if (offset == 2)
			{
				uint a = InternalRead32Ex(address - 2);
				uint b = InternalRead32Ex(address + 2);

				return ((a & 0x0000FFFF) << 16) | ((b & 0xFFFF0000) >> 16);
			}
			else if (offset == 3)
			{
				uint a = InternalRead32Ex(address - 3);
				uint b = InternalRead32Ex(address + 1);

				return ((a & 0x000000FF) << 24) | ((b & 0xFFFFFF00) >> 8);
			}

			throw new InvalidProgramException();
		}

		protected ushort InternalRead16(ulong address)
		{
			var offset = address % 4;

			uint a = InternalRead32Ex(address - offset);

			if (offset == 0)
			{
				return (ushort)((a & 0xFFFF0000) >> 16);
			}
			else if (offset == 1)
			{
				return (ushort)((a & 0x00FFFF00) >> 8);
			}
			else if (offset == 2)
			{
				return (ushort)(a & 0x0000FFFF);
			}
			else if (offset == 3)
			{
				uint b = InternalRead32Ex(address + 1);

				return (ushort)(((a & 0x000000FF) << 8) | ((b & 0xFF000000) >> 24));
			}

			throw new InvalidProgramException();
		}

		protected byte InternalRead8(ulong address)
		{
			uint offset = (uint)(address % 4);

			uint value = InternalRead32(address - offset);

			int shift = (3 - (int)offset) * 8;

			return (byte)((value >> shift) & 0xFF);
		}

		protected void InternalWrite32(ulong address, uint value)
		{
			uint offset = (uint)(address % 4);

			if (offset == 0)
			{
				InternalWrite32Ex(address, value, 0xFFFFFFFF);
			}
			else if (offset == 1)
			{
				InternalWrite32Ex(address - 1, value >> 8, 0x00FFFFFF);
				InternalWrite32Ex(address + 3, value << 24, 0xFF000000);
			}
			else if (offset == 2)
			{
				InternalWrite32Ex(address - 2, value >> 16, 0x0000FFFF);
				InternalWrite32Ex(address + 2, value << 16, 0xFFFF0000);
			}
			else if (offset == 3)
			{
				InternalWrite32Ex(address - 3, value >> 24, 0x000000FF);
				InternalWrite32Ex(address + 1, value << 8, 0xFFFFFF00);
			}

			// very slow performance if assert enabled
			//Debug.Assert(InternalRead32(address) == value);
		}

		protected void InternalWrite16(ulong address, ushort value)
		{
			uint offset = (uint)(address % 4);

			if (offset == 0)
			{
				InternalWrite32Ex(address, ((uint)value) << 16, 0xFFFF0000);
			}
			else if (offset == 1)
			{
				InternalWrite32Ex(address - 1, ((uint)value) << 8, 0x00FFFF00);
			}
			else if (offset == 2)
			{
				InternalWrite32Ex(address - 2, ((uint)value), 0x0000FFFF);
			}
			else if (offset == 3)
			{
				InternalWrite32Ex(address - 3, ((uint)value >> 8), 0x000000FF);
				InternalWrite32Ex(address + 1, ((uint)value << 24), 0xFF000000);
			}

			// very slow performance if assert enabled
			//Debug.Assert(InternalRead16(address) == value);
		}

		protected void InternalWrite8(ulong address, byte value)
		{
			uint offset = (uint)(address % 4);

			int shift = (3 - (int)offset) * 8;

			InternalWrite32Ex(address - offset, ((uint)value) << shift, (uint)0xFF << shift);

			// very slow performance if assert enabled
			//Debug.Assert(InternalRead8(address) == value);
		}

		public ulong DirectRead64(ulong address)
		{
			uint low = InternalRead32(address);
			uint high = InternalRead32(address + 0x4);
			ulong val = high | ((ulong)low << 32);

			var value = val;

			if (Endian.NativeIsLittleEndian)
			{
				value = Endian.Swap(value);
			}

			return value;
		}

		public uint DirectRead32(ulong address)
		{
			uint value = InternalRead32(address);

			if (Endian.NativeIsLittleEndian)
			{
				value = Endian.Swap(value);
			}

			return value;
		}

		public ushort DirectRead16(ulong address)
		{
			ushort value = InternalRead16(address);

			if (Endian.NativeIsLittleEndian)
			{
				value = Endian.Swap(value);
			}

			return value;
		}

		public byte DirectRead8(ulong address)
		{
			return InternalRead8(address);
		}

		public void DirectWrite64(ulong address, ulong value)
		{
			ulong val = value;

			if (Endian.NativeIsLittleEndian)
			{
				val = Endian.Swap(val);
			}

			uint low = (uint)(val >> 32);
			uint high = (uint)val;

			InternalWrite32(address, low);
			InternalWrite32(address + 0x4, high);

			// very slow performance if assert enabled
			//Debug.Assert(DirectRead64(address) == value);

			//Debug.WriteLine(address.ToString("X") + ": " + value.ToString("X"));

			MemoryUpdate(address, 64);
		}

		public void DirectWrite32(ulong address, uint value)
		{
			uint val = value;

			if (Endian.NativeIsLittleEndian)
			{
				val = Endian.Swap(val);
			}

			InternalWrite32(address, val);

			// very slow performance if assert enabled
			//Debug.Assert(DirectRead32(address) == value);

			//Debug.WriteLine(address.ToString("X") + ": " + value.ToString("X"));

			MemoryUpdate(address, 32);
		}

		public void DirectWrite16(ulong address, ushort value)
		{
			ushort val = value;

			if (Endian.NativeIsLittleEndian)
			{
				val = Endian.Swap(val);
			}

			InternalWrite16(address, val);

			// very slow performance if assert enabled
			//Debug.Assert(DirectRead16(address) == value);

			//Debug.WriteLine(address.ToString("X") + ": " + value.ToString("X"));

			MemoryUpdate(address, 16);
		}

		public void DirectWrite8(ulong address, byte value)
		{
			InternalWrite8(address, value);

			// very slow performance if assert enabled
			//Debug.Assert(DirectRead8(address) == value);

			//Debug.WriteLine(address.ToString("X") + ": " + value.ToString("X"));

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

		public void Write64(ulong address, ulong value)
		{
			DirectWrite64(TranslateToPhysical(address), value);
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

		public ulong Read64(ulong address)
		{
			return DirectRead64(TranslateToPhysical(address));
		}

		public void SetSymbol(string name, ulong address, ulong size)
		{
			Symbols.Add(name, new SimSymbol(name, address, size));
		}

		public void Write8Port(uint port, byte value)
		{
			var device = PortDevices[port];

			if (device == null)
				return;

			device.PortWrite(port, value);
		}

		public void Write16Port(uint port, ushort value)
		{
			Write8Port(port, (byte)(value & 0xFF));
			Write8Port(port + 1, (byte)((value >> 8) & 0xFF));
		}

		public void Write32Port(uint port, uint value)
		{
			Write8Port(port, (byte)(value & 0xFF));
			Write8Port(port + 1, (byte)((value >> 8) & 0xFF));
			Write8Port(port + 2, (byte)((value >> 16) & 0xFF));
			Write8Port(port + 3, (byte)((value >> 24) & 0xFF));
		}

		public byte Read8Port(uint port)
		{
			var device = PortDevices[port];

			if (device == null)
				return 0;

			return device.PortRead(port);
		}

		public ushort Read16Port(uint port)
		{
			return (ushort)(Read8Port(port) | (Read8Port(port + 1) >> 8));
		}

		public uint Read32Port(uint port)
		{
			return (uint)(Read8Port(port) | (Read8Port(port + 1) >> 8) | (Read8Port(port + 2) >> 16) | (Read8Port(port + 3) >> 24));
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

		public virtual string GetDumpHeaders()
		{
			return string.Empty;
		}

		public virtual string GetDump()
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
					string info = GetSourceInformation(LastProgramCounter) ?? string.Empty;

					Debug.Write(Tick.ToString());
					Debug.Write('\t');
					Debug.Write(GetDump());
					Debug.Write('\t');
					Debug.WriteLine(LastProgramCounter.ToString("X") + ": " + LastInstruction.ToString() + '\t' + info);
				}
			}
			catch (SimCPUException e)
			{
				if (Monitor.DebugOutput)
				{
					Debug.WriteLine("SIM: " + e.ToString());
				}
				LastException = e;
			}
			catch (NotSupportedException e)
			{
				if (Monitor.DebugOutput)
				{
					Debug.WriteLine(e.ToString());
				}
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
					Debug.Write("Tick\t");
					Debug.WriteLine(GetDumpHeaders());
				}

				for (;;)
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
