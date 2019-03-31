// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Application;
using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using System.IO;

namespace Mosa.AppSystem
{
	/// <summary>
	/// App Manager
	/// </summary>
	public class AppManager
	{
		protected IConsoleApp currentApp;

		protected IConsoleApp shell;

		protected IKeyboard keyboard;

		protected ConsoleSession debug;

		public ServiceManager ServiceManager;

		private uint tick = 0;

		public AppManager(ConsoleSession debug, IKeyboard keyboard, ServiceManager serviceManager)
		{
			this.ServiceManager = serviceManager;
			this.keyboard = keyboard;
			this.debug = debug;

			shell = new Shell();

			currentApp = shell;
		}

		public void Start()
		{
			var stream = new AppOutputStream(CoolWorld.x86.Boot.Console);

			StartApp(shell, null, stream, true, null);
		}

		public void CheckForKeyPress()
		{
			// get the key board input and directs it to the active application
			var key = keyboard.GetKeyPressed();

			if (key == null)
				return;

			if (key.KeyType == KeyType.NoKey)
				return;

			if (key.KeyType == KeyType.F1)
			{
				SetCurrentApplication(currentApp);
				return;
			}
			if (key.KeyType == KeyType.F2)
			{
				ConsoleManager.Controller.Active = ConsoleManager.Controller.Boot;
				return;
			}
			else if (key.KeyType == KeyType.F3)
			{
				ConsoleManager.Controller.Active = ConsoleManager.Controller.Debug;
				return;
			}

			if (currentApp.Console.Input is AppInputStream input)
			{
				input.Write((byte)key.Character);

				if (currentApp.Console.EnableEcho)
				{
					var output = currentApp.Console.Output as AppOutputStream;
					output?.WriteByte((byte)key.Character);
				}
			}
		}

		public void SetCurrentApplication(IConsoleApp app)
		{
			currentApp = app;

			if (currentApp.Console.Output is AppOutputStream console)
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
				var session = ConsoleManager.Controller.CreateSeason();

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
			tick++;

			uint column = debug.Column;
			uint row = debug.Row;
			var color = debug.Color;
			var background = debug.BackgroundColor;
			uint scrollrow = debug.ScrollRow;

			debug.Color = ScreenColor.Cyan;
			debug.BackgroundColor = ScreenColor.Black;
			debug.Row = 24;
			debug.Column = 0;
			debug.ScrollRow = debug.Rows;

			debug.Write("Shell Mode - ");
			debug.Write("Tick: ");
			debug.Write(tick, 10, 7);
			debug.Write(" Free Memory: ");
			debug.Write((PageFrameAllocator.TotalPages - PageFrameAllocator.TotalPagesInUse) * PageFrameAllocator.PageSize / (1024 * 1024));
			debug.Write(" MB ");

			debug.Write("IRQ: ");
			debug.Write(interrupt, 16, 2);

			if (interrupt >= 0x20 && interrupt < 0x30)
			{
				HAL.ProcessInterrupt((byte)(interrupt - 0x20));
			}

			debug.BackgroundColor = background;
			debug.ScrollRow = scrollrow;
			debug.Column = column;
			debug.Color = color;
			debug.Row = row;

			if (interrupt == 0x21)
			{
				CheckForKeyPress();
			}
		}

		public static void DumpData(string data)
		{
			ConsoleManager.Controller.Debug.Write(data);
		}

		public static void DumpDataLine(string data)
		{
			ConsoleManager.Controller.Debug.WriteLine(data);
		}

		public unsafe static void DumpStackTrace(int line)
		{
			uint depth = 0;

			ConsoleManager.Controller.Debug.Write("At Line: ");
			ConsoleManager.Controller.Debug.WriteLine(line.ToString());

			while (true)
			{
				var methodDef = Runtime.x86.Internal.GetMethodDefinitionFromStackFrameDepth(depth);

				if (methodDef.IsNull)
					return;

				string caller = methodDef.Name;

				if (caller == null)
					return;

				ConsoleManager.Controller.Debug.Write(depth, 10, 2);
				ConsoleManager.Controller.Debug.Write(":");
				ConsoleManager.Controller.Debug.WriteLine(caller);

				depth++;
			}
		}
	}
}
