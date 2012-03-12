/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public interface IHardwareAbstraction
	{
		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		IReadWriteIOPort RequestIOPort(ushort port);

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		IMemory RequestPhysicalMemory(uint address, uint size);

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
		void ProcessInterrupt(byte irq, byte error);

		/// <summary>
		/// Allocates the memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		IMemory AllocateMemory(uint size, uint alignment);

		/// <summary>
		/// Gets the physical address.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <returns></returns>
		uint GetPhysicalAddress(IMemory memory);
	}
}

