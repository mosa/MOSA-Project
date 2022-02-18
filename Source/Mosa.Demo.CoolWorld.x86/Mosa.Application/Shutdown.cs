// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;
using Mosa.DeviceSystem.Service;

namespace Mosa.Demo.Application
{
	/// <summary>
	/// Shutdown
	/// </summary>
	public class Shutdown : BaseApplication, IConsoleApp
	{
		public override int Start(string parameters)
		{
			Console.WriteLine("Shutting down...");

			var pc = AppManager.ServiceManager.GetFirstService<PCService>();
			pc.Shutdown();

			return 0;
		}
	}
}
