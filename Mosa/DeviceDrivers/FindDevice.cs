/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
{
	public class FindDevice
	{
		// Helper Find Classes

		public class WithParent : IFindDevice
		{
			private IDevice parent;

			public WithParent(IDevice parent)
			{
				this.parent = parent;
			}

			public bool IsMatch(IDevice device)
			{
				return (parent == device);
			}
		}

		public class WithName : IFindDevice
		{
			private string name;

			public WithName(string name)
			{
				this.name = name;
			}
			public bool IsMatch(IDevice device)
			{
				return (device.Name == name);
			}
		}

		public class IsOnline : IFindDevice
		{
			public IsOnline() { }

			public bool IsMatch(IDevice device)
			{
				return device.Status == DeviceStatus.Online;
			}
		}

		public class IsAvailable : IFindDevice
		{
			public IsAvailable() { }

			public bool IsMatch(IDevice device)
			{
				return device.Status == DeviceStatus.Available;
			}
		}

		public class WithStatus : IFindDevice
		{
			protected DeviceStatus deviceStatus;

			public WithStatus(DeviceStatus deviceStatus) { this.deviceStatus = deviceStatus; }

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

	}
}
