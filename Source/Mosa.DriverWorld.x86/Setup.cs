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
using System.Collections.Generic;
using Mosa.DriverWorld.x86.HAL;

namespace Mosa.DriverWorld.x86
{
	/// <summary>
	/// Setup for the Device Driver System.
	/// </summary>
	public static class Setup
	{
		static private DeviceDriverRegistry deviceDriverRegistry;
		static private IDeviceManager deviceManager;
		static private IResourceManager resourceManager;
		static private PCIControllerManager pciControllerManager;

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

		/// <summary>
		/// Gets the PCI Controller Manager
		/// </summary>
		static public PCIControllerManager PCIControllerManager { get { return pciControllerManager; } }

		static public StandardKeyboard Keyboard = null;
		static public CMOS CMOS = null;
		static public PIC PIC = null;

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

			// Create the PCI Controller Manager
			pciControllerManager = new PCIControllerManager(deviceManager);

			// Setup hardware abstraction interface
			IHardwareAbstraction hardwareAbstraction = new HardwareAbstraction();

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
			Mosa.Kernel.x86.Screen.RawWrite(4, 1, 'A', 0x0A);
			// Find all drivers
			deviceDriverRegistry.RegisterBuiltInDeviceDrivers();
			Mosa.Kernel.x86.Screen.RawWrite(4, 2, 'A', 0x0A);
			// Start drivers for ISA devices
			StartISADevices();
			Mosa.Kernel.x86.Screen.RawWrite(4, 3, 'A', 0x0A);
			// Start drivers for PCI devices
			StartPCIDevices();
			Mosa.Kernel.x86.Screen.RawWrite(4, 4, 'A', 0x0A);
			// Get CMOS, StandardKeyboard, and PIC driver instances
			CMOS = (CMOS)deviceManager.GetDevices(new FindDevice.WithName("CMOS")).First.Value;
			PIC = (PIC)deviceManager.GetDevices(new FindDevice.WithName("PIC_0x20")).First.Value;
			Keyboard = (StandardKeyboard)deviceManager.GetDevices(new FindDevice.WithName("StandardKeyboard")).First.Value;
			Mosa.Kernel.x86.Screen.RawWrite(4, 5, 'A', 0x0A);
			// Enable Interrupts
			for (byte i = 0; i < byte.MaxValue; i++)
				Setup.PIC.EnableIRQ(i);
			Mosa.Kernel.x86.Screen.RawWrite(4, 0, '6', 0x0A);
		}

		/// <summary>
		/// Starts the PCI devices.
		/// </summary>
		static public void StartPCIDevices()
		{
			PCIControllerManager.CreatePCIDevices();

			var devices = deviceManager.GetDevices(new FindDevice.IsPCIDevice(), new FindDevice.IsAvailable());

			foreach (IDevice device in devices)
			{
				var pciDevice = device as IPCIDevice;

				StartDevice(pciDevice);
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
				pciDevice.SetNoDriverFound();
				return;
			}

			var hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType);

			StartDevice(pciDevice, deviceDriver, hardwareDevice as IHardwareDevice);
		}

		private static void StartDevice(IPCIDevice pciDevice, Mosa.DeviceSystem.DeviceDriver deviceDriver, IHardwareDevice hardwareDevice)
		{
			var ioPortRegions = new LinkedList<IIOPortRegion>();
			var memoryRegions = new LinkedList<IMemoryRegion>();

			foreach (var pciBaseAddress in pciDevice.BaseAddresses)
			{
				switch (pciBaseAddress.Region)
				{
					case AddressType.IO: ioPortRegions.AddLast(new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size)); break;
					case AddressType.Memory: memoryRegions.AddLast(new MemoryRegion(pciBaseAddress.Address, pciBaseAddress.Size)); break;
					default: break;
				}
			}

			foreach (var memoryAttribute in deviceDriver.MemoryAttributes)
			{
				if (memoryAttribute.MemorySize > 0)
				{
					var memory = Mosa.DeviceSystem.HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
					memoryRegions.AddLast(new MemoryRegion(memory.Address, memory.Size));
				}
			}

			var hardwareResources = new HardwareResources(resourceManager, ioPortRegions.ToArray(), memoryRegions.ToArray(), new InterruptHandler(resourceManager.InterruptManager, pciDevice.IRQ, hardwareDevice), pciDevice as IDeviceResource);

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

		/// <summary>
		/// Starts the ISA devices.
		/// </summary>
		static public void StartISADevices()
		{
			var deviceDrivers = deviceDriverRegistry.GetISADeviceDrivers();

			foreach (var deviceDriver in deviceDrivers)
				StartDevice(deviceDriver);
		}

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="deviceDriver">The device driver.</param>
		static public void StartDevice(Mosa.DeviceSystem.DeviceDriver deviceDriver)
		{
			var driverAtttribute = deviceDriver.Attribute as ISADeviceDriverAttribute;

			if (driverAtttribute.AutoLoad)
			{
				var hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;

				var ioPortRegions = new LinkedList<IIOPortRegion>();
				var memoryRegions = new LinkedList<IMemoryRegion>();

				ioPortRegions.AddLast(new IOPortRegion(driverAtttribute.BasePort, driverAtttribute.PortRange));

				if (driverAtttribute.AltBasePort != 0x00)
					ioPortRegions.AddLast(new IOPortRegion(driverAtttribute.AltBasePort, driverAtttribute.AltPortRange));

				if (driverAtttribute.BaseAddress != 0x00)
					memoryRegions.AddLast(new MemoryRegion(driverAtttribute.BaseAddress, driverAtttribute.AddressRange));

				foreach (var memoryAttribute in deviceDriver.MemoryAttributes)
					if (memoryAttribute.MemorySize > 0)
					{
						IMemory memory = Mosa.DeviceSystem.HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
						memoryRegions.AddLast(new MemoryRegion(memory.Address, memory.Size));
					}

				var hardwareResources = new HardwareResources(resourceManager, ioPortRegions.ToArray(), memoryRegions.ToArray(), new InterruptHandler(resourceManager.InterruptManager, driverAtttribute.IRQ, hardwareDevice));

				hardwareDevice.Setup(hardwareResources);

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
}
