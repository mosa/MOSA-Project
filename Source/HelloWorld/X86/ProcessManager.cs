/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Kernel.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class ProcessManager
	{
		private static uint _tablepages = 16;

		private static uint _table;
		private static uint _nextid;
		private static uint _slots;

		/// <summary>
		/// Setups this instance.
		/// </summary>
		public unsafe static void Setup()
		{
			// Allocate pages for the process table
			_table = VirtualPageAllocator.Reserve(_tablepages);
			_slots = (uint)(_tablepages * PageFrameAllocator.PageSize / sizeof(Process));
			_nextid = 0;
		}

		/// <summary>
		/// Creates the process.
		/// </summary>
		/// <returns></returns>
		public unsafe static uint CreateProcess()
		{
			// TODO: Lock

			Process* process = FindEmptySlot();
			process->ProcessId = ++_nextid;
			process->MemoryMap = VirtualPageAllocator.Reserve(32);	// 32 pages for entire 4GB
			process->Status = Process.State.Running;

			// TODO: Unlock

			return process->ProcessId;
		}

		/// <summary>
		/// Terminates the process.
		/// </summary>
		/// <param name="pid">The pid.</param>
		public static void TerminateProcess(uint pid)
		{
			// TODO

			// 1. Set status to terminating
			// 2. Stop all threads
			// 3. Release all memory
		}

		/// <summary>
		/// Finds an empty slot.
		/// </summary>
		/// <returns></returns>
		private unsafe static Process* FindEmptySlot()
		{
			for (uint slot = 0; slot < _slots; slot++) {
				Process* process = (Process*)(_table + sizeof(Process) * slot);

				if (process->Status == Process.State.Empty)
					return process;
			}

			return null;
		}
	}
}
