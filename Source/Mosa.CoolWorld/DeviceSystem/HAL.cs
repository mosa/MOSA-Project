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
	public static class HAL
	{
		/// <summary>
		/// 
		/// </summary>
		static private IHardwareAbstraction hardwareAbstraction;

		/// <summary>
		/// 
		/// </summary>
		public delegate void HandleInterrupt(byte irq, byte error);

		static private HandleInterrupt handleInterrupt;

		/// <summary>
		/// Sets the hardware abstraction.
		/// </summary>
		/// <param name="hardwareAbstraction">The hardware abstraction.</param>
		public static void SetHardwareAbstraction(IHardwareAbstraction hardwareAbstraction)
		{
			HAL.hardwareAbstraction = hardwareAbstraction;
		}

		/// <summary>
		/// Sets the interrupt handler.
		/// </summary>
		/// <param name="handleInterrupt">The handle interrupt.</param>
		public static void SetInterruptHandler(HandleInterrupt handleInterrupt)
		{
			HAL.handleInterrupt = handleInterrupt;
		}

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public static void ProcessInterrupt(byte irq, byte error)
		{
			handleInterrupt(irq, error);
		}

		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		internal static IReadWriteIOPort RequestIOPort(ushort port)
		{
			return hardwareAbstraction.RequestIOPort(port);
		}

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		internal static IMemory RequestPhysicalMemory(uint address, uint size)
		{
			return hardwareAbstraction.RequestPhysicalMemory(address, size);
		}

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		internal static void DisableAllInterrupts()
		{
			hardwareAbstraction.DisableAllInterrupts();
		}

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		internal static void EnableAllInterrupts()
		{
			hardwareAbstraction.EnableAllInterrupts();
		}

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		public static void Sleep(uint milliseconds)
		{
			hardwareAbstraction.Sleep(milliseconds);
		}

		/// <summary>
		/// Gets the physical address.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <returns></returns>
		public static uint GetPhysicalAddress(IMemory memory)
		{
			return hardwareAbstraction.GetPhysicalAddress(memory);
		}

		/// <summary>
		/// Allocates the memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public static IMemory AllocateMemory(uint size, uint alignment)
		{
			return HAL.AllocateMemory(size, alignment);
		}
	}
}

