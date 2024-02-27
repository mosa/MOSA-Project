// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Describes a service event, sent by the <see cref="ServiceManager"/>. It can optionally contain a subject (usually a
/// <see cref="Device"/>) as well.
/// </summary>
public struct ServiceEvent
{
	public ServiceEventType ServiceEventType { get; }

	public object Subject { get; }

	public ServiceEvent(ServiceEventType serviceEventType, object subject = null)
	{
		ServiceEventType = serviceEventType;
		Subject = subject;
	}
}
