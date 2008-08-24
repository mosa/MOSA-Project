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
	public interface IDeviceManager
	{
		void Add(IDevice device);

		LinkedList<IDevice> GetAllDevices();
		LinkedList<IDevice> GetDevices(IFindDevice match);
		LinkedList<IDevice> GetDevices(IFindDevice match1, IFindDevice match2);
		LinkedList<IDevice> GetDevices(IFindDevice match1, IFindDevice match2, IFindDevice match3);
		LinkedList<IDevice> GetDevices(IFindDevice[] matches);
		LinkedList<IDevice> GetChildrenOf(IDevice parent);
	}
}
