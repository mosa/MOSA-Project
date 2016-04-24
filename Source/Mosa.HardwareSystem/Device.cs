// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.HardwareSystem
{
	/// <summary>
	/// Abstract base class for a device
	/// </summary>
	public abstract class Device : IDevice
	{
		/// <summary>
		/// Gets the name of the device.
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// Gets the parent device, if any, of the device.
		/// </summary>
		public IDevice Parent { get; protected set; }

		/// <summary>
		/// Gets the status of the device.
		/// </summary>
		public DeviceStatus DeviceStatus { get; protected set; }
	}
}
