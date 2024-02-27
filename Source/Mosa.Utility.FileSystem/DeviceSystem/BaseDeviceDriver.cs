// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Framework;

public abstract class BaseDeviceDriver
{
	protected Device Device;

	public void Setup(Device device)
	{
		Device = device;
		Device.Status = DeviceStatus.Initializing;
	}

	public abstract void Initialize();

	public virtual void Probe() => Device.Status = DeviceStatus.NotFound;

	public virtual void Start() => Device.Status = DeviceStatus.Error;

	public virtual bool OnInterrupt() => false;
}
