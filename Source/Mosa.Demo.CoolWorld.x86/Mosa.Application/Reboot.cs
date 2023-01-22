// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.CoolWorld.x86.Mosa.AppSystem;
using Mosa.DeviceSystem.Service;

namespace Mosa.Demo.CoolWorld.x86.Mosa.Application
{
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
}
