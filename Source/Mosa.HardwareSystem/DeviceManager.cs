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
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public List<IDevice> GetDevices(IFindDevice match)
		{
			spinLock.Enter();

			var list = new List<IDevice>();

			foreach (var device in devices)
			{
				if (match.IsMatch(device))
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
		/// <param name="match1">The match1.</param>
		/// <param name="match2">The match2.</param>
		/// <returns></returns>
		public List<IDevice> GetDevices(IFindDevice match1, IFindDevice match2)
		{
			spinLock.Enter();

			var list = new List<IDevice>();

			foreach (var device in devices)
			{
				if (match1.IsMatch(device) && (match2.IsMatch(device)))
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
		/// <param name="match1">The match1.</param>
		/// <param name="match2">The match2.</param>
		/// <param name="match3">The match3.</param>
		/// <returns></returns>
		public List<IDevice> GetDevices(IFindDevice match1, IFindDevice match2, IFindDevice match3)
		{
			spinLock.Enter();

			var list = new List<IDevice>();

			foreach (var device in devices)
			{
				if (match1.IsMatch(device) && (match2.IsMatch(device)) && (match3.IsMatch(device)))
					list.Add(device);
			}

			spinLock.Exit();

			return list;
		}

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <param name="matches">The matches.</param>
		/// <returns></returns>
		public List<IDevice> GetDevices(IFindDevice[] matches)
		{
			spinLock.Enter();

			var list = new List<IDevice>();

			foreach (var device in devices)
			{
				bool matched = true;

				foreach (IFindDevice find in matches)
				{
					if (!find.IsMatch(device))
					{
						matched = false;
						break;
					}
				}

				if (matched)
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
			return GetDevices(new WithParent(parent));
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
