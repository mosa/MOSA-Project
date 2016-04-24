// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem;

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class IsDiskDevice : IFindDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IsDiskDevice"/> class.
		/// </summary>
		public IsDiskDevice()
		{
		}

		/// <summary>
		/// Determines whether the specified device is match.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns>
		/// 	<c>true</c> if the specified device is match; otherwise, <c>false</c>.
		/// </returns>
		public bool IsMatch(IDevice device)
		{
			return device is IDiskDevice;
		}
	}

	/// <summary>
	///
	/// </summary>
	public class IsDiskControllerDevice : IFindDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IsDiskControllerDevice"/> class.
		/// </summary>
		public IsDiskControllerDevice()
		{
		}

		/// <summary>
		/// Determines whether the specified device is match.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns>
		/// 	<c>true</c> if the specified device is match; otherwise, <c>false</c>.
		/// </returns>
		public bool IsMatch(IDevice device)
		{
			return device is IDiskControllerDevice;
		}
	}

	/// <summary>
	///
	/// </summary>
	public class IsPartitionDevice : IFindDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IsPartitionDevice"/> class.
		/// </summary>
		public IsPartitionDevice()
		{
		}

		/// <summary>
		/// Determines whether the specified device is match.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns>
		/// 	<c>true</c> if the specified device is match; otherwise, <c>false</c>.
		/// </returns>
		public bool IsMatch(IDevice device)
		{
			return device is IPartitionDevice;
		}
	}
}
