MOSA is an open source software project aiming to create a high quality, cross-platform, optimizing .NET compiler designed specifically to support a managed operating system based on the .NET framework.

The MOSA project consists of:

* Compiler - a high quality, cross-platform, optimizing .NET compiler.
* Kernel - a small, micro-kernel operating system.
* Device Drivers Framework - a modular, device drivers framework and device drivers.

Read our [Frequently Asked Questions](https://github.com/mosa/MOSA-Project/wiki/Frequently-Asked-Questions) for more about this project.

### Getting Started

**Download**

The MOSA project is available as a [zip download](https://github.com/mosa/MOSA-Project/archive/master.zip) or via git:

<pre>
git clone https://github.com/tgiphil/MOSA-Project.git
</pre>

Visit our [Getting Started](https://github.com/mosa/MOSA-Project/wiki/Getting-Started) page on how to compile and run your first operating system.

**Join the Discussion**

We have our own IRC chat channel #mosa on irc.freenode.org. The IRC channel can be access via this [browser-based client](http://webchat.freenode.net/?channels=mosa).

### Current Status

The MOSA compiler supports:

* almost all non-object oriented code (arithmetic, assignment, bitwise logic, bitwise shifts, boolean logic, conditional evaluation, equality testing, calling functions, increment and decrement,  member selection, object size, order relations, reference and dereference, sequencing, and subexpression grouping), 
* basic object oriented code (such as new operator, member methods and virtual methods), 
* basic type conversion (implicit type and explicit type conversion on primitives types and "is" and "as" operators), 
* generic code (example, List<T>), and
* delegates (static and non-static) and with optional parameters.

### License

MOSA is licensed under the [New BSD License](http://en.wikipedia.org/wiki/New_BSD).

