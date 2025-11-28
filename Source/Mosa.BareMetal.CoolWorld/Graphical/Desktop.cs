using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Mosa.BareMetal.CoolWorld.Graphical.Apps;
using Mosa.BareMetal.CoolWorld.Graphical.Components;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem.Fonts;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Services;
using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.CoolWorld.Graphical;

public static class Desktop
{
	public static DeviceService DeviceService { get; private set; }

	public static PCService PCService { get; private set; }

	public static Random Random { get; private set; }

	public static Taskbar Taskbar { get; private set; }

	public static void Start()
	{
		Debug.WriteLine("Desktop::Start()");

		DeviceService = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<DeviceService>();
		PCService = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<PCService>();
		Random = new Random();

		Display.DefaultFont = Utils.Load(File.ReadAllBytes("font.bin"));

		Utils.Fonts = new List<ISimpleFont>
		{
			Display.DefaultFont,
			Utils.Load(File.ReadAllBytes("font2.bin"))
		};

		if (!Display.Initialize()) HAL.Abort("An error occurred when initializing the graphics driver.");

		Utils.Mouse = DeviceService.GetFirstDevice<StandardMouse>().DeviceDriver as StandardMouse;

		if (Utils.Mouse == null) HAL.Abort("Mouse not found.");

		Utils.BackColor = Color.Indigo;
		Utils.Mouse.SetScreenResolution(Display.Width, Display.Height);

		Mouse.Initialize();
		WindowManager.Initialize();

		Taskbar = new Taskbar();
		Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Shutdown", Color.Blue, Color.White, Color.Navy,
			() => { Environment.Exit(0); return null; }));
		Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Reset", Color.Blue, Color.White, Color.Navy,
			() => { PCService.Reset(); return null; }));
		Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Paint", Color.Coral, Color.White, Color.Red,
			() => { WindowManager.Open(new Paint(70, 90, 400, 200, Color.MediumPurple, Color.Purple, Color.White)); return null; }));
		Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Settings", Color.Coral, Color.White, Color.Red,
			() => { WindowManager.Open(new Settings(70, 90, 400, 200, Color.MediumPurple, Color.Purple, Color.White)); return null; }));

		for (; ; )
		{
			// Get current time
			var time = Platform.GetTime();

			// Clear screen
			Display.Clear(Utils.BackColor);

			// Draw MOSA logo
			Display.DrawMosaLogo(10);

			// Initialize background labels
			var labels = new List<Label>
			{
				new("Current resolution is " + Display.Width + "x" + Display.Height, Display.DefaultFont, 10, 10, Color.OrangeRed),
				new("FPS is " + FPSMeter.FPS, Display.DefaultFont, 10, 26, Color.Lime),
				new("Current font is " + Display.DefaultFont.Name, Display.DefaultFont, 10, 42, Color.MidnightBlue),
				new(
					(time.Hour < 10 ? "0" + time.Hour : time.Hour)
					+ ":" +
					(time.Minute < 10 ? "0" + time.Minute : time.Minute)
					+ ":" +
					(time.Second < 10 ? "0" + time.Second : time.Second),
					Display.DefaultFont, 10, 58, Color.LightSeaGreen
				)
			};

			// Draw all labels
			foreach (var label in labels) label.Draw();

			// Draw and update all windows
			WindowManager.Update();

			// Draw taskbar on top of everything else (except cursor) and update it
			Taskbar.Draw();
			Taskbar.Update();

			// Draw cursor
			Mouse.Draw();

			// Update graphics and FPS meter
			Display.Update();
			FPSMeter.Update(ref time);
		}
	}
}
