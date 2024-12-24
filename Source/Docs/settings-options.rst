################
Settings Options
################

Here are the setting options for the compiler tools:

Compiler Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Compiler.Platform,"Platform x86, x64, ARM32, ARM64"
    Compiler.BaseAddress,Base address of the compiled application
	Compiler.InitialStackAddress,Intial stack's starting address (note the stack grows in the downward direction).
    Compiler.TraceLevel,Trace level for debugging
    Compiler.MethodScanner,"If true, enable the experimental method scanner"
    Compiler.Multithreading,"If true, enables multithreading during compiling process"
    Compiler.Multithreading.MaxThreads,Maximum number of threads used by the compiler
    Compiler.Binary,"If true, emits object file, otherwise no object file is created"
    Compiler.OutputFile,Filename of the object file
    Compiler.SourceFiles,Filename(s) of the source files

Compiler Optimizations Settings
-------------------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Optimizations.Basic,"If true, enables prebuilt transformation optimizations, like constant folding and strength reduction"
    Optimizations.SSA,"If true, transforms instructions to Single Static Assignment (SSA) form"
    Optimizations.SCCP,"If true, enables Sparse Conditional Constant Propagation (SCCP) optimizations"
    Optimizations.ValueNumbering,"If true, enables the Value Numbering (VN) optimizations"
    Optimizations.LongExpansion,"If true, transforms some 64-bit instructions into 32-bit instructions prior to platform transformations"
    Optimizations.BitTracker,"If true, enables the bit tracker optimizations"
    Optimizations.LoopInvariantCodeMotion,"If true, enables the loop invariant code motion optimizations"
    Optimizations.Devirtualization,"If true, transforms virtual methods calls into static method calls"
    Optimizations.Platform,"If true, enable platform specific optimizations"
    Optimizations.Inline,"If true, small methods can be inlined"
    Optimizations.Inline.Maximum,Maximun number of instructions that can be inlined within a method
	Optimizations.ScanWindow,Maximun instruction window for the optimizer to scan
    Optimizations.Inline.AggressiveMaximum,Maximun number of instructions that can be inlined when a method is explicited marked to be inlined
    Optimizations.Inline.Explicit,"If true, explicitly marked methods are inlined"
    Optimizations.TwoPass,"If true, some optimization stages are executed twice"
    Optimizations.Inline.Aggressive,Methods to be aggressively inline
    Optimizations.Inline.Exclude,Methods that may not be inlined
	Optimizations.ReduceCodeSize,"If true, the compiler will favor smaller code size"

Linker Settings
---------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Linker.Format,Type of ELF object file elf32 or elf64
    Linker.Symbols,"If true, emits the symbols into the object  file"
    Linker.StaticRelocations,"If true, emits static relocation information into the object file"
    Linker.Dwarf,"If true, emits DWARF debug information into the object file"
    Linker.ShortSymbolNames,"If true, emits short symbol names into the object file"
    Linker.CustomSections.{Name}.SectionName,Emits a custom linker section with this section name
    Linker.CustomSections.{Name}.SourceFile,Emits a custom linker section using the specific file
    Linker.CustomSections.{Name}.Address,Emits a custom linker section with this address

Common Settings
---------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    SearchPaths,Folder to search for files
    DefaultFolder,Default folder for output files
    TemporaryFolder,Specifies a temporary folder

Compiler Debug Settings
-----------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    CompilerDebug.Statistics,"If true, enables statistics gathering"
    CompilerDebug.DebugFile,Filename to emit a MOSA specific debug information
    CompilerDebug.MapFile,Filename to emit a map of all symbols
    CompilerDebug.CompileTimeFile,Filename to emit compile times for each method
    CompilerDebug.AsmFile,Filename to emit ASM disassembly
    CompilerDebug.CountersFile,Filename to emit the global counters
	CompilerDebug.CountersFilter,Filters the global counters within the global counters file
    CompilerDebug.NasmFile,Filename to emit disassembly using the NASM tool
    CompilerDebug.InlinedFile,Filename to emit a list of all methods that were inlined
    CompilerDebug.PreLinkHashFile,Filename to emit a list of all methods with their hash value prior to linking
    CompilerDebug.PostLinkHashFile,Filename to emit a list of all methods with their hash value after linking
	CompilerDebug.FullCheckMode,"If true, causes the compiler to run internal validate checks (used to test the compiler)"

Compiler X86 Settings
---------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    X86.InterruptMethodName,Name of the method that handles interrupts

Explorer Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Explorer.Filter,Specifies the default method filter name when Explorer is launched
	Explorer.Start,"If true, immediately compile all methods upon launch"
	Explorer.DebugDiagnostic,"If true, debug diagnostics is captured"

Launcher Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Launcher.Start,"If true, immediately start the compiler upon launch"
    Launcher.Launch,"If true, launch a virtual machine after compiling the application and generating the virtual machine image"
    Launcher.Exit,"If true, exit immediately after launch"
    Launcher.PlugKorlib,"If true, automatically include the plugs for CoreLib"
    Launcher.GDB,"If true, launch the GNU GDB application after VM launch"
	Launcher.Serial,"If true, outputs the serial data"
    Launcher.Debugger,"If true, launch the MOSA debugger application after VM launch"
    Launcher.Test,"If true, monitors VM serial for success or failure messages"

Image Settings
--------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Image.Format,"Format of the vritual image file BIN, IMG, VHD, VDI, VMDK"
    Image.FileSystem,"File system of the primary partition in the image file FAT12, FAT16, FAT32"
    Image.Destination,Destination directory of the image file
    Image.ImageFile,Filename of the image file
	Image.FileSystem.RootInclude,Include files in specified directory
	Image.DiskBlocks,Number of blocks in image (0=automatic)
    Image.VolumeLabel,Volume Label

Emulator Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Emulator,"Type of Emulator QEMU, VMware, Bochs"
    Emulator.Memory,Amount of memory for the virtual machine in MB
    Emulator.Display,"If true, show the video display"
	Emulator.Graphics,"Graphics device: std, cirrus, vbe, virtio or vmware"
	Emulator.MaxRuntime,Maximum runtime of the virtual machine in seconds (future)
    Emulator.GDB,"If true, enables GDB within emulator"
    Emulator.Serial,"Serial Emulation type None, Pipe, TCPServer, TCPClient"
    Emulator.Serial.Host,Serial Host Name or IP address
    Emulator.Serial.Port,Serial Port
    Emulator.Serial.Pipe,Serial Pipename
    Emulator.Acceleration,"Enables CPU hardware acceleration on the emulator, if available"

GDB Settings
------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    GDB.Host,Host IP or Name for GDB
    GDB.Port,Port Number for GDB

Multiboot Settings
------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Multiboot.Version,"Multiboot version none, v2"
    Multiboot.Video,"If true, enable the framebuffer provided by Multiboot"
    Multiboot.Video.Width,Video Width
    Multiboot.Video.Height,Video Height

Debugger Settings
-----------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Debugger.WatchFile,Filename of the watch file
    Debugger.BreakpointFile,Filename of the breakpoint file

Application Location Settings
-----------------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    AppLocation.Bochs,Location of the BOCHS application
    AppLocation.Bochs.BIOS,Location of the BOCHS BIOS
    AppLocation.Bochs.VGABIOS,Location of the BOCHS VGA BIOS
    AppLocation.QemuX86,Location of the QEMU application for x86 (i386)
    AppLocation.QemuX64,Location of the QEMU application for x64
    AppLocation.QemuARM32,Location of the QEMU application for ARM32
    AppLocation.QemuARM64,Location of the QEMU application for ARM64
    AppLocation.QemuBIOS,Location of the QEMU BIOS
    AppLocation.QemuImg,Location of the QEMUImg application
	AppLocation.VmwareWorkstation, Location of the VMWorkstation application
    AppLocation.VmwarePlayer,Location of the VMPlayer application
    AppLocation.Ndisasm,Location of the Ndisasm application
    AppLocation.Mkisofs,Location of the Mkisofs application
    AppLocation.GDB,Location of the GDB application

OS Settings
-----------------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    OS.Name, Name of operating system
	OS.BootOptions, Specifies a boot string to pass to the operating system

Import Settings
---------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    Import,Filename of another settings file to import


Unit Test Settings
------------------

.. csv-table::
   :header: "Settings", "Description"
   :widths: 50, 200

    UnitTest.MaxErrors,Maximum number of errors before aborting the unit testing
	UnitTest.Connection.TimeOut,Maximum connection timeout in milliseconds before retrying
	UnitTest.Connection.MaxAttempts,Maximun number of restart attempts before aborting the unit tests
    UnitTest.Filter,Specifies the default method filter name when Explorer is launched
