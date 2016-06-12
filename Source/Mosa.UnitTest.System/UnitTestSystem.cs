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

namespace Mosa.UnitTest.System
{
	public class UnitTestSystem : IBuilderEvent, IStarterEvent, IDisposable
	{
		public Options Options { get; private set; }
		public string TestAssemblyPath { get; set; }
		public string Platform { get; set; }
		public string TestSuiteFile { get; set; }
		public AppLocations AppLocations { get; set; }

		protected Builder builder;
		protected BaseLinker linker;
		protected Starter starter;
		protected Process process;
		protected DebugServerEngine debugServerEngine;

		private bool fatalError = false;
		private bool compiled = false;
		private bool processStarted = false;
		private int retries = 0;
		private bool restartVM = false;
		private volatile bool ready = false;

		private volatile List<byte> resultData = null;

		private const uint MaxRetries = 10;
		private const uint RetryDelay = 1; // 1- seconds

		private Stopwatch stopwatch = new Stopwatch();

		public UnitTestSystem()
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

		public void Compile()
		{
			Options.SourceFile = Path.Combine(TestAssemblyPath, TestSuiteFile);

			builder = new Builder(Options, AppLocations, this);

			builder.Compile();

			linker = builder.Linker;

			fatalError = builder.HasCompileError;
			compiled = !fatalError;
		}

		public void LaunchVirtualMachine()
		{
			if (starter == null)
			{
				starter = new Starter(Options, AppLocations, builder.ImageFile, this);
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
				debugServerEngine.SetDispatchMethod(DebugResponse);
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

				Connect();
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
			if (fatalError)
				return false;

			if (!compiled)
				Compile();

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

		public T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			if (!PrepareUnitTest())
			{
				throw new Exception("Unable to prepare for unit test");
			}

			// Find the test method to execute
			var runtimeMethod = FindMethod(
				ns,
				type,
				method,
				parameters
			);

			Debug.Assert(runtimeMethod != null, runtimeMethod.ToString());

			var symbol = linker.GetSymbol(runtimeMethod.FullName, SectionKind.Text);

			ulong address = symbol.VirtualAddress;

			var cmd = new List<int>(4 + 4 + 4 + runtimeMethod.Signature.Parameters.Count);

			cmd.Add((int)address);
			cmd.Add(GetReturnResultType(runtimeMethod.Signature.ReturnType));
			cmd.Add(0);

			foreach (var parm in parameters)
			{
				AddParameters(cmd, parm);
			}

			cmd[2] = cmd.Count - 3;

			var message = new DebugMessage(DebugCode.ExecuteUnitTest, cmd);

			// enforce single thread execution only
			lock (this)
			{
				resultData = null;
				debugServerEngine.SendCommand(message);

				while (true)
				{
					Thread.Sleep(1);

					if (resultData != null)
						break;
				}

				var result = GetResult(runtimeMethod.Signature.ReturnType, resultData);

				if (runtimeMethod.Signature.ReturnType.IsVoid)
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
		}

		private void DebugResponse(DebugMessage response)
		{
			if (response == null)
				return;

			//Console.WriteLine(response.ToString());

			if (response.Code == DebugCode.Ready)
			{
				ready = true;
			}

			if (response.Code != DebugCode.ExecuteUnitTest)
				return;

			resultData = response.ResponseData;
		}

		private MosaMethod FindMethod(string ns, string type, string method, params object[] parameters)
		{
			foreach (var t in builder.TypeSystem.AllTypes)
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns))
					if (t.Namespace != ns)
						continue;

				foreach (var m in t.Methods)
				{
					if (m.Name == method)
					{
						if (m.Signature.Parameters.Count == parameters.Length)
						{
							return m;
						}
					}
				}

				break;
			}

			return null;
		}

		private int GetReturnResultType(MosaType type)
		{
			if (type.IsI1)
				return 1;
			else if (type.IsI2)
				return 1;
			else if (type.IsI4)
				return 1;
			else if (type.IsI8)
				return 2;
			else if (type.IsU1)
				return 1;
			else if (type.IsU2)
				return 1;
			else if (type.IsU4)
				return 1;
			else if (type.IsU8)
				return 2;
			else if (type.IsChar)
				return 1;
			else if (type.IsBoolean)
				return 1;
			else if (type.IsR4)
				return 3;
			else if (type.IsR8)
				return 3;
			else if (type.IsVoid)
				return 0;

			return 0;
		}

		public object GetResult(MosaType type, List<byte> data)
		{
			if (type.IsI1)
				return (sbyte)data[0];
			else if (type.IsI2)
				return (short)(data[0] | (data[1] << 8));
			else if (type.IsI4)
				return (int)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
			else if (type.IsI8)
			{
				ulong low = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
				ulong high = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

				return (long)(low | (high << 32));
			}
			else if (type.IsU1)
				return (byte)data[0];
			else if (type.IsU2)
				return (ushort)(data[0] | (data[1] << 8));
			else if (type.IsU4)
				return (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
			else if (type.IsU8)
			{
				ulong low = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
				ulong high = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

				return (ulong)(low | (high << 32));
			}
			else if (type.IsChar)
				return (char)(data[0] | (data[1] << 8));
			else if (type.IsBoolean)
				return (bool)(data[0] != 0);
			else if (type.IsR4)
			{
				var value = new byte[8];

				for (int i = 0; i < 8; i++)
					value[i] = data[i];

				var d = BitConverter.ToSingle(value, 0);

				return d;
			}
			else if (type.IsR8)
			{
				var value = new byte[8];

				for (int i = 0; i < 8; i++)
					value[i] = data[i];

				var d = BitConverter.ToDouble(value, 0);

				return d;
			}
			else if (type.IsVoid)
				return null;

			return null;
		}

		private void AddParameters(List<int> cmd, object parameter)
		{
			if ((parameter == null) || !(parameter is ValueType))
			{
				throw new InvalidProgramException();
			}

			if (parameter is Boolean)
			{
				cmd.Add((bool)parameter ? 1 : 0);
			}
			else if (parameter is Char)
			{
				cmd.Add((char)parameter);
			}
			else if (parameter is SByte)
			{
				cmd.Add((int)(sbyte)parameter);
			}
			else if (parameter is Int16)
			{
				cmd.Add((int)(short)parameter);
			}
			else if (parameter is int)
			{
				cmd.Add((int)(int)parameter);
			}
			else if (parameter is Byte)
			{
				cmd.Add((byte)parameter);
			}
			else if (parameter is UInt16)
			{
				cmd.Add((ushort)parameter);
			}
			else if (parameter is UInt32)
			{
				cmd.Add((int)((uint)parameter));
			}
			else if (parameter is UInt64)
			{
				cmd.Add((int)(ulong)parameter);
				cmd.Add((int)((ulong)parameter >> 32));
			}
			else if (parameter is Int64)
			{
				cmd.Add((int)(long)parameter);
				cmd.Add((int)((long)parameter >> 32));
			}
			else if (parameter is Single)
			{
				var b = BitConverter.GetBytes((float)parameter);
				var u = BitConverter.ToUInt32(b, 0);
				cmd.Add((int)u);
			}
			else if (parameter is Double)
			{
				var b = BitConverter.GetBytes((double)parameter);
				var u = BitConverter.ToUInt64(b, 0);
				cmd.Add((int)((long)u));
				cmd.Add((int)((long)u >> 32));
			}
			else
			{
				throw new InvalidProgramException();
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
			if (process == null)
				return;

			if (process.HasExited)
				return;

			process.Kill();

			process = null;
		}
	}
}
