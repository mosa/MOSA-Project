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

namespace Mosa.UnitTest.Engine
{
	public class UnitTestEngine : IBuilderEvent, IStarterEvent, IDisposable
	{
		public Options Options { get; private set; }
		public string TestAssemblyPath { get; set; }
		public string Platform { get; set; }
		public string TestSuiteFile { get; set; }
		public AppLocations AppLocations { get; set; }

		protected TypeSystem typeSystem;
		protected BaseLinker linker;
		protected string imagefile;
		protected Starter starter;
		protected Process process;
		protected DebugServerEngine debugServerEngine;

		private bool fatalError = false;
		private bool compiled = false;
		private bool processStarted = false;
		private int retries = 0;
		private bool restartVM = false;
		private volatile bool ready = false;

		private Stopwatch stopwatch = new Stopwatch();

		//private volatile List<byte> resultData = null;

		private const uint MaxRetries = 10;
		private const uint RetryDelay = 1; // 1- seconds

		private const int DefaultMaxSentQueue = 100;

		private Queue<UnitTestRequest> queue = new Queue<UnitTestRequest>();
		private HashSet<UnitTestRequest> sent = new HashSet<UnitTestRequest>();

		private int MaxSentQueue = DefaultMaxSentQueue;

		//private int SentQueueCountDown = 0;

		private Thread processThread;
		private volatile bool processThreadAbort = false;

		public UnitTestEngine()
		{
			Options = new Options()
			{
				EnableSSA = true,
				EnableIROptimizations = true,
				EnableVariablePromotion = true,
				EnableSparseConditionalConstantPropagation = true,
				Emulator = EmulatorType.Qemu,
				ImageFormat = ImageFormat.IMG,
				BootFormat = BootFormat.Multiboot_0_7,
				PlatformType = PlatformType.X86,
				LinkerFormatType = LinkerFormatType.Elf32,
				MemoryInMB = 128,
				DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA-UnitTest"),
				FileSystem = FileSystem.FAT16,
				UseMultipleThreadCompiler = true,
				EnableInlinedMethods = true,
				InlinedIRMaximum = 8,
				BootLoader = BootLoader.Syslinux_3_72,
				VBEVideo = false,
				Width = 640,
				Height = 480,
				Depth = 32,
				BaseAddress = 0x00400000,
				EmitRelocations = false,
				EmitSymbols = false,
				Emitx86IRQMethods = true,
				DebugConnectionOption = DebugConnectionOption.TCPServer,
				DebugConnectionPort = 9999,
				ExitOnLaunch = true
			};

			AppLocations = new AppLocations();

			AppLocations.FindApplications();

			Initialize();

			stopwatch.Start();

			processThread = new Thread(ProcessQueue);
			processThread.Name = "ProcesQueue";

			//processThread.IsBackground = true;
			processThread.Start();
		}

		public void Initialize()
		{
			if (Platform == null)
				Platform = "x86";

			if (TestAssemblyPath == null)
				TestAssemblyPath = AppContext.BaseDirectory;

			if (TestSuiteFile == null)
				TestSuiteFile = "Mosa.UnitTest." + Platform + ".exe";
		}

		private void ProcessQueue()
		{
			try
			{
				while (!processThreadAbort)
				{
					//Thread.Sleep(5);   // temporary

					// todo - wait for pulse (or timeout)

					UnitTestRequest request = null;

					lock (queue)
					{
						// check if queue has requests or too many have already been sent
						if (queue.Count <= 0 || sent.Count > MaxSentQueue)
						{
							continue;
						}

						request = queue.Dequeue();
					}

					PrepareUnitTest();

					var message = new DebugMessage(DebugCode.ExecuteUnitTest, request.CreateRequestMessage());
					message.CallBack = UnitTestResults;
					message.Other = request;

					debugServerEngine.SendCommand(message);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		private void UnitTestResults(DebugMessage response)
		{
			if (response == null)
				return;

			lock (this)
			{
				//Console.WriteLine(response.ToString());

				var request = response.Other as UnitTestRequest;

				sent.Remove(request);

				request.ParseResultData(response.ResponseData);
			}
		}

		private void QueueUnitTest(UnitTestRequest request)
		{
			lock (queue)
			{
				queue.Enqueue(request);
			}
		}

		public T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			CheckCompiled();

			var request = new UnitTestRequest(ns, type, method, parameters);

			request.Resolve(typeSystem, linker);

			QueueUnitTest(request);

			while (!request.HasResult)
			{
				Thread.Sleep(5); // temporary
			}

			var result = request.Result;

			if (request.RuntimeMethod.Signature.ReturnType.IsVoid)
				return default(T);

			try
			{
				if (default(T) is ValueType)
					return (T)result;
				else
					return default(T);
			}
			catch (InvalidCastException e)
			{
				Debug.Assert(false, String.Format("Failed to convert result {0} of destination {1} destination type {2}.", result, result.GetType(), typeof(T).ToString()));
				throw e;
			}
		}

		private bool CheckCompiled()
		{
			lock (this)
			{
				if (compiled)
					return true;

				if (fatalError)
					return false;

				Compile();

				return compiled;
			}
		}

		private void Compile()
		{
			Options.SourceFile = Path.Combine(TestAssemblyPath, TestSuiteFile);

			var builder = new Builder(Options, AppLocations, this);

			builder.Compile();

			linker = builder.Linker;
			typeSystem = builder.TypeSystem;
			imagefile = builder.ImageFile;

			fatalError = builder.HasCompileError;
			compiled = !fatalError;
		}

		public void LaunchVirtualMachine()
		{
			if (starter == null)
			{
				starter = new Starter(Options, AppLocations, imagefile, this);
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
				if (Options.DebugConnectionOption == DebugConnectionOption.TCPServer)
				{
					var client = new TcpClient(Options.DebugConnectionAddress, Options.DebugConnectionPort);
					debugServerEngine.Stream = new DebugNetworkStream(client.Client, true);
				}
				else if (Options.DebugConnectionOption == DebugConnectionOption.Pipe)
				{
					var pipeStream = new NamedPipeClientStream(".", Options.DebugPipeName, PipeDirection.InOut);
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

		public bool PrepareUnitTest()
		{
			lock (this)
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
