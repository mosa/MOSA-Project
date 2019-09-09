====================
MOSA Project Changes
====================

.. current developments

v1.9.7.146
==========

* Initial efforts to incorporate a separate and simpler type system for the compiler
* Future work on a new instruction optimization and transformation system

v1.9.7.131
==========

* Fixed Linux compile bug (file name case issue)
* Fixes for x86 long stage (incomplete) - the long expansion stage hides the problem
* Instruction ID work differently (implementation detail)
* Experimental code for more dynamic instruction transformation + optimizations (WIP)

v1.9.7.121
==========

**Renamed**:

* ``LauncherOptions.EnableIRLongExpansion`` to ``EnableLongExpansion``
