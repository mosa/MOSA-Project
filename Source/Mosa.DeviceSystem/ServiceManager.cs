﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mosa.DeviceSystem;

/// <summary>
/// Device Manager
/// </summary>
public sealed class ServiceManager
{
	private readonly List<BaseService> Services = new List<BaseService>();
	private readonly List<ServiceEvent> Events = new List<ServiceEvent>();

	private readonly object lockServices = new object();
	private readonly object lockEvents = new object();

	public ServiceManager()
	{
	}

	public void AddService(BaseService service)
	{
		lock (lockServices)
		{
			Services.Add(service);
		}

		service.Start(this);
	}

	public void AddEvent(ServiceEvent serviceEvent)
	{
		HAL.DebugWriteLine("ServiceManager:AddEvent()");

		HAL.DebugWriteLine(" # Lock");

		lock (lockEvents)
		{
			HAL.DebugWriteLine(" # Adding");

			Events.Add(serviceEvent);

			HAL.DebugWriteLine(" # Added");
		}

		HAL.DebugWriteLine(" # Unlock");

		HAL.DebugWriteLine(" > SendEvents");

		SendEvents();

		HAL.DebugWriteLine("ServiceManager:AddEvent() [Exit]");
	}

	public List<T> GetService<T>() where T : BaseService
	{
		var list = new List<T>();

		lock (lockServices)
		{
			foreach (var service in Services)
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
		lock (lockServices)
		{
			foreach (var service in Services)
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
		lock (lockServices)
		{
			var list = new List<BaseService>(Services.Count);

			foreach (var service in Services)
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

			lock (lockEvents)
			{
				if (Events.Count == 0)
					break;

				serviceEvent = Events[0];
				Events.RemoveAt(0);
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

			lock (lockServices)
			{
				if (i >= Services.Count)
					break;

				service = Services[i];
			}

			i++;

			service.PostEvent(serviceEvent);
		}

		HAL.DebugWriteLine("ServiceManager:DispatchEvents() [Exit]");
	}
}
