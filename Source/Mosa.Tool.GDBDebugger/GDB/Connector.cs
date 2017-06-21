// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using Mosa.Utility.RSP.Command;
using System.Net.Sockets;

namespace Mosa.Tool.GDBDebugger.GDB
{
	public delegate void OnStatusChange();

	public class Connector
	{
		public const int DefaultConnectionPort = 1234;

		public TcpClient TcpClient { get; set; }
		public GDBClient GDBClient { get; set; }

		public OnStatusChange OnStatusChange { get; set; }

		public bool IsRunning { get; set; } = false;

		public BasePlatform Platform { get; set; }

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
			Disconnect();

			TcpClient = new TcpClient("localhost", port);
			var stream = new GDBNetworkStream(TcpClient.Client, true);
			GDBClient = new GDBClient(stream);

			if (!GDBClient.IsConnected)
				return false;

			Break();

			return true;
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
		}

		public void Continue()
		{
			if (IsRunning)
				return;

			var command = new Continue(OnStop);

			GDBClient.SendCommandAsync(command);

			IsRunning = true;
		}

		public void Break()
		{
			GDBClient.SendBreak();
			IsRunning = false;
		}

		public void GetRegisters()
		{
			var command = new GetRegisters(OnGetRegisters);

			GDBClient.SendCommandAsync(command);
		}

		private void OnStop(GDBCommand command)
		{
			IsRunning = false;

			GetRegisters();

			CallOnChange();
		}

		private void OnStep(GDBCommand command)
		{
			IsRunning = false;

			GetRegisters();

			CallOnChange();
		}

		private void OnCompetion(GDBCommand command)
		{
			IsRunning = false;
			CallOnChange();
		}

		private void OnGetRegisters(GDBCommand command)
		{
			IsRunning = false;

			Platform.Parse(command);

			CallOnChange();
		}

		private void CallOnChange()
		{
			OnStatusChange?.Invoke();
		}
	}
}
