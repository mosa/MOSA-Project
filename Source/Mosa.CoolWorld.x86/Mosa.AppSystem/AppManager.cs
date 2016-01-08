// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

using System.IO;
using Mosa.Kernel.x86;
using Mosa.Application;

namespace Mosa.AppSystem
{
	/// <summary>
	///
	/// </summary>
	public class AppManager
	{
		protected IScanCodeMap keymap;

		protected IConsoleApp currentApp;

		protected IConsoleApp Shell;

		protected ConsoleSession debug;

		private uint counter = 0;

		public AppManager(ConsoleSession debug, IScanCodeMap keymap)
		{
			this.keymap = keymap;

			Shell = new Shell();

			currentApp = Shell;

			this.debug = debug;
		}

		public void Start()
		{
			var stream = new AppOutputStream(Mosa.CoolWorld.x86.Boot.Console);

			StartApp(Shell, null, stream, true, null);
		}

		public void Poll()
		{
			// takes key board input and directs it to the active application
			byte scancode = Mosa.CoolWorld.x86.Setup.Keyboard.GetScanCode();

			if (scancode == 0)
				return;

			var keyevent = keymap.ConvertScanCode(scancode);

			if (keyevent.KeyType == KeyType.F1)
			{
				SetCurrentApplication(currentApp);
				return;
			}
			if (keyevent.KeyType == KeyType.F2)
			{
				ConsoleManager.Controller.Active = ConsoleManager.Controller.Boot;
				return;
			}
			else if (keyevent.KeyType == KeyType.F3)
			{
				ConsoleManager.Controller.Active = ConsoleManager.Controller.Debug;
				return;
			}

			if (keyevent.KeyPress != KeyEvent.Press.Make)
				return;

			var input = currentApp.Console.Input as AppInputStream;

			if (input != null)
			{
				input.Write((byte)keyevent.Character);

				if (currentApp.Console.EnableEcho)
				{
					var output = currentApp.Console.Output as AppOutputStream;

					if (output != null)
					{
						output.WriteByte((byte)keyevent.Character);
					}
				}
			}
		}

		public void SetCurrentApplication(IConsoleApp app)
		{
			currentApp = app;

			var console = currentApp.Console.Output as AppOutputStream;

			if (console != null)
			{
				ConsoleManager.Controller.Active = console.Session;
			}
		}

		public void StartApp(IConsoleApp app, Stream input, Stream output, bool enableEcho, string parameters)
		{
			if (input == null)
			{
				input = new AppInputStream();
			}

			if (output == null)
			{
				var session = Mosa.Kernel.x86.ConsoleManager.Controller.CreateSeason();

				output = new AppOutputStream(session);
			}

			app.Console.Input = input;
			app.Console.Output = output;
			app.Console.EnableEcho = enableEcho;
			app.AppManager = this;

			SetCurrentApplication(app);

			app.Start(parameters);
		}

		public void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			uint c = debug.Column;
			uint r = debug.Row;
			byte col = debug.Color;
			byte back = debug.BackgroundColor;
			uint sr = debug.ScrollRow;

			debug.Color = Colors.Cyan;
			debug.ScrollRow = debug.Rows;

			debug.Row = 24;
			debug.Column = 1;

			debug.Write("Free: ");
			debug.Write((PageFrameAllocator.TotalPages - PageFrameAllocator.TotalPagesInUse) * PageFrameAllocator.PageSize / (1024 * 1024));
			debug.Write(" MB");

			debug.Column = 45;
			debug.BackgroundColor = Colors.Black;
			debug.Write("        ");
			debug.Column = 44;
			debug.Row = 24;

			debug.Write('X');
			counter++;
			debug.Write(counter, 10, 7);
			debug.Write(':');
			debug.Write(interrupt, 16, 2);
			debug.Write(':');
			debug.Write(errorCode, 16, 2);

			if (interrupt >= 0x20 && interrupt < 0x30)
			{
				debug.Write('-');
				debug.Write(counter, 10, 7);
				debug.Write(':');
				debug.Write(interrupt, 16, 2);

				Mosa.DeviceSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));
			}

			debug.Column = c;
			debug.Row = r;
			debug.Color = col;
			debug.BackgroundColor = back;
			debug.ScrollRow = sr;

			if (interrupt == 0x20)
			{
				Poll();
			}
		}

		public unsafe static void DumpStackTrace(int line)
		{
			uint depth = 0;

			Mosa.Kernel.x86.ConsoleManager.Controller.Debug.Write("At Line: ");
			Mosa.Kernel.x86.ConsoleManager.Controller.Debug.WriteLine(line.ToString());

			while (true)
			{
				var methodDef = Mosa.Platform.Internal.x86.Runtime.GetMethodDefinitionFromStackFrameDepth(depth);

				if (methodDef == null)
					return;

				string caller = Mosa.Platform.Internal.x86.Runtime.GetMethodDefinitionName(methodDef);

				if (caller == null)
					return;

				Mosa.Kernel.x86.ConsoleManager.Controller.Debug.Write(depth, 10, 2);
				Mosa.Kernel.x86.ConsoleManager.Controller.Debug.Write(":");
				Mosa.Kernel.x86.ConsoleManager.Controller.Debug.WriteLine(caller);

				depth++;
			}
		}
	}
}
