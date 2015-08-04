// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public interface IResourceManager
	{
		/// <summary>
		/// Gets the IO port resources.
		/// </summary>
		/// <value>The IO port resources.</value>
		IOPortResources IOPortResources { get; }

		/// <summary>
		/// Gets the memory resources.
		/// </summary>
		/// <value>The memory resources.</value>
		MemoryResources MemoryResources { get; }

		/// <summary>
		/// Gets the interrupt manager.
		/// </summary>
		/// <value>The interrupt manager.</value>
		InterruptManager InterruptManager { get; }

		/// <summary>
		/// Claims the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		/// <returns></returns>
		bool ClaimResources(IHardwareResources hardwareResources);

		/// <summary>
		/// Releases the resources.
		/// </summary>
		/// <param name="hardwareResources">The hardware resources.</param>
		void ReleaseResources(IHardwareResources hardwareResources);
	}
}
