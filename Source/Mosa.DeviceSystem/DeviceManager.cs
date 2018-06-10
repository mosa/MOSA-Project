// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Device Manager
	/// </summary>
	public sealed class DeviceManager
	{
		/// <summary>
		/// The maximum interrupts
		/// </summary>
		public const ushort MaxInterrupts = 32;

		/// <summary>
		/// Gets the platform architecture.
		/// </summary>
		public PlatformArchitecture PlatformArchitecture { get; }

		/// <summary>
		/// The registered device drivers
		/// </summary>
		private readonly List<DeviceDriverRegistryEntry> registry;

		/// <summary>
		/// The devices
		/// </summary>
		private readonly List<Device> devices;

		/// <summary>
		/// The interrupt handlers
		/// </summary>
		private readonly List<Device>[] irqDispatch;

		/// <summary>
		/// The mount daemons
		/// </summary>
		private readonly List<BaseMountDaemon> daemons;

		/// <summary>
		/// The pending on change
		/// </summary>
		private readonly List<Device> pendingOnChange;

		private object _lock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceManager" /> class.
		/// </summary>
		/// <param name="platform">The platform.</param>
		public DeviceManager(PlatformArchitecture platform)
		{
			registry = new List<DeviceDriverRegistryEntry>();
			devices = new List<Device>();
			daemons = new List<BaseMountDaemon>();
			pendingOnChange = new List<Device>();

			irqDispatch = new List<Device>[MaxInterrupts];
			PlatformArchitecture = platform;

			for (int i = 0; i < MaxInterrupts; i++)
			{
				irqDispatch[i] = new List<Device>();
			}
		}

		#region Device Driver Registry

		public void RegisterDeviceDriver(DeviceDriverRegistryEntry deviceDriver)
		{
			lock (_lock)
			{
				registry.Add(deviceDriver);
			}
		}

		public List<DeviceDriverRegistryEntry> GetDeviceDrivers(DeviceBusType busType)
		{
			var drivers = new List<DeviceDriverRegistryEntry>();

			lock (_lock)
			{
				foreach (var deviceDriver in registry)
				{
					if (deviceDriver.BusType == busType)
					{
						drivers.Add(deviceDriver);
					}
				}
			}

			return drivers;
		}

		#endregion Device Driver Registry

		#region Initialize Devices Drivers

		public Device Initialize(DeviceDriverRegistryEntry deviceDriverRegistryEntry, Device parent, BaseDeviceConfiguration configuration = null, HardwareResources resources = null)
		{
			var deviceDriver = deviceDriverRegistryEntry.Factory();

			return Initialize(deviceDriver, parent, configuration, resources, deviceDriverRegistryEntry);
		}

		public Device Initialize(BaseDeviceDriver deviceDriver, Device parent, BaseDeviceConfiguration configuration = null, HardwareResources resources = null, DeviceDriverRegistryEntry deviceDriverRegistryEntry = null)
		{
			var device = new Device()
			{
				DeviceDriver = deviceDriver,
				DeviceDriverRegistryEntry = deviceDriverRegistryEntry,
				Status = DeviceStatus.Initializing,
				Parent = parent,
				Configuration = configuration,
				Resources = resources,
				DeviceManager = this,

				//Name = string.Empty,
			};

			StartDevice(device);

			OnChangeNotification(device);

			ProcessOnChangeNotifications();

			return device;
		}

		/// <summary>
		/// Adds the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		private void StartDevice(Device device)
		{
			//HAL.DebugWriteLine("DeviceManger:StartDevice():Enter");

			lock (_lock)
			{
				devices.Add(device);

				if (device.Parent != null)
				{
					device.Parent.Children.Add(device);
				}
			}

			device.Status = DeviceStatus.Initializing;

			device.DeviceDriver.Setup(device);

			if (device.Status == DeviceStatus.Initializing)
			{
				device.DeviceDriver.Initialize();

				if (device.Status == DeviceStatus.Initializing)
				{
					device.DeviceDriver.Probe();

					if (device.Status == DeviceStatus.Available)
					{
						device.DeviceDriver.Start();
					}
				}
			}

			//HAL.DebugWriteLine("DeviceManger:StartDevice():Exit");
		}

		#endregion Initialize Devices Drivers

		#region Get Devices

		public List<Device> GetDevices<T>()
		{
			var list = new List<Device>();

			lock (_lock)
			{
				foreach (var device in devices)
				{
					if (device.DeviceDriver is T)
					{
						list.Add(device);
					}
				}
			}

			return list;
		}

		public List<Device> GetDevices<T>(DeviceStatus status)
		{
			var list = new List<Device>();

			lock (_lock)
			{
				foreach (var device in devices)
				{
					if (device.Status == status && device.DeviceDriver is T)
					{
						list.Add(device);
					}
				}
			}

			return list;
		}

		public List<Device> GetDevices(string name)
		{
			var list = new List<Device>();

			lock (_lock)
			{
				foreach (var device in devices)
				{
					if (device.Name == name)
					{
						list.Add(device);
					}
				}
			}

			return list;
		}

		public List<Device> GetChildrenOf(Device parent)
		{
			var list = new List<Device>();

			lock (_lock)
			{
				foreach (var device in parent.Children)
				{
					list.Add(device);
				}
			}

			return list;
		}

		public List<Device> GetAllDevices()
		{
			var list = new List<Device>();

			lock (_lock)
			{
				foreach (var device in devices)
				{
					list.Add(device);
				}
			}

			return list;
		}

		public bool CheckExists(Device parent, ulong componentID)
		{
			lock (_lock)
			{
				foreach (var device in devices)
				{
					if (device.Parent == parent && device.ComponentID == componentID)
					{
						return true;
					}
				}
			}

			return false;
		}

		#endregion Get Devices

		#region Interrupts

		public void ProcessInterrupt(byte irq)
		{
			lock (_lock)
			{
				foreach (var device in irqDispatch[irq])
				{
					var deviceDriver = device.DeviceDriver;
					deviceDriver.OnInterrupt();
				}
			}
		}

		public void AddInterruptHandler(byte irq, Device device)
		{
			if (irq >= MaxInterrupts)
				return;

			lock (_lock)
			{
				irqDispatch[irq].Add(device);
			}
		}

		public void ReleaseInterruptHandler(byte irq, Device device)
		{
			if (irq >= MaxInterrupts)
				return;

			lock (_lock)
			{
				irqDispatch[irq].Remove(device);
			}
		}

		#endregion Interrupts

		#region Daemons

		public void RegisterDaemon(BaseMountDaemon daemon)
		{
			lock (_lock)
			{
				daemons.Add(daemon);
			}
		}

		public void OnChangeNotification(Device device)
		{
			//HAL.DebugWriteLine("OnChangeNotification:OnChange():Enter");

			if (device == null)
				return;

			lock (_lock)
			{
				//if (pendingOnChange.Contains(device))
				//	return;

				pendingOnChange.Add(device);
			}

			//HAL.DebugWriteLine("OnChangeNotification:OnChange():Exit");

			ProcessOnChangeNotifications();
		}

		private Device GetPendingOnChangeNotification()
		{
			lock (_lock)
			{
				if (pendingOnChange.Count == 0)
					return null;

				var device = pendingOnChange[0];
				pendingOnChange.RemoveAt(0);

				return device;
			}
		}

		private void ProcessOnChangeNotifications()
		{
			//HAL.DebugWriteLine("DeviceManger:ProcessOnChangeNotifications():Enter");

			var device = GetPendingOnChangeNotification();

			if (device != null)
			{
				foreach (var daemon in daemons)
				{
					daemon.OnChange(device);
				}
			}

			//HAL.DebugWriteLine("DeviceManger:ProcessOnChangeNotifications():Exit");
		}

		#endregion Daemons
	}
}
