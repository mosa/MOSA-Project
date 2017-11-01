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

		/// <summary>
		/// Adds the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		public void Add(Device device)
		{
			spinLock.Enter();
			devices.Add(device);

			if (device.Parent != null)
			{
				device.Parent.Children.Add(device);
			}

			spinLock.Exit();
		}

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public List<T> GetDeviceServices<T>() /*where T : IService*/
		{
			spinLock.Enter();

			var list = new List<T>();

			foreach (var device in devices)
			{
				if (device.Service is T)
				{
					list.Add((T)device.Service);
				}
			}

			spinLock.Exit();

			return list;
		}

		public List<T> GetDeviceServices<T>(DeviceStatus status) /*where T : IService*/
		{
			spinLock.Enter();

			var list = new List<T>();

			foreach (var device in devices)
			{
				if (device.Status == status && device.Service is T)
				{
					list.Add((T)device.Service);
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
