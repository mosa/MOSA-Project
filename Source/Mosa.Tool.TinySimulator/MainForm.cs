/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.MosaTypeSystem;
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
	public partial class MainForm : Form, ICompilerEventListener
	{
		private AssembliesView assembliesView = new AssembliesView();
		private RegisterView registersView = new RegisterView();
		private DisplayView displayView = new DisplayView();
		private ControlView controlView = new ControlView();
		private CallStackView callStackView = new CallStackView();
		private StackFrameView stackFrameView = new StackFrameView();
		private StackView stackView = new StackView();
		private FlagView flagView = new FlagView();
		private StatusView statusView = new StatusView();
		private HistoryView historyView = new HistoryView();
		private SymbolView symbolView = new SymbolView();
		private WatchView watchView = new WatchView();
		private BreakPointView breakPointView = new BreakPointView();
		private OutputView outputView = new OutputView();
		private ScriptView scriptView = new ScriptView();

		public IInternalTrace InternalTrace = new BasicInternalTrace();
		public ConfigurableTraceFilter Filter = new ConfigurableTraceFilter();
		public TypeSystem TypeSystem;
		public MosaTypeLayout TypeLayout;
		public BaseArchitecture Architecture;
		public ILinker Linker;
		public SimCPU SimCPU;

		public int MaxHistory { get; set; }

		public string Status { set { this.toolStripStatusLabel1.Text = value; toolStrip1.Refresh(); } }

		public string CompileOnLaunch { get; set; }

		public List<Watch> watches = new List<Watch>();

		private Thread worker;
		private object workerLock = new object();

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
			Filter.MethodMatch = MatchType.None;
			Filter.Method = string.Empty;
			Filter.StageMatch = MatchType.Any;
			Filter.TypeMatch = MatchType.Any;
			Filter.ExcludeInternalMethods = false;

			InternalTrace.TraceFilter = Filter;
			InternalTrace.CompilerEventListener = this;

			MaxHistory = 1000;

			worker = new Thread(ExecuteThread);
			worker.Name = "SimCPU";
			worker.Start();
		}

		void ICompilerEventListener.SubmitTraceEvent(CompilerEvent compilerStage, string info)
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
				StartSimulator("x86");
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			var memoryView = new MemoryView();
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

				Status = "Assembly Loaded. Ready to Compile.";
			}
		}

		public void LoadAssembly(string filename)
		{
			MosaAssemblyLoader assemblyLoader = new MosaAssemblyLoader();

			assemblyLoader.AddPrivatePath(System.IO.Path.GetDirectoryName(filename));
			assemblyLoader.LoadModule(filename);
            assemblyLoader.LoadModule("Mosa.Internal.dll");

			TypeSystem = new TypeSystem();

			TypeSystem.Load(assemblyLoader);

			TypeLayout = new MosaTypeLayout(TypeSystem, 4, 4);

			assembliesView.UpdateTree();
		}

		public void StartSimulator(string platform)
		{
			if (TypeSystem == null)
				return;

			Status = "Compiling...";

			Architecture = GetArchitecture(platform);
			var simAdapter = GetSimAdaptor(platform);
			Linker = new SimLinker(simAdapter);

			compileStartTime = DateTime.Now;

			SimCompiler.Compile(TypeSystem, TypeLayout, InternalTrace, true, Architecture, simAdapter, Linker);
			SimCPU = simAdapter.SimCPU;

			SimCPU.Monitor.BreakAtTick = 1;
			SimCPU.Monitor.BreakOnException = true;
			SimCPU.Monitor.OnStateUpdate = UpdateSimState;
			SimCPU.Reset();

			Display32 = (uint)SimCPU.GetState().Values["Register.Size"] == 32;

			SimCPU.Monitor.OnExecutionStepCompleted(true);

			Status = "Compiled.";

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
			if (TypeSystem == null)
			{
				LoadAssembly();
			}

			StartSimulator("x86");
		}

		public void UpdateAllDocks(SimState simState)
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

		private void UpdateSimState(SimState simState, bool forceUpdate)
		{
			SimCPU.ExtendState(simState);

			double secs = stopwatch.Elapsed.TotalSeconds;

			if (secs == 0)
				secs = 1;

			simState.StoreValue("TotalElapsed", secs);

			AddHistory(simState);
			AddWatch(simState);

			if (forceUpdate || simState.Tick == 0 || DateTime.Now.Ticks > lastTimeTick + 2500000)
			{
				MethodInvoker method = delegate() { UpdateAllDocks(simState); };
				Invoke(method);

				lastTimeTick = DateTime.Now.Ticks;
			}
		}

		private void ExecuteThread()
		{
			for (; ; )
			{
				if (SimCPU == null)
				{
					Thread.Sleep(500);
					continue;
				}

				stopwatch.Start();
				SimCPU.Execute();
				stopwatch.Stop();

				lock (workerLock)
				{
					Monitor.Wait(workerLock);
				}
			}
		}

		private void AddHistory(SimState simState)
		{
			historyView.AddHistory(simState);
		}

		private void AddWatch(SimState simState)
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

		public void ExecuteSteps(uint steps)
		{
			if (SimCPU == null)
				return;

			SimCPU.Monitor.BreakFromCurrentTick(steps);
			SimCPU.Monitor.Stop = false;

			lock (workerLock)
			{
				Monitor.PulseAll(workerLock);
			}
		}

		public void Restart()
		{
			if (SimCPU == null)
				return;

			Record = false;

			SimCPU.Monitor.Stop = false;
			SimCPU.Monitor.StepOverBreakPoint = 0;

			lock (workerLock)
			{
				SimCPU.Reset();
				stopwatch.Reset();
			}

			SimCPU.Monitor.OnExecutionStepCompleted(true);
			Status = "Simulation Reset.";

			symbolView.CreateEntries();
		}

		public void Start()
		{
			if (SimCPU == null)
				return;

			SimCPU.Monitor.BreakAtTick = UInt32.MaxValue;
			SimCPU.Monitor.Stop = false;

			lock (workerLock)
			{
				Monitor.PulseAll(workerLock);
			}
		}

		public void Stop()
		{
			if (SimCPU == null)
				return;

			SimCPU.Monitor.Stop = true;

			// wait until worker done
			lock (workerLock) { }
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (SimCPU == null)
				return;

			SimCPU.Monitor.Stop = true;

			// wait until worker done
			lock (workerLock) { }

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
	}
}