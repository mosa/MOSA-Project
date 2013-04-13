/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Test.System
{
	/// <summary>
	/// Interface of a memory page manager.
	/// </summary>
	/// <remarks>
	/// This interface defines the abstract operations to allocate, free and manage ranges of memory at the page level.
	/// </remarks>
	public interface IMemoryPageManager
	{
		/// <summary>
		/// Reserves or commits a range of pages.
		/// </summary>
		/// <param name="address">A starting address from a previous call, which reserved memory or IntPtr.Zero.</param>
		/// <param name="size">The number of bytes to reserve.</param>
		/// <param name="protectionFlags">One or more flag that controls the protection of the retrieved pages.</param>
		/// <returns>An IntPtr to the allocated memory.</returns>
		IntPtr Allocate(IntPtr address, ulong size, PageProtectionFlags protectionFlags);

		/// <summary>
		/// Releases or decommits a range of pages.
		/// </summary>
		/// <param name="address">The starting address, where pages are freed.</param>
		/// <param name="size">The number of bytes to free.</param>
		void Free(IntPtr address, ulong size);

		/// <summary>
		/// Changes the protection bits of the pages associated with the given range of memory.
		/// </summary>
		/// <param name="address">The starting address.</param>
		/// <param name="size">The number of bytes.</param>
		/// <param name="protectionFlags">The new set of protection flags.</param>
		/// <returns>The old protection flags of the first page in the range of memory. </returns>
		PageProtectionFlags Protect(IntPtr address, ulong size, PageProtectionFlags protectionFlags);
	}
}