// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// MemoryRegion
	/// </summary>
	public sealed class MemoryRegion
	{
		/// <summary>
		/// Gets the base address.
		/// </summary>
		public uint BaseAddress { get; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		public uint Size { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryRegion"/> class.
		/// </summary>
		/// <param name="baseAddress">The base address.</param>
		/// <param name="size">The size.</param>
		public MemoryRegion(uint baseAddress, uint size)
		{
			BaseAddress = baseAddress;
			Size = size;
		}

		/// <summary>
		/// Determines whether [contains] [the specified address].
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified address]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(uint address)
		{
			return ((address >= BaseAddress) && (address <= BaseAddress + Size));
		}
	}
}
