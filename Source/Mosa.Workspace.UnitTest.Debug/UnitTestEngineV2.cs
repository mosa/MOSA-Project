// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
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

namespace Mosa.Workspace.UnitTest.Debug
{
	public class UnitTestEngineV2 : IBuilderEvent, IStarterEvent, IDisposable
	{
		public Options Options { get; }
		public string TestAssemblyPath { get; set; }
		public string Platform { get; set; }
		public string TestSuiteFile { get; set; }
		public AppLocations AppLocations { get; set; }

		public TypeSystem TypeSystem { get; internal set; }
		public BaseLinker Linker { get; internal set; }

		protected DebugServerEngine debugServerEngine;
		protected Starter starter;
		protected Process process;
		protected string imagefile;

		private bool fatalError = false;
		private bool compiled = false;
		private bool processStarted = false;
		private int retries = 0;
		private bool restartVM = false;
		private volatile bool ready = false;

		private const uint MaxRetries = 32;

		private const int DefaultMaxSentQueue = 10000;
		private const int MinSend = 2000;

		private readonly Queue<DebugMessage> queue = new Queue<DebugMessage>();
		private readonly HashSet<DebugMessage> sent = new HashSet<DebugMessage>();

		private readonly int MaxSentQueue = DefaultMaxSentQueue;

		//private int SentQueueCountDown = 0;

		private readonly Thread processThread;
		private volatile bool processThreadAbort = false;

		private int processCount = 0;
		private Stopwatch stopWatch = new Stopwatch();

		public UnitTestEngineV2()
		{
			Options = new Options()
			{
				EnableSSA = true,
				EnableIROptimizations = true,
				EnableSparseConditionalConstantPropagation = true,
				EnableInlinedMethods = true,
				IRLongExpansion = true,
				TwoPassOptimizations = true,

				Emulator = EmulatorType.Qemu,
				ImageFormat = ImageFormat.IMG,
				BootFormat = BootFormat.Multiboot_0_7,
				PlatformType = PlatformType.X86,
				LinkerFormatType = LinkerFormatType.Elf32,
				EmulatorMemoryInMB = 128,
				DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA-UnitTest"),
				FileSystem = FileSystem.FAT16,
				UseMultiThreadingCompiler = false,
				InlinedIRMaximum = 12,
				BootLoader = BootLoader.Syslinux_3_72,
				VBEVideo = false,
				Width = 640,
				Height = 480,
				Depth = 32,
				BaseAddress = 0x00500000,//0x00400000,
				EmitRelocations = false,
				EmitSymbols = false,
				Emitx86IRQMethods = true,
				SerialConnectionOption = SerialConnectionOption.TCPServer,
				SerialConnectionPort = 9999,
				SerialConnectionHost = "127.0.0.1",
				SerialPipeName = "MOSA",
				ExitOnLaunch = true,
				GenerateNASMFile = false,
				GenerateASMFile = true,
				GenerateMapFile = false,
				GenerateDebugFile = false,

				//BootLoaderImage = @"..\Tests\BootImage\Mosa.BootLoader.x86.img"
			};

			AppLocations = new AppLocations();

			AppLocations.FindApplications();

			Initialize();

			processThread = new Thread(ProcessQueue)
			{
				Name = "ProcesQueue"
			};
			processThread.Start();
		}

		public void Initialize()
		{
			if (Platform == null)
				Platform = "x86";

			if (TestAssemblyPath == null)
			{
#if __MonoCS__
				TestAssemblyPath = AppDomain.CurrentDomain.BaseDirectory;
#else
				TestAssemblyPath = AppContext.BaseDirectory;
			}
#endif

			if (TestSuiteFile == null)
			{
				TestSuiteFile = "Mosa.UnitTests." + Platform + ".exe";
			}
		}

		private void ProcessQueue()
		{
			var messages = new List<DebugMessage>();
			var last = DateTime.Now;

			try
			{
				while (!processThreadAbort)
				{
					lock (queue)
					{
						bool sendFlag = (queue.Count > 0 && sent.Count < MaxSentQueue);

						if ((MaxSentQueue - sent.Count < MinSend) && queue.Count > MinSend)
							sendFlag = false;

						// check if queue has requests or too many have already been sent
						while (sendFlag && sent.Count < MaxSentQueue && queue.Count > 0)
						{
							var message = queue.Dequeue();

							PrepareUnitTest();

							message.CallBack = MessageCallBack;

							sent.Add(message);

							messages.Add(message);
						}

						if (messages.Count > 0)
						{
							Console.WriteLine("Batch Sent: " + messages.Count.ToString());
							debugServerEngine.SendCommand2(messages);
							messages.Clear();
						}
					}

					Thread.Sleep(1);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		public void QueueUnitTests(List<UnitTest> unitTests)
		{
			lock (queue)
			{
				foreach (var unitTest in unitTests)
				{
					if (unitTest.Skipped)
						continue;

					var message = new DebugMessage(DebugCode.ExecuteUnitTest, unitTest.SerializedUnitTest, unitTest);

					queue.Enqueue(message);
				}
			}
		}

		public void WaitUntilComplete()
		{
			while (true)
			{
				lock (queue)
				{
					if (queue.Count == 0 && sent.Count == 0)
						return;
				}

				Thread.Sleep(10);
			}
		}

		private void MessageCallBack(DebugMessage response)
		{
			if (response == null)
				return;

			lock (queue)
			{
				processCount++;
				sent.Remove(response);

				//Console.WriteLine(response.ToString());

				if (processCount % 1000 == 0 && stopWatch.Elapsed.Seconds != 0)
				{
					Console.WriteLine("Unit Tests - Count: " + processCount.ToString() + " Elapsed: " + ((int)stopWatch.Elapsed.TotalSeconds).ToString() + " (" + (processCount / stopWatch.Elapsed.TotalSeconds).ToString("F2") + " per second)");
				}
			}

			if (response.Other is UnitTest message)
			{
				//message.ParseResultData(response.ResponseData);

				//Console.WriteLine("RECD: " + message.MethodTypeName + "." + message.MethodName);
			}
		}

		private bool CheckCompiled()
		{
			lock (_lock)
			{
				if (compiled)
					return true;

				if (fatalError)
					return false;

				Compile();

				return compiled;
			}
		}

		public void Compile()
		{
			Options.Paths.Add(TestAssemblyPath);
			Options.SourceFile = Path.Combine(TestAssemblyPath, TestSuiteFile);

			var builder = new Builder(Options, AppLocations, this);

			builder.Compile();

			Linker = builder.Linker;
			TypeSystem = builder.TypeSystem;
			imagefile = Options.BootLoaderImage ?? builder.ImageFile;

			fatalError = builder.HasCompileError;
			compiled = !fatalError;
		}

		public void LaunchVirtualMachine()
		{
			if (starter == null)
			{
				Options.ImageFile = imagefile;
				starter = new Starter(Options, AppLocations, this);
			}

			process = starter.Launch();

			if (process == null)
				fatalError = true;
			else if (process.HasExited)
				fatalError = true;

			processStarted = !fatalError;
			ready = false;
		}

		public void ConnectToDebugEngine()
		{
			if (debugServerEngine == null)
			{
				debugServerEngine = new DebugServerEngine();
				debugServerEngine.SetGlobalDispatch(GlobalDispatch);
			}

			while (!debugServerEngine.IsConnected)
			{
				if (retries > MaxRetries)
				{
					fatalError = true;
					return;
				}

				retries++;
				ready = false;

				try
				{
					Connect();
				}
				catch
				{
					Thread.Sleep(100);
				}
			}
		}

		private void Connect()
		{
			if (!debugServerEngine.IsConnected)
			{
				if (Options.SerialConnectionOption == SerialConnectionOption.TCPServer)
				{
					var client = new TcpClient(Options.SerialConnectionHost, Options.SerialConnectionPort);
					debugServerEngine.Stream = new DebugNetworkStream(client.Client, true);
				}
				else if (Options.SerialConnectionOption == SerialConnectionOption.Pipe)
				{
					var pipeStream = new NamedPipeClientStream(".", Options.SerialPipeName, PipeDirection.InOut);
					pipeStream.Connect();
					debugServerEngine.Stream = pipeStream;
				}
			}
		}

		private void RestartVirtualMachine()
		{
			if (process != null)
			{
				process.Kill();
				process.WaitForExit(5000); // wait for up to 5 seconds

				if (!process.HasExited)
					fatalError = true;
			}

			process = null;
			restartVM = false;
			processStarted = false;
		}

		private void WaitForReady()
		{
			if (ready)
				return;

			//todo --- add timeouts

			while (!ready)
			{
				Thread.Sleep(10);
			}
		}

		private volatile bool wait = false;
		private readonly object _lock = new object();
		private readonly object _lockObject = new object();

		public void SendMessageAndWait(DebugMessage message)
		{
			wait = true;

			message.CallBack = Acknowledge;

			debugServerEngine.SendCommand(message);

			while (wait)
			{
				Thread.Sleep(5);
			}
		}

		public void Acknowledge(DebugMessage message)
		{
			wait = false;
		}

		public bool PrepareUnitTest()
		{
			lock (_lockObject)
			{
				if (fatalError)
					return false;

				if (restartVM)
				{
					RestartVirtualMachine();
				}

				if (fatalError)
					return false;

				if (!processStarted)
				{
					LaunchVirtualMachine();
				}

				if (fatalError)
					return false;

				ConnectToDebugEngine();

				if (fatalError)
					return false;

				if (!ready)
				{
					WaitForReady();
				}

				if (fatalError)
					return false;

				if (!stopWatch.IsRunning)
				{
					stopWatch.Start();
				}

				return true;
			}
		}

		private void GlobalDispatch(DebugMessage response)
		{
			if (response == null)
				return;

			if (response.Code == DebugCode.Ready)
			{
				ready = true;
			}

			//Console.WriteLine(response.ToString());
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
			processThreadAbort = true;

			if (processThread != null)
			{
				if (processThread.IsAlive)
				{
					processThread.Abort();
				}
			}

			if (process == null)
				return;

			if (process.HasExited)
				return;

			process.Kill();

			process = null;
		}
	}
}
