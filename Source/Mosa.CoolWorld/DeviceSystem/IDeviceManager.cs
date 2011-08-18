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
	public interface IDeviceManager
	{
		/// <summary>
		/// Adds the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		void Add(IDevice device);

		/// <summary>
		/// Gets all devices.
		/// </summary>
		/// <returns></returns>
		LinkedList<IDevice> GetAllDevices();

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		LinkedList<IDevice> GetDevices(IFindDevice match);

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <param name="match1">The match1.</param>
		/// <param name="match2">The match2.</param>
		/// <returns></returns>
		LinkedList<IDevice> GetDevices(IFindDevice match1, IFindDevice match2);

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <param name="match1">The match1.</param>
		/// <param name="match2">The match2.</param>
		/// <param name="match3">The match3.</param>
		/// <returns></returns>
		LinkedList<IDevice> GetDevices(IFindDevice match1, IFindDevice match2, IFindDevice match3);

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <param name="matches">The matches.</param>
		/// <returns></returns>
		LinkedList<IDevice> GetDevices(IFindDevice[] matches);

		/// <summary>
		/// Gets the children of.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <returns></returns>
		LinkedList<IDevice> GetChildrenOf(IDevice parent);
	}
}
