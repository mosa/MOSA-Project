/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.FileSystem;
using Mosa.FileSystem.FAT;
using Mosa.EmulatedDevices.Synthetic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mosa.Emulator
{
    public class EmulatorDemo : Pictor.UI.EmulatorPlatform.PlatformSupport
    {
        double[] m_x = new double[2];
        double[] m_y = new double[2];
        double m_dx;
        double m_dy;
        int m_idx;
        Pictor.UI.ButtonWidget button;


        public EmulatorDemo(PixelFormats format, Pictor.UI.PlatformSupportAbstract.ERenderOrigin RenderOrigin)
            : base(format, RenderOrigin)
        {
            m_idx = (-1);

            m_x[0] = 100; m_y[0] = 100;
            m_x[1] = 500; m_y[1] = 350;
            //button = new Pictor.UI.ButtonWidget(200, 600, "Quit Emulator");
            //AddChild(button);
        }


        public override void OnDraw()
        {
            Pictor.GammaLookupTable gamma = new Pictor.GammaLookupTable(1.8);
            Pictor.PixelFormat.IBlender NormalBlender = new Pictor.PixelFormat.BlenderBGRA();
            Pictor.PixelFormat.IBlender GammaBlender = new Pictor.PixelFormat.BlenderGammaBGRA(gamma);
            Pictor.PixelFormat.FormatRGBA pixf = new Pictor.PixelFormat.FormatRGBA(RenderBufferWindow, NormalBlender);
            Pictor.PixelFormat.FormatClippingProxy clippingProxy = new Pictor.PixelFormat.FormatClippingProxy(pixf);

            clippingProxy.Clear(new Pictor.RGBA_Doubles(1, 1, 1));

            Pictor.AntiAliasedScanlineRasterizer ras = new Pictor.AntiAliasedScanlineRasterizer();
            Pictor.Scanline sl = new Pictor.Scanline();

            Pictor.VertexSource.Ellipse e = new Pictor.VertexSource.Ellipse();

            // TODO: If you drag the control circles below the bottom of the window we get an exception.  This does not happen in Pictor.
            // It needs to be debugged.  Turning on clipping fixes it.  But standard Pictor works without clipping.  Could be a bigger problem than this.
            //ras.clip_box(0, 0, width(), height());

            // Render two "control" circles
            e.Init(m_x[0], m_y[0], 3, 3, 16);
            ras.AddPath(e);
            Pictor.Renderer.RenderSolid(clippingProxy, ras, sl, new Pictor.RGBA_Bytes(127, 127, 127));
            e.Init(m_x[1], m_y[1], 3, 3, 16);
            ras.AddPath(e);
            Pictor.Renderer.RenderSolid(clippingProxy, ras, sl, new Pictor.RGBA_Bytes(127, 127, 127));

            double d = 0.0;

            // Creating a rounded rectangle
            Pictor.VertexSource.RoundedRect r = new Pictor.VertexSource.RoundedRect(m_x[0] + d, m_y[0] + d, m_x[1] + d, m_y[1] + d, 25.0);
            r.NormalizeRadius();

            ras.AddPath(r);

            pixf.Blender = GammaBlender;
            Pictor.Renderer.RenderSolid(clippingProxy, ras, sl, new Pictor.RGBA_Bytes(117, 115, 217));

            // this was in the original demo, but it does nothing because we changed the blender not the gamma function.
            //ras.gamma(new gamma_none());
            // so let's change the blender instead
            pixf.Blender = NormalBlender;

            // Render the controls
            //m_radius.Render(ras, sl, clippingProxy);
            //m_gamma.Render(ras, sl, clippingProxy);
            //m_offset.Render(ras, sl, clippingProxy);
            //m_white_on_black.Render(ras, sl, clippingProxy);
            //m_DrawAsOutlineCheckBox.Render(ras, sl, clippingProxy);
            base.OnDraw();
        }


        public override void OnMouseDown(Pictor.UI.MouseEventArgs mouseEvent)
        {
            if (mouseEvent.Button == Pictor.UI.MouseButtons.Left)
            {
                for (int i = 0; i < 2; i++)
                {
                    double x = mouseEvent.X;
                    double y = mouseEvent.Y;
                    if (System.Math.Sqrt((x - m_x[i]) * (x - m_x[i]) + (y - m_y[i]) * (y - m_y[i])) < 5.0)
                    {
                        m_dx = x - m_x[i];
                        m_dy = y - m_y[i];
                        m_idx = i;
                        break;
                    }
                }
            }

            base.OnMouseDown(mouseEvent);
        }


        public override void OnMouseMove(Pictor.UI.MouseEventArgs mouseEvent)
        {
            if (mouseEvent.Button == Pictor.UI.MouseButtons.Left)
            {
                if (m_idx >= 0)
                {
                    m_x[m_idx] = mouseEvent.X - m_dx;
                    m_y[m_idx] = mouseEvent.Y - m_dy;
                    ForceRedraw();
                }
            }

            base.OnMouseMove(mouseEvent);
        }

        override public void OnMouseUp(Pictor.UI.MouseEventArgs mouseEvent)
        {
            m_idx = -1;
            base.OnMouseUp(mouseEvent);
        }

        public static void StartDemo()
        {
            EmulatorDemo app = new EmulatorDemo(Pictor.UI.PlatformSupportAbstract.PixelFormats.Rgba32, Pictor.UI.PlatformSupportAbstract.ERenderOrigin.OriginBottomLeft);
            app.Caption = "MOSA :: Emulator :: Pictor Demonstration";

            if (app.Init(600, 400, (uint)Pictor.UI.PlatformSupportAbstract.EWindowFlags.Risizeable))
            {
                app.Run();
            }
        }

    };

	/// <summary>
	/// Program with CLR emulated devices
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args">The args.</param>
		[STAThread]
		public static void Main(string[] args)
		{
			// Setup hardware abstraction interface
			IHardwareAbstraction hardwareAbstraction = new Mosa.EmulatedKernel.HardwareAbstraction();

			// Set device driver system to the emulator port and memory methods
			Mosa.DeviceSystem.HAL.SetHardwareAbstraction(hardwareAbstraction);

			// Start the emulated devices
			Mosa.EmulatedDevices.Setup.Initialize();

			// Initialize the driver system
			Mosa.DeviceSystem.Setup.Initialize();

			// Registry device drivers
			Mosa.DeviceSystem.Setup.DeviceDriverRegistry.RegisterBuiltInDeviceDrivers();
			Mosa.DeviceSystem.Setup.DeviceDriverRegistry.RegisterDeviceDrivers(typeof(Mosa.DeviceDrivers.ISA.CMOS).Module.Assembly);

			// Set the interrupt handler
			Mosa.DeviceSystem.HAL.SetInterruptHandler(Mosa.DeviceSystem.Setup.ResourceManager.InterruptManager.ProcessInterrupt);

			// Start the driver system
			Mosa.DeviceSystem.Setup.Start();

			// Create pci controller manager
			PCIControllerManager pciControllerManager = new PCIControllerManager(Mosa.DeviceSystem.Setup.DeviceManager);

			// Create pci controller devices
			pciControllerManager.CreatePartitionDevices();

			// Create synthetic keyboard device
			Mosa.EmulatedDevices.Synthetic.Keyboard keyboard = new Mosa.EmulatedDevices.Synthetic.Keyboard();

			// Add the emulated keyboard device to the device drivers
			Mosa.DeviceSystem.Setup.DeviceManager.Add(keyboard);

			// Create synthetic graphic pixel device
			//Mosa.EmulatedDevices.Synthetic.PixelGraphicDevice pixelGraphicDevice = new Mosa.EmulatedDevices.Synthetic.PixelGraphicDevice(500, 500);

			// Added the synthetic graphic device to the device drivers
			//Mosa.DeviceSystem.Setup.DeviceManager.Add(pixelGraphicDevice);

			// Create synthetic ram disk device
			Mosa.EmulatedDevices.Synthetic.RamDiskDevice ramDiskDevice = new Mosa.EmulatedDevices.Synthetic.RamDiskDevice(1024 * 1024 * 10 / 512);

			// Add emulated ram disk device to the device drivers
			Mosa.DeviceSystem.Setup.DeviceManager.Add(ramDiskDevice);

			// Create disk controller manager
			DiskControllerManager diskControllerManager = new DiskControllerManager(Mosa.DeviceSystem.Setup.DeviceManager);
	
			// Create disk devices from disk controller devices
			diskControllerManager.CreateDiskDevices();

			// Get the text VGA device
			LinkedList<IDevice> devices = Mosa.DeviceSystem.Setup.DeviceManager.GetDevices(new FindDevice.WithName("VGAText"));

			// Create a screen interface to the text VGA device
			ITextScreen screen = new TextScreen((ITextDevice)devices.First.value);

			// Create master boot block record
			MasterBootBlock mbr = new MasterBootBlock(ramDiskDevice);
			mbr.DiskSignature = 0x12345678;
			mbr.Partitions[0].Bootable = true;
			mbr.Partitions[0].StartLBA = 17;
			mbr.Partitions[0].TotalBlocks = ramDiskDevice.TotalBlocks - 17;
			mbr.Partitions[0].PartitionType = PartitionType.FAT12;
			mbr.Write();

			// Create partition device 
			PartitionDevice partitionDevice = new PartitionDevice(ramDiskDevice, mbr.Partitions[0], false);

			// Set FAT settings
			FatSettings fatSettings = new FatSettings();

			fatSettings.FATType = FatType.FAT12;
			fatSettings.FloppyMedia = false;
			fatSettings.VolumeLabel = "MOSADISK";
			fatSettings.SerialID = new byte[4] { 0x01, 0x02, 0x03, 0x04 };

			// Create FAT file system
			FatFileSystem fat12 = new FatFileSystem(partitionDevice);
			fat12.Format(fatSettings);

			// Create partition manager
			PartitionManager partitionManager = new PartitionManager(Mosa.DeviceSystem.Setup.DeviceManager);

			// Create partition devices
			partitionManager.CreatePartitionDevices();

			// Get a list of all devices
			devices = Mosa.DeviceSystem.Setup.DeviceManager.GetAllDevices();

			// Print them 
			screen.WriteLine("Devices: ");
			foreach (IDevice device in devices) {

				screen.Write(device.Name);
				screen.Write(" [");

				switch (device.Status) {
					case DeviceStatus.Online: screen.Write("Online"); break;
					case DeviceStatus.Available: screen.Write("Available"); break;
					case DeviceStatus.Initializing: screen.Write("Initializing"); break;
					case DeviceStatus.NotFound: screen.Write("Not Found"); break;
					case DeviceStatus.Error: screen.Write("Error"); break;
				}
				screen.Write("]");

				if (device.Parent != null) {
					screen.Write(" - Parent: ");
					screen.Write(device.Parent.Name);
				}
				screen.WriteLine();

				if (device is IPartitionDevice) {
					FileSystem.FAT.FatFileSystem fat = new Mosa.FileSystem.FAT.FatFileSystem(device as IPartitionDevice);

					screen.Write("  File System: ");
					if (fat.IsValid) {
						switch (fat.FATType) {
							case FatType.FAT12: screen.WriteLine("FAT12"); break;
							case FatType.FAT16: screen.WriteLine("FAT16"); break;
							case FatType.FAT32: screen.WriteLine("FAT32"); break;
							default: screen.WriteLine("Unknown"); break;
						}
						screen.WriteLine("  Volume Name: " + fat.VolumeLabel);
					}
					else
						screen.WriteLine("Unknown");
				}

				if (device is PCIDevice) {
					PCIDevice pciDevice = (device as PCIDevice);

					screen.Write("  Vendor:0x");
					screen.Write(pciDevice.VendorID.ToString("X"));
					screen.Write(" [");
					screen.Write(DeviceTable.Lookup(pciDevice.VendorID));
					screen.WriteLine("]");

					screen.Write("  Device:0x");
					screen.Write(pciDevice.DeviceID.ToString("X"));
					screen.Write(" Rev:0x");
					screen.Write(pciDevice.RevisionID.ToString("X"));
					screen.Write(" [");
					screen.Write(DeviceTable.Lookup(pciDevice.VendorID, pciDevice.DeviceID));
					screen.WriteLine("]");

					screen.Write("  Class:0x");
					screen.Write(pciDevice.ClassCode.ToString("X"));
					screen.Write(" [");
					screen.Write(ClassCodeTable.Lookup(pciDevice.ClassCode));
					screen.WriteLine("]");

					screen.Write("  SubClass:0x");
					screen.Write(pciDevice.SubClassCode.ToString("X"));
					screen.Write(" [");
					screen.Write(SubClassCodeTable.Lookup(pciDevice.ClassCode, pciDevice.SubClassCode, pciDevice.ProgIF));
					screen.WriteLine("]");

					//					screen.Write("  ");
					//					screen.WriteLine(DeviceTable.Lookup(pciDevice.VendorID, pciDevice.DeviceID, pciDevice.SubDeviceID, pciDevice.SubVendorID));

					foreach (PCIBaseAddress address in pciDevice.PCIBaseAddresses) {
						if (address == null)
							continue;

						if (address.Address == 0)
							continue;

						screen.Write("    ");

						if (address.Region == PCIAddressType.IO)
							screen.Write("I/O Port at 0x");
						else
							screen.Write("Memory at 0x");

						screen.Write(address.Address.ToString("X"));

						screen.Write(" [size=");

						if ((address.Size & 0xFFFFF) == 0) {
							screen.Write((address.Size >> 20).ToString());
							screen.Write("M");
						}
						else if ((address.Size & 0x3FF) == 0) {
							screen.Write((address.Size >> 10).ToString());
							screen.Write("K");
						}
						else
							screen.Write(address.Size.ToString());

						screen.Write("]");

						if (address.Prefetchable)
							screen.Write("(prefetchable)");

						screen.WriteLine();
					}

					if (pciDevice.IRQ != 0) {
						screen.Write("    ");
						screen.Write("IRQ at ");
						screen.Write(pciDevice.IRQ.ToString());
						screen.WriteLine();
					}
				}
			}

            EmulatorDemo.StartDemo();


			//Key key = keyboard.GetKeyPressed();

			//return;
		}

	}
}
