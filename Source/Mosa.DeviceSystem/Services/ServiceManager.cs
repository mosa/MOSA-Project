// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mosa.DeviceSystem.HardwareAbstraction;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Manages all services and its corresponding events throughout the kernel.
/// </summary>
public sealed class ServiceManager
{
	public List<BaseService> Services { get; } = new List<BaseService>();

	public List<ServiceEvent> Events { get; } = new List<ServiceEvent>();

	private readonly object lockServices = new object();
	private readonly object lockEvents = new object();

	public void AddService(BaseService service)
	{
		HAL.DebugWriteLine("ServiceManager::AddService()");

		lock (lockServices)
			Services.Add(service);

		service.Start(this);

		HAL.DebugWriteLine("ServiceManager::AddService() [Exit]");
	}

	public void AddEvent(ServiceEvent serviceEvent)
	{
		HAL.DebugWriteLine("ServiceManager::AddEvent()");

		lock (lockEvents)
			Events.Add(serviceEvent);

		SendEvents();

		HAL.DebugWriteLine("ServiceManager::AddEvent() [Exit]");
	}

	public List<T> GetService<T>() where T : BaseService
	{
		var list = new List<T>();

		lock (lockServices)
			foreach (var service in Services)
				if (service is T s)
					list.Add(s);

		return list;
	}

	public T GetFirstService<T>() where T : BaseService
	{
		lock (lockServices)
			foreach (var service in Services)
				if (service is T s)
					return s;

		return null;
	}

	public List<BaseService> GetAllServices()
	{
		lock (lockServices)
		{
			var list = new List<BaseService>(Services.Count);

			foreach (var service in Services)
				list.Add(service);

			return list;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SendEvents()
	{
		HAL.DebugWriteLine("ServiceManager::SendEvents()");

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

			DispatchEvents(serviceEvent);
		}

		HAL.DebugWriteLine("ServiceManager::SendEvents() [Exit]");
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DispatchEvents(ServiceEvent serviceEvent)
	{
		HAL.DebugWriteLine("ServiceManager::DispatchEvents()");

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

		HAL.DebugWriteLine("ServiceManager::DispatchEvents() [Exit]");
	}
}
