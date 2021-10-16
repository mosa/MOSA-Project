// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// PCI Host Bridge interface
	/// </summary>
	public interface IHostBridgeController
	{
		bool CPUReset();

		void SetCPUResetInformation(ushort address, byte value);
	}
}
