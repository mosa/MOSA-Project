// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Mosa.Utility.RSP;
using Mosa.Utility.RSP.Command;

namespace Mosa.Tool.Debugger.GDB;

public delegate void OnStatusChange();

public delegate void OnLogEvent(string description);

public delegate void OnMemoryRead(ulong address, byte[] bytes);

public class Connector
{
	public TcpClient TcpClient { get; set; }

	public GDBClient GDBClient { get; set; }

	public OnStatusChange OnPause { get; set; }

	public OnStatusChange OnRunning { get; set; }

	public bool IsConnected => GDBClient == null ? false : GDBClient.IsConnected;

	public bool IsRunning => !IsPaused;

	public bool IsPaused { get; set; } = true;

	public string ConnectionHost { get; set; }

	public int ConnectionPort { get; set; }

	public BasePlatform Platform { get; set; }

	protected Dictionary<GDBCommand, OnMemoryRead> OnMemoryReadMap = new Dictionary<GDBCommand, GDB.OnMemoryRead>();

	public Connector(BasePlatform platform)
	{
		Platform = platform;
	}

	public Connector(BasePlatform platform, string host, int port)
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

	public void ClearAllBreakPoints()
	{
		GetReasonHalted();
	}

	public void GetReasonHalted()
	{
		var command = new GetReasonHalted();

		GDBClient.SendCommand(command);
	}

	public void ExtendedMode()
	{
		var command = new ExtendedMode();

		GDBClient.SendCommand(command);
	}

	public void Kill()
	{
		var command = new Kill();

		GDBClient.SendCommand(command);

		Thread.Sleep(100); // give it time to terminate

		Disconnect();
	}

	public void Continue()
	{
		if (IsRunning)
			return;

		IsPaused = false;
		CallOnRunning();

		var command = new Continue(OnStop);
		GDBClient.SendCommand(command);
	}

	public void Step(bool skipOnStep = false)
	{
		if (IsRunning)
			return;

		IsPaused = false;
		CallOnRunning();

		var command = skipOnStep ? new Step(OnStepQuiet) : new Step(OnStep);

		GDBClient.SendCommand(command);
	}

	public void StepN(uint stepCount)
	{
		if (IsRunning)
			return;

		var command = new Step();
		for (uint currentStep = 0; currentStep < stepCount - 1; currentStep++)
		{
			GDBClient.SendCommand(command);
		}

		Step();
	}

	public void AddBreakPoint(ulong address)
	{
		var command = new SetBreakPoint(address, 4, 0);
		GDBClient.SendCommand(command);
	}

	public void ClearBreakPoint(ulong address)
	{
		var command = new ClearBreakPoint(address, 4, 0);
		GDBClient.SendCommand(command);
	}

	public void Break()
	{
		if (!IsRunning)
			return;

		GDBClient.SendBreak();
		IsPaused = true;
	}

	public void GetRegisters()
	{
		var command = new GetRegisters(OnGetRegisters);

		GDBClient.SendCommand(command);
	}

	public void ReadMemory(ulong address, uint size, OnMemoryRead onMemoryRead)
	{
		var command = new ReadMemory(address, size, OnMemoryRead);

		OnMemoryReadMap.Add(command, onMemoryRead);

		GDBClient.SendCommand(command);
	}

	private void OnStop(GDBCommand command)
	{
		GetRegisters();
	}

	private void OnStep(GDBCommand command)
	{
		GetRegisters();
	}

	private void OnStepQuiet(GDBCommand command)
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
		//Debug.WriteLine($"Processing: {command}");

		var bytes = command.GetAllBytes();

		var invoke = OnMemoryReadMap[command];

		OnMemoryReadMap.Remove(command);

		invoke((command as ReadMemory).Address, bytes);
	}
}
