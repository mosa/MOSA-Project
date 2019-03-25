// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public abstract class BaseService
	{
		/// <summary>
		/// The service manager
		/// </summary>
		protected ServiceManager ServiceManager;

		/// <summary>
		/// Sets the service manager.
		/// </summary>
		/// <param name="serviceManager">The service manager.</param>
		public void Start(ServiceManager serviceManager)
		{
			ServiceManager = serviceManager;
			Initialize();
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public virtual void Initialize()
		{ }

		/// <summary>
		/// Posts the event.
		/// </summary>
		/// <param name="serviceEvent">The service event.</param>
		public virtual void PostEvent(ServiceEvent serviceEvent)
		{
		}
	}
}
