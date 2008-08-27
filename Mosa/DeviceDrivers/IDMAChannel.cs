/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.DeviceDrivers
{
	public interface IDMAChannel
	{
		void SetupChannel (DMAMode mode, DMATransferType type, bool auto, uint count);
		bool TransferOut (uint count, byte[] data, uint offset);
		bool TransferIn (uint count, byte[] data, uint offset);
	}

	public enum DMAMode : byte
	{
		ReadFromMemory,
		WriteToMemory
	}

	public enum DMATransferType : byte
	{
		OnDemand,
		Single,
		Block,
		CascadeMode
	}

}
