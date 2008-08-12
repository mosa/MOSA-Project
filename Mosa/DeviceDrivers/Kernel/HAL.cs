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

		static private CreateIOPort createIOPort;

		/// <summary>
		/// Sets the create IO port factory.
		/// </summary>
		/// <param name="createIOPort">The create IO port.</param>
		public static void SetIOPortFactory(CreateIOPort createIOPort)
		{
			HAL.createIOPort = createIOPort;
		}

		/// <summary>
		/// Requests an IO read/write port interface from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		public static IReadWriteIOPort RequestIOPort(ushort port)
		{
			return createIOPort(port);
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
	}
}

