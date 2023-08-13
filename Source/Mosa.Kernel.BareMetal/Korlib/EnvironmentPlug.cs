// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal.Korlib;

public class EnvironmentPlug
{
	[Plug("System.Environment::FailFast")]
	public static void FailFast(string message)
	{
		var deviceService = Kernel.ServiceManager.GetFirstService<DeviceService>();
		var graphicsDevices = deviceService.GetDevices<IGraphicsDevice>(DeviceStatus.Online);

		foreach (var device in graphicsDevices) ((IGraphicsDevice)device.DeviceDriver).Disable();

		Debug.WriteLine("*** FailFast ***");
		Debug.WriteLine(message);
		Debug.Fatal();
	}

	[Plug("System.Environment::Exit")]
	public static void Exit(int exitCode)
	{
		var pcService = Kernel.ServiceManager.GetFirstService<PCService>();
		pcService.Shutdown();
	}

	[Plug("System.Environment::GetProcessorCount")]
	public static int GetProcessorCount() => (int)Platform.GetCPU().Cores;
}
