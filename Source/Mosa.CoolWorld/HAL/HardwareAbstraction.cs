/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.Kernel;
using Mosa.Platform.x86.Intrinsic;

namespace Mosa.CoolWorld.HAL
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
			return new IOPort(port);
		}

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public IMemory RequestMemory(uint address, uint size)
		{
			return new Memory(address, size);
		}

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		public void DisableAllInterrupts()
		{
			Native.Cli();
		}

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		public void EnableAllInterrupts()
		{
			Native.Sti();
		}

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public void ProcessInterrupt(byte irq)
		{
			Mosa.DeviceSystem.HAL.ProcessInterrupt(irq);
		}

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		public void Sleep(uint milliseconds)
		{
		}

		/// <summary>
		/// Allocates the memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public IMemory AllocateMemory(uint size, uint alignment)
		{
			uint address = KernelMemory.AllocateMemory(size);

			return new Memory(address, size);
		}

	}
}

