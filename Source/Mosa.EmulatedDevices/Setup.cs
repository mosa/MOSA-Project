/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Threading;
using System.Windows.Forms;
using Mosa.EmulatedDevices.Emulated;
using Mosa.EmulatedDevices.Synthetic;
using Mosa.EmulatedKernel;

namespace Mosa.EmulatedDevices
{
	/// <summary>
	/// 
	/// </summary>
	public static class Setup
	{
		/// <summary>
		/// Primary Display Form (necessary for synthetic keyboard)
		/// </summary>
		public static DisplayForm PrimaryDisplayForm;

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public static void Initialize()
		{
			SetupPrimaryDisplayForm();

			// Emulate a ram chip (128Mb)
			//Mosa.EmulatedDevices.Emulated.RAMChip ramChip1 = 
			new Mosa.EmulatedDevices.Emulated.RAMChip(0, 1024 * 640);
			//Mosa.EmulatedDevices.Emulated.RAMChip ramChip2 = 
			new Mosa.EmulatedDevices.Emulated.RAMChip(1024 * 1024, 1024 * 1024 * 127);
			
			// Setup PCI Bus
			PCIBus pciBus = new PCIBus();

			// Setup PCI Controller
			pciBus.Add(new PCIController(PCIController.StandardIOBase, pciBus));
			IOPortDispatch.RegisterDevice(pciBus.Get(0) as IIOPortDevice);

			// Add CMOS
			IOPortDispatch.RegisterDevice(new CMOS(CMOS.StandardIOBase));

			// Add VGA Controller
			IOPortDispatch.RegisterDevice(new VGAConsole(PrimaryDisplayForm));

			// Add IDE Controller
			//string[] files = new string[1];
			//files[0] = @"..\Data\HardDriveImage\hd.img";

			// Fix for Linux
			//files[0] = files[0].Replace('\\', System.IO.Path.DirectorySeparatorChar);

			//IOPortDispatch.RegisterDevice(new IDEController(IDEController.PrimaryIOBase, files));

			// Simulate multiboot
			Mosa.EmulatedDevices.Multiboot.Setup();
		}

		/// <summary>
		/// Setups the primary display form.
		/// </summary>
		public static void SetupPrimaryDisplayForm()
		{
			PrimaryDisplayForm = new DisplayForm(800, 600);

			Thread thread = new Thread(new ThreadStart(CreatePrimaryDisplayForm));
			thread.Start();
		}

		/// <summary>
		/// Creates the form.
		/// </summary>
		private static void CreatePrimaryDisplayForm()
		{
			PrimaryDisplayForm.StartTimer();
			Application.Run(PrimaryDisplayForm);
		}
	}
}
