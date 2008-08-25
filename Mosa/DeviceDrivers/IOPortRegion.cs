/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers
{
	public class IOPortRegion : IIOPortRegion
	{
		protected IOPortResources portIOResources;
		protected ushort baseIOPort;
		protected ushort size;

		public ushort BaseIOPort { get { return baseIOPort; } }
		public ushort Size { get { return size; } }

		public IOPortRegion(IOPortResources portIOResources, ushort baseIOPort, ushort size)
		{
			this.portIOResources = portIOResources;
			this.baseIOPort = baseIOPort;
			this.size = size;
		}

		public IReadWriteIOPort GetIOPort(ushort index)
		{
			return portIOResources.GetIOPort(baseIOPort, index);
		}

	}

}
