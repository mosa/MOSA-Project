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

		private object _lock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceManager" /> class.
		/// </summary>
		public ServiceManager()
		{
			services = new List<BaseService>();
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
	}
}
