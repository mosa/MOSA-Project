// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class ResourceManager
	{
		/// <summary>
		/// Gets the IO port resources.
		/// </summary>
		/// <value>The IO port resources.</value>
		public IOPortResources IOPortResources { get; private set; }

		/// <summary>
		/// Gets the memory resources.
		/// </summary>
		/// <value>The memory resources.</value>
		public MemoryResources MemoryResources { get; private set; }

		/// <summary>
		/// Gets the interrupt manager.
		/// </summary>
		/// <value>The interrupt manager.</value>
		public InterruptManager InterruptManager { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceManager"/> class.
		/// </summary>
		public ResourceManager()
		{
			IOPortResources = new IOPortResources();
			MemoryResources = new MemoryResources();
			InterruptManager = new InterruptManager();
		}

		/// <summary>
		/// Claims the resources.
		/// </summary>
		/// <returns></returns>
		public bool ClaimResources(HardwareResources hardwareResources)
		{
			if (!IOPortResources.ClaimResources(hardwareResources))
				return false;

			if (!MemoryResources.ClaimResources(hardwareResources))
			{
				IOPortResources.ReleaseResources(hardwareResources);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Releases the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		/// <returns></returns>
		public void ReleaseResources(HardwareResources hardwareResources)
		{
			IOPortResources.ReleaseResources(hardwareResources);
			MemoryResources.ReleaseResources(hardwareResources);
		}
	}
}
