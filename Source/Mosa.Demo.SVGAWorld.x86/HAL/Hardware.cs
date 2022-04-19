// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Demo.SVGAWorld.x86.HAL
{
	/// <summary>
	/// Hardware
	/// </summary>
	public sealed class Hardware : BaseHardwareAbstraction
	{
		/// <summary>
		/// Gets the size of the page.
		/// </summary>
		public override uint PageSize { get { return PageFrameAllocator.PageSize; } }

		/// <summary>
		/// Gets a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public override ConstrainedPointer GetPhysicalMemory(Pointer address, uint size)
		{
			var start = (uint)address.ToInt32();

			// Map physical memory space to virtual memory space
			for (var at = start; at < start + size; at += PageSize)
			{
				PageTable.MapVirtualAddressToPhysical(at, at);
			}

			return new ConstrainedPointer(address, size);
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
		/// Allocates the virtual memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public override ConstrainedPointer AllocateVirtualMemory(uint size, uint alignment)
		{
			var address = KernelMemory.AllocateVirtualMemory(size);

			return new ConstrainedPointer(address, size);
		}

		/// <summary>
		/// Gets the physical address.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <returns></returns>
		public override Pointer TranslateVirtualToPhysicalAddress(Pointer virtualAddress)
		{
			return PageTable.GetPhysicalAddressFromVirtual(virtualAddress);
		}

		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public override BaseIOPortReadWrite GetReadWriteIOPort(ushort port)
		{
			return new X86IOPortReadWrite(port);
		}

		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public override BaseIOPortRead GetReadIOPort(ushort port)
		{
			return new X86IOPortReadWrite(port);
		}

		/// <summary>
		/// Requests an IO write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public override BaseIOPortWrite GetWriteIOPort(ushort port)
		{
			return new X86IOPortWrite(port);
		}

		/// <summary>
		/// Debugs the write.
		/// </summary>
		/// <param name="message">The message.</param>
		public override void DebugWrite(string message)
		{
			Console.Write(message);
		}

		/// <summary>
		/// Debugs the write line.
		/// </summary>
		/// <param name="message">The message.</param>
		public override void DebugWriteLine(string message)
		{
			Console.WriteLine(message);
		}

		/// <summary>
		/// Aborts with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public override void Abort(string message)
		{
			Panic.Error(message);
		}

		/// <summary>
		/// Pause
		/// </summary>
		public override void Pause()
		{
			for (var i = Scheduler.ClockTicks + 5; i > Scheduler.ClockTicks;)
				Native.Hlt();
		}
	}
}
