// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Device Manager
	/// </summary>
	public class DeviceManager
	{
		/// <summary>
		/// The devices
		/// </summary>
		private readonly List<Device> devices;

		/// <summary>
		/// The spin lock
		/// </summary>
		private SpinLock spinLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceManager"/> class.
		/// </summary>
		public DeviceManager()
		{
			devices = new List<Device>();
		}

		public BaseDeviceDriver Initialize(DeviceDriverRegistryEntry deviceDriverRegistryEntry, Device parent, BaseDeviceConfiguration configuration = null, HardwareResources resources = null)
		{
			var deviceDriver = deviceDriverRegistryEntry.Factory();

			Initialize(deviceDriver, parent, configuration, resources, deviceDriverRegistryEntry);

			return deviceDriver;
		}

		public void Initialize(BaseDeviceDriver deviceDriver, Device parent, BaseDeviceConfiguration configuration = null, HardwareResources resources = null, DeviceDriverRegistryEntry deviceDriverRegistryEntry = null)
		{
			var device = new Device()
			{
				DeviceDriver = deviceDriver,
				DeviceDriverRegistryEntry = deviceDriverRegistryEntry,
				Status = DeviceStatus.Initializing,
				Parent = parent,
				Configuration = configuration,
				Resources = resources,

				//Service = deviceDriver as IService
			};

			Initialize(device);
		}

		/// <summary>
		/// Adds the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		private void Initialize(Device device)
		{
			spinLock.Enter();
			devices.Add(device);

			if (device.Parent != null)
			{
				device.Parent.Children.Add(device);
			}

			spinLock.Exit();

			device.DeviceDriver.Setup(device);
		}

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public List<Device> GetDevices<T>()
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in devices)
			{
				if (device.DeviceDriver is T)
				{
					list.Add(device);
				}
			}

			spinLock.Exit();

			return list;
		}

		public List<Device> GetDevices<T>(DeviceStatus status)
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in devices)
			{
				if (device.Status == status && device.DeviceDriver is T)
				{
					list.Add(device);
				}
			}

			spinLock.Exit();

			return list;
		}

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public List<Device> GetDevices(string name)
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in devices)
			{
				if (device.Name == name)
				{
					list.Add(device);
				}
			}

			spinLock.Exit();

			return list;
		}

		/// <summary>
		/// Gets the children of.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <returns></returns>
		public List<Device> GetChildrenOf(Device parent)
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in parent.Children)
			{
				list.Add(device);
			}

			spinLock.Exit();

			return list;
		}

		/// <summary>
		/// Gets all devices.
		/// </summary>
		/// <returns></returns>
		public List<Device> GetAllDevices()
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in devices)
			{
				list.Add(device);
			}

			spinLock.Exit();

			return list;
		}
	}
}
