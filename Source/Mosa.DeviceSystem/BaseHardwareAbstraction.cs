// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Hardware
	/// </summary>
	public abstract class BaseHardwareAbstraction
	{
		/// <summary>
		/// Gets the size of the page.
		/// </summary>
		public abstract uint PageSize { get; }

		/// <summary>
		/// Gets an IO read/write port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public abstract BaseIOPortReadWrite GetReadWriteIOPort(ushort port);

		/// <summary>
		/// Gets an IO read port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public abstract BaseIOPortRead GetReadIOPort(ushort port);

		/// <summary>
		/// Gets an IO write port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public abstract BaseIOPortWrite GetWriteIOPort(ushort port);

		/// <summary>
		/// Gets a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public abstract ConstrainedPointer GetPhysicalMemory(IntPtr address, uint size);

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		public abstract void DisableAllInterrupts();

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		public abstract void EnableAllInterrupts();

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		public abstract void Sleep(uint milliseconds);

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public abstract void ProcessInterrupt(byte irq);

		/// <summary>
		/// Allocates the virtual memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public abstract ConstrainedPointer AllocateVirtualMemory(uint size, uint alignment);

		/// <summary>
		/// Gets the physical address.
		/// </summary>
		/// <param name="virtualAddress"></param>
		/// <returns></returns>
		public abstract IntPtr TranslateVirtualToPhysicalAddress(IntPtr virtualAddress);

		/// <summary>
		/// Debugs the write.
		/// </summary>
		/// <param name="message">The message.</param>
		public abstract void DebugWrite(string message);

		/// <summary>
		/// Debugs the write line.
		/// </summary>
		/// <param name="message">The message.</param>
		public abstract void DebugWriteLine(string message);

		/// <summary>
		/// Aborts with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public abstract void Abort(string message);
	}
}
