// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.HardwareSystem
{
	/// <summary>
	///
	/// </summary>
	public sealed class IOPortRegion
	{
		/// <summary>
		/// Gets the base IO port.
		/// </summary>
		/// <value>The base IO port.</value>
		public ushort BaseIOPort { get; private set; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public ushort Size { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IOPortRegion"/> class.
		/// </summary>
		/// <param name="baseIOPort">The base IO port.</param>
		/// <param name="size">The size.</param>
		public IOPortRegion(ushort baseIOPort, ushort size)
		{
			this.BaseIOPort = baseIOPort;
			this.Size = size;
		}
	}
}
