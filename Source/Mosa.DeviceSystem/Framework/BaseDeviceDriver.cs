// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Services;

namespace Mosa.DeviceSystem.Framework;

/// <summary>
/// The base class for device drivers. See the Mosa.DeviceDriver project (and some classes in the Mosa.DeviceSystem project) for
/// implementations of this class.
/// </summary>
public abstract class BaseDeviceDriver
{
	public Device Device; // Future: Set to private

	protected DeviceService DeviceService => Device.DeviceService;

	protected readonly object DriverLock = new object();

	public virtual void Setup(Device device)
	{
		Device = device;
		Device.Status = DeviceStatus.Initializing;
	}

	public abstract void Initialize();

	public virtual void Probe() => Device.Status = DeviceStatus.NotFound;

	public virtual void Start() => Device.Status = DeviceStatus.Error;

	public virtual void Stop() { }

	public virtual bool OnInterrupt() => false;
}
