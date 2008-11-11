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
using Mosa.EmulatedDevices.Synthetic;

using Pictor;
using Pictor.Renderer;
using Pictor.Renderer.PixelFormats;
using Pictor.Renderer.ColorTypes;

namespace Mosa.Emulator
{
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
			IHardwareAbstraction hardwareAbstraction = new Mosa.EmulatedKernel.HardwareAbstraction();

			// Set Device Driver system to the emulator port and memory methods
			DeviceSystem.HAL.SetHardwareAbstraction(hardwareAbstraction);

			// Start the emulated devices
			EmulatedDevices.Setup.Initialize();

			// Initialize the driver system
			DeviceSystem.Setup.Initialize();

			// Start the driver system
			DeviceSystem.Setup.AddDriverAssembly(typeof(Mosa.DeviceDrivers.ISA.CMOS).Module.Assembly);

			// Start the driver system
			DeviceSystem.Setup.Start();

			// Set the interrupt handler
			DeviceSystem.HAL.SetInterruptHandler(DeviceSystem.Setup.ResourceManager.InterruptManager.ProcessInterrupt);

			// Create Emulated Keyboard device
			Mosa.EmulatedDevices.Synthetic.Keyboard keyboard = new Mosa.EmulatedDevices.Synthetic.Keyboard();

			// Added the emulated keyboard device to the device drivers
			DeviceSystem.Setup.DeviceManager.Add(keyboard);

			// Create Emulated Graphic Pixel device
			Mosa.EmulatedDevices.Synthetic.PixelGraphicDevice pixelGraphicDevice = new Mosa.EmulatedDevices.Synthetic.PixelGraphicDevice(500, 500);

			// Added the emulated keyboard device to the device drivers
			DeviceSystem.Setup.DeviceManager.Add(pixelGraphicDevice);

			// Get the Text VGA device
			LinkedList<IDevice> devices = DeviceSystem.Setup.DeviceManager.GetDevices(new FindDevice.WithName("VGAText"));

			// Create a screen interface to the Text VGA device
			ITextScreen screen = new TextScreen((ITextDevice)devices.First.value);

			// Get a list of all devices
			devices = DeviceSystem.Setup.DeviceManager.GetAllDevices();

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

				if (device is PCIDevice) {
					PCIDevice pciDevice = (device as PCIDevice);

					screen.Write("  Vendor:0x");
					screen.Write(pciDevice.VendorID.ToString("X"));
					screen.Write(" Device:0x");
					screen.Write(pciDevice.DeviceID.ToString("X"));
					screen.Write(" Class:0x");
					screen.Write(pciDevice.ClassCode.ToString("X"));
					screen.Write(" Rev:0x");
					screen.Write(pciDevice.RevisionID.ToString("X"));
					screen.WriteLine();

					foreach (BaseAddress address in pciDevice.BaseAddresses) {
						if (address == null)
							continue;
						
						if (address.Address == 0)
							continue;

						screen.Write("    ");

						if (address.Region == AddressRegion.IO)
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

			byte[] buffer = new byte[pixelGraphicDevice.Width * pixelGraphicDevice.Height * 3];
			RenderingBuffer<byte> renderbuffer = new RenderingBuffer<byte>(buffer, (uint)pixelGraphicDevice.Width, (uint)pixelGraphicDevice.Height, 3);

			RgbPixelFormat<byte> pixfmt = new RgbPixelFormat<byte>(pixelGraphicDevice.Width, pixelGraphicDevice.Height, renderbuffer);
			pixfmt.Width = pixelGraphicDevice.Width;
			pixfmt.Height = pixelGraphicDevice.Height;

			BaseRenderer<RgbPixelFormat<byte>, RgbColor<byte>> renderer = new BaseRenderer<RgbPixelFormat<byte>, RgbColor<byte>>();
			renderer.Attach(pixfmt);

			// Create picture
			double[] spline_r_x = { 0.000000, 0.200000, 0.400000, 0.910484, 0.957258, 1.000000 };
			double[] spline_r_y = { 1.000000, 0.800000, 0.600000, 0.066667, 0.169697, 0.600000 };

			double[] spline_g_x = { 0.000000, 0.292244, 0.485655, 0.564859, 0.795607, 1.000000 };
			double[] spline_g_y = { 0.000000, 0.607260, 0.964065, 0.892558, 0.435571, 0.000000 };

			double[] spline_b_x = { 0.000000, 0.055045, 0.143034, 0.433082, 0.764859, 1.000000 };
			double[] spline_b_y = { 0.385480, 0.128493, 0.021416, 0.271507, 0.713974, 1.000000 };

			Pictor.Objects.Splines.BSpline _splineRed = new Pictor.Objects.Splines.BSpline(6, spline_r_x, spline_r_y);
			Pictor.Objects.Splines.BSpline _splineGreen = new Pictor.Objects.Splines.BSpline(6, spline_g_x, spline_g_y);
			Pictor.Objects.Splines.BSpline _splineBlue = new Pictor.Objects.Splines.BSpline(6, spline_b_x, spline_b_y);

			System.Random rnd = new Random();

			double rx = pixelGraphicDevice.Width / 3.5;
			double ry = pixelGraphicDevice.Height / 3.5;

			double alpha = 1.0;
			double step = 0.35;
			while (true) {
				buffer = new byte[pixelGraphicDevice.Width * pixelGraphicDevice.Height * 3];
				renderbuffer = new RenderingBuffer<byte>(buffer, (uint)pixelGraphicDevice.Width, (uint)pixelGraphicDevice.Height, 3);

				pixfmt = new RgbPixelFormat<byte>(pixelGraphicDevice.Width, pixelGraphicDevice.Height, renderbuffer);
				pixfmt.Width = pixelGraphicDevice.Width;
				pixfmt.Height = pixelGraphicDevice.Height;

				renderer = new BaseRenderer<RgbPixelFormat<byte>, RgbColor<byte>>();
				renderer.Attach(pixfmt);

				renderer.Ren.Buffer.Clear(240);
				for (int i = 0; i < 50000; ++i) {
					double z = rnd.NextDouble();
					double x = System.Math.Cos(z * 2.0 * System.Math.PI) * rx;
					double y = System.Math.Sin(z * 2.0 * System.Math.PI) * ry;
					double angle = rnd.NextDouble() * System.Math.PI * 2.0;
					double dist = rnd.NextDouble() * (rx / (alpha + 1.0));

					RgbColor<byte> color = new RgbColor<byte>();
					color.r = (byte)((_splineRed.Get(z) * 0.8) * 255);
					color.g = (byte)((_splineGreen.Get(z) * 0.8) * 255);
					color.b = (byte)((_splineBlue.Get(z) * 0.8) * 255);

					renderer.CopyPixel((int)(pixelGraphicDevice.Width / 2.0 + x + System.Math.Cos(angle) * dist), (int)(pixelGraphicDevice.Height / 2.0 + y + System.Math.Sin(angle) * dist), color);
				}

				pixelGraphicDevice.Lock();
				for (ushort x = 0; x < pixelGraphicDevice.Width; ++x) {
					for (ushort y = 0; y < pixelGraphicDevice.Height; ++y) {
						System.ArraySegment<byte> color = renderer.Ren.Buffer.GetPartialRow(x, y, 1);
						pixelGraphicDevice.WritePixelFast(new Color(color.Array[0 + color.Offset], color.Array[1 + color.Offset], color.Array[2 + color.Offset]), x, y);
					}
				}
				pixelGraphicDevice.Unlock();
				alpha = (alpha + step);
				if (alpha >= 6.0)
					step = -step;
				else if (alpha <= 0.0)
					step = -step;
				pixelGraphicDevice.Update();
			}

			//Key key = keyboard.GetKeyPressed();

			//return;
		}


	}
}
