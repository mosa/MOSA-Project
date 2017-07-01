// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using Mosa.Utility.RSP.Command;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.GDB
{
	public delegate void OnStatusChange();

	public delegate void OnLogEvent(string description);

	public delegate void OnMemoryRead(ulong address, byte[] bytes);

	public class Connector
	{
		public const int DefaultConnectionPort = 1234;

		public TcpClient TcpClient { get; set; }
		public GDBClient GDBClient { get; set; }

		public OnStatusChange OnPause { get; set; }
		public OnStatusChange OnRunning { get; set; }

		public bool IsConnected { get { return GDBClient.IsConnected; } }
		public bool IsRunning { get { return !IsPaused; } }
		public bool IsPaused { get; set; } = true;

		public BasePlatform Platform { get; set; }

		protected Dictionary<GDBCommand, OnMemoryRead> OnMemoryReadMap = new Dictionary<GDBCommand, GDB.OnMemoryRead>();

		public Connector(BasePlatform platform)
		{
			Platform = platform;
		}

		public Connector(BasePlatform platform, int port = DefaultConnectionPort)
			: this(platform)
		{
			Connect(port);
		}

		public bool Connect(int port)
		{
			try
			{
				Disconnect();

				// todo: better error handling
				TcpClient = new TcpClient("localhost", port);
				var stream = new GDBNetworkStream(TcpClient.Client, true);
				GDBClient = new GDBClient(stream);

				if (!GDBClient.IsConnected)
					return false;

				Break();
				GetRegisters();

				return true;
			}
			catch
			{
				MessageBox.Show($"Oh no :(\n\nSomething went terribly wrong. Could not connect to 'localhost' on port {port}.");
				return false;
			}
		}

		public void Disconnect()
		{
			if (GDBClient == null)
				return;

			TcpClient.Close();

			TcpClient = null;
			GDBClient = null;
		}

		public void Restart()
		{ }

		public void Step()
		{
			var command = new Step(OnStep);

			GDBClient.SendCommandAsync(command);

			CallOnRunning();
		}

		public void AddBreakPoint(BreakPoint bp)
		{
			var command1 = new SetBreakPoint(bp.Address, 4, 0);
			GDBClient.SendCommandAsync(command1);
			Break();
			Continue();
			CallOnRunning();
		}

		public void ClearBreakPoint(BreakPoint bp)
		{
			var command = new ClearBreakPoint(bp.Address, 4, 0);
			GDBClient.SendCommandAsync(command);
			CallOnRunning();
		}

		public void StepN(uint stepCount)
		{
			var command = new Step();
			for(uint currentStep = 0; currentStep<stepCount - 1; currentStep++)
			{
				GDBClient.SendCommandAsync(command);
			}
			command = new Step(OnStep);
			GDBClient.SendCommandAsync(command);
			CallOnRunning();
		}

		public void Continue()
		{
			if (IsRunning)
				return;

			var command = new Continue(OnStop);

			GDBClient.SendCommandAsync(command);

			IsPaused = false;

			CallOnRunning();
		}

		public void Break()
		{
			GDBClient.SendBreak();
			IsPaused = true;
		}

		public void GetRegisters()
		{
			var command = new GetRegisters(OnGetRegisters);

			GDBClient.SendCommandAsync(command);
		}

		public void ReadMemory(ulong address, int size, OnMemoryRead onMemoryRead)
		{
			var command = new ReadMemory(address, size, OnMemoryRead);

			OnMemoryReadMap.Add(command, onMemoryRead);

			GDBClient.SendCommandAsync(command);
		}

		private void OnStop(GDBCommand command)
		{
			IsPaused = true;

			GetRegisters();
		}

		private void OnStep(GDBCommand command)
		{
			IsPaused = true;

			GetRegisters();

			CallOnRunning();
		}

		private void OnCompetion(GDBCommand command)
		{
			IsPaused = true;
		}

		private void OnGetRegisters(GDBCommand command)
		{
			IsPaused = true;

			Platform.Parse(command);

			CallOnPause();
		}

		private void CallOnPause()
		{
			OnPause?.Invoke();
		}

		private void CallOnRunning()
		{
			OnRunning?.Invoke();
		}

		private void OnMemoryRead(GDBCommand command)
		{
			var bytes = command.GetAllBytes();

			var invoke = OnMemoryReadMap[command];

			OnMemoryReadMap.Remove(command);

			invoke((command as ReadMemory).Address, bytes);
		}
	}
}
