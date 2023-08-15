********************************
Create your own operating system
********************************

Introduction
============

Now that you've learned to either launch a pre-existing demo project or make your own, we'll teach you the basics of the MOSA framework, and how to make your OS more interesting! Let's get started!

.. tip:: You don't need to follow this guide if you're using the BareMetal kernel (which the template is using). This guide is only for those who wish to create a more barebones kernel.

Theory
======

But first, a bit of theory (or how the MOSA framework works). The idea around MOSA is modularity, and it is its sort of key selling point. You can either use all of it, part of it, or close to none at all (at least the Mosa.Runtime.* projects are required for the architecture you want to compile for). It thus means that getting started is quite more difficult than one might think. But it's also not that difficult!

Let's take the example of device drivers. Typically, a device driver might want to talk to the rest of the system (for example, it might want to print to the standard output/console, write to an I/O port, etc...). To be able to do this, it uses the OS' **Hardware Abstraction Layer**. Essentially, it's an interface which lets parts of the OS communicate with each other.

The HAL needs to be implemented in some way. In MOSA, it is implemented by the end user, for modularity reasons. It inherits the ``BaseHardwareAbstraction`` class which provides a couple of methods commonly used by device drivers, and other code (it doesn't have to be used just by the device drivers!).

Now that we've seen how device drivers typically work, let's look more about how the MOSA framework specifically works. In a typical MOSA project, you'll see the following structure:

**Architecture-specific kernel initialization code**
- Here you would initialize architecture-specific stuff like the GDT or IDT on x86 for example.

**Initialize system services**
- The most useful one is the ``DeviceService`` which allows one to get either all devices via their respective device driver, or to get a specific device. But you also have important ones, like the PCI services for controlling PCI devices, and the ``PCService`` for shutting down and rebooting the system, amongst others.

**Initialize the HAL**
- While we might have created the class, we certainly haven't used it. This part would let the device drivers framework know about the HAL we've made.

**Initialize device drivers**
- This is pretty self-explanatory: it initializes all the device drivers which exist on the system. However, if we want to, we can omit certain device drivers, since the method to register all device drivers simply takes in a list of device driver entries.

**Initialize specific services**
- For example, you could initialize the ``PartitionService``, yet another service which allows you to work with all the partitions on the system.

**Your code!**
- At this point, we've finished initializing everything MOSA needs, and we're ready to go. We'll look at how we initialize all of this below.

All of this might seem a bit daunting at first, however, once you have all the initialization code in place, you don't have to touch it anymore! With that being said, let's look at how we initialize all of this:

Implementation
==============

.. tip:: This guide will assume you're compiling for the **x86** architecture.

**Architecture-specific kernel initialization code**

We need to initialize things like the GDT, IDT, etc... if we want to proceed any further. You can check out the x86 implementation of the BareMetal kernel to find out how these are implemented. Alternatively, if you wish to learn more about those, you can check the `OSDev Wiki <https://wiki.osdev.org>` which contains a ton of useful information about OS development, as its name would indicate.

**Initialize system services**

Let's first start by creating a ``ServiceManager``:

.. code-block:: csharp

	var serviceManager = new ServiceManager();

The service manager will take care of initializing all services for us. Next, we can create and add some service to it:

.. code-block:: csharp

	var deviceService = new DeviceService();
	var pciControllerService = new PCIControllerService();
	var pciDeviceService = new PCIDeviceService();

	serviceManager.AddService(deviceService);
	serviceManager.AddService(pciControllerService);
	serviceManager.AddService(pciDeviceService);

To see the complete list of services, check out the ``Mosa.DeviceSystem`` project, and more specifically, the files under the ``Service`` folder.

**Initialize the HAL**

A single line of code will initialize the HAL:

.. code-block:: csharp

	var hal = new Hardware();
	var processInterrupt = deviceService.ProcessInterrupt;

	Mosa.DeviceSystem.Setup.Initialize(hal, processInterrupt);

If you're wondering, the ``Hardware`` class is our implementation of the HAL. Unfortunately, it is a bit too in depth to cover here. Instead, you can find pre-existing implementations in the CoolWorld and SVGAWorld demos, each in the ``HAL`` folder. They don't differ much, so knowing one will suffice.

The ``deviceService.ProcessInterrupt`` method will process any interrupt for a specific device driver.

**Initialize device drivers**

To initialize device drivers, we first need to register them all, and then we can initialize them. You can do this like so:

.. code-block:: csharp

	var system = new X86System(); // This will depend on the architecture you wish to compile your OS for!
	var entries = Mosa.DeviceDriver.Setup.GetDeviceDriverRegistryEntries();

	deviceService.RegisterDeviceDriver(entries);
	deviceService.Initialize(system, null);

Here, ``Mosa.DeviceDriver.Setup.GetDeviceDriverRegistryEntries()`` is a method returning a List of all device drivers in MOSA. However, you can register your own list too, if you wish to do so! Check out the method's implementation for more details.

If you're asking yourself what the ``null`` is for here, it should be the ``parent`` argument. That is, the Device parent which will register the current device driver (because ``X86System`` is a device driver in MOSA!). However, because ``X86System`` here is actually the Device parent, we should set the parent argument to null (it doesn't have any!).

**Initialize specific services**

Finally, you can initialize specific services here. For example, you could call the ``CreatePartitionDevices`` method of ``PartitionService`` in order to register all partitions on the system. The reason we're doing this after initializing device drivers is because we need to initialize ATA (IDE), AHCI (SATA), or NVMe drivers for example.

**Your code!**

Well, we can't write the code for you, but you get it now. After all these steps, you can finally build the OS you've always wanted to create!

If you have any more questions, don't hesitate to ask on our `Discord <https://discord.gg/tRNMn3npsv>`__ server! We'll happily answer them all :D
