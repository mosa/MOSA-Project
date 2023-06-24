// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Threading;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.DebugEngine;
using Mosa.Utility.Launcher;

namespace Mosa.Utility.UnitTests;

public class UnitTestEngine : IDisposable
{
	#region Constants

	protected static class Constant
	{
		public const int MaxErrors = 1000;
		public const int ConnectionTimeOut = 8000; // in milliseconds
		public const int TimeOut = 750; // in milliseconds
		public const int MaxAttempts = 20;
		public const int Port = 11110;
	}

	#endregion Constants

	#region Public Methods

	public bool IsAborted => Aborted;

	public string TestAssemblyPath { get; set; }

	public string TestSuiteFile { get; set; }

	public TypeSystem TypeSystem { get; internal set; }

	public MosaLinker Linker { get; internal set; }

	#endregion Public Methods

	#region Constants

	private const int MaxSentQueue = 10000;
	private const int MinSend = 2000;

	private const int ConnectionDelay = 150;

	#endregion Constants

	#region Private Data Members

	protected DebugServerEngine DebugServerEngine;
	protected Starter Starter;
	protected Process Process;

	private Settings Settings;

	private readonly object _lock = new object();

	private volatile bool Aborted;
	private volatile bool Ready;

	private int MaxErrors = Constant.MaxErrors;
	private int ConnectionTimeOut = Constant.ConnectionTimeOut; // in milliseconds
	private int MaxAttempts;
	private int TimeOut = Constant.TimeOut; // in milliseconds

	private readonly Queue<DebugMessage> Queue = new Queue<DebugMessage>();
	private readonly HashSet<DebugMessage> Pending = new HashSet<DebugMessage>();

	private Thread ProcessThread;

	private readonly Stopwatch Stopwatch = new Stopwatch();
	private WatchDog WatchDog;

	private int CompletedUnitTestCount;

	private int SendOneCount = -1;
	private int Errors;

	#endregion Private Data Members

	public UnitTestEngine(Settings settings)
	{
		Settings = AppLocationsSettings.GetAppLocations();

		Settings.SetValue("Compiler.MethodScanner", false);
		Settings.SetValue("Compiler.Multithreading", true);
		Settings.SetValue("Compiler.Platform", "x86");
		Settings.SetValue("CompilerDebug.DebugFile", "%DEFAULT%");
		Settings.SetValue("CompilerDebug.AsmFile", "%DEFAULT%");
		Settings.SetValue("CompilerDebug.MapFile", "%DEFAULT%");
		Settings.SetValue("CompilerDebug.InlinedFile", "%DEFAULT%");
		Settings.SetValue("CompilerDebug.NasmFile", string.Empty);

		Settings.SetValue("Optimizations.Basic", true);
		Settings.SetValue("Optimizations.BitTracker", true);
		Settings.SetValue("Optimizations.Inline", true);
		Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
		Settings.SetValue("Optimizations.Inline.Explicit", true);
		Settings.SetValue("Optimizations.Inline.Maximum", 12);
		Settings.SetValue("Optimizations.Basic.Window", 5);
		Settings.SetValue("Optimizations.LongExpansion", true);
		Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
		Settings.SetValue("Optimizations.Platform", true);
		Settings.SetValue("Optimizations.SCCP", true);
		Settings.SetValue("Optimizations.Devirtualization", true);
		Settings.SetValue("Optimizations.SSA", true);
		Settings.SetValue("Optimizations.TwoPass", true);
		Settings.SetValue("Optimizations.ValueNumbering", true);

		Settings.SetValue("Multiboot.Video", false);
		Settings.SetValue("Multiboot.Video.Width", 640);
		Settings.SetValue("Multiboot.Video.Height", 480);
		Settings.SetValue("Multiboot.Video.Depth", 32);
		Settings.SetValue("Emulator.Display", false);
		Settings.SetValue("Emulator.Serial", "TCPServer");
		Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
		Settings.SetValue("Emulator.Serial.Port", Constant.Port);
		Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
		Settings.SetValue("Multiboot.Version", "v1");
		Settings.SetValue("Image.Firmware", "bios");
		Settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA-UnitTest"));
		Settings.SetValue("Image.Format", "IMG");
		Settings.SetValue("Image.FileSystem", "FAT16");
		Settings.SetValue("Image.ImageFile", "%DEFAULT%");
		Settings.SetValue("UnitTest.MaxErrors", Constant.MaxErrors);
		Settings.SetValue("UnitTest.TimeOut", Constant.TimeOut);
		Settings.SetValue("UnitTest.Connection.TimeOut", Constant.ConnectionTimeOut);
		Settings.SetValue("UnitTest.Connection.MaxAttempts", Constant.MaxAttempts);
		Settings.SetValue("OS.Name", "MOSA");

		Settings.Merge(settings);

		Settings.SetValue("Compiler.BaseAddress", 0x00500000);
		Settings.SetValue("Compiler.Binary", true);
		Settings.SetValue("Compiler.TraceLevel", 0);
		Settings.SetValue("Launcher.PlugKorlib", true);
		Settings.SetValue("Emulator", "Qemu");
		Settings.SetValue("Emulator.Memory", 128);
		Settings.SetValue("Emulator.Cores", 1);
		Settings.SetValue("Launcher.Start", false);
		Settings.SetValue("Launcher.Launch", false);
		Settings.SetValue("Launcher.Exit", true);

		Initialize();
	}

	private void Initialize()
	{
		MaxErrors = Settings.GetValue("UnitTest.MaxErrors", Constant.MaxErrors);
		TimeOut = Settings.GetValue("UnitTest.TimeOut", Constant.TimeOut);
		ConnectionTimeOut = Settings.GetValue("UnitTest.Connection.TimeOut", Constant.ConnectionTimeOut);
		MaxAttempts = Settings.GetValue("nitTest.Connection.MaxAttempts", Constant.MaxAttempts);

		if (TestAssemblyPath == null)
		{
			TestAssemblyPath = AppContext.BaseDirectory;
		}

		if (TestSuiteFile == null)
		{
			var platform = Settings.GetValue("Compiler.Platform", "x86");

			TestSuiteFile = $"Mosa.UnitTests.{platform}.dll";
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
		WatchDog = new WatchDog(TimeOut);

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

		var messages = new List<DebugMessage>();

		Stopwatch.Start();
		Aborted = !StartEngine();

		while (!Aborted)
		{
			lock (Queue)
			{
				if (Queue.Count == 0 && Pending.Count == 0)
				{
					WatchDog.Restart();
				}

				if (Queue.Count > 0 || Pending.Count > 0)
				{
					CheckEngine();
				}

				if (Aborted)
					return;

				var sendFlag = Queue.Count > 0 && Pending.Count < MaxSentQueue;

				if (MaxSentQueue - Pending.Count < MinSend && Queue.Count > MinSend)
				{
					sendFlag = false;
				}

				// check if queue has requests or too many have already been sent
				while ((SendOneCount < 0 && sendFlag && Pending.Count < MaxSentQueue && Queue.Count > 0)
					   || (SendOneCount >= 0 && Queue.Count > 0 && Pending.Count == 0))
				{
					var message = Queue.Dequeue();

					message.CallBack = MessageCallBack;

					Pending.Add(message);

					messages.Add(message);

					//OutputStatus("Sent: " + (message.Other as UnitTest).FullMethodName);

					if (SendOneCount >= 0)
					{
						--SendOneCount;
					}
				}

				if (messages.Count > 0)
				{
					//OutputStatus("Batch Sent: " + messages.Count.ToString());
					DebugServerEngine.SendCommand2(messages);
					messages.Clear();
				}
			}

			Thread.Sleep(10);
		}
	}

	public void QueueUnitTests(List<UnitTest> unitTests)
	{
		lock (Queue)
		{
			foreach (var unitTest in unitTests)
			{
				if (unitTest.Status == UnitTestStatus.Skipped)
					continue;

				var message = new DebugMessage(DebugCode.ExecuteUnitTest, unitTest.SerializedUnitTest, unitTest);

				Queue.Enqueue(message);
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
				if (Queue.Count == 0 && Pending.Count == 0)
					return;
			}

			Thread.Sleep(10);
		}
	}

	public bool Compile()
	{
		Stopwatch.Restart();

		Settings.AddPropertyListValue("SearchPaths", TestAssemblyPath);

		Settings.ClearProperty("Compiler.SourceFiles");
		Settings.AddPropertyListValue("Compiler.SourceFiles", Path.Combine(TestAssemblyPath, TestSuiteFile));

		var compilerHook = CreateCompilerHook();

		var builder = new Builder(Settings, compilerHook);

		builder.Build();

		Linker = builder.Linker;
		TypeSystem = builder.TypeSystem;
		Settings = builder.Settings;

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
		Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:0.00} | {status}");
	}

	public bool LaunchVirtualMachine()
	{
		Process = null;

		if (Starter == null)
		{
			var compilerHook = CreateCompilerHook();

			Starter = new Starter(Settings, compilerHook);
			Settings = Starter.Settings;
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
			DebugServerEngine.SetGlobalDispatch(GlobalDispatch);
		}

		Thread.Sleep(50); // small delay to let emulator launch

		var watchdog = new WatchDog(ConnectionTimeOut);

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

			Thread.Sleep(10);
		}

		return false;
	}

	private void Connect()
	{
		DebugServerEngine.Stream = null;

		var serial = Settings.GetValue("Emulator.Serial", string.Empty).ToLowerInvariant();

		switch (serial)
		{
			case "tcpserver":
				{
					var client = new TcpClient(Settings.GetValue("Emulator.Serial.Host", "localhost"), Settings.GetValue("Emulator.Serial.Port", 11110));
					DebugServerEngine.Stream = new DebugNetworkStream(client.Client, true);
					break;
				}

			case "pipe":
				{
					var pipeStream = new NamedPipeClientStream(".", Settings.GetValue("Emulator.Serial.Pipe", "MOSA"), PipeDirection.InOut);
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
			Process.WaitForExit(10000); // wait for up to 10 seconds
			Thread.Sleep(250);
		}

		Process = null;
	}

	private bool WaitForReady()
	{
		var watchdog = new WatchDog(ConnectionTimeOut);

		while (!watchdog.IsTimedOut)
		{
			if (Ready)
			{
				WatchDog.Restart();
				return true;
			}

			Thread.Sleep(10);
		}

		return false;
	}

	public bool StartEngine()
	{
		lock (_lock)
		{
			for (var attempt = 0; attempt < MaxAttempts; attempt++)
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
				KillVirtualMachine();

				if (Pending.Count == 1)
				{
					foreach (var failed in Pending)
					{
						// OutputStatus("Failed - Requeueing: " + (failed.Other as UnitTest).FullMethodName);

						(failed.Other as UnitTest).Status = UnitTestStatus.FailedByCrash;
					}
					Pending.Clear();
				}

				foreach (var test in Pending)
				{
					Queue.Enqueue(test);
				}

				Pending.Clear();
				SendOneCount = 10;

				OutputStatus("Re-starting engine...");

				if (!StartEngine())
				{
					KillVirtualMachine();
					Aborted = true;
				}
			}
		}
	}

	private void GlobalDispatch(DebugMessage response)
	{
		if (response == null)
			return;

		if (response.Code == DebugCode.Ready)
		{
			Ready = true;
		}
	}

	private void MessageCallBack(DebugMessage response)
	{
		if (response == null)
			return;

		lock (Queue)
		{
			WatchDog.Restart();

			CompletedUnitTestCount++;
			Pending.Remove(response);

			//OutputStatus("Received: " + (response.Other as UnitTest).FullMethodName);
			//OutputStatus(response.ToString());

			if (CompletedUnitTestCount % 1000 == 0 && Stopwatch.Elapsed.Seconds != 0)
			{
				OutputStatus($"Unit Tests - Count: {CompletedUnitTestCount} Elapsed: {(int)Stopwatch.Elapsed.TotalSeconds} ({(CompletedUnitTestCount / Stopwatch.Elapsed.TotalSeconds).ToString("F2")} per second)");
			}
		}

		if (response.Other is UnitTest unitTest)
		{
			UnitTestSystem.ParseResultData(unitTest, response.ResponseData);

			if (Equals(unitTest.Expected, unitTest.Result))
			{
				unitTest.Status = UnitTestStatus.Passed;
			}
			else
			{
				unitTest.Status = UnitTestStatus.Failed;
				var value = Equals(unitTest.Expected, unitTest.Result);
				Errors++;

				OutputStatus($"ERROR: {UnitTestSystem.OutputUnitTestResult(unitTest)}");

				if (Errors >= MaxErrors)
				{
					Aborted = true;
				}
			}

			//OutputStatus("RECD: " + unitTest.MethodTypeName + "." + unitTest.MethodName);
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
