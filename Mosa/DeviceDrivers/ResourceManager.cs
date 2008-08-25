/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceDrivers
{
	public class ResourceManager : IResourceManager
	{
		protected IOPortResources ioPortResources;
		protected MemoryResources memoryResources;
		protected InterruptHandler interruptHandler;

		public IOPortResources IOPortResources { get { return ioPortResources; } }
		public MemoryResources MemoryResources { get { return memoryResources; } }
		public InterruptHandler InterruptHandler { get { return interruptHandler; } }

		public ResourceManager()
		{
			ioPortResources = new IOPortResources();
			memoryResources = new MemoryResources();
			interruptHandler = new InterruptHandler();
		}
	}
}
