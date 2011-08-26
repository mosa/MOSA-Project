/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.ISA;

namespace Mosa.CoolWorld
{
	/// <summary>
	/// Setup for the Device Driver System.
	/// </summary>
	public static class Setup
	{
		static private IDeviceManager deviceManager;
		static private IResourceManager resourceManager;

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

		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize()
		{
			// Create Resource Manager
			resourceManager = new ResourceManager();

			// Create Device Manager
			deviceManager = new DeviceManager();

			// Setup hardware abstraction interface
			IHardwareAbstraction hardwareAbstraction = new Mosa.CoolWorld.HAL.HardwareAbstraction();

			// Set device driver system to the emulator port and memory methods
			Mosa.DeviceSystem.HAL.SetHardwareAbstraction(hardwareAbstraction);

			// Set the interrupt handler
			Mosa.DeviceSystem.HAL.SetInterruptHandler(ResourceManager.InterruptManager.ProcessInterrupt);
		}

		/// <summary>
		/// Start the Device Driver System.
		/// </summary>
		static public void Start()
		{
			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x60, PortRange = 1, AltBasePort = 0x64, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute keyboardDeviceAttributes = new ISADeviceDriverAttribute();
			keyboardDeviceAttributes.AutoLoad = true;
			keyboardDeviceAttributes.BasePort = 0x60;
			keyboardDeviceAttributes.PortRange = 1;
			keyboardDeviceAttributes.AltBasePort = 0x64;
			keyboardDeviceAttributes.IRQ = 1;
			keyboardDeviceAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute pciAttributes = new ISADeviceDriverAttribute();
			pciAttributes.AutoLoad = true;
			pciAttributes.BasePort = 0x0CF8;
			pciAttributes.PortRange = 8;
			pciAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x20, PortRange = 2, AltBasePort = 0xA0, AltPortRange = 2, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute picAttributes = new ISADeviceDriverAttribute();
			picAttributes.AutoLoad = true;
			picAttributes.BasePort = 0x20;
			picAttributes.PortRange = 2;
			picAttributes.AltBasePort = 0xA0;
			picAttributes.AltPortRange = 2;
			picAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x40, PortRange = 4, IRQ = 0, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute pitAttributes = new ISADeviceDriverAttribute();
			pitAttributes.AutoLoad = true;
			pitAttributes.BasePort = 0x40;
			pitAttributes.PortRange = 4;
			pitAttributes.IRQ = 0;
			pitAttributes.Platforms = PlatformArchitecture.X86AndX64;

			Keyboard = new StandardKeyboard();
			PCI = new PCIController();
			PIC = new PIC();
			PIT = new PIT();

			StartDevice(picAttributes, PIC);
			StartDevice(pitAttributes, PIT);
			StartDevice(pciAttributes, PCI);
			StartDevice(keyboardDeviceAttributes, Keyboard);
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

			IIOPortRegion[] ioPortRegions = new IIOPortRegion[ioRegionCount];
			IMemoryRegion[] memoryRegions = new IMemoryRegion[memoryRegionCount];

			ioPortRegions[0] = new IOPortRegion(driverAtttribute.BasePort, driverAtttribute.PortRange);

			if (driverAtttribute.AltBasePort != 0x00)
			{
				ioPortRegions[1] = new IOPortRegion(driverAtttribute.AltBasePort, driverAtttribute.AltPortRange);
			}

			if (driverAtttribute.BaseAddress != 0x00)
			{
				memoryRegions[0] = new MemoryRegion(driverAtttribute.BaseAddress, driverAtttribute.AddressRange);
			}

			IHardwareResources hardwareResources = new HardwareResources(resourceManager, ioPortRegions, memoryRegions, new InterruptHandler(resourceManager.InterruptManager, driverAtttribute.IRQ, hardwareDevice));

			hardwareDevice.Setup(hardwareResources);

			Mosa.Kernel.x86.Screen.NextLine();
			Mosa.CoolWorld.Boot.BulletPoint();
			Console.Write("Adding device ");
			Boot.InBrackets(hardwareDevice.Name, Mosa.Kernel.Colors.White, Mosa.Kernel.Colors.LightGreen);
			Console.WriteLine();

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
	}
}
