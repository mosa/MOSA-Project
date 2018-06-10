// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// IO Port Resources
	/// </summary>
	public sealed class IOPortResources
	{
		// All legacy ISA cards occupy the IO region from 0x0100 through 0x3FF
		/// <summary>
		/// The start legacy isa port
		/// </summary>
		public const ushort StartLegacyISAPort = 0x0100;

		/// <summary>
		/// The end legacy isa port
		/// </summary>
		public const ushort EndLegacyISAPort = 0x3FF;

		/// <summary>
		/// The maximum ports
		/// </summary>
		public const ushort MaxPorts = 0xFFFF;

		/// <summary>
		/// The port used
		/// </summary>
		public bool[] portUsed;

		private object _lock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="IOPortResources"/> class.
		/// </summary>
		public IOPortResources()
		{
			portUsed = new bool[MaxPorts];

			for (int p = 0; p < MaxPorts; p++)
			{
				portUsed[p] = false;
			}
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
			return (port >= StartLegacyISAPort) && (port <= EndLegacyISAPort);
		}

		/// <summary>
		/// Claims the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		/// <returns></returns>
		public bool ClaimResources(HardwareResources hardwareResources)
		{
			lock (_lock)
			{
				for (byte r = 0; r < hardwareResources.IOPointRegionCount - 1; r++)
				{
					var region = hardwareResources.GetIOPortRegion(r);

					for (int p = 0; p < region.Size; p++)
					{
						if (portUsed[region.BaseIOPort + p])
							return false;
					}
				}

				for (byte r = 0; r < hardwareResources.IOPointRegionCount; r++)
				{
					var region = hardwareResources.GetIOPortRegion(r);

					for (int p = 0; p < region.Size; p++)
					{
						portUsed[region.BaseIOPort + p] = true;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Releases the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		public void ReleaseResources(HardwareResources hardwareResources)
		{
			lock (_lock)
			{
				for (byte r = 0; r < hardwareResources.IOPointRegionCount; r++)
				{
					var region = hardwareResources.GetIOPortRegion(r);

					for (int p = 0; p < region.Size; p++)
					{
						portUsed[region.BaseIOPort + p] = false;
					}
				}
			}
		}
	}
}
