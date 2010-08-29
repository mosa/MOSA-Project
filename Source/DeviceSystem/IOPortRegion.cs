/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class IOPortRegion : IIOPortRegion
	{
		/// <summary>
		/// 
		/// </summary>
		protected ushort baseIOPort;
		/// <summary>
		/// 
		/// </summary>
		protected ushort size;

		/// <summary>
		/// Gets the base IO port.
		/// </summary>
		/// <value>The base IO port.</value>
		public ushort BaseIOPort { get { return baseIOPort; } }
		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public ushort Size { get { return size; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="IOPortRegion"/> class.
		/// </summary>
		/// <param name="baseIOPort">The base IO port.</param>
		/// <param name="size">The size.</param>
		public IOPortRegion(ushort baseIOPort, ushort size)
		{
			this.baseIOPort = baseIOPort;
			this.size = size;
		}

	}

}
