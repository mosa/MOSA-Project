
#####
Demos
#####

MOSA includes three demos applications to demonstrate various capabilities of the MOSA project.

.. tip:: You can start any demo by launching its respective script in the Demos folder.

CoolWorld
=========

It's MOSA's flagship demo. It can boot into either console mode (containing a shell with a few basic commands) or
graphical mode (which makes use of the GPU and some other features offered by MOSA). You can boot into console mode by
inserting the ``-bootoptions coolworldui=consolemode`` command line option when starting the MOSA launcher.

Console mode:

.. image:: images/mosa-demo-coolworld-console.png

Graphical mode:

.. image:: images/mosa-demo-coolworld-graphical.png

TestWorld
=========

It's primarily used for testing and debugging, and thus doesn't serve any practical purpose other than finding bugs in
the MOSA compiler for example.

.. image:: images/mosa-demo-testworld.png

Starter
=======

It's used as a simple demo template project (just like the templates on NuGet), except it's directly bundled within the
MOSA solution.

.. image:: images/mosa-demo-starter.png
