// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

/// <summary>
/// Abstract class for hardware devices
/// </summary>
public abstract class BaseDeviceDriver
{
	protected Device Device;

	/// <summary>
	/// Sets up the this device.
	/// </summary>
	/// <param name="device">The device.</param>
	public void Setup(Device device)
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
	public virtual void Probe() => Device.Status = DeviceStatus.NotFound;

	/// <summary>
	/// Starts this hardware device.
	/// </summary>
	public virtual void Start() => Device.Status = DeviceStatus.Error;

	/// <summary>
	/// Called when an interrupt is received.
	/// </summary>
	/// <returns></returns>
	public virtual bool OnInterrupt() => false;
}
