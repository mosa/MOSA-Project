// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Memory Resources
	/// </summary>
	public class MemoryResources
	{
		/// <summary>
		/// The memory regions
		/// </summary>
		protected LinkedList<AddressRegion> memoryRegions;

		/// <summary>
		/// The spin lock
		/// </summary>
		protected object _lock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryResources"/> class.
		/// </summary>
		public MemoryResources()
		{
			memoryRegions = new LinkedList<AddressRegion>();
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
				for (byte r = 0; r < hardwareResources.AddressRegionCount; r++)
				{
					var region = hardwareResources.GetMemoryRegion(r);

					foreach (var memoryRegion in memoryRegions)
					{
						if (memoryRegion.Contains(region.Address) || memoryRegion.Contains(region.Address + (int)region.Size))
							return false;
					}
				}

				for (byte r = 0; r < hardwareResources.AddressRegionCount; r++)
				{
					memoryRegions.AddLast(hardwareResources.GetMemoryRegion(r));
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
				for (byte r = 0; r < hardwareResources.AddressRegionCount; r++)
				{
					memoryRegions.Remove(hardwareResources.GetMemoryRegion(r));
				}
			}
		}
	}
}
