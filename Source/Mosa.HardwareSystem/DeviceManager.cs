// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.HardwareSystem
{
	/// <summary>
	/// Device Manager
	/// </summary>
	public class DeviceManager
	{
		/// <summary>
		/// The devices
		/// </summary>
		private readonly List<IDevice> devices;

		/// <summary>
		/// The spin lock
		/// </summary>
		private SpinLock spinLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceManager"/> class.
		/// </summary>
		public DeviceManager()
		{
			devices = new List<IDevice>();
		}

		/// <summary>
		/// Adds the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		public void Add(IDevice device)
		{
			spinLock.Enter();
			devices.Add(device);
			spinLock.Exit();
		}

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public List<IDevice> GetDevices<T>()
		{
			spinLock.Enter();

			var list = new List<IDevice>();

			foreach (var device in devices)
			{
				if (device is T)
				{
					list.Add(device);
				}
			}

			spinLock.Exit();

			return list;
		}

		public List<IDevice> GetDevices<T>(DeviceStatus status)
		{
			spinLock.Enter();

			var list = new List<IDevice>();

			foreach (var device in devices)
			{
				if (device.DeviceStatus == status && device is T)
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
		public List<IDevice> GetDevices(string name)
		{
			spinLock.Enter();

			var list = new List<IDevice>();

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
		public List<IDevice> GetChildrenOf(IDevice parent)
		{
			spinLock.Enter();

			var list = new List<IDevice>();

			foreach (var device in devices)
			{
				if (device.Parent == parent)
				{
					list.Add(device);
				}
			}

			spinLock.Exit();

			return list;
		}

		/// <summary>
		/// Gets all devices.
		/// </summary>
		/// <returns></returns>
		public List<IDevice> GetAllDevices()
		{
			spinLock.Enter();

			var list = new List<IDevice>();

			foreach (var device in devices)
			{
				list.Add(device);
			}

			spinLock.Exit();

			return list;
		}
	}
}
