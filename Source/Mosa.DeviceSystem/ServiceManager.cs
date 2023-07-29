// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mosa.DeviceSystem;

/// <summary>
/// Device Manager
/// </summary>
public sealed class ServiceManager
{
	private readonly List<BaseService> services;

	private readonly List<ServiceEvent> events;

	private readonly object _lockServices = new object();
	private readonly object _lockEvents = new object();

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
		HAL.DebugWriteLine("ServiceManager:AddEvent()");

		lock (_lockEvents)
		{
			events.Add(serviceEvent);
		}

		HAL.DebugWriteLine(" > SendEvents");

		SendEvents();

		HAL.DebugWriteLine("ServiceManager:AddEvent() [Exit]");
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

		return null;
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SendEvents()
	{
		HAL.DebugWriteLine("ServiceManager:SendEvents()");

		while (true)
		{
			ServiceEvent serviceEvent;

			lock (_lockEvents)
			{
				if (events.Count == 0)
					break;

				serviceEvent = events[0];
				events.RemoveAt(0);
			}

			HAL.DebugWriteLine(" > DispatchEvents");

			DispatchEvents(serviceEvent);
		}

		HAL.DebugWriteLine("ServiceManager:SendEvents() [Exit]");
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DispatchEvents(ServiceEvent serviceEvent)
	{
		HAL.DebugWriteLine("ServiceManager:DispatchEvents()");

		var i = 0;

		while (true)
		{
			BaseService service;

			lock (_lockServices)
			{
				if (i >= services.Count)
					break;

				service = services[i];
			}

			i++;

			service.PostEvent(serviceEvent);
		}

		HAL.DebugWriteLine("ServiceManager:DispatchEvents() [Exit]");
	}
}
