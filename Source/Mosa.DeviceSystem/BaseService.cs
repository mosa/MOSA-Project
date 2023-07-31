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
	protected virtual void Initialize()
	{ }

	/// <summary>
	/// Posts the event.
	/// </summary>
	/// <param name="serviceEvent">The service event.</param>
	public virtual void PostEvent(ServiceEvent serviceEvent)
	{
	}

	//[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static Device MatchEvent<SERVICE>(ServiceEvent serviceEvent, ServiceEventType eventType) where SERVICE : class
	{
		if (serviceEvent.ServiceEventType != eventType)
			return null;

		var device = serviceEvent.Subject as Device;

		if (device == null)
			return null;

		var service = device.DeviceDriver as SERVICE;

		if (service == null)
			return null;

		//HAL.DebugWriteLine("BaseService:MatchEvent()-Z");
		//HAL.Pause();

		return device;
	}
}
