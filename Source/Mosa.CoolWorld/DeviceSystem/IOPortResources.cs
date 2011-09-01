/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class IOPortResources
	{
		// All legacy ISA cards occupy the IO region from 0x0100 through 0x3FF
		/// <summary>
		/// 
		/// </summary>
		public const ushort StartLegacyISAPort = 0x0100;
		/// <summary>
		/// 
		/// </summary>
		public const ushort EndLegacyISAPort = 0x3FF;
		/// <summary>
		/// 
		/// </summary>
		public const ushort MaxPorts = 0xFFFF;
		/// <summary>
		/// 
		/// </summary>
		public bool[] portUsed;
		/// <summary>
		/// 
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="IOPortResources"/> class.
		/// </summary>
		public IOPortResources()
		{
			portUsed = new bool[MaxPorts];
			for (int p = 0; p < MaxPorts; p++)
				portUsed[p] = false;
		}

		/// <summary>
		/// Determines whether the specific port in ISA legacy region.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns>
		/// 	<c>true</c> if true if specific port in ISA legacy region; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPortInISALegacyRegion(ushort port)
		{
			return ((port >= StartLegacyISAPort) && (port <= EndLegacyISAPort));
		}

		/// <summary>
		/// Gets the IO port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public IReadWriteIOPort GetIOPort(ushort port, ushort offset)
		{
			//Mosa.Kernel.x86.Screen.Write('J');
			IReadWriteIOPort port2 = HAL.RequestIOPort((ushort)(port + offset));
			//Mosa.Kernel.x86.Screen.Write('K');
			return port2;
		}

		/// <summary>
		/// Claims the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		/// <returns></returns>
		public bool ClaimResources(IHardwareResources hardwareResources)
		{
			spinLock.Enter();

			for (byte r = 0; r < hardwareResources.IOPointRegionCount - 1; r++)
			{
				IIOPortRegion region = hardwareResources.GetIOPortRegion(r);
				for (int p = 0; p < region.Size; p++)
					if (portUsed[region.BaseIOPort + p])
						return false;
			}

			for (byte r = 0; r < hardwareResources.IOPointRegionCount; r++)
			{
				IIOPortRegion region = hardwareResources.GetIOPortRegion(r);
				for (int p = 0; p < region.Size; p++)
					portUsed[region.BaseIOPort + p] = true;
			}

			spinLock.Exit();

			return true;
		}

		/// <summary>
		/// Releases the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		public void ReleaseResources(IHardwareResources hardwareResources)
		{
			spinLock.Enter();

			for (byte r = 0; r < hardwareResources.IOPointRegionCount; r++)
			{
				IIOPortRegion region = hardwareResources.GetIOPortRegion(r);
				for (int p = 0; p < region.Size; p++)
					portUsed[region.BaseIOPort + p] = false;
			}

			spinLock.Exit();
		}

	}
}
