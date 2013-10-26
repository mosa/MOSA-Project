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
using Mosa.TinyCPUSimulator.Adaptor;
using Mosa.TinyCPUSimulator;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading;

namespace Mosa.Tool.Simulator
{
	public partial class MainForm : Form
	{
		private AssembliesView assembliesView = new AssembliesView();
		private GeneralPurposeRegistersView generalPurposeRegistersView = new GeneralPurposeRegistersView();
		private DisplayView displayView = new DisplayView();
		private ControlView controlView = new ControlView();

		public IInternalTrace InternalTrace = new BasicInternalTrace();
		public ConfigurableTraceFilter Filter = new ConfigurableTraceFilter();
		public ITypeSystem TypeSystem;
		public ITypeLayout TypeLayout;
		public IArchitecture Architecture;
		public ILinker Linker;
		public SimCPU SimCPU;

		public bool Record = false;

		public string Status { set { this.toolStripStatusLabel1.Text = value; } }

		//private Thread excutionThread;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Filter.MethodMatch = MatchType.None;
			Filter.Method = string.Empty;
			Filter.StageMatch = MatchType.Any;
			Filter.TypeMatch = MatchType.Any;
			Filter.ExcludeInternalMethods = false;

			InternalTrace.TraceFilter = Filter;

			dockPanel.SuspendLayout(true);
			assembliesView.Show(dockPanel, DockState.DockLeft);
			generalPurposeRegistersView.Show(dockPanel, DockState.DockRight);
			displayView.Show(dockPanel, DockState.Document);
			controlView.Show(dockPanel, DockState.DockBottom);

			displayView.StartTimer();

			dockPanel.ResumeLayout(true, true);
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

		protected void LoadAssembly(string filename, string platform)
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

			string platform = "x86"; // cbPlatform.Text.Trim().ToLower();

			Architecture = GetArchitecture(platform);
			var simAdapter = GetSimAdaptor(platform);
			Linker = new SimLinker(simAdapter);

			SimCompiler.Compile(TypeSystem, TypeLayout, InternalTrace, true, Architecture, simAdapter, Linker);

			SimCPU = simAdapter.SimCPU;
			SimCPU.Monitor.EnableStepping = true;
			SimCPU.Reset();

			//excutionThread = new Thread(new ThreadStart(SimCPU.Execute));
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
				LoadAssembly();

			StartSimulator();
		}

		private void UpdateAll()
		{
			if (SimCPU == null)
				return;

			foreach (var dock in dockPanel.Contents)
			{
				if (dock.DockHandler.Content is SimulatorDockContent)
				{
					(dock.DockHandler.Content as SimulatorDockContent).Update();
				}
			}
		}

		public void ExecuteSteps(int steps)
		{
			if (SimCPU == null)
				return;

			try
			{
				for (int i = 0; i < steps; i++)
				{
					SimCPU.Execute();
					if (Record)
					{
						GetCurrentStateAndUpdate();
					}

					if (i % 10000 == 0)
						UpdateAll();
				}
			}
			catch (CPUException e)
			{
				GetCurrentStateAndUpdate();
				//lbInstructionHistory.Items.Add(e);
			}
			finally
			{
				UpdateAll();
			}
		}

		public void GetCurrentStateAndUpdate()
		{
			var state = SimCPU.GetState();

			//lbInstructionHistory.Items.Add(state);
			//lbInstructionHistory.SelectedIndex = lbInstructionHistory.Items.Count - 1;
		}
	}
}