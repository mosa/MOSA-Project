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
        public delegate IReadWriteIOPort CreatePort(ushort port);

        static private CreatePort createPort;

        /// <summary>
        /// Sets the create port method.
        /// </summary>
        /// <param name="createPort">The create port.</param>
        public static void SetCreatePortMethod(CreatePort createPort)
        {
            HAL.createPort = createPort;
        }

        /// <summary>
        /// Requests an IO read/write port interface from the kernel
        /// </summary>
        /// <param name="portNbr">The port number.</param>
        /// <returns></returns>
        public static IReadWriteIOPort RequestIOPort(ushort port)
        {
            return createPort(port);
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

