// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Extension;
using System;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// MemoryRegion
	/// </summary>
	public struct AddressRegion
	{
		/// <summary>
		/// Gets the base address.
		/// </summary>
		public IntPtr Address { get; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		public uint Size { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AddressRegion"/> class.
		/// </summary>
		/// <param name="address">The base address.</param>
		/// <param name="size">The size.</param>
		public AddressRegion(IntPtr address, uint size)
		{
			Address = address;
			Size = size;
		}

		/// <summary>
		/// Determines whether [contains] [the specified address].
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified address]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(IntPtr address)
		{
			return address.GreaterThanOrEqual(Address) && address.LessThan(Address + (int)Size);
		}
	}
}
