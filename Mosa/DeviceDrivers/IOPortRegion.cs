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
	public class IOPortRegion
	{
		protected PortIOSpace portIOSpace;
		protected ushort baseIOPort;
		protected ushort size;

		public ushort BaseIOPort { get { return baseIOPort; } }
		public ushort Size { get { return size; } }

		public IOPortRegion(PortIOSpace portIOSpace, ushort baseIOPort, ushort size)
		{
			this.portIOSpace = portIOSpace;
			this.baseIOPort = baseIOPort;
			this.size = size;
		}

		public IReadWriteIOPort GetPort(ushort index)
		{
			return portIOSpace.GetPort(baseIOPort, index);
		}

	}

}
