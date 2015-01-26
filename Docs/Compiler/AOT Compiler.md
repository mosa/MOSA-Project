The MOSA Ahead-Of-Time (AOT) Compiler is used to create native code binaries and bootable kernel images from managed code. The compiler uses the facilities provided by the MOSA Compiler Framework. 

### General Usage

The compiler is a command line application, which provides a set of command line switches to control various operations. Use the following example command line to check the  availability and version of the compiler.

<pre>
mosacl
</pre>
If the compiler is available on your system, the following output should be visible (as of release 1.3)

<pre>
MOSA AOT Compiler, Version 1.4 'Neptune'
Copyright 2014 by the MOSA Project. Licensed under the New BSD License.
Copyright 2008 by Novell. NDesk.Options is released under the MIT/X11 license.

Usage: mosacl -o outputfile --arch=[x86] --format=[ELF32|ELF64|PE] {--boot=[mb0.7]} {additional options} inputfiles

Run 'mosacl --help' for more information.
</pre>

### Command Line Switches

The compiler command line switches are split into various categories described below. There may be additional switches available in your mosacl version. These are documented if you run mosacl with the -h switch.

### General

| Switch | Description |
|-----------|-----------|
| -a | Sets the compilation target architecture. Currently x86 is the only supported target architecture. |
| -h, -?, --help | Displays the full help of all compiler switches available. |
| -o | Sets the output file of the compiler. |
| -v | Displays the mosa compiler version |

### Optimizations

| Switch | Description |
|-----------|-----------|
| -ssa | Enabled transformation of IR code into [Static Single Assignment (SSA) form](http://en.wikipedia.org/wiki/Static_single_assignment_form) |
| -optimize | Enabled optimization; such as [strength reduction](http://en.wikipedia.org/wiki/Strength_reduction), [constant folding](http://en.wikipedia.org/wiki/Constant_folding), [copy propagation](http://en.wikipedia.org/wiki/Copy_propagation) and [dead code elimination](http://en.wikipedia.org/wiki/Dead_code_elimination) |
| -promote-temps | Enables temporary variables promotion optimization |
| -sa | Performs static allocations at compile time |

### Linker

| Switch | Description |
|-----------|-----------|
| -f, --format | Sets the format of the output file to produce. Currently this switch can take the values PE or ELF32. The first indicates a portable executable format binary, the second a 32-bit ELF binary. |
| --map | Emits a map file of the generated binary. The map file specifies the file and memory positions of functions, static data members and compiler generated members. |

### Booting

| Switch | Description |
|-----------|-----------|
| -b, --boot | Indicates that the generated binary is a bootable kernel image. A regular executable is generated without this switch. Currently, only multiboot specification 0.6.95 is supported. To use it, specify mb0.7. |

### Examples

The following examples show the usage of mosacl for certain tasks.

**Creating a native portable executable for a managed executable**

<pre>
mosacl -a=x86 -f=ELF32 -o name-native.exe name.exe
</pre>

**Generating a map file for a binary**

<pre>
mosacl -a=x86 -f=ELF32 --map=name-native.map -o name-native.exe name.exe
</pre>

**Creating a bootable kernel image using the portable executable format and a multiboot compliant bootloader**

<pre>
mosacl -a=x86 -f=ELF32 -b=mb0.7 -o kernel-image.exe kernel.exe
</pre>

