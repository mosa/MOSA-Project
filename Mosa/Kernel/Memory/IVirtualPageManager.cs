/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace Mosa.Kernel.Memory
{
	/// <summary>
	/// Interface of a virtual page manager.
	/// </summary>
	/// <remarks>
	/// This interface defines the abstract operations to allocate, free and manage ranges
	/// of virtual memory at the page level.
	/// </remarks>
	public interface IVirtualPageManager
	{
		/// <summary>
		/// Reserves or commits a range of pages.
		/// </summary>
		/// <param name="pages">The pages to allocate</param>
		/// <param name="protectionFlags">One or more flag that controls the protection of the retrieved pages.</param>
		/// <returns>An IntPtr to the allocated memory.</returns>
		System.IntPtr Allocate(ulong pages, PageProtectionFlags protectionFlags);

		/// <summary>
		/// Releases or decommits a range of pages.
		/// </summary>
		/// <param name="address">The starting address, where pages are freed.</param>
		/// <param name="pages">The number of pages.</param>
		void Free(System.IntPtr address, ulong pages);

		/// <summary>
		/// Changes the protection bits of the pages associated with the given range of memory.
		/// </summary>
		/// <param name="address">The starting address.</param>
		/// <param name="pages">The number of pages.</param>
		/// <param name="protectionFlags">The new set of protection flags.</param>
		/// <returns>The old protection flags of the first page in the range of memory. </returns>
		PageProtectionFlags Protect(System.IntPtr address, ulong pages, PageProtectionFlags protectionFlags);

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		ulong PageSize { get; }

		/// <summary>
		/// Retrieves the amount of virtual memory available in the system.
		/// </summary>
		ulong TotalMemory { get; }

		/// <summary>
		/// Retrieves the amount of virtual memory currently in use.
		/// </summary>
		ulong TotalMemoryInUse { get; }
	}
}
