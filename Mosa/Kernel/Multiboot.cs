/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// http://www.gnu.org/software/grub/manual/multiboot/multiboot.html

namespace Mosa.Kernel
{

	/// <summary>
	/// 
	/// </summary>
	public class Multiboot
	{
		/// <summary>
		/// Magic number for the Multiboot header
		/// </summary>
		public static uint Magic = 0x1BADB002;

		/// <summary>
		/// The starting location of the kernel
		/// </summary>
		protected static uint kernelStart;
		/// <summary>
		/// The ending location of the kernel
		/// </summary>
		protected static uint kernelEnd;

		/// <summary>
		/// Gets the kernel start.
		/// </summary>
		/// <value>The kernel start.</value>
		public static uint KernelStart { get { return kernelStart; } }

		/// <summary>
		/// Gets the kernel end.
		/// </summary>
		/// <value>The kernel end.</value>
		public static uint KernelEnd { get { return kernelEnd; } }

		/// <summary>
		/// Reads the multiboot info table.
		/// </summary>
		public static void ReadMultibootInfoTable()
		{
			// TODO
			// Set kernel start
			// Set kernel end
		}

	}

}

