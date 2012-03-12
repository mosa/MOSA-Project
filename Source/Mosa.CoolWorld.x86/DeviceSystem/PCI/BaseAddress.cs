/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// 
	/// </summary>
	public class BaseAddress
	{
		/// <summary>
		/// 
		/// </summary>
		protected uint address;
		/// <summary>
		/// 
		/// </summary>
		protected uint size;
		/// <summary>
		/// 
		/// </summary>
		protected AddressType region;
		/// <summary>
		/// 
		/// </summary>
		protected bool prefetchable;

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public uint Address { get { return address; } }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>		
		public uint Size { get { return size; } }

		/// <summary>
		/// Gets the region.
		/// </summary>
		/// <value>The region.</value>		
		public AddressType Region { get { return region; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="BaseAddress"/> is prefetchable.
		/// </summary>
		/// <value><c>true</c> if prefetchable; otherwise, <c>false</c>.</value>
		public bool Prefetchable { get { return prefetchable; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseAddress"/> class.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <param name="prefetchable">if set to <c>true</c> [prefetchable].</param>
		public BaseAddress(AddressType region, uint address, uint size, bool prefetchable)
		{
			this.region = region;
			this.address = address;
			this.size = size;
			this.prefetchable = prefetchable;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (region == AddressType.Undefined)
				return string.Empty;

			if (region == AddressType.IO)
				return "I/O Port at 0x" + address.ToString("X") + " [size=" + size.ToString() + "]";

			if (prefetchable)
				return "Memory at 0x" + address.ToString("X") + " [size=" + size.ToString() + "] (prefetchable)";

			return "Memory at 0x" + address.ToString("X") + " [size=" + size.ToString() + "] (non-prefetchable)";
		}

	}
}