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
		/// The spin lock
		/// </summary>
		private SpinLock spinLock;

		/// <summary>
		/// The interrupt handlers
		/// </summary>
		private readonly List<Device>[] irqDispatch;

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceManager" /> class.
		/// </summary>
		/// <param name="platform">The platform.</param>
		public DeviceManager(PlatformArchitecture platform)
		{
			registry = new List<DeviceDriverRegistryEntry>();
			devices = new List<Device>();

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
			try
			{
				spinLock.Enter();

				registry.Add(deviceDriver);
			}
			finally
			{
				spinLock.Exit();
			}
		}

		public List<DeviceDriverRegistryEntry> GetDeviceDrivers(DeviceBusType busType)
		{
			var drivers = new List<DeviceDriverRegistryEntry>();

			try
			{
				spinLock.Enter();

				foreach (var deviceDriver in registry)
				{
					if (deviceDriver.BusType == busType)
					{
						drivers.Add(deviceDriver);
					}
				}
			}
			finally
			{
				spinLock.Exit();
			}

			return drivers;
		}

		#endregion Device Driver Registry

		#region Initialize Devices Drivers

		public BaseDeviceDriver Initialize(DeviceDriverRegistryEntry deviceDriverRegistryEntry, Device parent, BaseDeviceConfiguration configuration = null, HardwareResources resources = null)
		{
			var deviceDriver = deviceDriverRegistryEntry.Factory();

			Initialize(deviceDriver, parent, configuration, resources, deviceDriverRegistryEntry);

			return deviceDriver;
		}

		public void Initialize(BaseDeviceDriver deviceDriver, Device parent, BaseDeviceConfiguration configuration = null, HardwareResources resources = null, DeviceDriverRegistryEntry deviceDriverRegistryEntry = null)
		{
			var device = new Device()
			{
				DeviceDriver = deviceDriver,
				DeviceDriverRegistryEntry = deviceDriverRegistryEntry,
				Status = DeviceStatus.Initializing,
				Parent = parent,
				Configuration = configuration,
				Resources = resources,

				//Service = deviceDriver as IService

				DeviceManager = this,
			};

			Initialize(device);
		}

		/// <summary>
		/// Adds the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		private void Initialize(Device device)
		{
			spinLock.Enter();
			devices.Add(device);

			if (device.Parent != null)
			{
				device.Parent.Children.Add(device);
			}

			spinLock.Exit();

			device.DeviceDriver.Setup(device);
		}

		#endregion Initialize Devices Drivers

		#region Get Devices

		public List<Device> GetDevices<T>()
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in devices)
			{
				if (device.DeviceDriver is T)
				{
					list.Add(device);
				}
			}

			spinLock.Exit();

			return list;
		}

		public List<Device> GetDevices<T>(DeviceStatus status)
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in devices)
			{
				if (device.Status == status && device.DeviceDriver is T)
				{
					list.Add(device);
				}
			}

			spinLock.Exit();

			return list;
		}

		public List<Device> GetDevices(string name)
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in devices)
			{
				if (device.Name == name)
				{
					list.Add(device);
				}
			}

			spinLock.Exit();

			return list;
		}

		public List<Device> GetChildrenOf(Device parent)
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in parent.Children)
			{
				list.Add(device);
			}

			spinLock.Exit();

			return list;
		}

		public List<Device> GetAllDevices()
		{
			spinLock.Enter();

			var list = new List<Device>();

			foreach (var device in devices)
			{
				list.Add(device);
			}

			spinLock.Exit();

			return list;
		}

		#endregion Get Devices

		#region Interrupts

		public void ProcessInterrupt(byte irq)
		{
			try
			{
				spinLock.Enter();

				foreach (var device in irqDispatch[irq])
				{
					var deviceDriver = device.DeviceDriver;
					deviceDriver.OnInterrupt();
				}
			}
			finally
			{
				spinLock.Exit();
			}
		}

		public void AddInterruptHandler(byte irq, Device device)
		{
			if (irq >= MaxInterrupts)
				return;

			try
			{
				spinLock.Enter();
				irqDispatch[irq].Add(device);
			}
			finally
			{
				spinLock.Exit();
			}
		}

		public void ReleaseInterruptHandler(byte irq, Device device)
		{
			if (irq >= MaxInterrupts)
				return;

			try
			{
				spinLock.Enter();
				irqDispatch[irq].Remove(device);
			}
			finally
			{
				spinLock.Exit();
			}
		}

		#endregion Interrupts
	}
}
