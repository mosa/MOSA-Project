################
Settings options
################

Here are all the settings options available:

Application Location Settings
-----------------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    AppLocation.Bochs,Location of the Bochs application
    AppLocation.Bochs.BIOS,Location of the Bochs BIOS
    AppLocation.Bochs.VGABIOS,Location of the Bochs VGA BIOS
    AppLocation.GDB,Location of the GDB application
    AppLocation.Mkisofs,Location of the mkisofs application
    AppLocation.Ndisasm,Location of the ndisasm application
    AppLocation.QemuX86,Location of the QEMU application for 32-bit x86 (qemu-system-i386)
    AppLocation.QemuX64,Location of the QEMU application for 64-bit x86 (qemu-system-x86\_64)
    AppLocation.QemuARM32,Location of the QEMU application for ARM32
    AppLocation.QemuARM64,Location of the QEMU application for ARM64
    AppLocation.QemuBIOS,Location of the QEMU BIOS
    AppLocation.QemuImg,Location of the QEMU image application (qemu-img)
    AppLocation.VmwarePlayer,Location of the VMware Workstation Player application
	AppLocation.VmwareWorkstation, Location of the VMware Workstation Pro application
	AppLocation.Graphviz, Location of the graphviz application
	AppLocation.QemuEDK2X86, Location of the QEMU 32-bit x86 UEFI firmware
	AppLocation.QemuEDK2X64, Location of the QEMU 64-bit x86 UEFI firmware
	AppLocation.QemuEDK2ARM32, Location of the QEMU ARM32 UEFI firmware
	AppLocation.QemuEDK2ARM64, Location of the QEMU ARM64 UEFI firmware
	AppLocation.VirtualBox, Location of the VirtualBox application

Compiler Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Compiler.BaseAddress,Base address of the compiled application
	Compiler.InitialStackAddress,Initial stack's starting address (note: the stack grows downwards)
    Compiler.Binary,"If true, emits object file, otherwise no object file is created"
    Compiler.MethodScanner,"If true, enable the experimental method scanner"
    Compiler.Multithreading,"If true, enables multithreading during compiling process"
    Compiler.Multithreading.MaxThreads,Maximum number of threads used by the compiler
    Compiler.OutputFile,Filename of the object file
    Compiler.Platform,"Target platform: x86, x64, ARM32, ARM64"
    Compiler.SourceFiles,File name(s) of the source file(s)
    Compiler.TraceLevel,Trace level for debugging

Compiler Debug Settings
-----------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    CompilerDebug.AsmFile,File name to emit ASM disassembly
    CompilerDebug.CompileTimeFile,File name to emit compile times for each method
    CompilerDebug.DebugFile,File name to emit MOSA-specific debug information
	CompilerDebug.FullCheckMode,"If true, causes the compiler to run internal validate checks (used to test the compiler)"
    CompilerDebug.InlinedFile,File name to emit a list of all inlined methods
    CompilerDebug.MapFile,File name to emit a map of all symbols
    CompilerDebug.CounterFile,File name to emit the global counters
    CompilerDebug.NasmFile,File name to emit disassembly using the NASM tool
    CompilerDebug.PostLinkHashFile,File name to emit a list of all methods with their hash value after linking
    CompilerDebug.PreLinkHashFile,File name to emit a list of all methods with their hash value prior to linking
    CompilerDebug.Statistics,"If true, enables statistics gathering"
	CompilerDebug.CounterFilter,Filters the global counters within the global counters file

Debugger Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Debugger.BreakpointFile,File name of the breakpoint file (not implemented)
    Debugger.WatchFile,File name of the watch file (not implemented)

Emulator Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Emulator,"Type of emulator: qemu, vmware, bochs"
    Emulator.Cores,Amount of cores to allocate for the virtual machine
    Emulator.Display,"If true, shows the video display"
    Emulator.GDB,"If true, enables GDB integration within the emulator"
	Emulator.MaxRuntime,Maximum run time of the virtual machine in seconds
    Emulator.Memory,Amount of memory to allocate for the virtual machine in MB
    Emulator.Serial,"Serial emulation type: none, pipe, tcpserver, tcpclient"
    Emulator.Serial.Host,Serial host name or IP address
    Emulator.Serial.Port,Serial port
    Emulator.Serial.Pipe,Serial pipe name
	Emulator.Graphics,"Graphics device: std, cirrus, vbe, virtio, vmware"
    Emulator.Acceleration,"Enables CPU hardware acceleration on the emulator, if available"

Explorer Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

	Explorer.DebugDiagnostic,"If true, debug diagnostics are captured"
    Explorer.Filter,Specifies the default method filter name when Explorer is launched
	Explorer.Start,"If true, immediately compiles all methods upon launch"

GDB Settings
------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    GDB.Host,Host IP or name
    GDB.Port,Port number

Image Settings
--------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Image.FileSystem,"File system of the primary partition in the image file: fat12, fat16, fat32"
    Image.FileSystem.RootInclude,Include files from the specified directory into the final image
    Image.Folder,Destination directory of the image file
    Image.Format,"Format of the virtual image file: bin, img, vhd, vdi, vmdk"
    Image.ImageFile,File name of the image file
    Image.Firmware,"Firmware to build the target image for: bios, uefi"
    Image.DiskBlocks,Number of blocks in the image, or 0 for automatically determining it (not implemented)
    Image.VolumeLabel,Volume label of the whole image (not implemented)

Import Settings
---------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Import,File name of a settings file to import (not implemented)

Launcher Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Launcher.Debugger,"If true, launches the MOSA debugger application after VM launch"
    Launcher.Exit,"If true, immediately exits after launch"
    Launcher.GDB,"If true, launches the GDB application after VM launch"
    Launcher.Launch,"If true, launches a virtual machine after compiling the application and generating the virtual machine image"
    Launcher.PlugKernel,"If true, automatically includes the plugs for the BareMetal kernel"
    Launcher.PlugKorlib,"If true, automatically includes the plugs for the core library"
	Launcher.Serial,"If true, outputs the serial data"
    Launcher.Start,"If true, immediately starts the compiler upon launch"
    Launcher.Test,"If true, monitors VM serial for success or failure messages"

Linker Settings
---------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Linker.Dwarf,"If true, emits DWARF debug information into the object file"
    Linker.Format,"Type of ELF object file: elf32, elf64"
    Linker.ShortSymbolNames,"If true, emits short symbol names into the object file"
    Linker.StaticRelocations,"If true, emits static relocation information into the object file"
    Linker.Symbols,"If true, emits the symbols into the object file"
    Linker.CustomSections.{Name}.SectionName,Emits a custom linker section with this section name
    Linker.CustomSections.{Name}.SourceFile,Emits a custom linker section using the specific file
    Linker.CustomSections.{Name}.Address,Emits a custom linker section with this address

Multiboot Settings
------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Multiboot.Version,"Multiboot version: none, v2"
    Multiboot.Video,"If true, enables the framebuffer provided by Multiboot"
    Multiboot.Video.Width,Framebuffer width
    Multiboot.Video.Height,Framebuffer height

Compiler Optimizations Settings
-------------------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Optimizations.Basic,"If true, enables prebuilt transformation optimizations, like constant folding and strength reduction"
	Optimizations.ScanWindow,Maximum instruction window for the optimizer to scan
    Optimizations.BitTracker,"If true, enables the bit tracker optimizations"
    Optimizations.LoopRangeTracker,"If true, sets a minimum and maximum value for a loop's counter according to its range"
    Optimizations.Devirtualization,"If true, transforms virtual methods calls into static method calls"
    Optimizations.Inline,"If true, small methods can be inlined"
    Optimizations.Inline.Aggressive,Methods to be aggressively inlined
    Optimizations.Inline.AggressiveMaximum,Maximum number of instructions that can be inlined when a method is explicitly marked to be inlined
    Optimizations.Inline.Exclude,Methods that may not be inlined
    Optimizations.Inline.Explicit,"If true, explicitly marked methods are inlined"
    Optimizations.Inline.Maximum,Maximum number of instructions that can be inlined within a method
    Optimizations.LongExpansion,"If true, transforms some 64-bit instructions into 32-bit instructions prior to platform transformations"
	Optimizations.ReduceCodeSize,"If true, the compiler will favor smaller code size"
    Optimizations.LoopInvariantCodeMotion,"If true, enables the loop invariant code motion optimizations"
    Optimizations.Platform,"If true, enable platform specific optimizations"
    Optimizations.SCCP,"If true, enables Sparse Conditional Constant Propagation (SCCP) optimizations"
    Optimizations.SSA,"If true, transforms instructions to Single Static Assignment (SSA) form"
    Optimizations.TwoPass,"If true, some optimization stages are executed twice"
    Optimizations.ValueNumbering,"If true, enables the Value Numbering (VN) optimizations"

OS Settings
-----------------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    OS.Name,Name of the operating system
	OS.BootOptions,Specifies a boot string to pass to the operating system

Bootloader Settings
-----------------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    BootLoader.Timeout,Specifies the bootloader timeout (in seconds) before the OS starts

Common Settings
---------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    DefaultFolder,Default folder for output files
    SearchPaths,Folder to search for files
    TemporaryFolder,Specifies a temporary folder

Unit Test Settings
------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

	UnitTest.Connection.MaxAttempts,Maximum number of restart attempts before aborting the unit tests
	UnitTest.Connection.TimeOut,Maximum connection timeout in milliseconds before retrying
    UnitTest.Filter,Specifies the default unit test filter name when the unit tests are launched
    UnitTest.MaxErrors,Maximum number of errors before aborting the unit testing

Compiler X86 Settings
---------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    X86.InterruptMethodName,Name of the method that handles interrupts
