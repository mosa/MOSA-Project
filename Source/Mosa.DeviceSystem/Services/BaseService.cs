// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// The base class for all services. It can initialize and start services, and it can receive and match specific <see cref="ServiceEvent"/>s
/// sent via the <see cref="ServiceManager"/>. See the Mosa.DeviceSystem.Services namespace for implementations of this class.
/// </summary>
public abstract class BaseService
{
	protected ServiceManager ServiceManager;

	public void Start(ServiceManager serviceManager)
	{
		ServiceManager = serviceManager;
		Initialize();
	}

	protected virtual void Initialize() { }

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
