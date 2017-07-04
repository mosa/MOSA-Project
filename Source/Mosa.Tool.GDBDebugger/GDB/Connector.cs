// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using Mosa.Utility.RSP.Command;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Mosa.Tool.GDBDebugger.GDB
{
	public delegate void OnStatusChange();

	public delegate void OnLogEvent(string description);

	public delegate void OnMemoryRead(ulong address, byte[] bytes);

	public class Connector
	{
		public const string DefaultConnectionHost = "localhost";
		public const int DefaultConnectionPort = 1234;

		public TcpClient TcpClient { get; set; }
		public GDBClient GDBClient { get; set; }

		public OnStatusChange OnPause { get; set; }
		public OnStatusChange OnRunning { get; set; }

		public bool IsConnected { get { return GDBClient == null ? false : GDBClient.IsConnected; } }
		public bool IsRunning { get { return !IsPaused; } }
		public bool IsPaused { get; set; } = true;

		public string ConnectionHost { get; set; }
		public int ConnectionPort { get; set; }
		public BasePlatform Platform { get; set; }

		protected List<ulong> BreakPoints = new List<ulong>();
		protected Dictionary<GDBCommand, OnMemoryRead> OnMemoryReadMap = new Dictionary<GDBCommand, GDB.OnMemoryRead>();

		public Connector(BasePlatform platform)
		{
			Platform = platform;

			ConnectionHost = DefaultConnectionHost;
			ConnectionPort = DefaultConnectionPort;
		}

		public Connector(BasePlatform platform, string host = DefaultConnectionHost, int port = DefaultConnectionPort)
			: this(platform)
		{
			ConnectionHost = host;
			ConnectionPort = port;
		}

		public bool Connect()
		{
			return Connect(ConnectionHost, ConnectionPort);
		}

		public bool Connect(int port)
		{
			return Connect(ConnectionHost, port);
		}

		public bool Connect(string host, int port)
		{
			try
			{
				Disconnect();

				TcpClient = new TcpClient();
				TcpClient.Connect(host, port);

				var stream = new GDBNetworkStream(TcpClient.Client, true);
				GDBClient = new GDBClient(stream);

				if (!GDBClient.IsConnected)
					return false;

				Break();
				GetRegisters();
			}
			catch (SocketException)
			{
				return false;
			}

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

		public void ExtendedMode()
		{
			var command = new ExtendedMode();

			GDBClient.SendCommandAsync(command);
		}

		public void Restart()
		{
			var command = new Reset();

			GDBClient.SendCommandAsync(command);
		}

		public void Kill()
		{
			var command = new Kill();

			GDBClient.SendCommandAsync(command);

			Disconnect();
		}

		public void Step()
		{
			var command = new Step(OnStep);

			var bps = ClearBreakpointsInternal();
			GDBClient.SendCommandAsync(command);
			RestoreBreakpointsInternal(bps);

			CallOnRunning();
		}

		public void AddBreakPoint(ulong address)
		{
			bool wasRunning = IsRunning && !IsPaused;
			if (IsRunning)
			{
				Break();
			}
			BreakPoints.Add(address);
			var command = new SetBreakPoint(address, 4, 0, (GDBCommand c) => OnSetBreakpoint(c, wasRunning));
			GDBClient.SendCommandAsync(command);
		}

		public void ClearBreakPoint(ulong address)
		{
			var command = new ClearBreakPoint(address, 4, 0);
			BreakPoints.Remove(address);
			GDBClient.SendCommandAsync(command);
		}

		public void StepN(uint stepCount)
		{
			var bps = ClearBreakpointsInternal();
			var command = new Step();
			for (uint currentStep = 0; currentStep < stepCount - 1; currentStep++)
			{
				GDBClient.SendCommandAsync(command);
			}
			RestoreBreakpointsInternal(bps);

			Step();
		}

		public void Continue()
		{
			if (IsRunning)
				return;

			ClearBreakpointsStepAndRestore();
			var command = new Continue(OnStop);
			GDBClient.SendCommandAsync(command);

			IsPaused = false;

			CallOnRunning();
		}

		private void ClearBreakpointsStepAndRestore()
		{
			var bps = ClearBreakpointsInternal();
			var step = new Step();
			GDBClient.SendCommandAsync(step);
			RestoreBreakpointsInternal(bps);
		}

		private List<ulong> ClearBreakpointsInternal()
		{
			var bps = BreakPoints.ToList();
			foreach (var bp in bps)
			{
				ClearBreakPoint(bp);
			}
			return bps;
		}

		private void RestoreBreakpointsInternal(IEnumerable<ulong> breakpoints)
		{
			foreach (var bp in breakpoints)
			{
				AddBreakPoint(bp);
			}
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

		private void OnSetBreakpoint(GDBCommand command, bool wasRunning)
		{
			if (wasRunning)
			{
				Continue();
			}
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
