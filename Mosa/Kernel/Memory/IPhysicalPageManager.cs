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
	/// Interface of a physical page manager.
	/// </summary>
	/// <remarks>
	/// This interface defines the abstract operations to allocate and free physical page level.
	/// </remarks>
	public interface IPhysicalPageManager
	{
		/// <summary>
		/// Remove a physical page
		/// </summary>
		/// <returns>An IntPtr to the allocated memory.</returns>
		ulong Allocate();

		/// <summary>
		/// Releases a page
		/// </summary>
		/// <param name="address">The starting address of the page to freed.</param>
		void Free(ulong address);

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		ulong PageSize { get; }

		/// <summary>
		/// Retrieves the amount of total physical memory pages available in the system.
		/// </summary>
		ulong TotalPages { get; }

		/// <summary>
		/// Retrieves the amount of number of physical pages in use.
		/// </summary>
		ulong TotalPagesInUse { get; }
	}
}
