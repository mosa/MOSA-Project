/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.DeviceDrivers.PCI
{
	public interface IPCIController
	{
		uint ReadConfig(uint bus, uint slot, uint function, uint register);
		void WriteConfig(uint bus, uint slot, uint function, uint register, uint value);
	}
}
