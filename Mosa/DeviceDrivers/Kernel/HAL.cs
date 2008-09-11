/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers.Kernel
{
	public static class HAL
	{
		public delegate IReadWriteIOPort CreateIOPort(ushort port);
		public delegate IMemory CreateMemory(uint address, uint size);
		public delegate void HandleInterrupt(byte irq);

		static private CreateIOPort createIOPort;
		static private CreateMemory createMemory;
		static private HandleInterrupt handleInterrupt;

		/// <summary>
		/// Sets the create IO port factory.
		/// </summary>
		/// <param name="createIOPort">The create IO port factory method.</param>
		public static void SetIOPortFactory(CreateIOPort createIOPort)
		{
			HAL.createIOPort = createIOPort;
		}

		/// <summary>
		/// Sets the create memory factory.
		/// </summary>
		/// <param name="createMemory">The memory factory method.</param>
		public static void SetMemoryFactory(CreateMemory createMemory)
		{
			HAL.createMemory = createMemory;
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
		public static void ProcessInterrupt(byte irq)
		{
			HAL.handleInterrupt(irq);
		}

		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		internal static IReadWriteIOPort RequestIOPort(ushort port)
		{
			return HAL.createIOPort(port);
		}

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		internal static IMemory RequestMemory(uint address, uint size)
		{
			return HAL.createMemory(address, size);
		}

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		internal static void DisableAllInterrupts()
		{
			// TODO
		}

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		internal static void EnableAllInterrupts()
		{
			// TODO
		}

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		internal static void Sleep(uint milliseconds)
		{
			// TODO
		}

		/// <summary>
		/// Delays for port IO.
		/// </summary>
		internal static void DelayForPortIO()
		{
			// Must delay for at least 4 ISA bus clocks
			// Common approaches are to read and write to port 0x80, or 0xED 
			// More advanced approaches check DMI (blacklist) to determine if port 0x80 is unsafe, and to use port 0xED instead
			// Port 0x80 might not be safe on some HP laptops
		}
	}
}

