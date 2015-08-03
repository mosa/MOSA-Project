// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// Type of PCI address region
	/// </summary>
	public enum AddressType
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