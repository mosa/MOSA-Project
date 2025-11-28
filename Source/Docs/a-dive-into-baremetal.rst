********************************
A dive into the BareMetal kernel
********************************

Introduction
============

MOSA's platform-agnostic, main kernel implementation is called **BareMetal**. It emerged after wanting to unify all
kernel implementations (which was really x86 and a bit of x64) into 1, and instead implement the platform-specific parts
via **plugs**.

At the end of this document, you'll hopefully understand better how the BareMetal kernel works internally. Let's get
started!

What is it?
===========

But first, it's wise to tell you exactly what the BareMetal kernel is. To put it simply, it's the main kernel
implementation for MOSA, and abstracts away the **platform-specific details** into separate sub-projects via **plugs**,
like some other projects already do. The primary advantage of doing this being able to keep a relatively clean code base
while being able to port the kernel (and other projects) to other platforms relatively easily.

With that being said, let's focus on the kernel startup process:

Startup
=======

The kernel's initialization process happens in the ``Mosa.Kernel.BareMetal.Startup`` class. It's split into 2 methods:
- ``Initialize()``: For setting up **the platform code used later**, as well as other critical code like the initial
allocator.
- ``EntryPoint()``: For setting up **everything else**, like the main allocator, the HAL, the device drivers, etc...

We'll break down each method individually to find out what actually happens. Let's start with ``Initialize()``:

Startup.Initialize()
====================

.. code-block:: csharp

    Platform.Interrupt.Disable();
    Platform.Setup(stackFrame);

First, we have to disable all interrupts because we don't have an interrupt handler set up (on x86, this is done using
the ``cli`` instruction). Then, we setup early platform-specific code (on x86, this initializes the Multiboot v2
structures). ``stackFrame`` is a ``Pointer`` to the current stack address, its primary use is to retrieve the Multiboot
address and magic number.

.. code-block:: csharp

    BootOptions.Setup();
    Debug.Setup();
    BootStatus.Initalize();

We then proceed to initialize the boot options, which includes both the static ones in ``BootSettings`` and the runtime
ones in the kernel command line. Next, if set in the kernel command line (by ``bootoptions=serialdebug``), we set a
field for later which indicates that serial-based debugging is enabled. It can give us lots of useful insight about
what's happening in real time (like allocations for example). Finally, we initialize static fields in the ``BootStatus``
class, which holds useful information at runtime, like if the GC is enabled for example.

.. code-block:: csharp

    InitialGCMemory.Initialize();

There are many allocators in the BareMetal kernel, each having their own specific purpose (we'll explain them in a bit).
This one temporarily allows the use of ``Mosa.Runtime.GC.AllocateMemory()`` method before setting up the true GC
allocator. To do this, it retrieves a platform-defined **memory pool**, which is nothing more than a memory region
(which is itself a memory address delimited by a size in bytes). It doesn't have to be *too* big, but it should be big
*enough* to accomodate for any potential objects created in between that time.

.. code-block:: csharp

    Platform.Initialize();

To conclude this part of the setup, we initialize some more platform-defined structures. On x86, these include the SSE
instruction set, the PIC, the RTC, and the serial controller if the field for enabling serial-based debugging is set.

Now that we've seen the ``Initialize()`` method, let's take a look at the much more interesting ``EntryPoint()`` method:

Startup.EntryPoint()
====================

.. code-block:: csharp

    BootPageAllocator.Setup();

The initial page allocator has a similar concept to the initial GC memory allocator seen above, except this one is used
for allocating platform-defined structures at startup (for example, on x86, this would be the GDT, IDT, etc...). Like
the latter, it takes in a specific **memory pool** defined by the platform kernel implementation.

.. code-block:: csharp

    BootMemoryMap.Setup();
    BootMemoryMap.Dump();

Would you be able to guess what the ``BootMemoryMap.Setup()`` method does? That's right! It sets up the kernel's memory
map. It imports the one defined by the bootloader via Multiboot v2, and imports the platform memory map, i.e. the same
memory pools defined for the ``InitialGCMemory`` allocator and the ``BootPageAllocator``. Then the ``Dump()`` method
simply outputs the memory map to standard output.

.. code-block:: csharp

    PageFrameAllocator.Setup();

This is the first "real" allocator in the kernel. It's a physical page allocator using a **bitmap** (not the image
format, but an actual bit map) to store information about individual pages. It sets the individual page size based on a
platform **page shift**, and defines each page using the now-initialized memory map from above.

.. code-block:: csharp

    PageTable.Setup();
    VirtualPageAllocator.Setup();

The ``PageTable`` class simply acts as a thin wrapper around the ``Platform.PageTable``, so it's not that interesting.
However, we can spot a new allocator here: the ``VirtualPageAllocator``. As its name would suggest, it allocates a
specific number of pages, and maps those to become virtual pages instead of physical pages.

.. code-block:: csharp

    InterruptManager.Setup();

Just like ``PageTable``, the ``InterruptManager`` class is a tiny wrapper around ``Platform.Interrupt``, as well as
``InterruptQueue``. The latter is pretty self-explanatory, it allows interrupts to be queued for execution, so to speak.

.. code-block:: csharp

    GCMemory.Setup();

Yet another allocator! This one is designed to replace the ``InitialGCMemory`` we talked about earlier. Internally, it
uses the ``VirtualPageAllocator``, except it allocates a specified size in **bytes** and keeps track of that and the
number of allocated pages in a separate heap, called the **GC heap**.

.. code-block:: csharp

    VirtualMemoryAllocator.Setup();

The ``VirtualMemoryAllocator`` is almost virtually (no pun intended) identical to the ``GCMemory`` allocator: it
allocates a specific number of pages given a size in bytes, and store that information in a heap. So, what's the
difference? Well, it's that the 2 heaps are different: where the ``GCMemory`` allocator stores the information in its
**GC heap**, the ``VirtualMemoryAllocator`` does so in its **memory heap**. This is an important distinction because
the GC heap is used to keep track of all **automatically** allocated objects so they can be freed when needed, whereas
the memory heap keeps track of all **manually** allocated objects, usually by the kernel or by the end user, so they
can be freed whenever desired.

.. code-block:: csharp

    Scheduler.Setup();

We're **finally** done with allocators. Here, we set up the ``Scheduler``, which allows the kernel to schedule tasks
(or **threads**) on the fly. However, since MOSA can currently only use up to 1 CPU core, the scheduler isn't very
useful.

.. code-block:: csharp

    var hardware = new HardwareAbstractionLayer();
    var deviceService = new DeviceService();

    HAL.Set(hardware);
    HAL.SetInterruptHandler(deviceService.ProcessInterrupt);

This part of the code initializes the HAL (Hardware Abstraction Layer). This is essentially the "man in the middle" of
the OS: it allows parts of the entire OS to intercommunicate. We also initialize our first **service**, and we set our
global interrupt handler to the device service's. We'll get into what services are, and particularly what the
``DeviceService`` is.

.. code-block:: csharp

    deviceService.RegisterDeviceDriver(Setup.GetDeviceDriverRegistryEntries());

As you may have guessed, the ``DeviceService`` is the base service for initializing all **devices** in the system (along
with their corresponding **device drivers**). This line of code does exactly that: it registers all device drivers from
the MOSA device driver framework into the ``DeviceService`` in order to start them.

An interesting point to make is this ``Setup.GetDeviceDriverRegistryEntries()`` method. This method returns a list
of device driver registry entries, which means that you could very well add/remove drivers to/from that list in order to
initialize only certain drivers, or more drivers, if you want.

.. code-block:: csharp

    var serviceManager = new ServiceManager();
    var diskDeviceService = new DiskDeviceService();
    var partitionService = new PartitionService();
    var isaDeviceService = new ISADeviceService();
    var pciDeviceService = new PCIDeviceService();
    var pcService = new PCService();

    serviceManager.AddService(deviceService);
    serviceManager.AddService(diskDeviceService);
    serviceManager.AddService(partitionService);
    serviceManager.AddService(isaDeviceService);
    serviceManager.AddService(pciDeviceService);
    serviceManager.AddService(pcService);

The previous code is then followed by the service initialization code. We create a new ``ServiceManager``, alongside a
bunch of other **services**, and we add them all into the ``ServiceManager``. So, what exactly are services?

In short, services are **background tasks** that fulfill a **specific purpose**, and can be **queried at any time**. A
good example of this is the ``PCService``, or even the aforementioned ``DeviceService``. Indeed, the ``PCService``
fulfills the purpose of handling power management, like shutting down or rebooting the system. This service must be
queryable at any point in time, whenever the user wishes to shut down or reboot their PC. Similarly (but more so
to the end user), the ``DeviceService`` allows querying any initialized device driver in the system. This is
particularly useful if you want to, say, get all ``IGraphicsDevice`` devices in the system, or even get a very specific device
like a ``StandardKeyboard``.

Either way, here, we initialize a total of **5 new services**, all of which most likely need no introduction now. But,
in doubt, here's a short summary of what all the services do:

- ``DeviceService``: Starts and handles all devices in the system (if any), including any **generic** devices.
- ``DiskDeviceService``: Manages all disks in the system.
- ``PartitionService``: Manages all partitions inside a disk.
- ``ISADeviceService``: Starts and handles all ISA devices in the system (if any).
- ``PCIDeviceService``: Starts and handles all PCI devices in the system (if any).
- ``PCService``: Handles power management in the system.

Wait, what's that? **Generic** devices? What are those? To put it simply, they're devices that don't have a specific
**bus** attached to them. A **bus** is typically ISA, PCI, USB, etc... but some devices (or standards) simply don't
have any actual bus connected to them (e.g. **ACPI**). For this, the term "generic devices" was coined to handle all
these devices in the system.

.. code-block:: csharp

    partitionService.CreatePartitionDevices();

    foreach (var partition in deviceService.GetDevices<IPartitionDevice>())
		FileManager.Register(new FatFileSystem(partition.DeviceDriver as IPartitionDevice));

We haven't finished talking about services, though. This part of the code here first **initializes** all partitions
in **all disks**, then iterates over all the partitions to register them as FAT file systems, if they contain one.
Indeed, the ``FileManager.Register()`` method will not actually register the file system if it doesn't contain a valid
FAT (File Allocation Table).

.. code-block:: csharp

    var stdKeyboard = deviceService.GetFirstDevice<StandardKeyboard>().DeviceDriver as IKeyboardDevice;
    if (stdKeyboard == null)
    {
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(" [FAIL]");
		Console.WriteLine("No keyboard detected!");

		for (;;)
			HAL.Yield();
    }

    Kernel.Keyboard = new Keyboard(stdKeyboard, new US());

We're almost done. An essential device to initialize is the **keyboard**, which is what we'll do here. Note that, if no
keyboard is detected, the system will **halt**. This is because, currently, MOSA is practically useless for anything
other than for user interaction (it doesn't have any network stack, for example). This restriction is bound to
be removed in the future however.

.. code-block:: csharp

    InterruptManager.SetHandler(ProcessInterrupt);
    Platform.Interrupt.Enable();

Finally, we arrive at the last bit of initialization code. And fortunately for us, it's not very complicated: it sets
the CPU's interrupt handler to one defined in the same ``Startup`` class (whose sole purpose is to redirect the
interrupts to the **HAL** if they're coming from a device), and enable interrupts (on x86 for example, this would
execute the ``sti`` instruction).

**Note**: A limitation of this ``ProcessInterrupt()`` method is it's **x86 specific**. This is because it checks if the
interrupt is within a specific range of interrupts, which is specific to x86. With the ever growing support for ARM in
MOSA, this limitation eventually ought to be surpassed, but for now, it is what it is.

Any questions?
==============

And we're done! We hope you now understand how the **BareMetal** kernel works better. If you have any questions
regarding the content of this document, or even any other question, don't hesitate to join our
`Discord <https://discord.gg/tRNMn3npsv>`__ server! We'll happily answer your questions :D
