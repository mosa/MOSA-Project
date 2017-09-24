// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a region of memory
	/// </summary>
	public abstract class BaseMemory
	{
		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public uint Address { get; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public uint Size { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseMemory" /> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		protected BaseMemory(uint address, uint size)
		{
			this.Address = address;
			this.Size = size;
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Byte" /> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="System.Byte"/>.
		/// </value>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public abstract byte this[uint index] { get; set; }

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public abstract byte Read8(uint index);

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public abstract void Write8(uint index, byte value);

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public abstract ushort Read16(uint index);

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public abstract void Write24(uint index, uint value);

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public abstract uint Read24(uint index);

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public abstract void Write16(uint index, ushort value);

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public abstract uint Read32(uint index);

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public abstract void Write32(uint index, uint value);
	}
}
