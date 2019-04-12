// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// HAL
	/// </summary>
	public static class HAL
	{
		/// <summary>
		/// The hardware abstraction
		/// </summary>
		static private BaseHardwareAbstraction hardwareAbstraction;

		/// <summary>
		/// Interrupt Delegate
		/// </summary>
		/// <param name="irq">The irq.</param>
		public delegate void HandleInterrupt(byte irq);

		static private HandleInterrupt handleInterrupt;

		/// <summary>
		/// Sets the hardware abstraction.
		/// </summary>
		/// <param name="hardwareAbstraction">The hardware abstraction.</param>
		public static void SetHardwareAbstraction(BaseHardwareAbstraction hardwareAbstraction)
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
		public static void ProcessInterrupt(byte irq)
		{
			handleInterrupt?.Invoke(irq);
		}

		/// <summary>
		/// Requests an IO read/write port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		internal static BaseIOPortReadWrite GetReadWriteIOPort(ushort port)
		{
			return hardwareAbstraction.GetReadWriteIOPort(port);
		}

		/// <summary>
		/// Requests an IO read port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		internal static BaseIOPortRead GetReadIOPort(ushort port)
		{
			return hardwareAbstraction.GetReadIOPort(port);
		}

		/// <summary>
		/// Requests an IO read/write port object from the kernel
		/// </summary>
		/// <param name="port">The port number.</param>
		/// <returns></returns>
		internal static BaseIOPortWrite GetWriteIOPort(ushort port)
		{
			return hardwareAbstraction.GetWriteIOPort(port);
		}

		/// <summary>
		/// Requests a block of memory from the kernel
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static ConstrainedPointer GetPhysicalMemory(IntPtr address, uint size)
		{
			return hardwareAbstraction.GetPhysicalMemory(address, size);
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
		/// Allocates the memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public static ConstrainedPointer AllocateMemory(uint size, uint alignment)
		{
			return hardwareAbstraction.AllocateVirtualMemory(size, alignment);
		}

		/// <summary>
		/// Gets the physical address.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <returns></returns>
		public static IntPtr TranslateVirtualToPhysicalAddress(IntPtr memory)
		{
			return hardwareAbstraction.TranslateVirtualToPhysicalAddress(memory);
		}

		/// <summary>
		/// Debugs the write.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void DebugWrite(string message)
		{
			hardwareAbstraction.DebugWrite(message);
		}

		/// <summary>
		/// Debugs the write line.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void DebugWriteLine(string message)
		{
			hardwareAbstraction.DebugWriteLine(message);
		}

		/// <summary>
		/// Aborts with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void Abort(string message)
		{
			hardwareAbstraction.Abort(message);
		}
	}
}
