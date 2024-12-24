######################
Command Line Arguments
######################

The command line arguments serve as shortcuts to the common set of :doc:`settings-options` used by the MOSA tools.

.. tip:: Specific settings may also be specified on the command line using the ``-setting`` argument (or using the shorthand version ``-s``).
	For example, to set the ``Compiler.OutputFile`` setting with ``Mosa.BareMetal.HelloWorld.x86.bin``, pass the following two arguments ``-setting Compiler.OutputFile=Mosa.BareMetal.HelloWorld.x86.bin`` on the command line.

Below are the command line arguments available:

.. note:: ``{value}`` is the next argument

.. csv-table::
   :header: "Argument","Setting","Value Set"
   :widths: 100, 100, 50

    -s,{name},{value}
    -setting,{name},{value}

    -settings,Settings,{value}

    Compiler:
    {none},Compiler.SourceFiles,{value}
    -o,Compiler.OutputFile,{value}

    -threading,Compiler.Multithreading,true
    -threading-off,Compiler.Multithreading,false
    -threads,Compiler.Multithreading.MaxThreads,{value}

    -base,Compiler.BaseAddress,{value}
    -scanner,Compiler.MethodScanner,true
    -no-code,Compiler.Binary,false
    -path,SearchPaths,{list}

    -inline,Optimizations.Inline,true
    -inline-off,Optimizations.Inline,false
    -ssa,Optimizations.SSA,true
    -ssa-off,Optimizations.SSA,false
    -sccp,Optimizations.SCCP,true
    -sccp-off,Optimizations.SCCP,false
    -basic-optimizations,Optimizations.Basic,true
    -basic-optimizations-off,Optimizations.Basic,false
    -inline-explicit,Optimizations.Inline.Explicit,true
    -inline-explicit-off,Optimizations.Inline.Explicit,false
    -long-expansion,Optimizations.LongExpansion,true
    -long-expansion-off,Optimizations.LongExpansion,false
    -two-pass,Optimizations.TwoPass,true
    -two-pass-off,Optimizations.TwoPass,true
    -value-numbering,Optimizations.ValueNumbering,true
    -value-numbering-off,Optimizations.ValueNumbering,false
    -loop-invariant-code-motion,Optimizations.LoopInvariantCodeMotion,true
    -loop-invariant-code-motion-off,Optimizations.LoopInvariantCodeMotion,false
    -platform-optimizations,Optimizations.Platform,true
    -platform-optimizations-off,Optimizations.Platform,false
    -bittracker,Optimizations.BitTracker,true
    -bittracker-off,Optimizations.BitTracker,false
    -looprange,Optimizations.LoopRangeTracker,true
    -looprange-off,Optimizations.LoopRangeTracker,false
    -devirtualization,Optimizations.Devirtualization,true
    -devirtualization-off,Optimizations.Devirtualization,false
    -inline-level,Optimizations.Inline.Maximum,{value}
	-reduce-size,Optimizations.ReduceCodeSize,true
	-scanwindow,Optimizations.ScanWindow,{value}

    Compiler - Platform:
    -platform,Compiler.Platform,{value}
    -x86,Compiler.Platform,x86
    -x64,Compiler.Platform,x64
    -arm32,Compiler.Platform,ARM32
    -arm64,Compiler.Platform,ARM64

    Compiler - Debug Output:
    -output-nasm,CompilerDebug.NasmFile,%DEFAULT%
    -output-asm,CompilerDebug.AsmFile,%DEFAULT%
    -output-map,CompilerDebug.MapFile,%DEFAULT%
    -output-counters,CompilerDebug.CountersFile,%DEFAULT%
    -output-time,CompilerDebug.CompilerTimeFile,%DEFAULT%
    -output-debug,CompilerDebug.DebugFile,%DEFAULT%
    -output-inlined,CompilerDebug.InlinedFile,%DEFAULT%
    -output-hash,CompilerDebug.PreLinkHashFile,%DEFAULT%
    -output-hash,CompilerDebug.PostLinkHashFile,%DEFAULT%
    -asm,CompilerDebug.AsmFile,%DEFAULT%
    -map,CompilerDebug.MapFile,%DEFAULT%
    -counters,CompilerDebug.CountersFile,{value}
    -counters-filter,CompilerDebug.CountersFilter,{value}

    Compiler - Debug:
    -inline-exclude,Optimizations.Inline.Exclude,{list}
    -test-filter,CompilerDebug.TestFilter,{value}
    -check,CompilerDebug.FullCheckMode,true

    -interrupt-method,X86.InterruptMethodName,{value}

    Linker:
    -emit-all-symbols,Linker.Symbols,true
    -emit-all-symbols-off,Linker.Symbols,false
    -emit-relocations,Linker.StaticRelocations,true
    -emit-relocations-off,Linker.StaticRelocations,false
    -emit-dwarf,Linker.Dwarf,true
    -emit-dwarf-off,Linker.Dwarf,false

    Explorer:
    -filter,Explorer.Filter,{value}
	-explorer-debug,Explorer.DebugDiagnostic,true
	-autostart,Explorer.Start,true

    Launcher:
    -autoexit,Launcher.Exit,true
    -autoexit-off,Launcher.Exit,false
    -autostart,Launcher.Start,true
    -autostart-off,Launcher.Start,false
    -autolaunch,Launcher.Launch,true
    -autolaunch-off,Launcher.Launch,false

    Launcher - Emulator:
    -emulator,Emulator,qemu|vmware|bochs|virtualbox
    -qemu,Emulator,qemu
    -vmware,Emulator,vmware
    -bochs,Emulator,bochs
    -virtualbox,Emulator,virtualbox

    -display,Emulator.Display,on
    -display-off,Emulator.Display,off
    -memory,Emulator.Memory,{value}
    -cores,Emulator.Cores,{value}
    -gdb,Emulator.GDB,true
    -vmware-svga,Emulator.Graphics,vmware
	-virtio-vga,Emulator.Graphics,virtio
	-acceleration,Emulator.Acceleration,true

    Launcher - Image:
    -image,Image.ImageFile,{value}
    -destination,Image.Folder,{value}

    -vhd,Image.Format,vhd
    -img,Image.Format,img
    -vdi,Image.Format,vdi
    -vmdk,Image.Format,vmdk

    -blocks,Image.DiskBlocks,{value}
    -volume-label,Image.VolumeLabel,{value}

    -include,Image.FileSystem.RootInclude,{value}

	-bios,Image.Firmware,bios
	-uefi,Image.Firmware,uefi
	-firmware,Image.Firmware,{value}

    -video,Multiboot.Video,true
    -video-width,Multiboot.Video.Width,{value}
    -video-height,Multiboot.Video.Height,{value}

    -osname,OS.Name,{value}
    -bootoptions,OS.BootOptions,{value}
    -bootloader-timeout,BootLoader.Timeout,{value}

    Launcher - Boot:
    -multiboot-v2,Multiboot.Version,v2
    -multiboot-none,Multiboot.Version,
    -multiboot,Multiboot.Version,{value}

    Launcher - Serial:
    -serial-connection,Emulator.Serial,{value}
    -serial-pipe,Emulator.Serial,pipe
    -serial-tcpclient,Emulator.Serial,tcpclient
    -serial-tcpserver,Emulator.Serial,tcpserver
    -serial-connection-port,Emulator.Serial.Port,{value}
    -serial-connection-host,Emulator.Serial.Host,{value}

    -gdb-port,GDB.Port,{value}
    -gdb-host,GDB.Host,{value}

    -launch-debugger,Launcher.GDB,true
    -launch-gdb,Launcher.Debugger,true

    -output-serial-connection,Launcher.Serial,{value}
    -output-serial-file,Launcher.Serial.File,{value}
    -debug,Launcher.Serial,true
    -debug,OS.BootOptions,bootoptions=serialdebug

    -timeout,Emulator.MaxRuntime,{value}

    Advanced:
    -plug-kernel,Launcher.PlugKernel,true
    -plug-korlib,Launcher.PlugKorlib,true
    -test,Launcher.Test,true
    -test,OS.BootOptions,bootoptions=serialdebug

    Debugger:
    -breakpoints,Debugger.BreakpointFile,{value}
    -watch,Debugger.WatchFile,{value}

	Unit Tests:
	-filter,UnitTest.Filter,{value}
	-maxerrors,UnitTest.MaxErrors,{value}

    Optimization Levels:
    -o0,Optimizations.Basic,false
    -o0,Optimizations.SSA,false
    -o0,Optimizations.ValueNumbering,false
    -o0,Optimizations.SCCP,false
    -o0,Optimizations.Devirtualization,false
    -o0,Optimizations.LongExpansion,false
    -o0,Optimizations.Platform,false
    -o0,Optimizations.Inline,false
    -o0,Optimizations.LoopInvariantCodeMotion,false
    -o0,Optimizations.BitTracker,false
    -o0,Optimizations.TwoPass,false
    -o0,Optimizations.Inline.Maximum,0
    -o0,Optimizations.ScanWindow,0
	-o0,Optimizations.ReduceCodeSize,false

    -o1,Optimizations.Basic,true
    -o1,Optimizations.SSA,false
    -o1,Optimizations.ValueNumbering,false
    -o1,Optimizations.SCCP,false
    -o1,Optimizations.Devirtualization,true
    -o1,Optimizations.LongExpansion,false
    -o1,Optimizations.Platform,false
    -o1,Optimizations.Inline,false
    -o1,Optimizations.LoopInvariantCodeMotion,false
    -o1,Optimizations.BitTracker,false
    -o1,Optimizations.TwoPass,false
    -o1,Optimizations.Inline.Maximum,0
    -o1,Optimizations.ScanWindow,30
	-o1,Optimizations.ReduceCodeSize,false

    -o2,Optimizations.Basic,true
    -o2,Optimizations.SSA,true
    -o2,Optimizations.ValueNumbering,true
    -o2,Optimizations.SCCP,false
    -o2,Optimizations.Devirtualization,true
    -o2,Optimizations.LongExpansion,false
    -o2,Optimizations.Platform,false
    -o2,Optimizations.Inline,false
    -o2,Optimizations.LoopInvariantCodeMotion,false
    -o2,Optimizations.BitTracker,false
    -o2,Optimizations.TwoPass,false
    -o2,Optimizations.Inline.Maximum,0
    -o2,Optimizations.ScanWindow,30
	-o2,Optimizations.ReduceCodeSize,false

    -o3,Optimizations.Basic,true
    -o3,Optimizations.SSA,true
    -o3,Optimizations.ValueNumbering,true
    -o3,Optimizations.SCCP,true
    -o3,Optimizations.Devirtualization,true
    -o3,Optimizations.LongExpansion,false
    -o3,Optimizations.Platform,false
    -o3,Optimizations.Inline,false
    -o3,Optimizations.LoopInvariantCodeMotion,false
    -o3,Optimizations.BitTracker,false
    -o3,Optimizations.TwoPass,false
    -o3,Optimizations.Inline.Maximum,0
    -o3,Optimizations.ScanWindow,30
	-o3,Optimizations.ReduceCodeSize,false

    -o4,Optimizations.Basic,true
    -o4,Optimizations.SSA,true
    -o4,Optimizations.ValueNumbering,true
    -o4,Optimizations.SCCP,true
    -o4,Optimizations.Devirtualization,true
    -o4,Optimizations.LongExpansion,true
    -o4,Optimizations.Platform,false
    -o4,Optimizations.Inline,false
    -o4,Optimizations.LoopInvariantCodeMotion,false
    -o4,Optimizations.BitTracker,false
    -o4,Optimizations.TwoPass,false
    -o4,Optimizations.Inline.Maximum,0
    -o4,Optimizations.ScanWindow,30
	-o4,Optimizations.ReduceCodeSize,false

    -o5,Optimizations.Basic,true
    -o5,Optimizations.SSA,true
    -o5,Optimizations.ValueNumbering,true
    -o5,Optimizations.SCCP,true
    -o5,Optimizations.Devirtualization,true
    -o5,Optimizations.LongExpansion,true
    -o5,Optimizations.Platform,true
    -o5,Optimizations.Inline,false
    -o5,Optimizations.LoopInvariantCodeMotion,false
    -o5,Optimizations.BitTracker,false
    -o5,Optimizations.TwoPass,false
    -o5,Optimizations.Inline.Maximum,0
    -o5,Optimizations.ScanWindow,30
	-o5,Optimizations.ReduceCodeSize,false

    -o6,Optimizations.Basic,true
    -o6,Optimizations.SSA,true
    -o6,Optimizations.ValueNumbering,true
    -o6,Optimizations.SCCP,true
    -o6,Optimizations.Devirtualization,true
    -o6,Optimizations.LongExpansion,true
    -o6,Optimizations.Platform,true
    -o6,Optimizations.Inline,true
    -o6,Optimizations.LoopInvariantCodeMotion,false
    -o6,Optimizations.BitTracker,false
    -o6,Optimizations.TwoPass,false
    -o6,Optimizations.Inline.Maximum,5
    -o6,Optimizations.ScanWindow,30
	-o6,Optimizations.ReduceCodeSize,false

    -o7,Optimizations.Basic,true
    -o7,Optimizations.SSA,true
    -o7,Optimizations.ValueNumbering,true
    -o7,Optimizations.SCCP,true
    -o7,Optimizations.Devirtualization,true
    -o7,Optimizations.LongExpansion,true
    -o7,Optimizations.Platform,true
    -o7,Optimizations.Inline,true
    -o7,Optimizations.LoopInvariantCodeMotion,true
    -o7,Optimizations.BitTracker,false
    -o7,Optimizations.TwoPass,false
    -o7,Optimizations.Inline.Maximum,10
    -o7,Optimizations.ScanWindow,30
	-o7,Optimizations.ReduceCodeSize,false

    -o8,Optimizations.Basic,true
    -o8,Optimizations.SSA,true
    -o8,Optimizations.ValueNumbering,true
    -o8,Optimizations.SCCP,true
    -o8,Optimizations.Devirtualization,true
    -o8,Optimizations.LongExpansion,true
    -o8,Optimizations.Platform,true
    -o8,Optimizations.Inline,true
    -o8,Optimizations.LoopInvariantCodeMotion,true
    -o8,Optimizations.BitTracker,true
    -o8,Optimizations.TwoPass,true
    -o8,Optimizations.Inline.Maximum,10
    -o8,Optimizations.ScanWindow,30
	-o8,Optimizations.ReduceCodeSize,false

    -o9,Optimizations.Basic,true
    -o9,Optimizations.SSA,true
    -o9,Optimizations.ValueNumbering,true
    -o9,Optimizations.SCCP,true
    -o9,Optimizations.Devirtualization,true
    -o9,Optimizations.LongExpansion,true
    -o9,Optimizations.Platform,true
    -o9,Optimizations.Inline,true
    -o9,Optimizations.LoopInvariantCodeMotion,true
    -o9,Optimizations.BitTracker,true
    -o9,Optimizations.TwoPass,true
    -o9,Optimizations.Inline.Maximum,15
    -o9,Optimizations.ScanWindow,50
	-o9,Optimizations.ReduceCodeSize,false

    -oNone,Optimizations.Basic,false
    -oNone,Optimizations.SSA,false
    -oNone,Optimizations.ValueNumbering,false
    -oNone,Optimizations.SCCP,false
    -oNone,Optimizations.Devirtualization,false
    -oNone,Optimizations.LongExpansion,false
    -oNone,Optimizations.Platform,false
    -oNone,Optimizations.Inline,false
    -oNone,Optimizations.LoopInvariantCodeMotion,false
    -oNone,Optimizations.BitTracker,false
    -oNone,Optimizations.TwoPass,false
    -oNone,Optimizations.Inline.Maximum,0
    -oNone,Optimizations.ScanWindow,1
	-oNone,Optimizations.ReduceCodeSize,false

    -oMax,Optimizations.Basic,true
    -oMax,Optimizations.SSA,true
    -oMax,Optimizations.ValueNumbering,true
    -oMax,Optimizations.SCCP,true
    -oMax,Optimizations.Devirtualization,true
    -oMax,Optimizations.LongExpansion,true
    -oMax,Optimizations.Platform,true
    -oMax,Optimizations.Inline,true
    -oMax,Optimizations.LoopInvariantCodeMotion,true
    -oMax,Optimizations.BitTracker,true
    -oMax,Optimizations.TwoPass,true
    -oMax,Optimizations.Inline.Maximum,15
    -oMax,Optimizations.ScanWindow,50
	-oMax,Optimizations.ReduceCodeSize,false

    -oSize,Optimizations.Basic,true
    -oSize,Optimizations.SSA,true
    -oSize,Optimizations.ValueNumbering,true
    -oSize,Optimizations.SCCP,true
    -oSize,Optimizations.Devirtualization,true
    -oSize,Optimizations.LongExpansion,true
    -oSize,Optimizations.Platform,true
    -oSize,Optimizations.Inline,true
    -oSize,Optimizations.LoopInvariantCodeMotion,true
    -oSize,Optimizations.BitTracker,true
    -oSize,Optimizations.TwoPass,true
    -oSize,Optimizations.Inline.Maximum,3
    -oSize,Optimizations.ScanWindow,50
	-oSize,Optimizations.ReduceCodeSize,true

    -oFast,Optimizations.Basic,true
    -oFast,Optimizations.SSA,true
    -oFast,Optimizations.ValueNumbering,true
    -oFast,Optimizations.SCCP,false
    -oFast,Optimizations.Devirtualization,true
    -oFast,Optimizations.LongExpansion,false
    -oFast,Optimizations.Platform,false
    -oFast,Optimizations.Inline,false
    -oFast,Optimizations.LoopInvariantCodeMotion,false
    -oFast,Optimizations.BitTracker,false
    -oFast,Optimizations.TwoPass,false
    -oFast,Optimizations.Inline.Maximum,0
    -oFast,Optimizations.ScanWindow,5
	-oFast,Optimizations.ReduceCodeSize,false
