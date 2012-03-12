/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class DeviceManager : IDeviceManager
	{
		/// <summary>
		/// 
		/// </summary>
		private LinkedList<IDevice> devices;

		/// <summary>
		/// 
		/// </summary>
		private SpinLock spinLock;

		/// <summary>
		/// 
		/// </summary>
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

			foreach (IDevice device in devices)
			{
				bool matched = true;

				foreach (IFindDevice find in matches)
					if (!find.IsMatch(device))
					{
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
			return GetDevices(new FindDevice.WithParent(parent));
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
	}
}
