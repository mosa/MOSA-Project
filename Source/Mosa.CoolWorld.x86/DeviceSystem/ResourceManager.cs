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
	public class ResourceManager : IResourceManager
	{
		/// <summary>
		/// 
		/// </summary>
		protected IOPortResources ioPortResources;
		/// <summary>
		/// 
		/// </summary>
		protected MemoryResources memoryResources;
		/// <summary>
		/// 
		/// </summary>
		protected InterruptManager interruptManager;

		/// <summary>
		/// Gets the IO port resources.
		/// </summary>
		/// <value>The IO port resources.</value>
		public IOPortResources IOPortResources { get { return ioPortResources; } }

		/// <summary>
		/// Gets the memory resources.
		/// </summary>
		/// <value>The memory resources.</value>
		public MemoryResources MemoryResources { get { return memoryResources; } }

		/// <summary>
		/// Gets the interrupt manager.
		/// </summary>
		/// <value>The interrupt manager.</value>
		public InterruptManager InterruptManager { get { return interruptManager; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceManager"/> class.
		/// </summary>
		public ResourceManager()
		{
			ioPortResources = new IOPortResources();
			memoryResources = new MemoryResources();
			interruptManager = new InterruptManager();
		}

		/// <summary>
		/// Claims the resources.
		/// </summary>
		/// <returns></returns>
		public bool ClaimResources(IHardwareResources hardwareResources)
		{
			if (!ioPortResources.ClaimResources(hardwareResources))
				return false;

			if (!memoryResources.ClaimResources(hardwareResources))
			{
				ioPortResources.ReleaseResources(hardwareResources);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Releases the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		/// <returns></returns>
		public void ReleaseResources(IHardwareResources hardwareResources)
		{
			ioPortResources.ReleaseResources(hardwareResources);
			memoryResources.ReleaseResources(hardwareResources);
		}
	}
}
