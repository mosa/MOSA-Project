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
	public interface IIOPortRegion
	{
		ushort BaseIOPort { get ; }
		ushort Size { get ;  }
		//IReadWriteIOPort GetIOPort(ushort index);
	}

}
