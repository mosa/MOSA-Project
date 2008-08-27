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

		static private CreateIOPort createIOPort;
		static private CreateMemory createMemory;

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
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public static IReadWriteIOPort RequestIOPort(ushort port)
		{
			return HAL.createIOPort(port);
		}

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static IMemory RequestMemory(uint address, uint size)
		{
			return HAL.createMemory(address, size);
		}

		/// <summary>
		/// Disables all interrupts.
		/// </summary>
		public static void DisableAllInterrupts()
		{
			// TODO
		}

		/// <summary>
		/// Enables all interrupts.
		/// </summary>
		public static void EnableAllInterrupts()
		{
			// TODO
		}

		/// <summary>
		/// Sleeps the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The milliseconds.</param>
		public static void Sleep(uint milliseconds)
		{
			// TODO
		}
	}
}

