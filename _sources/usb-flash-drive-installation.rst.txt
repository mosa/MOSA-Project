#################################
Installing onto a USB flash drive
#################################

While most of the development and testing of MOSA is done using virtualization software, MOSA does indeed boot on real hardware too.

There are several ways to put MOSA on a USB flash drive. Below are the most common ones, including the one we'll be following here:

- `Ventoy <https://ventoy.net/>`__ (Windows, Linux), allows reusing the same drive for multiple images and other content
- `Rufus <https://rufus.ie/>`__ (Windows only)
- `dd <https://en.wikipedia.org/wiki/Dd_(Unix)>`__ (Linux, `unofficial Windows version <http://www.chrysocome.net/dd>`__ exists)

We'll be following the ``Ventoy`` path here. Let's get started!

1. Create a MOSA disk image using the :doc:`launcher<tool-launcher>` or the :doc:`console launcher<tool-launcher-console>` tool.

2. Download the latest version of Ventoy for your platform and extract the archive.

3. Launch the GUI program for your computer's architecture. You should now see a window that looks roughly like this:

.. image:: images/ventoy2disk-gui.png

Go to ``Option``, and select the correct partition style for the target machine (``MBR`` for BIOS, ``GPT`` for UEFI, although some recent BIOS-based machines can still read GPT-based drives). You'll need to follow `extra steps <https://www.ventoy.net/en/doc_secure.html>`__ for booting with Secure Boot though.

4. Select your drive in the drop down, then, when you're ready, click ``Install``.

.. danger:: When installing Ventoy for the first time, all data on the USB flash drive will be lost!

If you just wish to update an existing Ventoy installation on the drive though, click ``Update``. This will not erase any data on the drive and will instead only update Ventoy's partition on the drive.

5. Close out of the Ventoy GUI, and navigate to your USB flash drive's main partition. All that's left to do is to copy the disk image you previously created, and paste it onto your USB flash drive, and you're done!
