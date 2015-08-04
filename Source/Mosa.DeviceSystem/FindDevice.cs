// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class FindDevice
	{
		// Helper Find Classes

		/// <summary>
		///
		/// </summary>
		public class WithParent : IFindDevice
		{
			/// <summary>
			///
			/// </summary>
			private readonly IDevice parent;

			/// <summary>
			/// Initializes a new instance of the <see cref="WithParent"/> class.
			/// </summary>
			/// <param name="parent">The parent.</param>
			public WithParent(IDevice parent)
			{
				this.parent = parent;
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
				return (parent == device);
			}
		}

		/// <summary>
		///
		/// </summary>
		public class WithName : IFindDevice
		{
			/// <summary>
			///
			/// </summary>
			private string name;

			/// <summary>
			/// Initializes a new instance of the <see cref="WithName"/> class.
			/// </summary>
			/// <param name="name">The name.</param>
			public WithName(string name)
			{
				this.name = name;
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
				return (device.Name == name);
			}
		}

		/// <summary>
		///
		/// </summary>
		public class IsOnline : IFindDevice
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="IsOnline"/> class.
			/// </summary>
			public IsOnline()
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
				return device.Status == DeviceStatus.Online;
			}
		}

		/// <summary>
		///
		/// </summary>
		public class IsAvailable : IFindDevice
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="IsAvailable"/> class.
			/// </summary>
			public IsAvailable()
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
				return device.Status == DeviceStatus.Available;
			}
		}

		/// <summary>
		///
		/// </summary>
		public class WithStatus : IFindDevice
		{
			/// <summary>
			///
			/// </summary>
			protected DeviceStatus deviceStatus;

			/// <summary>
			/// Initializes a new instance of the <see cref="WithStatus"/> class.
			/// </summary>
			/// <param name="deviceStatus">The device status.</param>
			public WithStatus(DeviceStatus deviceStatus)
			{
				this.deviceStatus = deviceStatus;
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
				return device.Status == deviceStatus;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T">The Type to check.</typeparam>
		public class IsTypeOf<T> : IFindDevice
			where T : class
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="IsTypeOf&lt;T>"/> class.
			/// </summary>
			public IsTypeOf()
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
				return (device as T) != null;
			}
		}

		/// <summary>
		///
		/// </summary>
		public class IsPCIDevice : IFindDevice
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="IsPCIDevice"/> class.
			/// </summary>
			public IsPCIDevice()
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
				return device is IPCIDevice;
			}
		}

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
		public class IsPCIController : IFindDevice
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="IsPCIController"/> class.
			/// </summary>
			public IsPCIController()
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
				return device is IPCIController;
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
}
