// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.DebugEngine;
using Mosa.Utility.Launcher;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Threading;

namespace Mosa.Utility.UnitTests
{
	public class UnitTestEngine : IDisposable
	{
		public string TestAssemblyPath { get; set; }

		public string TestSuiteFile { get; set; }

		public TypeSystem TypeSystem { get; internal set; }

		public MosaLinker Linker { get; internal set; }

		protected DebugServerEngine DebugServerEngine;
		protected Starter Starter;
		protected Process Process;

		private Settings Settings;

		private readonly object _lock = new object();

		internal volatile bool Aborted = false;
		private volatile bool Ready = false;

		private const int MaxSentQueue = 10000;
		private const int MinSend = 2000;
		private const int MaxErrors = 30;

		private readonly Queue<DebugMessage> Queue = new Queue<DebugMessage>();
		private readonly HashSet<DebugMessage> Pending = new HashSet<DebugMessage>();

		private Thread ProcessThread;

		private int CompletedUnitTestCount = 0;
		private readonly Stopwatch StopWatch = new Stopwatch();

		private volatile int LastResponse = 0;

		private int SendOneCount = -1;
		private int Errors = 0;

		private DateTime CompileStartTime;

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
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
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
			Settings.SetValue("Emulator.Display", true);

			Settings.Merge(settings);

			Settings.SetValue("Compiler.BaseAddress", 0x00500000);
			Settings.SetValue("Compiler.Binary", true);
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Launcher.PlugKorlib", true);
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Serial", "TCPServer");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			Settings.SetValue("Launcher.Start", false);
			Settings.SetValue("Launcher.Launch", false);
			Settings.SetValue("Launcher.Exit", true);
			Settings.SetValue("Launcher.HuntForCorLib", true);
			Settings.SetValue("Image.BootLoader", "syslinux3.72");
			Settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA-UnitTest"));
			Settings.SetValue("Image.Format", "IMG");
			Settings.SetValue("Image.FileSystem", "FAT16");
			Settings.SetValue("Image.ImageFile", "%DEFAULT%");

			Initialize();
		}

		private void Initialize()
		{
			if (TestAssemblyPath == null)
			{
#if __MonoCS__
				TestAssemblyPath = AppDomain.CurrentDomain.BaseDirectory;
#else
				TestAssemblyPath = AppContext.BaseDirectory;
#endif
			}

			if (TestSuiteFile == null)
			{
				var platform = Settings.GetValue("Compiler.Platform", "x86");

				TestSuiteFile = $"Mosa.UnitTests.{platform}.exe";
			}

			Aborted = !Compile();

			if (!Aborted)
			{
				ProcessThread = new Thread(ProcessQueueLaunch)
				{
					Name = "ProcessQueue"
				};

				ProcessThread.Start();
			}
		}

		private void ProcessQueueLaunch()
		{
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
				Console.WriteLine(e.ToString());
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

			StopWatch.Start();
			Aborted = !StartEngine();

			while (!Aborted)
			{
				lock (Queue)
				{
					if (Queue.Count == 0 && Pending.Count == 0)
					{
						LastResponse = (int)StopWatch.ElapsedMilliseconds;
					}

					if (Queue.Count > 0 || Pending.Count > 0)
					{
						CheckEngine();
					}

					bool sendFlag = Queue.Count > 0 && Pending.Count < MaxSentQueue;

					if ((MaxSentQueue - Pending.Count < MinSend) && Queue.Count > MinSend)
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

						//Console.WriteLine("Sent: " + (message.Other as UnitTest).FullMethodName);

						if (SendOneCount >= 0)
						{
							--SendOneCount;
						}
					}

					if (messages.Count > 0)
					{
						//Console.WriteLine("Batch Sent: " + messages.Count.ToString());
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
			CompileStartTime = DateTime.Now;

			Settings.AddPropertyListValue("SearchPaths", TestAssemblyPath);

			Settings.ClearProperty("Compiler.SourceFiles");
			Settings.AddPropertyListValue("Compiler.SourceFiles", Path.Combine(TestAssemblyPath, TestSuiteFile));

			var compilerHook = CreateCompilerHook();

			var builder = new Builder(Settings, compilerHook);

			builder.Build();

			Linker = builder.Linker;
			TypeSystem = builder.TypeSystem;
			Settings = builder.Settings;

			return !builder.HasCompileError;
		}

		private CompilerHooks CreateCompilerHook()
		{
			var compilerHooks = new CompilerHooks
			{
				NotifyEvent = NotifyEvent,
				NotifyProgress = NotifyProgress
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
				message = string.IsNullOrWhiteSpace(message) ? string.Empty : $": {message}";
				Console.WriteLine($"{(DateTime.Now - CompileStartTime).TotalSeconds:0.00} [{threadID.ToString()}] {compilerEvent.ToText()}{message}");
			}
		}

		private void NotifyProgress(int totalMethods, int completedMethods)
		{
		}

		public bool LaunchVirtualMachine()
		{
			if (Starter == null)
			{
				var compilerHook = CreateCompilerHook();

				Starter = new Starter(Settings, compilerHook);
			}

			//Settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));

			Process = Starter.Launch();

			return Process != null || !Process.HasExited;
		}

		public bool ConnectToDebugEngine()
		{
			if (DebugServerEngine == null)
			{
				DebugServerEngine = new DebugServerEngine();
				DebugServerEngine.SetGlobalDispatch(GlobalDispatch);
			}

			for (int attempt = 0; attempt < 100; attempt++)
			{
				Thread.Sleep(100);

				if (DebugServerEngine.IsConnected)
					return true;

				try
				{
					Connect();
				}
				catch
				{
				}
			}

			Console.Write("Unable to connect to DebugEngine");

			return false;
		}

		private void Connect()
		{
			var serial = Settings.GetValue("Emulator.Serial", string.Empty).ToLower();

			switch (serial)
			{
				case "tcpserver":
					{
						var client = new TcpClient(Settings.GetValue("Emulator.Serial.Host", "localhost"), Settings.GetValue("Emulator.Serial.Port", 1234));
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

			if (Process.HasExited)
				return;

			Process.Kill();
			Process.WaitForExit(5000); // wait for up to 5000 milliseconds
			Process = null;
		}

		private bool WaitForReady()
		{
			for (int attempt = 0; attempt < 100; attempt++)
			{
				if (Ready)
				{
					LastResponse = (int)StopWatch.ElapsedMilliseconds;
					return true;
				}

				Thread.Sleep(100);
			}

			return false;
		}

		public bool StartEngine()
		{
			lock (_lock)
			{
				for (int attempt = 0; attempt < 10; attempt++)
				{
					Console.Write("Starting Engine...");

					if (StartEngineEx())
					{
						Console.WriteLine();
						return true;
					}
					else
					{
						KillVirtualMachine();
						Console.WriteLine("Failed");
					}

					Thread.Sleep(250);
				}

				return false;
			}
		}

		private bool StartEngineEx()
		{
			Ready = false;

			if (!LaunchVirtualMachine())
				return false;

			if (!ConnectToDebugEngine())
				return false;

			if (!WaitForReady())
				return false;

			return true;
		}

		private void CheckEngine()
		{
			bool restart = false;

			lock (_lock)
			{
				// Has the process not started? If yes, start
				if (Process == null)
				{
					restart = true;
				}

				// Has VM exited? If yes, restart
				else if (Process.HasExited)
				{
					restart = true;
				}

				// Have communications been terminated? If yes, restart
				else if (!DebugServerEngine.IsConnected)
				{
					KillVirtualMachine();
					restart = true;
				}

				// Has process stop responding (more than 2 seconds)? If yes, restart
				else if (LastResponse > 0 && StopWatch.ElapsedMilliseconds - LastResponse > 2000)
				{
					KillVirtualMachine();
					restart = true;
				}

				if (restart)
				{
					if (Pending.Count == 1)
					{
						foreach (var failed in Pending)
						{
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

					Console.WriteLine("Re-starting Engine...");

					//Settings.SetValue("Emulator.Serial.Port", Settings.GetValue("Emulator.Serial.Port", 1234) + 1);

					StartEngine();
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

			//Console.WriteLine(response.ToString());
		}

		private void MessageCallBack(DebugMessage response)
		{
			if (response == null)
				return;

			lock (Queue)
			{
				LastResponse = (int)StopWatch.ElapsedMilliseconds;

				CompletedUnitTestCount++;
				Pending.Remove(response);

				//Console.WriteLine("Received: " + (response.Other as UnitTest).FullMethodName);
				//Console.WriteLine(response.ToString());

				if (CompletedUnitTestCount % 1000 == 0 && StopWatch.Elapsed.Seconds != 0)
				{
					Console.WriteLine("Unit Tests - Count: " + CompletedUnitTestCount.ToString() + " Elapsed: " + ((int)StopWatch.Elapsed.TotalSeconds).ToString() + " (" + (CompletedUnitTestCount / StopWatch.Elapsed.TotalSeconds).ToString("F2") + " per second)");
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
					Errors++;

					Console.WriteLine("ERROR: " + UnitTestSystem.OutputUnitTestResult(unitTest));

					if (Errors == MaxErrors)
						Aborted = true;
				}

				//Console.WriteLine("RECD: " + unitTest.MethodTypeName + "." + unitTest.MethodName);
			}
		}

		void IDisposable.Dispose()
		{
			Aborted = true;

			if (Process?.HasExited == false)
			{
				Process.Kill();
				Process = null;
			}

			if (ProcessThread?.IsAlive == true)
			{
				ProcessThread.Join();
			}
		}
	}
}
