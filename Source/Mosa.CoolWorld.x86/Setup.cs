/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.DeviceDriver.PCI.VideoCard;
using System.Reflection;

namespace Mosa.CoolWorld.x86
{
	/// <summary>
	/// Setup for the Device Driver System.
	/// </summary>
	public static class Setup
	{
		static private DeviceDriverRegistry deviceDriverRegistry;
		static private IDeviceManager deviceManager;
		static private IResourceManager resourceManager;

		/// <summary>
		/// Gets the device driver library
		/// </summary>
		/// <value>The device driver library.</value>
		static public DeviceDriverRegistry DeviceDriverRegistry { get { return deviceDriverRegistry; } }

		/// <summary>
		/// Gets the device manager.
		/// </summary>
		/// <value>The device manager.</value>
		static public IDeviceManager DeviceManager { get { return deviceManager; } }

		/// <summary>
		/// Gets the resource manager.
		/// </summary>
		/// <value>The resource manager.</value>
		static public IResourceManager ResourceManager { get { return resourceManager; } }

		static public StandardKeyboard Keyboard = null;
		static public PCIController PCI = null;
		static public PIC PIC = null;
		static public PIT PIT = null;
		static public VGAText VGAText = null;
		static public CMOS CMOS = null;
		static public PCIControllerManager PCIControllerManager = null;

		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize()
		{
			// Create Resource Manager
			resourceManager = new ResourceManager();

			// Create Device Manager
			deviceManager = new DeviceManager();

			// Create the Device Driver Manager
			deviceDriverRegistry = new DeviceDriverRegistry(PlatformArchitecture.X86);

			// Setup hardware abstraction interface
			IHardwareAbstraction hardwareAbstraction = new Mosa.CoolWorld.x86.HAL.HardwareAbstraction();

			// Set device driver system to the hardware HAL
			Mosa.DeviceSystem.HAL.SetHardwareAbstraction(hardwareAbstraction);

			// Set the interrupt handler
			Mosa.DeviceSystem.HAL.SetInterruptHandler(ResourceManager.InterruptManager.ProcessInterrupt);
		}

		/// <summary>
		/// Start the Device Driver System.
		/// </summary>
		static public void Start()
		{
			// Start drivers for ISA devices
			StartISADevices();

			// Start drivers for PCI devices
			StartPCIDevices();
		}

		/// <summary>
		/// Starts the PCI devices.
		/// </summary>
		static public void StartPCIDevices()
		{
			Boot.Console.Write("Finding Drivers...");
			deviceDriverRegistry.RegisterBuiltInDeviceDrivers();
			Boot.Console.WriteLine("[Completed]");

			Boot.Console.Write("Starting PCI devices... ");

			var devices = deviceManager.GetDevices(new FindDevice.IsPCIDevice(), new FindDevice.IsAvailable());

			Boot.Console.Write(devices.Count.ToString());
			Boot.Console.WriteLine(" Devices");

			foreach (IDevice device in devices)
			{
				var pciDevice = device as IPCIDevice;

				Mosa.CoolWorld.x86.Boot.BulletPoint();

				Boot.Console.WriteLine(device.Name + ": " + pciDevice.VendorID.ToString("x") + "." + pciDevice.DeviceID.ToString("x") + "." + pciDevice.Function.ToString("x") + "." + pciDevice.ClassCode.ToString("x"));

				StartDevice(pciDevice);
			}

			Boot.Console.WriteLine("Found " + deviceDriverRegistry.GetPCIDeviceDrivers().Count.ToString("x") + " PCI drivers.");
			foreach (var a in Assembly.GetAssemblies())
				foreach (var t in a.DefinedTypes)
					if (t.AsType() == typeof(VMwareSVGAII))
						foreach (var c in t.CustomAttributes)
						{
							Boot.Console.Write(c.AttributeType.FullName);
							foreach (var n in c.NamedArguments)
							{
								Boot.Console.Write(n.MemberName + ((uint)n.TypedValue.Value).ToString());
							}
						}
						
		}

		/// <summary>
		/// Starts the ISA devices.
		/// </summary>
		static public void StartISADevices()
		{
			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x60, PortRange = 1, AltBasePort = 0x64, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.X86AndX64)]
			var keyboardDeviceAttributes = new ISADeviceDriverAttribute();
			keyboardDeviceAttributes.AutoLoad = true;
			keyboardDeviceAttributes.BasePort = 0x60;
			keyboardDeviceAttributes.PortRange = 1;
			keyboardDeviceAttributes.AltBasePort = 0x64;
			keyboardDeviceAttributes.IRQ = 1;
			keyboardDeviceAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
			var pciAttributes = new ISADeviceDriverAttribute();
			pciAttributes.AutoLoad = true;
			pciAttributes.BasePort = 0x0CF8;
			pciAttributes.PortRange = 8;
			pciAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x20, PortRange = 2, AltBasePort = 0xA0, AltPortRange = 2, Platforms = PlatformArchitecture.X86AndX64)]
			var picAttributes = new ISADeviceDriverAttribute();
			picAttributes.AutoLoad = true;
			picAttributes.BasePort = 0x20;
			picAttributes.PortRange = 2;
			picAttributes.AltBasePort = 0xA0;
			picAttributes.AltPortRange = 2;
			picAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x40, PortRange = 4, IRQ = 0, Platforms = PlatformArchitecture.X86AndX64)]
			var pitAttributes = new ISADeviceDriverAttribute();
			pitAttributes.AutoLoad = true;
			pitAttributes.BasePort = 0x40;
			pitAttributes.PortRange = 4;
			pitAttributes.IRQ = 0;
			pitAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x03B0, PortRange = 0x1F, BaseAddress = 0xB0000, AddressRange = 0x10000, Platforms = PlatformArchitecture.X86AndX64)]
			var vgaTextAttributes = new ISADeviceDriverAttribute();
			vgaTextAttributes.AutoLoad = true;
			vgaTextAttributes.BasePort = 0x03B0;
			vgaTextAttributes.PortRange = 0x1F;
			vgaTextAttributes.BaseAddress = 0xB0000;
			vgaTextAttributes.AddressRange = 0x10000;
			vgaTextAttributes.IRQ = 0;
			vgaTextAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x0070, PortRange = 2, Platforms = PlatformArchitecture.X86)]
			var cmosAttributes = new ISADeviceDriverAttribute();
			cmosAttributes.AutoLoad = true;
			cmosAttributes.BasePort = 0x0070;
			cmosAttributes.PortRange = 0x02;
			cmosAttributes.Platforms = PlatformArchitecture.X86AndX64;

			Keyboard = new StandardKeyboard();
			PCI = new PCIController();
			PIC = new PIC();
			PIT = new PIT();
			VGAText = new VGAText();
			CMOS = new CMOS();

			//StartDevice(picAttributes, PIC);
			StartDevice(pitAttributes, PIT);
			StartDevice(pciAttributes, PCI);
			StartDevice(keyboardDeviceAttributes, Keyboard);
			StartDevice(cmosAttributes, CMOS);
			//StartDevice(vgaTextAttributes, VGAText);

			PCIControllerManager = new PCIControllerManager(deviceManager);

			Boot.Console.Write("Probing PCI devices...");

			PCIControllerManager.CreatePCIDevices();

			Boot.Console.WriteLine("[Completed]");
		}

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="deviceDriver">The device driver.</param>
		static public void StartDevice(ISADeviceDriverAttribute driverAtttribute, IHardwareDevice hardwareDevice)
		{
			int ioRegionCount = 1;
			int memoryRegionCount = 0;

			if (driverAtttribute.AltBasePort != 0x00)
			{
				ioRegionCount++;
			}

			if (driverAtttribute.BaseAddress != 0x00)
				memoryRegionCount++;

			var ioPortRegions = new IIOPortRegion[ioRegionCount];
			var memoryRegions = new IMemoryRegion[memoryRegionCount];

			ioPortRegions[0] = new IOPortRegion(driverAtttribute.BasePort, driverAtttribute.PortRange);

			if (driverAtttribute.AltBasePort != 0x00)
			{
				ioPortRegions[1] = new IOPortRegion(driverAtttribute.AltBasePort, driverAtttribute.AltPortRange);
			}

			if (driverAtttribute.BaseAddress != 0x00)
			{
				memoryRegions[0] = new MemoryRegion(driverAtttribute.BaseAddress, driverAtttribute.AddressRange);
			}

			var hardwareResources = new HardwareResources(resourceManager, ioPortRegions, memoryRegions, new InterruptHandler(resourceManager.InterruptManager, driverAtttribute.IRQ, hardwareDevice));

			hardwareDevice.Setup(hardwareResources);

			Mosa.CoolWorld.x86.Boot.BulletPoint();
			Boot.Console.Write("Adding device ");
			Boot.InBrackets(hardwareDevice.Name, Mosa.Kernel.x86.Colors.White, Mosa.Kernel.x86.Colors.LightGreen);
			Boot.Console.WriteLine();

			if (resourceManager.ClaimResources(hardwareResources))
			{
				hardwareResources.EnableIRQ();
				if (hardwareDevice.Start() == DeviceDriverStartStatus.Started)
				{
					deviceManager.Add(hardwareDevice);
				}
				else
				{
					hardwareResources.DisableIRQ();
					resourceManager.ReleaseResources(hardwareResources);
				}
			}
		}

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="pciDevice">The pci device.</param>
		static public void StartDevice(IPCIDevice pciDevice)
		{
			var deviceDriver = deviceDriverRegistry.FindDriver(pciDevice);

			if (deviceDriver == null)
			{
				Boot.Console.Write(".");
				pciDevice.SetNoDriverFound();
				return;
			}

			var hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;

			//if (pciDevice.VendorID == 0x15AD && pciDevice.DeviceID == 0x0405)
			//{
			//	hardwareDevice = new VMwareSVGAII();
			//	Boot.Console.WriteLine("VMwareSVGAII");
			//}

			if (hardwareDevice != null)
			{
				StartDevice(pciDevice, hardwareDevice);
			}
			else
			{
				Boot.Console.WriteLine("Failed to load driver.");
				return;
			}

			if (pciDevice.VendorID == 0x15AD && pciDevice.DeviceID == 0x0405)
			{
				var display = hardwareDevice as IPixelGraphicsDevice;

				var color = new Color(255, 0, 0);

				display.WritePixel(color, 100, 100);
			}
		}

		private static void StartDevice(IPCIDevice pciDevice, IHardwareDevice hardwareDevice)
		{
			int ioRegionCount = 0;
			int memoryRegionCount = 0;

			foreach (var pciBaseAddress in pciDevice.BaseAddresses)
			{
				switch (pciBaseAddress.Region)
				{
					case AddressType.IO: ioRegionCount++; break;
					case AddressType.Memory: memoryRegionCount++; break;
					default: break;
				}
			}

			var ioPortRegions = new IIOPortRegion[ioRegionCount];
			var memoryRegions = new IMemoryRegion[memoryRegionCount];

			int ioRegionIndex = 0;
			int memoryRegionIndex = 0;

			foreach (var pciBaseAddress in pciDevice.BaseAddresses)
			{
				switch (pciBaseAddress.Region)
				{
					case AddressType.IO: ioPortRegions[ioRegionIndex++] = new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size); break;
					case AddressType.Memory: memoryRegions[memoryRegionIndex++] = new MemoryRegion(pciBaseAddress.Address, pciBaseAddress.Size); break;
					default: break;
				}
			}

			var hardwareResources = new HardwareResources(resourceManager, ioPortRegions, memoryRegions, new InterruptHandler(resourceManager.InterruptManager, pciDevice.IRQ, hardwareDevice), pciDevice as IDeviceResource);

			if (resourceManager.ClaimResources(hardwareResources))
			{
				hardwareResources.EnableIRQ();

				hardwareDevice.Setup(hardwareResources);

				if (hardwareDevice.Start() == DeviceDriverStartStatus.Started)
				{
					pciDevice.SetDeviceOnline();
				}
				else
				{
					hardwareResources.DisableIRQ();
					resourceManager.ReleaseResources(hardwareResources);
				}
			}
		}
	}
}
