// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.BootImage;
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
	public class UnitTestEngine : IBuilderEvent, IStarterEvent, IDisposable
	{
		public LauncherOptions LauncherOptions { get; }
		public string TestAssemblyPath { get; set; }
		public string Platform { get; set; }
		public string TestSuiteFile { get; set; }
		public AppLocations AppLocations { get; set; }

		public TypeSystem TypeSystem { get; internal set; }
		public MosaLinker Linker { get; internal set; }

		protected DebugServerEngine DebugServerEngine;
		protected Starter Starter;
		protected Process Process;
		protected string ImageFile;

		private readonly object _lock = new object();

		private volatile bool Aborted = false;
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

		public UnitTestEngine(bool display = false)
		{
			LauncherOptions = new LauncherOptions()
			{
				EnableSSA = true,
				EnableIROptimizations = true,
				EnableSparseConditionalConstantPropagation = true,
				EnableInlinedMethods = true,
				EnableIRLongExpansion = true,
				EnableValueNumbering = true,
				TwoPassOptimizations = true,
				EnableMethodScanner = false,
				EnableBitTracker = true,
				EnableMultiThreading = false,

				Emulator = EmulatorType.Qemu,
				ImageFormat = ImageFormat.IMG,
				MultibootSpecification = Compiler.Framework.MultibootSpecification.V1,
				PlatformType = PlatformType.x86,
				LinkerFormatType = LinkerFormatType.Elf32,
				EmulatorMemoryInMB = 128,
				DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA-UnitTest"),
				FileSystem = FileSystem.FAT16,
				InlinedIRMaximum = 12,
				BootLoader = BootLoader.Syslinux_3_72,
				VBEVideo = false,
				Width = 640,
				Height = 480,
				Depth = 32,
				BaseAddress = 0x00500000,
				EmitStaticRelocations = false,
				EmitAllSymbols = false,
				SerialConnectionOption = SerialConnectionOption.TCPServer,
				SerialConnectionPort = 9999,
				SerialConnectionHost = "127.0.0.1",
				SerialPipeName = "MOSA",
				ExitOnLaunch = true,
				GenerateNASMFile = false,
				GenerateASMFile = true,
				GenerateMapFile = true,
				GenerateDebugFile = false,
				PlugKorlib = true,
				NoDisplay = !display
			};

			AppLocations = new AppLocations();

			AppLocations.FindApplications();

			Initialize();
		}

		private void Initialize()
		{
			if (Platform == null)
			{
				Platform = "x86";
			}

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
				TestSuiteFile = "Mosa.UnitTests." + Platform + ".exe";
			}

			Aborted = !Compile();

			if (!Aborted)
			{
				ProcessThread = new Thread(ProcessQueueLaunch)
				{
					Name = "ProcesQueue"
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
			LauncherOptions.Paths.Add(TestAssemblyPath);
			LauncherOptions.SourceFile = Path.Combine(TestAssemblyPath, TestSuiteFile);

			var builder = new Builder(LauncherOptions, AppLocations, this);

			builder.Compile();

			Linker = builder.Linker;
			TypeSystem = builder.TypeSystem;
			ImageFile = LauncherOptions.BootLoaderImage ?? builder.ImageFile;

			return !builder.HasCompileError;
		}

		public bool LaunchVirtualMachine()
		{
			if (Starter == null)
			{
				LauncherOptions.ImageFile = ImageFile;
				Starter = new Starter(LauncherOptions, AppLocations, this);
			}

			LauncherOptions.SerialConnectionPort++;

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
			if (LauncherOptions.SerialConnectionOption == SerialConnectionOption.TCPServer)
			{
				var client = new TcpClient(LauncherOptions.SerialConnectionHost, LauncherOptions.SerialConnectionPort);
				DebugServerEngine.Stream = new DebugNetworkStream(client.Client, true);
			}
			else if (LauncherOptions.SerialConnectionOption == SerialConnectionOption.Pipe)
			{
				var pipeStream = new NamedPipeClientStream(".", LauncherOptions.SerialPipeName, PipeDirection.InOut);
				pipeStream.Connect();
				DebugServerEngine.Stream = pipeStream;
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

		void IBuilderEvent.NewStatus(string status)
		{
			//Console.WriteLine(status);
		}

		void IBuilderEvent.UpdateProgress(int total, int at)
		{
		}

		void IStarterEvent.NewStatus(string status)
		{
			//Console.WriteLine(status);
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
				ProcessThread.Abort();
				ProcessThread.Join();
			}
		}
	}
}
