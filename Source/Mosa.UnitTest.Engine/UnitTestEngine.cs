// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.ClassLib;
using Mosa.Compiler.Common;
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
		public Options Options { get; }
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
		private bool imageSent = false;
		private bool kernelInit = false;
		private volatile bool ready = false;

		private readonly Stopwatch stopwatch = new Stopwatch();

		private const uint MaxRetries = 32;
		private const uint RetryDelay = 1; // 1- seconds

		private const int DefaultMaxSentQueue = 100;

		private readonly Queue<DebugMessage> queue = new Queue<DebugMessage>();
		private readonly HashSet<DebugMessage> sent = new HashSet<DebugMessage>();

		private readonly int MaxSentQueue = DefaultMaxSentQueue;

		//private int SentQueueCountDown = 0;

		private readonly Thread processThread;
		private volatile bool processThreadAbort = false;

		private int processCount = 0;
		private Stopwatch stopWatch;

		public UnitTestEngine()
		{
			Options = new Options()
			{
				EnableSSA = true,
				EnableIROptimizations = true,
				EnableSparseConditionalConstantPropagation = true,
				EnableInlinedMethods = false,
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
				UseMultipleThreadCompiler = false,
				InlinedIRMaximum = 8,
				BootLoader = BootLoader.Syslinux_3_72,
				VBEVideo = false,
				Width = 640,
				Height = 480,
				Depth = 32,
				BaseAddress = 0x00500000,//0x00400000,
				EmitRelocations = false,
				EmitSymbols = false,
				Emitx86IRQMethods = true,
				DebugConnectionOption = DebugConnectionOption.TCPServer,
				DebugConnectionPort = 9999,
				DebugConnectionAddress = "127.0.0.1",
				DebugPipeName = "MOSA",
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

			stopwatch.Start();

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

			//ThreadPool.SetMaxThreads(64, 64);
		}

		private void ProcessQueue()
		{
			var last = DateTime.Now;

			try
			{
				while (!processThreadAbort)
				{
					var messages = new List<DebugMessage>();

					DebugMessage message = null;

					lock (queue)
					{
						// check if queue has requests or too many have already been sent
						while (queue.Count > 0 && sent.Count < MaxSentQueue)
						{
							message = queue.Dequeue();

							PrepareUnitTest();

							message.CallBack = MessageCallBack;

							sent.Add(message);

							//Console.WriteLine("[" + queue.Count.ToString() + "/" + messages.Count.ToString() + "] SENT: " + (message.Other as UnitTestRequest).MethodTypeName + "." + (message.Other as UnitTestRequest).MethodName);

							messages.Add(message);

							//debugServerEngine.SendCommand(message);
						}

						if (messages.Count > 0)
						{
							//Console.Write(messages.Count.ToString() + ":");
							debugServerEngine.SendCommand2(messages);
							messages.Clear();
						}
					}

					// Thread.Sleep(10);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
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

				if (processCount % 1000 == 0 && stopwatch.Elapsed.Seconds != 0)
				{
					Console.WriteLine("Unit Tests - Count: " + processCount.ToString() + " Elapsed: " + ((int)stopwatch.Elapsed.TotalSeconds).ToString() + " (" + (processCount / stopwatch.Elapsed.TotalSeconds).ToString("F2") + " per second)");
				}
			}

			if (response.Other is UnitTestRequest message)
			{
				message.ParseResultData(response.ResponseData);

				//Console.WriteLine("RECD: " + message.MethodTypeName + "." + message.MethodName);
			}
		}

		private void QueueMessage(DebugMessage request)
		{
			lock (queue)
			{
				queue.Enqueue(request);
			}
		}

		public static MosaMethod FindMethod(TypeSystem typeSystem, string ns, string type, string method, params object[] parameters)
		{
			foreach (var t in typeSystem.AllTypes)
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns) && t.Namespace != ns)
					continue;

				foreach (var m in t.Methods)
				{
					if (m.Name == method && m.Signature.Parameters.Count == parameters.Length)
					{
						return m;
					}
				}

				break;
			}

			return null;
		}

		public static ulong GetMethodAddress(MosaMethod method, BaseLinker linker)
		{
			var symbol = linker.GetSymbol(method.FullName, SectionKind.Text);

			return symbol.VirtualAddress;
		}

		public static ulong GetMethodAddress(TypeSystem typeSystem, BaseLinker linker, string ns, string type, string method, params object[] parameters)
		{
			var mosaMethod = FindMethod(
				typeSystem,
				ns,
				type,
				method,
				parameters
			);

			Debug.Assert(mosaMethod != null, ns + "." + type + "." + method);

			return GetMethodAddress(mosaMethod, linker);
		}

		public T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			CheckCompiled();

			var request = new UnitTestRequest(ns, type, method, parameters);

			request.Resolve(typeSystem, linker);

			var message = new DebugMessage(DebugCode.ExecuteUnitTest, request.Message, request);

			QueueMessage(message);

			//// for performance test --- create 1000 more to send
			//var testMessage = new DebugMessage(message.Code, message.CommandData);
			//testMessage.CallBack = MessageCallBack;

			//for (int i = 0; i < 1000; i++)
			//{
			//	QueueMessage(testMessage);
			//}

			while (!request.HasResult)
			{
				Thread.Sleep(25);
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
				Debug.Fail(String.Format("Failed to convert result {0} of destination {1} destination type {2}.", result, result.GetType(), typeof(T).ToString()));
				throw;
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

		private void Compile()
		{
			Options.SourceFile = Path.Combine(TestAssemblyPath, TestSuiteFile);

			var builder = new Builder(Options, AppLocations, this);

			builder.Compile();

			linker = builder.Linker;
			typeSystem = builder.TypeSystem;
			imagefile = Options.BootLoaderImage ?? builder.ImageFile;

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
			imageSent = false;
			kernelInit = false;
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
				imageSent = false;

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

		public void SendImage()
		{
			const uint maxsize = 1024 * 16;

			uint originalSize = 0;

			var bss = linker.LinkerSections[(int)SectionKind.BSS];

			var message = new DebugMessage(DebugCode.ClearMemory, new int[] { (int)bss.VirtualAddress, (int)bss.Size });

			SendMessageAndWait(message);

			var compressed = new byte[maxsize * 2];

			foreach (var section in linker.LinkerSections)
			{
				if (section.SectionKind == SectionKind.BSS)
					continue;

				var stream = new MemoryStream((int)section.Size);

				// similar code in the Section.WriteTo method
				foreach (var symbol in section.Symbols)
				{
					stream.Seek(symbol.SectionOffset, SeekOrigin.Begin);
					if (symbol.IsDataAvailable)
					{
						symbol.Stream.Position = 0;
						symbol.Stream.WriteTo(stream);
					}
				}

				stream.WriteZeroBytes((int)(section.AlignedSize - stream.Position));
				stream.Position = 0;

				var array = stream.ToArray();
				uint at = 0;

				while (at < array.Length)
				{
					uint size = (uint)array.Length - at;

					if (size > maxsize) size = maxsize;

					var raw = new byte[size];
					Array.Copy(array, at, raw, 0, size);

					originalSize += size;

					var data = new byte[size + 8];
					uint address = (uint)(section.VirtualAddress + at);

					data[0] = (byte)(address & 0xFF);
					data[1] = (byte)((address >> 8) & 0xFF);
					data[2] = (byte)((address >> 16) & 0xFF);
					data[3] = (byte)((address >> 24) & 0xFF);

					data[4] = (byte)(size & 0xFF);
					data[5] = (byte)((size >> 8) & 0xFF);
					data[6] = (byte)((size >> 16) & 0xFF);
					data[7] = (byte)((size >> 24) & 0xFF);

					Array.Copy(raw, 0, data, 8, size);

					message = new DebugMessage(DebugCode.WriteMemory, data);

					Console.WriteLine(section.SectionKind.ToString() + " @ 0x" + address.ToString("X") + " [size: " + size.ToString() + "]");

					SendMessageAndWait(message);

					at += size;
				}
			}

			Console.WriteLine("Total Size: " + originalSize.ToString());

			imageSent = true;
			return;
		}

		public void SendImageCompressed()
		{
			const uint maxsize = 1024 * 64;

			var lzf = new LZF();

			uint compressSize = 0;
			uint originalSize = 0;

			var bss = linker.LinkerSections[(int)SectionKind.BSS];

			var message = new DebugMessage(DebugCode.ClearMemory, new int[] { (int)bss.VirtualAddress, (int)bss.Size });

			SendMessageAndWait(message);

			var compressed = new byte[maxsize * 2];

			foreach (var section in linker.LinkerSections)
			{
				if (section.SectionKind == SectionKind.BSS)
					continue;

				var stream = new MemoryStream((int)section.Size);

				// similar code in the Section.WriteTo method
				foreach (var symbol in section.Symbols)
				{
					stream.Seek(symbol.SectionOffset, SeekOrigin.Begin);
					if (symbol.IsDataAvailable)
					{
						symbol.Stream.Position = 0;
						symbol.Stream.WriteTo(stream);
					}
				}

				stream.WriteZeroBytes((int)(section.AlignedSize - stream.Position));
				stream.Position = 0;

				var array = stream.ToArray();
				uint at = 0;

				while (at < array.Length)
				{
					uint size = (uint)array.Length - at;

					if (size > maxsize) size = maxsize;

					// compress
					var raw = new byte[size];
					Array.Copy(array, at, raw, 0, size);

					uint crc = CRC.InitialCRC;

					for (int i = 0; i < size; i++)
					{
						crc = CRC.Update(crc, raw[i]);
					}

					var len = lzf.Compress(raw, raw.Length, compressed, compressed.Length);

					compressSize += (uint)len;
					originalSize += size;

					// data
					var data = new byte[len + 16];
					uint address = (uint)(section.VirtualAddress + at);

					data[0] = (byte)(address & 0xFF);
					data[1] = (byte)((address >> 8) & 0xFF);
					data[2] = (byte)((address >> 16) & 0xFF);
					data[3] = (byte)((address >> 24) & 0xFF);

					data[4] = (byte)(len & 0xFF);
					data[5] = (byte)((len >> 8) & 0xFF);
					data[6] = (byte)((len >> 16) & 0xFF);
					data[7] = (byte)((len >> 24) & 0xFF);

					data[8] = (byte)(size & 0xFF);
					data[9] = (byte)((size >> 8) & 0xFF);
					data[10] = (byte)((size >> 16) & 0xFF);
					data[11] = (byte)((size >> 24) & 0xFF);

					data[12] = (byte)(crc & 0xFF);
					data[13] = (byte)((crc >> 8) & 0xFF);
					data[14] = (byte)((crc >> 16) & 0xFF);
					data[15] = (byte)((crc >> 24) & 0xFF);

					Array.Copy(compressed, 0, data, 16, len);

					message = new DebugMessage(DebugCode.CompressedWriteMemory, data);

					Console.WriteLine(section.SectionKind.ToString() + " @ 0x" + address.ToString("X") + " [size: " + size.ToString() + " compressed: " + len.ToString() + "]");

					SendMessageAndWait(message);

					at += size;
				}
			}

			Console.WriteLine("Original: " + originalSize.ToString());
			Console.WriteLine("Compressed: " + compressSize.ToString());
			Console.WriteLine("Compacted: " + (compressSize * 100 / originalSize).ToString());

			kernelInit = false;
			imageSent = true;
		}

		public void PrepareKernel()
		{
			ulong address = GetMethodAddress(typeSystem, linker, "Mosa.Runtime", "StartUp", "Initialize", new object[] { });

			var message = new DebugMessage(DebugCode.HardJump, new int[] { (int)address });

			SendMessageAndWait(message);

			kernelInit = true;
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

				if (ready && !imageSent)
				{
					if (Options.BootLoaderImage == null)
					{
						imageSent = true;
						kernelInit = true;
					}
				}

				if (ready && !imageSent)
				{
					SendImageCompressed();
				}

				if (ready && imageSent && !kernelInit)
				{
					PrepareKernel();
				}

				if (fatalError)
					return false;

				if (stopWatch == null)
				{
					stopWatch = new Stopwatch();
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
