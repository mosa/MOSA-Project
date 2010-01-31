/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class MemoryResources
	{

		/// <summary>
		/// 
		/// </summary>
		protected LinkedList<IMemoryRegion> memoryRegions;
		/// <summary>
		/// 
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryResources"/> class.
		/// </summary>
		public MemoryResources()
		{
			memoryRegions = new LinkedList<IMemoryRegion>();
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
		public bool ClaimResources(IHardwareResources hardwareResources)
		{
			spinLock.Enter();

			for (byte r = 0; r < hardwareResources.MemoryRegionCount; r++)
			{
				IMemoryRegion region = hardwareResources.GetMemoryRegion(r);

				foreach (IMemoryRegion memoryRegion in memoryRegions)
					if ((memoryRegion.Contains(region.BaseAddress) || memoryRegion.Contains(region.BaseAddress + region.Size)))
						return false;
			}

			for (byte r = 0; r < hardwareResources.MemoryRegionCount; r++)
				memoryRegions.Add(hardwareResources.GetMemoryRegion(r));

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

			for (byte r = 0; r < hardwareResources.MemoryRegionCount; r++)
				memoryRegions.Remove(hardwareResources.GetMemoryRegion(r));

			spinLock.Exit();
		}
	}
}
