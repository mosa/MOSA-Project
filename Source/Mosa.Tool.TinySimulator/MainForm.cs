// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using Mosa.TinyCPUSimulator;
using Mosa.TinyCPUSimulator.Adaptor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.TinySimulator
{
	public partial class MainForm : Form, ITraceListener
	{
		private AssembliesView assembliesView;
		private RegisterView registersView;
		private DisplayView displayView;
		private ControlView controlView;
		private CallStackView callStackView;
		private StackFrameView stackFrameView;
		private StackView stackView;
		private FlagView flagView;
		private StatusView statusView;
		private HistoryView historyView;
		private SymbolView symbolView;
		private WatchView watchView;
		private BreakPointView breakPointView;
		private OutputView outputView;
		private ScriptView scriptView;

		public MosaCompiler Compiler = new MosaCompiler();

		public SimCPU SimCPU;

		public int MaxHistory { get; set; }

		public string Status { set { this.toolStripStatusLabel1.Text = value; toolStrip1.Refresh(); } }

		public string CompileOnLaunch { get; set; }

		public List<Watch> watches = new List<Watch>();

		private Thread worker;
		private object signalLock = new object();

		private Stopwatch stopwatch = new Stopwatch();

		private DateTime compileStartTime = DateTime.Now;

		public bool Display32 { get; private set; }

		public bool Record
		{
			get { return controlView.Record; }
			set { controlView.Record = value; }
		}

		public MainForm()
		{
			InitializeComponent();

			Compiler.CompilerTrace.TraceFilter.Active = false;
			Compiler.CompilerTrace.TraceListener = this;

			Compiler.CompilerOptions.Architecture = GetArchitecture("x86");

			MaxHistory = 1000;

			assembliesView = new AssembliesView(this);
			registersView = new RegisterView(this);
			displayView = new DisplayView(this);
			controlView = new ControlView(this);
			callStackView = new CallStackView(this);
			stackFrameView = new StackFrameView(this);
			stackView = new StackView(this);
			flagView = new FlagView(this);
			statusView = new StatusView(this);
			historyView = new HistoryView(this);
			symbolView = new SymbolView(this);
			watchView = new WatchView(this);
			breakPointView = new BreakPointView(this);
			outputView = new OutputView(this);
			scriptView = new ScriptView(this);

			Thread.CurrentThread.Priority = ThreadPriority.Highest;
			worker = new Thread(ExecuteThread);
			worker.IsBackground = false;
			worker.Name = "SimCPU";
			worker.Start();
		}

		private void SubmitTraceEvent(CompilerEvent compilerStage, string info)
		{
			if (compilerStage == CompilerEvent.CompilerStageStart || compilerStage == CompilerEvent.CompilerStageEnd)
			{
				string status = "Compiling: " + String.Format("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds) + " secs: " + compilerStage.ToText() + ": " + info;

				toolStripStatusLabel1.Text = status;
				toolStripStatusLabel1.GetCurrentParent().Refresh();

				AddOutput(status);
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			dockPanel.SuspendLayout(true);
			dockPanel.DockTopPortion = 150;

			statusView.Show(dockPanel, DockState.DockTop);
			controlView.Show(statusView.PanelPane, DockAlignment.Right, 0.50);
			callStackView.Show(controlView.PanelPane, DockAlignment.Bottom, 0.50);

			breakPointView.Show(dockPanel, DockState.DockBottom);
			watchView.Show(breakPointView.PanelPane, DockAlignment.Right, 0.50);

			displayView.Show(dockPanel, DockState.Document);
			historyView.Show(dockPanel, DockState.Document);
			assembliesView.Show(dockPanel, DockState.Document);
			outputView.Show(dockPanel, DockState.Document);
			scriptView.Show(dockPanel, DockState.Document);
			symbolView.Show(dockPanel, DockState.Document);

			registersView.Show(dockPanel, DockState.DockRight);
			flagView.Show(dockPanel, DockState.DockRight);
			stackView.Show(dockPanel, DockState.DockRight);
			stackFrameView.Show(dockPanel, DockState.DockRight);

			registersView.Show();

			dockPanel.ResumeLayout(true, true);

			if (CompileOnLaunch != null)
			{
				LoadAssembly(CompileOnLaunch);
				StartSimulator();
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			var memoryView = new MemoryView(this);
			memoryView.Show(dockPanel, DockState.Document);
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			LoadAssembly();
		}

		protected void LoadAssembly()
		{
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				LoadAssembly(openFileDialog.FileName);

				Status = "Assembly Loaded.";
			}
		}

		public void LoadAssembly(string filename)
		{
			var moduleLoader = new MosaModuleLoader();

			moduleLoader.AddPrivatePath(System.IO.Path.GetDirectoryName(filename));
			moduleLoader.LoadModuleFromFile(filename);

			Compiler.Load(TypeSystem.Load(moduleLoader.CreateMetadata()));

			assembliesView.UpdateTree();
		}

		public void StartSimulator()
		{
			if (Compiler.TypeSystem == null)
				return;

			Status = "Compiling...";

			toolStrip1.Enabled = false;

			ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
			{
				try
				{
					Compile();
				}
				finally
				{
					OnCompileCompleted();
				}
			}
			));
		}

		private void Compile()
		{
			var simAdapter = GetSimAdaptor("x86");

			compileStartTime = DateTime.Now;

			Compiler.CompilerOptions.EnableSSA = false;
			Compiler.CompilerOptions.EnableOptimizations = false;
			Compiler.CompilerOptions.EnableVariablePromotion = false;
			Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = false;
			Compiler.CompilerOptions.EnableInlinedMethods = false;

			Compiler.CompilerOptions.LinkerFactory = delegate { return new SimLinker(simAdapter); };
			Compiler.CompilerFactory = delegate { return new SimCompiler(simAdapter); };

			Compiler.Execute(Environment.ProcessorCount);

			SimCPU = simAdapter.SimCPU;

			SimCPU.Monitor.BreakAtTick = 1;
			SimCPU.Monitor.BreakOnException = true;
			SimCPU.Monitor.OnStateUpdate = UpdateSimState;
			SimCPU.Reset();

			Display32 = SimCPU.GetState().NativeRegisterSize == 32;
		}

		private void OnCompileCompleted()
		{
			MethodInvoker method = delegate ()
			{
				CompileCompleted();
			};

			Invoke(method);
		}

		private void CompileCompleted()
		{
			toolStrip1.Enabled = true;

			Status = "Compiled.";

			SimCPU.Monitor.OnExecutionStepCompleted(true);

			symbolView.CreateEntries();
		}

		public static string Format(object value)
		{
			if (value is string)
				return value as string;
			else if (value is uint)
				return "0x" + ((uint)value).ToString("X8");
			else if (value is int)
				return "0x" + ((int)value).ToString("X8");
			else if (value is ulong)
				return "0x" + ((ulong)value).ToString("X16");
			else if (value is long)
				return "0x" + ((long)value).ToString("X16");
			else if (value is double)
				return ((double)value).ToString();
			else if (value is float)
				return ((float)value).ToString();
			else if (value is bool)
				return (bool)value ? "TRUE" : "FALSE";

			return value.ToString();
		}

		public static string Format(object value, bool display32)
		{
			if (display32)
			{
				if (value is ulong)
					return "0x" + ((ulong)value).ToString("X8");
				else if (value is long)
					return "0x" + ((long)value).ToString("X8");
			}

			return Format(value);
		}

		private static BaseArchitecture GetArchitecture(string platform)
		{
			switch (platform.ToLower())
			{
				case "x86": return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				default: return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
			}
		}

		private ISimAdapter GetSimAdaptor(string platform)
		{
			switch (platform.ToLower())
			{
				case "x86": return new Mosa.TinyCPUSimulator.x86.Adaptor.SimStandardPCAdapter(displayView);
				default: return new Mosa.TinyCPUSimulator.x86.Adaptor.SimStandardPCAdapter(displayView);
			}
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (Compiler.TypeSystem == null)
			{
				LoadAssembly();
			}

			outputView.Show();

			StartSimulator();
		}

		public void UpdateAllDocks(BaseSimState simState)
		{
			if (simState == null)
				return;

			if (SimCPU == null)
				return;

			foreach (var dock in dockPanel.Contents)
			{
				if (dock.DockHandler.Content is SimulatorDockContent)
				{
					(dock.DockHandler.Content as SimulatorDockContent).UpdateDock(simState);
				}
			}
		}

		private long lastTimeTick = 0;
		private Queue<BaseSimState> stateQueue = new Queue<BaseSimState>();

		private void UpdateSimState(BaseSimState simState, bool forceUpdate)
		{
			SimCPU.ExtendState(simState);

			double secs = stopwatch.Elapsed.TotalSeconds;

			if (secs == 0)
				secs = 1;

			simState.TotalElapsedSeconds = secs;

			if (this.Record)
			{
				stateQueue.Enqueue(simState);
				if (stateQueue.Count > MaxHistory)
					stateQueue.Dequeue(); // Throw away
			}

			AddWatch(simState);

			if (forceUpdate)
			{
				while (stateQueue.Count > 0)
				{
					AddHistory(stateQueue.Dequeue());
				}
			}

			if (forceUpdate || simState.Tick == 0 || DateTime.Now.Ticks > lastTimeTick + 2500000)
			{
				MethodInvoker method = delegate ()
				{
					UpdateAllDocks(simState);
				};

				BeginInvoke(method);

				lastTimeTick = DateTime.Now.Ticks;
			}
		}

		private void UpdateExecutionCompleted()
		{
			MethodInvoker method = delegate ()
			{
				Status = "Simulation stopped.";
				scriptView.ExecutingCompleted();
			};

			Invoke(method);
		}

		private void ExecuteThread()
		{
			for (;;)
			{
				lock (signalLock)
				{
					Monitor.Wait(signalLock);
				}

				if (SimCPU == null)
				{
					continue;
				}

				stopwatch.Start();
				SimCPU.Execute();
				stopwatch.Stop();

				UpdateExecutionCompleted();
			}
		}

		private void AddHistory(BaseSimState simState)
		{
			historyView.AddHistory(simState);
		}

		private void AddWatch(BaseSimState simState)
		{
			var toplist = new Dictionary<int, Dictionary<ulong, object>>();

			for (int size = 8; size <= 64; size = size * 2)
			{
				var list = new Dictionary<ulong, object>();

				foreach (var entry in watches)
				{
					if (entry.Size == size)
					{
						switch (size)
						{
							case 8: list.Add(entry.Address, SimCPU.Read8(entry.Address)); break;
							case 16: list.Add(entry.Address, SimCPU.Read16(entry.Address)); break;
							case 32: list.Add(entry.Address, SimCPU.Read32(entry.Address)); break;

							//case 64: list.Add(entry.Address, SimCPU.Read64(entry.Address)); break;
							default: break;
						}
					}
				}

				toplist.Add(size, list);
			}

			simState.StoreValue("WatchValues", toplist);
		}

		public void Restart()
		{
			if (SimCPU == null)
				return;

			Record = false;

			SimCPU.Monitor.Stop = true;
			SimCPU.Monitor.StepOverBreakPoint = 0;

			SimCPU.Reset();
			stopwatch.Reset();

			Status = "Simulation Reset.";
			SimCPU.Monitor.OnExecutionStepCompleted(true);

			symbolView.CreateEntries();
		}

		public void Start()
		{
			if (SimCPU == null)
				return;

			Status = "Simulation running...";
			SimCPU.Monitor.BreakAtTick = UInt32.MaxValue;
			SimCPU.Monitor.Stop = false;

			lock (signalLock)
			{
				Monitor.PulseAll(signalLock);
			}
		}

		public void ExecuteSteps(uint steps)
		{
			if (SimCPU == null)
				return;

			Status = "Simulation running...";
			SimCPU.Monitor.BreakFromCurrentTick(steps);
			SimCPU.Monitor.Stop = false;

			lock (signalLock)
			{
				Monitor.PulseAll(signalLock);
			}
		}

		public void Stop()
		{
			if (SimCPU == null)
				return;

			Status = "Simulation stopping...";
			SimCPU.Monitor.Stop = true;

			while (SimCPU.Monitor.IsExecuting)
			{
				Application.DoEvents();
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (SimCPU == null)
				return;

			Stop();

			worker.Abort();
		}

		public void AddWatch(string name, ulong address, int size)
		{
			if (!(size == 8 || size == 16 || size == 32 || size == 64))
				return;

			var watch = new Watch(address, size);

			watches.Add(watch);

			watchView.AddWatch(name, address, size, false);
		}

		public void AddBreakpoint(string name, ulong address)
		{
			breakPointView.AddBreakPoint(name, address);
		}

		public void AddOutput(string data)
		{
			outputView.AddOutput(data);
		}

		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerStage, string info, int threadID)
		{
			MethodInvoker call = delegate ()
			{
				SubmitTraceEvent(compilerStage, info);
			};

			Invoke(call);
		}

		void ITraceListener.OnUpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
		}

		void ITraceListener.OnNewTraceLog(TraceLog traceLog)
		{
		}
	}
}
