﻿/*
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

		private byte[][] MemoryBlocks;

		internal static ulong BlockSize = 1024 * 1024; // 1 MB
		internal static ulong MaxMemory = 1024L * 1024L * 1024L * 4L; // 4 GB

		public SimCPU()
		{
			MemoryBlocks = new byte[MaxMemory / BlockSize][];
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

		private byte InternalRead8(ulong address)
		{
			ulong index = address / BlockSize;

			var block = MemoryBlocks[index];

			if (block == null)
			{
				// for performance we only check if the memory access is valid when memory is allocated by the simulator
				if (!IsValidMemoryReference(address))
					throw new InvalidMemoryAccess(address);

				block = new byte[BlockSize];

				MemoryBlocks[index] = block;

				return 0;
			}

			return block[address % BlockSize];
		}

		private void InternalWrite8(ulong address, byte value)
		{
			ulong index = address / BlockSize;
			var block = MemoryBlocks[index];

			if (block == null)
			{
				if (value == 0)
					return;

				// for performance we only check if the memory access is valid when memory is allocated by the simulator
				if (!IsValidMemoryReference(address))
					throw new InvalidMemoryAccess(address);

				block = new byte[BlockSize];

				MemoryBlocks[index] = block;
			}

			block[address % BlockSize] = value;
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
					throw new NotSupportedException();

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