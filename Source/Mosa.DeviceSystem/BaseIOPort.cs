// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public abstract class BaseIOPort
	{
		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>
		/// The address.
		/// </value>
		public ushort Address { get; protected set; }
	}

	/// <summary>
	/// Interface to IOPort with read only permission
	/// </summary>
	public abstract class BaseIOPortRead : BaseIOPort
	{
		/// <summary>
		/// Read8s this instance.
		/// </summary>
		/// <returns></returns>
		public abstract byte Read8();

		/// <summary>
		/// Read16s this instance.
		/// </summary>
		/// <returns></returns>
		public abstract ushort Read16();

		/// <summary>
		/// Read32s this instance.
		/// </summary>
		/// <returns></returns>
		public abstract uint Read32();
	}

	/// <summary>
	/// Interface to IOPort with write only permission
	/// </summary>
	public abstract class BaseIOPortWrite : BaseIOPort
	{
		/// <summary>
		/// Write8s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public abstract void Write8(byte data);

		/// <summary>
		/// Write16s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public abstract void Write16(ushort data);

		/// <summary>
		/// Write32s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public abstract void Write32(uint data);
	}

	/// <summary>
	/// class to IOPort with full read/write permissions
	/// </summary>
	public abstract class BaseIOPortReadWrite : BaseIOPortRead
	{
		/// <summary>
		/// Write8s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public abstract void Write8(byte data);

		/// <summary>
		/// Write16s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public abstract void Write16(ushort data);

		/// <summary>
		/// Write32s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public abstract void Write32(uint data);
	}
}
