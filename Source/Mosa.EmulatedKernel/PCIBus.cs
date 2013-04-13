/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.EmulatedKernel
{
	/// <summary>
	///
	/// </summary>
	public class PCIBus
	{
		/// <summary>
		///
		/// </summary>
		protected List<PCIDevice> devices;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCIBus"/> class.
		/// </summary>
		public PCIBus()
		{
			devices = new List<PCIDevice>();
		}

		/// <summary>
		/// Adds the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		public void Add(PCIDevice device)
		{
			devices.Add(device);
		}

		/// <summary>
		/// Gets the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public PCIDevice Get(uint index)
		{
			if (devices.Count > index)
				return devices[(int)index];
			else
				return null;
		}
	}
}