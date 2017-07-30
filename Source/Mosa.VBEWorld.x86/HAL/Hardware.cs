// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;
using System;

namespace Mosa.VBEWorld.x86.HAL
{
	/// <summary>
	///
	/// </summary>
	public sealed class Hardware : IHardwareAbstraction
	{
		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		IReadWriteIOPort IHardwareAbstraction.RequestIOPort(ushort port)
		{
			throw new Exception("Unimplemented");
		}

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public IMemory RequestPhysicalMemory(uint address, uint size)
		{
			// Map physical memory space to virtual memory space
			for (uint at = address; at < address + size; at += 4096)
			{
				PageTable.MapVirtualAddressToPhysical(at, at, true);
			}

			return new Memory(address, size);
		}

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		void IHardwareAbstraction.DisableAllInterrupts()
		{
			Native.Cli();
		}

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		void IHardwareAbstraction.EnableAllInterrupts()
		{
			Native.Sti();
		}

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		void IHardwareAbstraction.ProcessInterrupt(byte irq)
		{
			Mosa.HardwareSystem.HAL.ProcessInterrupt(irq);
		}

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		void IHardwareAbstraction.Sleep(uint milliseconds)
		{
		}

		/// <summary>
		/// Allocates the memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		IMemory IHardwareAbstraction.AllocateMemory(uint size, uint alignment)
		{
			uint address = KernelMemory.AllocateMemory(size);

			return new Memory(address, size);
		}

		/// <summary>
		/// Gets the physical address.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <returns></returns>
		uint IHardwareAbstraction.GetPhysicalAddress(IMemory memory)
		{
			return PageTable.GetPhysicalAddressFromVirtual(memory.Address);
		}

		/// <summary>
		/// Debugs the write.
		/// </summary>
		/// <param name="message">The message.</param>
		void IHardwareAbstraction.DebugWrite(string message)
		{
			Boot.Log(message);
		}

		/// <summary>
		/// Debugs the write line.
		/// </summary>
		/// <param name="message">The message.</param>
		void IHardwareAbstraction.DebugWriteLine(string message)
		{
			Boot.Log(message);
		}

		/// <summary>
		/// Aborts with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		void IHardwareAbstraction.Abort(string message)
		{
			Panic.Error(message);
		}
	}
}
