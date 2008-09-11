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
    /// <summary>
    /// 
    /// </summary>
	public interface IDeviceManager
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
		void Add(IDevice device);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		LinkedList<IDevice> GetAllDevices();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
		LinkedList<IDevice> GetDevices(IFindDevice match);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match1"></param>
        /// <param name="match2"></param>
        /// <returns></returns>
		LinkedList<IDevice> GetDevices(IFindDevice match1, IFindDevice match2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match1"></param>
        /// <param name="match2"></param>
        /// <param name="match3"></param>
        /// <returns></returns>
		LinkedList<IDevice> GetDevices(IFindDevice match1, IFindDevice match2, IFindDevice match3);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matches"></param>
        /// <returns></returns>
		LinkedList<IDevice> GetDevices(IFindDevice[] matches);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
		LinkedList<IDevice> GetChildrenOf(IDevice parent);
	}
}
