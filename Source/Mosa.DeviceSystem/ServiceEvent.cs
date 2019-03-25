// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
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
}
