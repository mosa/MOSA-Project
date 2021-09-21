// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;
using Mosa.DeviceSystem;
using Mosa.DeviceDriver.ISA;

namespace Mosa.Demo.Application
{
	/// <summary>
	/// Shutdown
	/// </summary>
	public class Shutdown : BaseApplication, IConsoleApp
	{
		public override int Start(string parameters)
		{
			var deviceService = AppManager.ServiceManager.GetFirstService<DeviceService>();

			Console.Write("> Shutting down...");
			var acpiDevices = deviceService.GetDevices("ACPI");

			if (acpiDevices.Count == 0)
			{
				Console.WriteLine("This computer doesn't support ACPI!");
				return 1;
			}

			var acpi = acpiDevices[0].DeviceDriver as ACPI;
			acpi.Shutdown();

			return 0;
		}
	}
}
