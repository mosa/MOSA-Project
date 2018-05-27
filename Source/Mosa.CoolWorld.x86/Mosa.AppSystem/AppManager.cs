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

		private uint tick = 0;

		public AppManager(ConsoleSession debug, IKeyboard keyboard)
		{
			this.keyboard = keyboard;

			this.debug = debug;

			shell = new Shell();

			currentApp = shell;
		}

		public void Start()
		{
			var stream = new AppOutputStream(Mosa.CoolWorld.x86.Boot.Console);

			StartApp(shell, null, stream, true, null);
		}

		public void Poll()
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

			var input = currentApp.Console.Input as AppInputStream;

			if (input != null)
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

			debug.Color = Kernel.x86.Color.Cyan;
			debug.BackgroundColor = Kernel.x86.Color.Black;
			debug.Row = 24;
			debug.Column = 0;
			debug.ScrollRow = debug.Rows;

			tick++;
			debug.Write("Shell Mode - ");
			debug.Write("Tick: ");
			debug.Write(tick, 10, 7);
			debug.Write(" Free Memory: ");
			debug.Write((PageFrameAllocator.TotalPages - PageFrameAllocator.TotalPagesInUse) * PageFrameAllocator.PageSize / (1024 * 1024));
			debug.Write(" MB    ");

			if (interrupt >= 0x20 && interrupt < 0x30)
			{
				DeviceSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));
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

		public static void DumpData(string data)
		{
			Mosa.Kernel.x86.ConsoleManager.Controller.Debug.Write(data);
		}

		public static void DumpDataLine(string data)
		{
			Mosa.Kernel.x86.ConsoleManager.Controller.Debug.WriteLine(data);
		}

		public unsafe static void DumpStackTrace(int line)
		{
			uint depth = 0;

			Mosa.Kernel.x86.ConsoleManager.Controller.Debug.Write("At Line: ");
			Mosa.Kernel.x86.ConsoleManager.Controller.Debug.WriteLine(line.ToString());

			while (true)
			{
				var methodDef = Mosa.Runtime.x86.Internal.GetMethodDefinitionFromStackFrameDepth(depth);

				if (methodDef.IsNull)
					return;

				string caller = methodDef.Name;

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
