// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.AppSystem;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.Application
{
	/// <summary>
	/// ShowPCI
	/// </summary>
	public class ShowPCI : BaseApplication, IConsoleApp
	{
		public override int Start(string parameters)
		{
			var deviceService = AppManager.ServiceManager.GetFirstService<DeviceService>();

			Console.Write("> Probing for PCI devices...");
			var devices = deviceService.GetDevices<PCIDevice>();
			Console.WriteLine("[Completed: " + devices.Count.ToString() + " found]");

			foreach (var device in devices)
			{
				var pciDevice = device.DeviceDriver as PCIDevice;
				Console.Write(device.Name);

				Console.Write(": ");
				Console.Write(pciDevice.VendorID.ToString("x"));
				Console.Write(":");
				Console.Write(pciDevice.DeviceID.ToString("x"));
				Console.Write(" ");
				Console.Write(pciDevice.SubSystemID.ToString("x"));
				Console.Write(":");
				Console.Write(pciDevice.SubSystemVendorID.ToString("x"));
				Console.Write(" (");
				Console.Write(pciDevice.Function.ToString("x"));
				Console.Write(":");
				Console.Write(pciDevice.ClassCode.ToString("x"));
				Console.Write(":");
				Console.Write(pciDevice.SubClassCode.ToString("x"));
				Console.Write(":");
				Console.Write(pciDevice.ProgIF.ToString("x"));
				Console.Write(":");
				Console.Write(pciDevice.RevisionID.ToString("x"));
				Console.Write(")");

				var children = deviceService.GetChildrenOf(device);

				if (children.Count != 0)
				{
					var child = children[0];

					Console.WriteLine();
					Console.Write("    ");

					var pciDevice2 = child.DeviceDriver as PCIDevice;
					Console.Write(child.Name);
				}

				Console.WriteLine();
			}

			return 0;
		}
	}
}
