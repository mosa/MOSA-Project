/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
*/

using System.Runtime.InteropServices;

namespace Mosa.Platform.Internal.x86
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Intrinsic

		/// <summary>
		/// Wraps the x86 ldit instruction to load the interrupt descriptor table.
		/// </summary>
		/// <param name="address">The address.</param>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Lidt, Mosa.Platform.x86")]
		public extern static void Lidt(uint address);

		/// <summary>
		/// Wraps the x86 cli instruction to disable interrupts
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Cli, Mosa.Platform.x86")]
		public extern static void Cli();

		/// <summary>
		/// Wraps the x86 lgdt instruction to load global descriptor table
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Lgdt, Mosa.Platform.x86")]
		public extern static void Lgdt(uint address);

		/// <summary>
		/// Wraps the x86 sti instruction to enable interrupts
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Sti, Mosa.Platform.x86")]
		public extern static void Sti();

		/// <summary>
		/// Wraps the x86 in instruction to read from an 8-bit port.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.In, Mosa.Platform.x86")]
		public extern static byte In(ushort address);

		/// <summary>
		/// Wraps the x86 in instruction to read from an 8-bit port.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.In, Mosa.Platform.x86")]
		public extern static byte In8(ushort address);

		/// <summary>
		/// Wraps the x86 in instruction to read from a 16-bit port.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.In, Mosa.Platform.x86")]
		public extern static ushort In16(ushort address);

		/// <summary>
		/// Wraps the x86 in instruction to read from a 32-bit port.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.In, Mosa.Platform.x86")]
		public extern static uint In32(ushort address);

		/// <summary>
		/// Wraps the x86 out instruction to write to an 8-bit port.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Out, Mosa.Platform.x86")]
		public extern static void Out8(ushort address, byte value);

		/// <summary>
		/// Wraps the x86 out instruction to write to an 8-bit port.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Out, Mosa.Platform.x86")]
		public extern static void Out(ushort address, byte value);

		/// <summary>
		/// Wraps the x86 out instruction to write to a 16-bit port.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Out, Mosa.Platform.x86")]
		public extern static void Out16(ushort address, ushort value);

		/// <summary>
		/// Wraps the x86 out instruction to write to a 32-bit port.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Out, Mosa.Platform.x86")]
		public extern static void Out32(ushort address, uint value);

		/// <summary>
		/// Wraps the x86 out instruction to write to a nop instruction.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Nop, Mosa.Platform.x86")]
		public extern static void Nop();

		/// <summary>
		/// Wraps the x86 hlt instruction.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Hlt, Mosa.Platform.x86")]
		public extern static void Hlt();

		/// <summary>
		///
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Invlpg, Mosa.Platform.x86")]
		public extern static void Invlpg(uint address);

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CpuIdEax, Mosa.Platform.x86")]
		public extern static uint CpuIdEax(uint function);

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CpuIdEbx, Mosa.Platform.x86")]
		public extern static uint CpuIdEbx(uint function);

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CpuIdEcx, Mosa.Platform.x86")]
		public extern static uint CpuIdEcx(uint function);

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CpuIdEdx, Mosa.Platform.x86")]
		public extern static uint CpuIdEdx(uint function);

		/// <summary>
		///
		/// </summary>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.BochsDebug, Mosa.Platform.x86")]
		public extern static void BochsDebug();

		/// <summary>
		/// Sets the control register 0.
		/// </summary>
		/// <param name="status">The status.</param>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetCR0, Mosa.Platform.x86")]
		public extern static void SetCR0(uint status);

		/// <summary>
		/// Sets the control register 2.
		/// </summary>
		/// <param name="status">The status.</param>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetCR2, Mosa.Platform.x86")]
		public extern static void SetCR2(uint status);

		/// <summary>
		/// Sets the control register 3.
		/// </summary>
		/// <param name="status">The status.</param>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetCR3, Mosa.Platform.x86")]
		public extern static void SetCR3(uint status);

		/// <summary>
		/// Sets the control register 4.
		/// </summary>
		/// <param name="status">The status.</param>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetCR4, Mosa.Platform.x86")]
		public extern static void SetCR4(uint status);

		/// <summary>
		/// Gets the control register 0.
		/// </summary>
		/// <returns></returns>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCR0, Mosa.Platform.x86")]
		public extern static uint GetCR0();

		/// <summary>
		/// Gets the control register 2.
		/// </summary>
		/// <returns></returns>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCR2, Mosa.Platform.x86")]
		public extern static uint GetCR2();

		/// <summary>
		/// Gets the control register 3.
		/// </summary>
		/// <returns></returns>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCR3, Mosa.Platform.x86")]
		public extern static uint GetCR3();

		/// <summary>
		/// Gets the control register 4.
		/// </summary>
		/// <returns></returns>
		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCR4, Mosa.Platform.x86")]
		public extern static uint GetCR4();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Get, Mosa.Platform.x86")]
		public extern static byte Get8(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Get, Mosa.Platform.x86")]
		public extern static ushort Get16(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Get, Mosa.Platform.x86")]
		public extern static uint Get32(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Get, Mosa.Platform.x86")]
		public extern static ulong Get64(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Set, Mosa.Platform.x86")]
		public extern static void Set8(uint address, byte value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Set, Mosa.Platform.x86")]
		public extern static void Set16(uint address, ushort value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Set, Mosa.Platform.x86")]
		public extern static void Set32(uint address, uint value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Set, Mosa.Platform.x86")]
		public extern static void Set64(uint address, ulong value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetEBP, Mosa.Platform.x86")]
		public extern static uint GetEBP();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Div, Mosa.Platform.x86")]
		public extern static uint Div(ulong n, uint d);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SyncCompareAndSwap, Mosa.Platform.x86")]
		public extern static bool SyncCompareAndSwap(ref bool spinlock, int oldval, int newval);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.LockSet, Mosa.Platform.x86")]
		public extern static void SyncSet(ref bool spinlock, uint value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Pause, Mosa.Platform.x86")]
		public extern static void Pause();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetIDTJumpLocation, Mosa.Platform.x86")]
		public extern static uint GetIDTJumpLocation(uint irq);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetAssemblyListTable, Mosa.Platform.x86")]
		public extern static uint* GetAssemblyListTable();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetMethodLookupTable, Mosa.Platform.x86")]
		public extern static uint* GetMethodLookupTable();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SwitchTask, Mosa.Platform.x86")]
		public extern static void SwitchTask(uint esp);

		#endregion Intrinsic
	}
}