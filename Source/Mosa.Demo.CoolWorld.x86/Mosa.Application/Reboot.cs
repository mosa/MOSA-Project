// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;
using Mosa.DeviceSystem.Service;

namespace Mosa.Demo.Application;

/// <summary>
/// Reboot
/// </summary>
public class Reboot : BaseApplication, IConsoleApp
{
	public override int Start(string parameters)
	{
		Console.WriteLine("Rebooting...");

		var pc = AppManager.ServiceManager.GetFirstService<PCService>();
		pc.Reset();

		return 0;
	}
}
