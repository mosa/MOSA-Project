// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.HardwareSystem
{
	/// <summary>
	///
	/// </summary>
	public class MemoryResources
	{
		/// <summary>
		///
		/// </summary>
		protected LinkedList<MemoryRegion> memoryRegions;

		/// <summary>
		///
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryResources"/> class.
		/// </summary>
		public MemoryResources()
		{
			memoryRegions = new LinkedList<MemoryRegion>();
		}

		/// <summary>
		/// Gets the memory.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public IMemory GetMemory(uint address, uint size)
		{
			return HAL.RequestPhysicalMemory(address, size);
		}

		/// <summary>
		/// Claims the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		/// <returns></returns>
		public bool ClaimResources(HardwareResources hardwareResources)
		{
			spinLock.Enter();

			for (byte r = 0; r < hardwareResources.MemoryRegionCount; r++)
			{
				var region = hardwareResources.GetMemoryRegion(r);

				foreach (var memoryRegion in memoryRegions)
				{
					if ((memoryRegion.Contains(region.BaseAddress) || memoryRegion.Contains(region.BaseAddress + region.Size)))
						return false;
				}
			}

			for (byte r = 0; r < hardwareResources.MemoryRegionCount; r++)
			{
				memoryRegions.AddLast(hardwareResources.GetMemoryRegion(r));
			}

			spinLock.Exit();

			return true;
		}

		/// <summary>
		/// Releases the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		public void ReleaseResources(HardwareResources hardwareResources)
		{
			spinLock.Enter();

			for (byte r = 0; r < hardwareResources.MemoryRegionCount; r++)
			{
				memoryRegions.Remove(hardwareResources.GetMemoryRegion(r));
			}

			spinLock.Exit();
		}
	}
}
