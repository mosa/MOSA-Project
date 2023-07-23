// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Service;

namespace Mosa.DeviceSystem;

/// <summary>
/// Abstract class for hardware devices
/// </summary>
public abstract class BaseDeviceDriver
{
	public Device Device;   // Future: Set to private

	protected DeviceService DeviceService => Device.DeviceService;

	protected object _lock = new object();

	/// <summary>
	/// Sets up the this device.
	/// </summary>
	/// <param name="device">The device.</param>
	public virtual void Setup(Device device)
	{
		Device = device;
		Device.Status = DeviceStatus.Initializing;
	}

	/// <summary>
	/// Initializes this device.
	/// </summary>
	public abstract void Initialize();

	/// <summary>
	/// Probes this instance.
	/// </summary>
	public virtual void Probe()
	{
		Device.Status = DeviceStatus.NotFound;
	}

	/// <summary>
	/// Starts this hardware device.
	/// </summary>
	public virtual void Start()
	{
		Device.Status = DeviceStatus.Error;
	}

	/// <summary>
	/// Stops this hardware device.
	/// </summary>
	public virtual void Stop()
	{
	}

	/// <summary>
	/// Called when an interrupt is received.
	/// </summary>
	/// <returns></returns>
	public virtual bool OnInterrupt()
	{
		return false;
	}
}
