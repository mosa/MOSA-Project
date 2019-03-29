// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Device Manager
	/// </summary>
	public sealed class ServiceManager
	{
		/// <summary>
		/// The devices
		/// </summary>
		private readonly List<BaseService> services;

		private readonly List<ServiceEvent> events;

		private object _lock = new object();
		private object _eventLock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceManager" /> class.
		/// </summary>
		public ServiceManager()
		{
			services = new List<BaseService>();
			events = new List<ServiceEvent>();
		}

		public void AddService(BaseService service)
		{
			lock (_lock)
			{
				services.Add(service);
			}

			service.Start(this);
		}

		public void AddEvent(ServiceEvent serviceEvent)
		{
			lock (_eventLock)
			{
				events.Add(serviceEvent);
			}

			SendEvents();
		}

		public List<T> GetService<T>() where T : BaseService
		{
			var list = new List<T>();

			lock (_lock)
			{
				foreach (var service in services)
				{
					if (service is T s)
					{
						list.Add(s);
					}
				}
			}

			return list;
		}

		public T GetFirstService<T>() where T : BaseService
		{
			lock (_lock)
			{
				foreach (var service in services)
				{
					if (service is T s)
					{
						return s;
					}
				}
			}

			return (T)null;
		}

		public List<BaseService> GetAllServices()
		{
			lock (_lock)
			{
				var list = new List<BaseService>(services.Count);

				foreach (var service in services)
				{
					list.Add(service);
				}

				return list;
			}
		}

		private void SendEvents()
		{
			while (true)
			{
				ServiceEvent serviceEvent;

				lock (_eventLock)
				{
					if (events.Count == 0)
						return;

					serviceEvent = events[0];
					events.RemoveAt(0);
				}

				int i = 0;
				while (true)
				{
					BaseService service;

					lock (_lock)
					{
						if (i >= services.Count)
							break;

						service = services[i++];
					}

					service.PostEvent(serviceEvent);
				}
			}
		}
	}
}
