// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem.Service;

/// <summary>
/// Device Manager
/// </summary>
public sealed class DeviceService : BaseService
{
	/// <summary>
	/// The maximum interrupts
	/// </summary>
	public const ushort MaxInterrupts = 32;

	/// <summary>
	/// The registered device drivers
	/// </summary>
	private readonly List<DeviceDriverRegistryEntry> Registry;

	/// <summary>
	/// The devices
	/// </summary>
	private readonly List<Device> Devices;

	/// <summary>
	/// The interrupt handlers
	/// </summary>
	private readonly List<Device>[] IRQDispatch;

	/// <summary>
	/// The pending on change
	/// </summary>
	private readonly List<Device> pendingOnChange;

	private readonly object sync = new object();

	/// <summary>
	/// Initializes a new instance of the <see cref="DeviceService" /> class.
	/// </summary>
	public DeviceService()
	{
		Registry = new List<DeviceDriverRegistryEntry>();
		Devices = new List<Device>();
		pendingOnChange = new List<Device>();

		IRQDispatch = new List<Device>[MaxInterrupts];

		for (int i = 0; i < MaxInterrupts; i++)
		{
			IRQDispatch[i] = new List<Device>();
		}
	}

	#region Device Driver Registry

	public void RegisterDeviceDriver(List<DeviceDriverRegistryEntry> deviceDrivers)
	{
		var platformArchitecture = HAL.PlatformArchitecture;

		foreach (var deviceDriver in deviceDrivers)
		{
			if ((deviceDriver.Platform & platformArchitecture) == platformArchitecture)
			{
				RegisterDeviceDriver(deviceDriver);
			}
		}
	}

	public void RegisterDeviceDriver(DeviceDriverRegistryEntry deviceDriver)
	{
		lock (sync)
		{
			Registry.Add(deviceDriver);
		}
	}

	public List<DeviceDriverRegistryEntry> GetDeviceDrivers(DeviceBusType busType)
	{
		var drivers = new List<DeviceDriverRegistryEntry>();

		lock (sync)
		{
			foreach (var deviceDriver in Registry)
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

	public Device Initialize(DeviceDriverRegistryEntry deviceDriverRegistryEntry, Device parent, bool autoStart = true, BaseDeviceConfiguration configuration = null, HardwareResources resources = null)
	{
		var deviceDriver = deviceDriverRegistryEntry.Factory();

		return Initialize(deviceDriver, parent, autoStart, configuration, resources, deviceDriverRegistryEntry);
	}

	public Device Initialize(BaseDeviceDriver deviceDriver, Device parent, bool autoStart = true, BaseDeviceConfiguration configuration = null, HardwareResources resources = null, DeviceDriverRegistryEntry deviceDriverRegistryEntry = null)
	{
		HAL.DebugWriteLine("DeviceService:Initialize()");

		var device = new Device
		{
			DeviceDriver = deviceDriver,
			DeviceDriverRegistryEntry = deviceDriverRegistryEntry,
			Status = DeviceStatus.Initializing,
			Parent = parent,
			Configuration = configuration,
			Resources = resources,
			DeviceService = this,

			//Name = string.Empty,
		};

		if (autoStart)
		{
			StartDevice(device);
		}

		HAL.DebugWriteLine("DeviceService:Initialize() [Exit]");

		return device;
	}

	/// <summary>
	/// Adds the specified device.
	/// </summary>
	/// <param name="device">The device.</param>
	private void StartDevice(Device device)
	{
		HAL.DebugWriteLine("DeviceService:StartDevice()");

		lock (sync)
		{
			Devices.Add(device);

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

			HAL.DebugWrite(" # Initialized: ");
			HAL.DebugWriteLine(device.Name);

			if (device.Status == DeviceStatus.Initializing)
			{
				device.DeviceDriver.Probe();

				if (device.Status == DeviceStatus.Available)
				{
					device.DeviceDriver.Start();

					AddInterruptHandler(device);
				}
			}
		}

		ServiceManager.AddEvent(new ServiceEvent(ServiceEventType.Start, device));

		HAL.DebugWriteLine("DeviceService:StartDevice():Exit");
	}

	#endregion Initialize Devices Drivers

	#region Get Devices

	public Device GetFirstDevice<T>()
	{
		lock (sync)
		{
			foreach (var device in Devices)
			{
				if (device.DeviceDriver is T)
				{
					return device;
				}
			}
		}

		return null;
	}

	public List<Device> GetDevices<T>()
	{
		var list = new List<Device>();

		lock (sync)
		{
			foreach (var device in Devices)
			{
				if (device.DeviceDriver is T)
				{
					list.Add(device);
				}
			}
		}

		return list;
	}

	public Device GetFirstDevice<T>(DeviceStatus status)
	{
		lock (sync)
		{
			foreach (var device in Devices)
			{
				if (device.Status == status && device.DeviceDriver is T)
				{
					return device;
				}
			}
		}

		return null;
	}

	public List<Device> GetDevices<T>(DeviceStatus status)
	{
		var list = new List<Device>();

		lock (sync)
		{
			foreach (var device in Devices)
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

		lock (sync)
		{
			foreach (var device in Devices)
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

		lock (sync)
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
		lock (sync)
		{
			var list = new List<Device>(Devices.Count);

			foreach (var device in Devices)
			{
				list.Add(device);
			}

			return list;
		}
	}

	public bool CheckExists(Device parent, ulong componentID)
	{
		lock (sync)
		{
			foreach (var device in Devices)
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
		lock (sync)
		{
			foreach (var device in IRQDispatch[irq])
			{
				var deviceDriver = device.DeviceDriver;
				deviceDriver.OnInterrupt();
			}
		}
	}

	public void AddInterruptHandler(Device device)
	{
		HAL.DebugWriteLine("DeviceService::AddInterruptHandler()");

		if (device.Resources != null)
		{
			var irq = device.Resources.IRQ;

			if (irq >= MaxInterrupts)
				return;

			lock (sync)
			{
				IRQDispatch[irq].Add(device);
			}
		}

		HAL.DebugWriteLine("DeviceService::AddInterruptHandler() [Exit]");
	}

	public void ReleaseInterruptHandler(Device device)
	{
		if (device.Resources != null)
		{
			byte irq = device.Resources.IRQ;

			if (irq >= MaxInterrupts)
				return;

			lock (sync)
			{
				IRQDispatch[irq].Remove(device);
			}
		}
	}

	#endregion Interrupts
}
