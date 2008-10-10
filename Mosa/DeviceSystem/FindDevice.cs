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
			private IDevice parent;

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
			public IsOnline() { }

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
			public IsAvailable() { }

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
			public WithStatus(DeviceStatus deviceStatus) { this.deviceStatus = deviceStatus; }

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

		//public class IsTypeOf : IFindDevice
		//{
		//    private Type type;

		//    public IsTypeOf(Type type)
		//    {
		//        this.type = type;
		//    }

		//    public bool IsMatch(IDevice device)
		//    {
		//        return ((device.IsSubclassOf(type)) || (device == type) || (device.IsAssignableFrom(type)) || (type.IsAssignableFrom(device)));
		//    }
		//}

        /// <summary>
        /// 
        /// </summary>
		public class IsPCIDevice : IFindDevice
		{
            /// <summary>
            /// Initializes a new instance of the <see cref="IsPCIDevice"/> class.
            /// </summary>
			public IsPCIDevice() { }

            /// <summary>
            /// Determines whether the specified device is match.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <returns>
            /// 	<c>true</c> if the specified device is match; otherwise, <c>false</c>.
            /// </returns>
			public bool IsMatch(IDevice device)
			{
				return device is PCI.PCIDevice;
			}
		}

	}
}
