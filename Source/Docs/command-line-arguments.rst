######################
Command Line Arguments
######################

The command line arguments serve as shortcuts to the common set of :doc:`settings-options` used by the MOSA tools.

.. tip:: Specific settings may also be specified on the command line using the ``-setting`` or ``-s`` arguments. For example to set the ``Compiler.OutputFile`` settings with ``Mosa.Demo.HelloWorld.x86.bin``, pass the following two arguments ``-setting Compiler.OutputFile=Mosa.Demo.HelloWorld.x86.bin`` on the command line.

Below are the command line arguments available:

.. csv-table:: 
   :header: "Argument","Setting","Value Set"
   :widths: 100, 100, 50
   
   Compiler:
    {none},Compiler.SourceFiles,{value}
    -settings,Settings,{value}
    -s,Settings,{value}
    -o,Compiler.OutputFile,{value}
    -threading,Compiler.Multithreading,true
    -threading-off,Compiler.Multithreading,false
	-threads,Compiler.Multithreading.MaxThreads,{value}
    -base,Compiler.BaseAddress,{value}
    -scanner,Compiler.MethodScanner,true
    -no-code,Compiler.Binary,false
    -path,SearchPaths, 
    -inline,Optimizations.Inline,true
    -inline-off,Optimizations.Inline,false
    -ssa,Optimizations.SSA,true
    -ssa-off,Optimizations.SSA,false
    -sccp,Optimizations.SCCP,true
    -sccp-off,Optimizations.SCCP,false
    -basic-optimizations,Optimizations.Basic,true
    -basic-optimizations-off,Optimizations.Basic,false
    -basic-optimization-window,Optimizations.Basic.Window,{value}
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
    -bit-tracker,Optimizations.BitTracker,true
    -bit-tracker-off,Optimizations.BitTracker,false
    -devirtualization,Optimizations.Devirtualization,true
    -devirtualization-off,Optimizations.Devirtualization,false
    -inline-level,Optimizations.Inline.Maximum,{value}
    
    -platform,Compiler.Platform,{value}
    -x86,Compiler.Platform,x86
    -x64,Compiler.Platform,x64
    -armv8a32,Compiler.Platform,armv8a32

    Compiler - Debug Output Information:
    -output-nasm,CompilerDebug.NasmFile,%DEFAULT%
    -output-asm,CompilerDebug.AsmFile,%DEFAULT%
    -output-map,CompilerDebug.MapFile,%DEFAULT%
    -output-time,CompilerDebug.CompilerTimeFile,%DEFAULT%
    -output-debug,CompilerDebug.DebugFile,%DEFAULT%
    -output-inlined,CompilerDebug.InlinedFile,%DEFAULT%
    -output-hash,CompilerDebug.PreLinkHashFile,%DEFAULT%
    -output-hash,CompilerDebug.PostLinkHashFile,%DEFAULT%
    -check,CompilerDebug.FullCheckMode,true

    Compiler - X86:
    -interrupt-method,X86.InterruptMethodName,{value}

    Linker:
    -emit-all-symbols,Linker.Symbols,true
    -emit-all-symbols-off,Linker.Symbols,false
    -emit-relocations,Linker.StaticRelocations,true
    -emit-relocations-off,Linker.StaticRelocations,false
    -emit-static-relocations,Linker.StaticRelocations,true
    -emit-Dwarf,Linker.Dwarf,true
    -emit-Dwarf-off,Linker.Dwarf,false
    -Dwarf,Linker.Dwarf,true

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
    -launch,Launcher.Launch,true
    -launch-off,Launcher.Launch,false

    Launcher - Emulator:
    -emulator,Emulator,qemu|vmware|bochs
    -qemu,Emulator,qemu
    -vmware,Emulator,vmware
    -bochs,Emulator,bochs
    -display,Emulator.Display,on
    -display-off,Emulator.Display,off
    -memory,Emulator.Memory
    -gdb,Emulator.GDB,true
	-timeout,Emulator.MaxRuntime,{value}
	-debug,Launcher.Serial,true
    
    Launcher - Emulator - Qemu & VMWare:
    -vmware-svga,Emulator.SVGA,vmware
	-virtio-vga,Emulator.SVGA,virtio

    Launcher - Image:
    -image,Image.ImageFile,{value}
    -destination,Image.Folder,{value}
    -dest,Image.Folder,{value}
    -vhd,Image.Format,vhd
    -img,Image.Format,img
    -vdi,Image.Format,vdi
    -vmdk,Image.Format,vmdk
    -blocks,Image.DiskBlocks,
    -volume-label,Image.VolumeLabel,
    -mbr,Image.MasterBootRecordFile,
    -boot,Image.BootBlockFile,
	-include,Image.FileSystem.RootInclude,{value}

    Launcher - Boot:
    -multiboot-v1,Multiboot.Version,v1
    -multiboot-v2,Multiboot.Version,v2
    -multiboot-none,Multiboot.Version,
    -multiboot,Multiboot.Version,{value}

    Launcher - Serial:
    -serial-connection,Emulator.Serial,
    -serial-pipe,Emulator.Serial,pipe
    -serial-tcpclient,Emulator.Serial,tcpclient
    -serial-tcpserver,Emulator.Serial,tcpserver
    -serial-connection-port,Emulator.Serial.Port,{value}
    -serial-connection-host,Emulator.Serial.Host,{value}

    Launcher - Video BIOS Extension (VBE):
    -video,Multiboot.Video,true
    -video-width,Multiboot.Video.Width,{value}
    -video-height,Multiboot.Video.Height,{value}
    -video-depth,Multiboot.Video.Depth,{value}

    Launcher - GDB:
    -launch-debugger,Launcher.GDB,true
	-launch-gdb,Launcher.Debugger,true

    Launcher & Debugger - GDB
    -gdb-port,GDB.Port,{value}
    -gdb-host,GDB.Host,{value}

    Launcher - Advance:
    -plug-korlib,Launcher.PlugKorlib,true

    Operating System:
    -osname,OS.Name,{value}

    Debugger:
    -breakpoints,Debugger.BreakpointFile,{value}
    -watch,Debugger.WatchFile,{value}

	Unit Testings:
	-unittest-maxerrors,UnitTest.MaxErrors,{value}

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
    -o0,Optimizations.Basic.Window,1

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
    -o1,Optimizations.Basic.Window,1

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
    -o2,Optimizations.Basic.Window,1

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
    -o3,Optimizations.Basic.Window,5

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
    -o4,Optimizations.Basic.Window,5

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
    -o5,Optimizations.Basic.Window,5

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
    -o6,Optimizations.Basic.Window,5

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
    -o7,Optimizations.Basic.Window,5

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
    -o8,Optimizations.Basic.Window,5

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
    -o9,Optimizations.Basic.Window,10

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
    -oNone,Optimizations.Basic.Window,1

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
    -oMax,Optimizations.Basic.Window,20

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
    -oSize,Optimizations.Basic.Window,10

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
    -oFast,Optimizations.Basic.Window,1

.. note:: ``{value}`` is the next argument
