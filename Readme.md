[![Build status][build-status-image]][build-status]   [![License][github-license]][github-license-link]  [![Issues][github-issues]][github-issues-link]  [![Stars][github-stars]][github-stars-link]  [![Forks][github-forks]][github-forks-link]  [![Gitter Chat][gitter-image]][gitter-chat]

[![Issue Stats][pull-requests-image]][pull-requests]  [![Issue Stats][issues-closed-image]][issues-closed]

[![Bountysource][bounty-image]][bounty-issues]  

[build-status-image]: https://ci.appveyor.com/api/projects/status/gmeosk7sa6su8rb3/branch/master?svg=true
[build-status]: https://ci.appveyor.com/project/MOSA/mosa-project
[pull-requests-image]: http://www.issuestats.com/github/mosa/mosa-project/badge/pr
[pull-requests]: http://www.issuestats.com/github/mosa/mosa-project
[issues-closed-image]: http://www.issuestats.com/github/mosa/mosa-project/badge/issue
[issues-closed]: http://www.issuestats.com/github/mosa/mosa-project
[bounty-image]: https://api.bountysource.com/badge/team?team_id=55027&style=bounties_received
[bounty-issues]: https://www.bountysource.com/teams/mosa/issues?utm_source=MOSA%20Project&utm_medium=shield&utm_campaign=bounties_received
[gitter-image]: https://img.shields.io/badge/gitter-join%20chat%20-blue.svg
[gitter2-image]: https://badges.gitter.im/Join%20Chat.svg
[gitter-chat]: https://gitter.im/mosa/MOSA-Project
[github-issues]: https://img.shields.io/github/issues/mosa/MOSA-Project.svg
[github-forks]: https://img.shields.io/github/forks/mosa/MOSA-Project.svg
[github-stars]: https://img.shields.io/github/stars/mosa/MOSA-Project.svg
[github-license]: https://img.shields.io/badge/license-New%20BSD-blue.svg
[github-link]: https://github.com/mosa/MOSA-Project
[github-stars-link]: https://github.com/mosa/MOSA-Project/stargazers
[github-forks-link]: https://github.com/mosa/MOSA-Project/network
[github-issues-link]: https://github.com/mosa/MOSA-Project/issues
[github-license-link]: https://raw.githubusercontent.com/mosa/MOSA-Project/master/LICENSE.txt

MOSA is an open source software project aiming to create a high quality, cross-platform, optimizing .NET compiler designed specifically to support a managed operating system based on the .NET framework.

The MOSA project consists of:

* Compiler - a high quality, multithreaded, cross-platform, optimizing .NET compiler.
* Kernel - a small, micro-kernel operating system.
* Device Drivers Framework - a modular, device drivers framework and device drivers.

Read our [Frequently Asked Questions](https://github.com/mosa/MOSA-Project/wiki/Frequently-Asked-Questions) for more information about this project.

### Getting Started

**Download**

The MOSA project is available as a [zip download](https://github.com/mosa/MOSA-Project/archive/master.zip) or via git:

<pre>
git clone https://github.com/mosa/MOSA-Project.git
</pre>

### Prerequisites

You will also need the following prerequisites:

**All Platforms**

[EditorConfig](http://editorconfig.org/) plugin for your IDE.

**Windows**

Install any [Visual Studio](http://www.visualstudio.com) version 2015 or newer.

All editions are supported including the fully-featured free [Community Edition](https://www.visualstudio.com/products/visual-studio-community-vs).

**Linux**

Install [Mono](http://www.mono-project.com) and [Qemu](http://wiki.qemu.org/Main_Page).

<pre>
sudo apt-get -y install mono-devel qemu
</pre>

**Mac**

Install [Mono](http://www.mono-project.com) and [Qemu](http://wiki.qemu.org/Main_Page).

### Running

**Windows**

Double click on the "Compile.bat" script in the root directory to compile all the tools, sample kernels, and demos.

Next double click on the "Launcher.bat" script, which will bring up the MOSA Launcher tool (screenshot below) that can:

* Compile the operating system
* Create a virtual disk image, with the compiled binary and boot loader
* Launch a virtual machine instance (QEMU by default)

By default, the CoolWorld operating system demo is pre-selected. Click the "Compile and Run" button to compile and launch the demo.

![MOSA Launcher](https://raw.githubusercontent.com/mosa/MOSA-Screenshots/master/MOSA%20Launcher.png)

![MOSA CoolWorld Demo](https://raw.githubusercontent.com/mosa/MOSA-Screenshots/master/MOSA%20QEMU%20CoolWorld.png)

### Current Status

The MOSA compiler supports:

* almost all non-object oriented code (arithmetic, assignment, bitwise logic, bitwise shifts, boolean logic, conditional evaluation, equality testing, calling functions, increment and decrement,  member selection, object size, order relations, reference and dereference, sequencing, and subexpression grouping), 
* basic object oriented code (such as new operator, member methods and virtual methods), 
* basic type conversion (implicit type and explicit type conversion on primitives types and "is" and "as" operators), 
* generic code (example, List<T>), and
* delegates (static and non-static) and with optional parameters.
* exception handling (try, finally, and catch code blocks)

### Join the Discussion

Join us on [Gitter chat][gitter-chat]. This is the most interactive way to connect to MOSA's development team.


### License

MOSA is licensed under the [New BSD License](http://en.wikipedia.org/wiki/New_BSD).

