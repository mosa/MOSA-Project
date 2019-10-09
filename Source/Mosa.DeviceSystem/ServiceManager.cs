// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Device Manager
	/// </summary>
	public sealed class ServiceManager
	{
		/// <summary>
		/// The services
		/// </summary>
		private readonly List<BaseService> services;

		private readonly List<ServiceEvent> events;

		private object _lockServices = new object();
		private object _lockEvents = new object();

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
			lock (_lockServices)
			{
				services.Add(service);
			}

			service.Start(this);
		}

		public void AddEvent(ServiceEvent serviceEvent)
		{
			//HAL.DebugWriteLine("ServiceManager:AddEvent():Start");

			lock (_lockEvents)
			{
				events.Add(serviceEvent);
			}

			HAL.DebugWriteLine("ServiceManager:AddEvent():SendEvents");

			SendEvents();

			HAL.DebugWriteLine("ServiceManager:AddEvent():End");
		}

		public List<T> GetService<T>() where T : BaseService
		{
			var list = new List<T>();

			lock (_lockServices)
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
			lock (_lockServices)
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
			lock (_lockServices)
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
			//HAL.DebugWriteLine("ServiceManager:SendEvents():Start");

			while (true)
			{
				ServiceEvent serviceEvent;

				lock (_lockEvents)
				{
					if (events.Count == 0)
						return;

					serviceEvent = events[0];
					events.RemoveAt(0);
				}

				//HAL.DebugWriteLine("ServiceManager:SendEvents():Middle");
				DispatchEvents(serviceEvent);
			}
		}

		private void DispatchEvents(ServiceEvent serviceEvent)
		{
			int i = 0;

			while (true)
			{
				BaseService service;

				//HAL.DebugWriteLine("ServiceManager:SendEvents():Loop" + i.ToString());

				lock (_lockServices)
				{
					if (i >= services.Count)
						return;

					service = services[i];
				}

				i++;

				service.PostEvent(serviceEvent);

				//HAL.DebugWriteLine("ServiceManager:SendEvents():Post-PostEvent");
			}
		}
	}
}
