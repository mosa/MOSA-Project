/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceDrivers
{
	public class DeviceManager
	{
		private LinkedList<IDevice> devices;
		private SpinLock spinLock;

		public DeviceManager()
		{
			devices = new LinkedList<IDevice>();
		}

		public void Add(IDevice device)
		{
			spinLock.Enter();
			devices.Add(device);
			spinLock.Exit();
		}

		public LinkedList<IDevice> GetDevices(IFindDevice match)
		{
			spinLock.Enter();

			LinkedList<IDevice> list = new LinkedList<IDevice>();

			foreach (IDevice device in devices)
				if (match.IsMatch(device))
					list.Add(device);

			spinLock.Exit();

			return list;
		}

		public LinkedList<IDevice> GetDevices(IFindDevice match1, IFindDevice match2)
		{
			spinLock.Enter();

			LinkedList<IDevice> list = new LinkedList<IDevice>();

			foreach (IDevice device in devices)
				if (match1.IsMatch(device) && (match2.IsMatch(device)))
					list.Add(device);

			spinLock.Exit();

			return list;
		}

		public LinkedList<IDevice> GetDevices(IFindDevice match1, IFindDevice match2, IFindDevice match3)
		{
			spinLock.Enter();

			LinkedList<IDevice> list = new LinkedList<IDevice>();

			foreach (IDevice device in devices)
				if (match1.IsMatch(device) && (match2.IsMatch(device)) && (match3.IsMatch(device)))
					list.Add(device);

			spinLock.Exit();

			return list;
		}

		public LinkedList<IDevice> GetDevices(IFindDevice[] matches)
		{
			spinLock.Enter();

			LinkedList<IDevice> list = new LinkedList<IDevice>();

			foreach (IDevice device in devices) {
				bool matched = true;

				foreach (IFindDevice find in matches)
					if (!find.IsMatch(device)) {
						matched = false;
						break;
					}

				if (matched)
					list.Add(device);
			}

			spinLock.Exit();

			return list;
		}

		public LinkedList<IDevice> GetChildrenOf(IDevice parent)
		{
			return GetDevices(new ParentOf(parent));
		}

		public LinkedList<IDevice> GetAllDevices()
		{
			spinLock.Enter();

			LinkedList<IDevice> list = new LinkedList<IDevice>();

			foreach (IDevice device in devices)
				list.Add(device);

			spinLock.Exit();

			return list;
		}

		public interface IFindDevice
		{
			bool IsMatch(IDevice device);
		}

		// Helper Find Classes

		public class ParentOf : IFindDevice
		{
			private IDevice parent;
			public ParentOf(IDevice parent)
			{
				this.parent = parent;
			}
			public bool IsMatch(IDevice device)
			{
				return (parent == device);
			}
		}

		//public class TypeOf : IFindDevice
		//{
		//    private Type type;

		//    public TypeOf(IDevice parent)
		//    {
		//        this.type = type;
		//    }

		//    public bool IsMatch(IDevice device)
		//    {
		//        return ((device.IsSubclassOf(type)) || (device == type) || (device.IsAssignableFrom(type)) || (type.IsAssignableFrom(device)));
		//    }
		//}

		public class WithName : IFindDevice
		{
			private string name;

			public WithName(IDevice parent)
			{
				this.name = name;
			}
			public bool IsMatch(IDevice device)
			{
				return (device.Name == name);
			}
		}

		public class Online : IFindDevice
		{
			public Online() { }

			public bool IsMatch(IDevice device)
			{
				return device.Status == DeviceStatus.Online;
			}
		}

		public class Available : IFindDevice
		{
			public Available() { }

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
	}
}
