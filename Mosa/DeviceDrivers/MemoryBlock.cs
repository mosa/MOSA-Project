/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

#if XXXXXX

namespace Mosa.DeviceDrivers
{
	/// <summary>
	/// Represents a block of memory
	/// </summary>
	public sealed class MemoryBlock : IMemory
	{
		/// <summary>
		/// Start of memory block
		/// </summary>
		private IntPtr address;

		/// <summary>
		/// Size of memory block
		/// </summary>
		private uint size;

		public uint Address { get { return (uint)((sizeof(uint) == 4) ? (address.ToInt32()) : (address.ToInt64())); } }
		public uint Size { get { return size; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryBlock"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		public MemoryBlock(IntPtr address, uint size)
		{
			this.address = address;
			this.size = size;
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Byte"/> at the specified index.
		/// </summary>
		/// <value></value>
		public unsafe byte this[uint index]
		{
			get
			{
				if (index > size) return 0;

				return (byte)*((byte*)address.ToPointer() + index);
			}
			set
			{
				if (index > size) return;

				*((byte*)address.ToPointer() + index) = value;
			}
		}
	}
}

#endif