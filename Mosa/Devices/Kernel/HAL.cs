/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Devices.Kernel
{
    public static class HAL
    {
        /// <summary>
        /// Requests an IO read/write port interface from the kernel
        /// </summary>
        /// <param name="portNbr">The port number.</param>
        /// <returns></returns>
        public static IReadWriteIOPort RequestIOPort(ushort port)
        {
            return new IOPortKernelAdapter(Ensemble.Kernel.BootEntry.ResourceManager.RequestIOPort(port));
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
