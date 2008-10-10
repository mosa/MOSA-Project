/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.EmulatedKernel
{
	/// <summary>
	/// 
	/// </summary>
	public class HardwareAbstraction : IHardwareAbstraction
	{
		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public IReadWriteIOPort RequestIOPort(ushort port)
		{
			return IOPortDispatch.RegisterIOPort(port);
		}

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public IMemory RequestMemory(uint address, uint size)
		{
			return MemoryDispatch.RegisterMemory(address, size);
		}

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		public void DisableAllInterrupts()
		{
		}

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		public void EnableAllInterrupts()
		{
		}

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public void ProcessInterrupt(byte irq)
		{
			HAL.ProcessInterrupt(irq);
		}

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		public void Sleep(uint milliseconds)
		{
		}
	}
}

