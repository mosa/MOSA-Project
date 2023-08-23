// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Threading;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using Mosa.Utility.UnitTests.DebugEngine;

namespace Mosa.Utility.UnitTests;

public class UnitTestEngine : IDisposable
{
	#region Public Methods

	public bool IsAborted => Aborted;

	public TypeSystem TypeSystem { get; internal set; }

	public MosaLinker Linker { get; internal set; }

	#endregion Public Methods

	#region Constants

	private const int MaxSentQueue = 10000;
	private const int MaxSendBatch = 1000;
	private const int MinSendBatch = 200;
	private const int ConnectionDelay = 150;

	#endregion Constants

	#region Private Data Members

	protected DebugServerEngine DebugServerEngine;
	protected Starter Starter;
	protected Process Process;

	private MosaSettings MosaSettings = new MosaSettings();

	private readonly object _lock = new object();

	private volatile bool Aborted;
	private volatile bool Ready;
	private volatile int Errors;
	private volatile int SendOneCount = 0;

	private readonly Queue<UnitTest> Queue = new Queue<UnitTest>();
	private readonly Dictionary<int, UnitTest> Active = new Dictionary<int, UnitTest>();

	private Thread ProcessThread;

	private readonly Stopwatch Stopwatch = new Stopwatch();
	private WatchDog WatchDog;

	private int CompletedUnitTestCount;

	#endregion Private Data Members

	public UnitTestEngine(MosaSettings mosaSettings)
	{
		MosaSettings.LoadAppLocations();
		MosaSettings.SetDetfaultSettings();
		MosaSettings.Merge(mosaSettings);
		SetRequiredSettings();
		MosaSettings.NormalizeSettings();
		MosaSettings.ExpandSearchPaths();

		Initialize();
	}

	private void SetRequiredSettings()
	{
		MosaSettings.BaseAddress = 0x00500000;
		MosaSettings.EmitBinary = true;
		MosaSettings.PlugKorlib = true;
		MosaSettings.Emulator = "qemu";
		MosaSettings.EmulatorMemory = 128;
		MosaSettings.EmulatorCores = 1;
		MosaSettings.Launcher = true;
		MosaSettings.LauncherStart = false;
		MosaSettings.LauncherExit = true;
		MosaSettings.TraceLevel = 0;
	}

	private void Initialize()
	{
		if (MosaSettings.SourceFiles == null || MosaSettings.SourceFiles.Count == 0)
		{
			MosaSettings.AddSourceFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Mosa.UnitTests.BareMetal.{MosaSettings.Platform}.dll"));
		}

		Aborted = !Compile();

		if (Aborted)
			return;

		ProcessThread = new Thread(ProcessQueueLaunch)
		{
			Name = "ProcessQueue"
		};

		ProcessThread.Start();
	}

	private void ProcessQueueLaunch()
	{
		WatchDog = new WatchDog(MosaSettings.TimeOut);

		try
		{
			ProcessQueue();
		}
		catch (ThreadAbortException)
		{
			return;
		}
		catch (Exception e)
		{
			OutputStatus(e.ToString());
		}
	}

	public void Terminate()
	{
		(this as IDisposable).Dispose();
	}

	private void ProcessQueue()
	{
		if (Aborted)
			return;

		Stopwatch.Start();
		Aborted = !StartEngine();

		while (!Aborted)
		{
			lock (Queue)
			{
				if (Queue.Count == 0 && Active.Count == 0)
				{
					WatchDog.Restart();
				}

				if (Queue.Count > 0 || Active.Count > 0)
				{
					CheckEngine();
				}

				if (Aborted)
					return;

				var count = MaxSendBatch;

				count = Math.Min(count, Queue.Count);
				count = Math.Min(count, MaxSentQueue - Active.Count);

				if (count < MinSendBatch & SendOneCount == 0 & Queue.Count > MinSendBatch)
				{
					count = 0;
				}

				if (count > 0 && SendOneCount > 0)
				{
					count = Active.Count == 0 ? 1 : 0;
				}

				Send(count);

				if (count == 1 && SendOneCount > 0)
					SendOneCount--;
			}

			Thread.Sleep(100);
		}
	}

	private void Send(int count)
	{
		if (count <= 0)
			return;

		//OutputStatus($"Batch: {count}");

		var messages = new List<UnitTest>(count);

		for (int i = 0; i < count; ++i)
		{
			var message = Queue.Dequeue();

			Active.Add(message.UnitTestID, message);

			messages.Add(message);
		}

		DebugServerEngine.Send(messages);
	}

	public void QueueUnitTests(List<UnitTest> unitTests)
	{
		lock (Queue)
		{
			foreach (var unitTest in unitTests)
			{
				if (unitTest.Status == UnitTestStatus.Skipped)
					continue;

				Queue.Enqueue(unitTest);
			}
		}
	}

	public void WaitUntilComplete()
	{
		while (true)
		{
			if (Aborted)
				return;

			lock (Queue)
			{
				if (Queue.Count == 0 && Active.Count == 0)
					return;
			}

			Thread.Yield();
		}
	}

	public bool Compile()
	{
		Stopwatch.Restart();

		//MosaSettings.AddSearchPath(TestAssemblyPath);
		//MosaSettings.ClearSourceFiles();
		//MosaSettings.AddSourceFile(Path.Combine(TestAssemblyPath, TestSuiteFile));

		var compilerHook = CreateCompilerHook();

		var builder = new Builder(MosaSettings, compilerHook);

		builder.Build();

		Linker = builder.Linker;
		TypeSystem = builder.TypeSystem;

		MosaSettings = builder.MosaSettings; // Switch to builder settings

		return builder.IsSucccessful;
	}

	private CompilerHooks CreateCompilerHook()
	{
		var compilerHooks = new CompilerHooks
		{
			NotifyEvent = NotifyEvent,
			NotifyProgress = NotifyProgress,
			NotifyStatus = NotifyStatus,
		};

		return compilerHooks;
	}

	private void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
	{
		if (compilerEvent != CompilerEvent.MethodCompileEnd
			&& compilerEvent != CompilerEvent.MethodCompileStart
			&& compilerEvent != CompilerEvent.Counter
			&& compilerEvent != CompilerEvent.SetupStageStart
			&& compilerEvent != CompilerEvent.SetupStageEnd
			&& compilerEvent != CompilerEvent.FinalizationStageStart
			&& compilerEvent != CompilerEvent.FinalizationStageEnd
		   )
		{
			var eventname = compilerEvent.ToText();
			message = string.IsNullOrWhiteSpace(message) ? eventname : $"{eventname}: {message}";

			OutputStatus(message);
		}
	}

	private void NotifyProgress(int totalMethods, int completedMethods)
	{
	}

	private void NotifyStatus(string status)
	{
		OutputStatus($"[{status}]");
	}

	private void OutputStatus(string status)
	{
		Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}

	public bool LaunchVirtualMachine()
	{
		Process = null;

		if (Starter == null)
		{
			var compilerHook = CreateCompilerHook();

			Starter = new Starter(MosaSettings, compilerHook);

			MosaSettings = Starter.MosaSettings; // Switch to starter settings
		}

		if (!Starter.Launch())
			return false;

		if (Starter.Process == null)
			return false;

		Process = Starter.Process;

		if (Starter.Process.HasExited)
			return false;

		return true;
	}

	public bool ConnectToDebugEngine()
	{
		if (DebugServerEngine == null)
		{
			DebugServerEngine = new DebugServerEngine();
			DebugServerEngine.SetDispatch(MessageCallBack);
		}

		Thread.Sleep(50); // small delay to let emulator launch

		var watchdog = new WatchDog(MosaSettings.ConnectionTimeOut);

		while (!watchdog.IsTimedOut)
		{
			try
			{
				Connect();

				if (DebugServerEngine.IsConnected)
				{
					return true;
				}
			}
			catch
			{
			}

			Thread.Yield();
		}

		return false;
	}

	private void Connect()
	{
		DebugServerEngine.Stream = null;

		switch (MosaSettings.EmulatorSerial)
		{
			case "tcpserver":
				{
					var client = new TcpClient(MosaSettings.EmulatorSerialHost, MosaSettings.EmulatorSerialPort);
					DebugServerEngine.Stream = new DebugNetworkStream(client.Client, true);
					break;
				}

			case "pipe":
				{
					var pipeStream = new NamedPipeClientStream(".", MosaSettings.EmulatorSerialPipe, PipeDirection.InOut);
					pipeStream.Connect();
					DebugServerEngine.Stream = pipeStream;
					break;
				}
		}
	}

	private void KillVirtualMachine()
	{
		if (Process == null)
			return;

		DebugServerEngine.Disconnect();

		if (!Process.HasExited)
		{
			Process.Kill();
			Process.WaitForExit(8000); // wait for up to 10 seconds
			Thread.Sleep(250);
		}

		Process = null;
	}

	private bool WaitForReady()
	{
		var watchdog = new WatchDog(MosaSettings.ConnectionTimeOut);

		while (!watchdog.IsTimedOut)
		{
			if (Ready)
			{
				WatchDog.Restart();
				return true;
			}

			Thread.Yield();
		}

		return false;
	}

	public bool StartEngine()
	{
		lock (_lock)
		{
			for (var attempt = 0; attempt < MosaSettings.MaxAttempts; attempt++)
			{
				OutputStatus("Starting Engine...");

				if (StartEngineEx())
				{
					//OutputStatus($"Engine started!");
					return true;
				}
				else
				{
					OutputStatus("ERROR: Engine start failed!");
					KillVirtualMachine();
				}

				Thread.Sleep(ConnectionDelay);
			}

			return false;
		}
	}

	private bool StartEngineEx()
	{
		Ready = false;

		if (LaunchVirtualMachine())
		{
			OutputStatus("Virtual machine launched");
		}
		else
		{
			OutputStatus("ERROR: Unable to launch virtual machine");
			return false;
		}

		if (ConnectToDebugEngine())
		{
			OutputStatus($"Engine connected!");
		}
		else
		{
			OutputStatus("ERROR: Unable to connect");
			return false;
		}

		if (WaitForReady())
		{
			OutputStatus($"Engine ready!");
			WatchDog.Restart();
		}
		else
		{
			OutputStatus("ERROR: No ready signal received");
			return false;
		}

		return true;
	}

	private void CheckEngine()
	{
		var restart = false;

		lock (_lock)
		{
			if (Aborted)
				return;

			// Has the process not started? If yes, start
			if (Process == null)
			{
				OutputStatus("ERROR: Proces not started");
				restart = true;
			}

			// Has VM exited? If yes, restart
			else if (Process.HasExited)
			{
				OutputStatus("ERROR: Virtual machine process terminated");
				restart = true;
			}

			// Have communications been terminated? If yes, restart
			else if (!DebugServerEngine.IsConnected)
			{
				OutputStatus("ERROR: Connection lost");
				restart = true;
			}

			// Has process stop responding more than X milliseconds; if yes, restart
			else if (WatchDog.IsTimedOut)
			{
				OutputStatus("ERROR: Timed out");
				restart = true;
			}

			if (restart)
			{
				IncrementErrorCount();

				KillVirtualMachine();

				SendOneCount = 5;

				if (Active.Count == 1)
				{
					foreach (var entry in Active)
					{
						entry.Value.Status = UnitTestStatus.FailedByCrash;
						OutputStatus($"ERROR: {UnitTestSystem.OutputUnitTestResult(entry.Value)}");
					}
				}
				else
				{
					foreach (var test in Active)
					{
						Queue.Enqueue(test.Value);
					}
				}

				Active.Clear();

				if (!Aborted)
				{
					OutputStatus("Re-starting engine...");

					MosaSettings.EmulatorSerialPort++;

					if (!StartEngine())
					{
						KillVirtualMachine();
						Aborted = true;
					}
				}
			}
		}

		return;
	}

	private void MessageCallBack(int id, ulong data)
	{
		if (id == 0)
		{
			Ready = true;
			return;
		}

		lock (Queue)
		{
			WatchDog.Restart();

			var unittest = Active[id];
			unittest.SerializedResult = data;

			Active.Remove(id);

			CompletedUnitTestCount++;

			if (CompletedUnitTestCount % 1000 == 0 && Stopwatch.Elapsed.Seconds != 0)
			{
				OutputStatus($"Unit Tests - Count: {CompletedUnitTestCount} Elapsed: {(int)Stopwatch.Elapsed.TotalSeconds} ({(CompletedUnitTestCount / Stopwatch.Elapsed.TotalSeconds).ToString("F2")} per second)");
			}

			UnitTestSystem.ParseResultData(unittest, data);

			if (Equals(unittest.Expected, unittest.Result))
			{
				unittest.Status = UnitTestStatus.Passed;
			}
			else
			{
				unittest.Status = UnitTestStatus.Failed;

				OutputStatus($"ERROR: {UnitTestSystem.OutputUnitTestResult(unittest)}");

				IncrementErrorCount();
			}
		}
	}

	private void IncrementErrorCount()
	{
		Errors++;

		if (Errors >= MosaSettings.MaxErrors)
		{
			Aborted = true;
		}
	}

	void IDisposable.Dispose()
	{
		if (Process?.HasExited == false)
		{
			Process.Kill();
			Process = null;
		}
	}
}
