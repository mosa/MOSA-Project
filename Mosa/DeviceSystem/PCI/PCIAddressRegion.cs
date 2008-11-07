/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// Type of PCI address region
	/// </summary>
	public enum PCIAddressRegion
	{
		/// <summary>
		/// Port IO Address Region
		/// </summary>
		IO,
		/// <summary>
		/// Memory Address Region
		/// </summary>
		Memory,
		/// <summary>
		/// Undefined Address Region
		/// </summary>
		Undefined
	}
}