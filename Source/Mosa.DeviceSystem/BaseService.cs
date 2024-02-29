// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.DeviceSystem;

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
	protected virtual void Initialize() { }

	/// <summary>
	/// Posts the event.
	/// </summary>
	/// <param name="serviceEvent">The service event.</param>
	public virtual void PostEvent(ServiceEvent serviceEvent) { }

	//[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static Device MatchEvent<TService>(ServiceEvent serviceEvent, ServiceEventType eventType) where TService : class
	{
		if (serviceEvent.ServiceEventType != eventType)
			return null;

		if (serviceEvent.Subject is not Device device)
			return null;

		return device.DeviceDriver is not TService ? null : device;
	}
}
