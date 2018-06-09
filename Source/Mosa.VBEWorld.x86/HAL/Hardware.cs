// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;
using System;

namespace Mosa.VBEWorld.x86.HAL
{
	/// <summary>
	/// Hardware
	/// </summary>
	public sealed class Hardware : BaseHardwareAbstraction
	{
		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public override Memory RequestPhysicalMemory(uint address, uint size)
		{
			// Map physical memory space to virtual memory space
			for (uint at = address; at < address + size; at += 4096)
			{
				PageTable.MapVirtualAddressToPhysical(at, at);
			}

			return new Memory(new IntPtr(address), size);
		}

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		public override void DisableAllInterrupts()
		{
			Native.Cli();
		}

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		public override void EnableAllInterrupts()
		{
			Native.Sti();
		}

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public override void ProcessInterrupt(byte irq)
		{
			DeviceSystem.HAL.ProcessInterrupt(irq);
		}

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		public override void Sleep(uint milliseconds)
		{
		}

		/// <summary>
		/// Allocates the memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public override Memory AllocateMemory(uint size, uint alignment)
		{
			var address = KernelMemory.AllocateMemory(size);

			return new Memory(address, size);
		}

		/// <summary>
		/// Gets the physical address.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <returns></returns>
		public override uint GetPhysicalAddress(Memory memory)
		{
			return PageTable.GetPhysicalAddressFromVirtual(memory.Address);
		}

		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public override IOPortReadWrite RequestReadWriteIOPort(ushort port)
		{
			throw new Exception("Unimplemented");
		}

		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public override IOPortRead RequestReadIOPort(ushort port)
		{
			throw new Exception("Unimplemented");
		}

		/// <summary>
		/// Requests an IO write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public override IOPortWrite RequestWriteIOPort(ushort port)
		{
			throw new Exception("Unimplemented");
		}

		/// <summary>
		/// Debugs the write.
		/// </summary>
		/// <param name="message">The message.</param>
		public override void DebugWrite(string message)
		{
		}

		/// <summary>
		/// Debugs the write line.
		/// </summary>
		/// <param name="message">The message.</param>
		public override void DebugWriteLine(string message)
		{
		}

		/// <summary>
		/// Aborts with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public override void Abort(string message)
		{
			Panic.Error(message);
		}
	}
}
