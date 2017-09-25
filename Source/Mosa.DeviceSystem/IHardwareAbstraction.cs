// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Hardware Abstraction intereface
	/// </summary>
	public interface IHardwareAbstraction
	{
		/// <summary>
		/// Requests an IO read/write port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		IOPortReadWrite RequestReadWriteIOPort(ushort port);

		/// <summary>
		/// Requests an IO read port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		IOPortRead RequestReadIOPort(ushort port);

		/// <summary>
		/// Requests an IO write port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		IOPortWrite RequestWriteIOPort(ushort port);

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		BaseMemory RequestPhysicalMemory(uint address, uint size);

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		void DisableAllInterrupts();

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		void EnableAllInterrupts();

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		void Sleep(uint milliseconds);

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		void ProcessInterrupt(byte irq);

		/// <summary>
		/// Allocates the memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		BaseMemory AllocateMemory(uint size, uint alignment);

		/// <summary>
		/// Gets the physical address.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <returns></returns>
		uint GetPhysicalAddress(BaseMemory memory);

		/// <summary>
		/// Debugs the write.
		/// </summary>
		/// <param name="message">The message.</param>
		void DebugWrite(string message);

		/// <summary>
		/// Debugs the write line.
		/// </summary>
		/// <param name="message">The message.</param>
		void DebugWriteLine(string message);

		/// <summary>
		/// Aborts with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Abort(string message);
	}
}
