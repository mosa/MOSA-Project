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
using Mosa.Compiler.TypeSystem;
using Mosa.TinyCPUSimulator;
using Mosa.TinyCPUSimulator.Adaptor;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.Simulator
{
	public partial class MainForm : Form
	{
		private AssembliesView assembliesView = new AssembliesView();
		private RegisterView generalPurposeRegistersView = new RegisterView();
		private DisplayView displayView = new DisplayView();
		private ControlView controlView = new ControlView();
		private CallStackView callStackView = new CallStackView();
		private StackFrameView stackFrameView = new StackFrameView();
		private StackView stackView = new StackView();
		private FlagView flagView = new FlagView();
		private StatusView statusView = new StatusView();
		private HistoryView historyView = new HistoryView();
		private SymbolView symbolView = new SymbolView();

		public IInternalTrace InternalTrace = new BasicInternalTrace();
		public ConfigurableTraceFilter Filter = new ConfigurableTraceFilter();
		public ITypeSystem TypeSystem;
		public ITypeLayout TypeLayout;
		public IArchitecture Architecture;
		public ILinker Linker;
		public SimCPU SimCPU;

		public bool Record = false;
		public int MaxHistory { get; set; }

		public string Status { set { this.toolStripStatusLabel1.Text = value; toolStrip1.Refresh(); } }

		public string CompileOnLaunch { get; set; }

		private Queue<SimState> history = new Queue<SimState>();
		private object historyLock = new object();
		private SimState lastHistory = null;

		private Thread worker;
		private object workerLock = new object();

		public MainForm()
		{
			InitializeComponent();
			Filter.MethodMatch = MatchType.None;
			Filter.Method = string.Empty;
			Filter.StageMatch = MatchType.Any;
			Filter.TypeMatch = MatchType.Any;
			Filter.ExcludeInternalMethods = false;

			InternalTrace.TraceFilter = Filter;

			MaxHistory = 1000;

			worker = new Thread(ExecuteThread);
			worker.Name = "SimCPU";
			worker.Start();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			dockPanel.SuspendLayout(true);

			assembliesView.Show(dockPanel, DockState.DockLeftAutoHide);
			generalPurposeRegistersView.Show(dockPanel, DockState.DockRight);
			callStackView.Show(dockPanel, DockState.DockRight);
			displayView.Show(dockPanel, DockState.Document);
			statusView.Show(dockPanel, DockState.DockBottom);
			controlView.Show(dockPanel, DockState.DockBottom);
			stackFrameView.Show(dockPanel, DockState.DockRight);
			stackView.Show(dockPanel, DockState.DockRight);
			flagView.Show(dockPanel, DockState.DockRight);
			historyView.Show(dockPanel, DockState.Document);
			symbolView.Show(dockPanel, DockState.Document);

			dockPanel.ResumeLayout(true, true);

			if (CompileOnLaunch != null)
			{
				LoadAssembly(CompileOnLaunch, "x86");
				StartSimulator();
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
				LoadAssembly(openFileDialog.FileName, "x86");
			}
		}

		public void LoadAssembly(string filename, string platform)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();

			assemblyLoader.AddPrivatePath(System.IO.Path.GetDirectoryName(filename));
			assemblyLoader.LoadModule(filename);

			TypeSystem = new TypeSystem();
			TypeSystem.LoadModules(assemblyLoader.Modules);

			TypeLayout = new TypeLayout(TypeSystem, 4, 4);

			assembliesView.UpdateTree();
		}

		public void StartSimulator()
		{
			if (TypeSystem == null)
				return;

			Status = "Compiling...";

			string platform = "x86"; // cbPlatform.Text.Trim().ToLower();

			Architecture = GetArchitecture(platform);
			var simAdapter = GetSimAdaptor(platform);
			Linker = new SimLinker(simAdapter);

			SimCompiler.Compile(TypeSystem, TypeLayout, InternalTrace, true, Architecture, simAdapter, Linker);

			SimCPU = simAdapter.SimCPU;

			SimCPU.Monitor.BreakAtTick = 1;
			SimCPU.Monitor.BreakOnException = true;
			SimCPU.Monitor.OnStateUpdate = UpdateSimState;
			SimCPU.Reset();
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

		public static string Format(object value, bool force32)
		{
			if (force32)
			{
				if (value is ulong)
					return "0x" + ((ulong)value).ToString("X8");
				else if (value is long)
					return "0x" + ((long)value).ToString("X8");
			}

			return Format(value);
		}

		private static IArchitecture GetArchitecture(string platform)
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

			StartSimulator();
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

			AddHistory(simState);

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

				SimCPU.Execute();

				lock (workerLock)
				{
					Monitor.Wait(workerLock);
				}
			}
		}

		private void AddHistory(SimState simState)
		{
			if (!Record)
				return;

			lock (historyLock)
			{
				if (lastHistory == null || lastHistory.Tick != simState.Tick)
				{
					history.Enqueue(simState);
					lastHistory = simState;
				}

				// Prune
				int max = MaxHistory;

				if (max <= 1)
					max = 1000;

				while (history.Count > max)
				{
					history.Dequeue();
				}
			}
		}

		public void Stop()
		{
			SimCPU.Monitor.Stop = true;
		}

		public void ExecuteSteps(uint steps)
		{
			if (SimCPU == null)
				return;

			SimCPU.Monitor.BreakAtTick += steps;
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

			lock (workerLock)
			{
				SimCPU.Reset();
			}

			lock (historyLock)
			{
				history.Clear();
			}

			SimCPU.Monitor.OnExecutionStepCompleted(true);
			Status = "Simulation Reset.";

			symbolView.CreateEntries();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			SimCPU.Monitor.Stop = true;
			worker.Abort();
		}
	}
}