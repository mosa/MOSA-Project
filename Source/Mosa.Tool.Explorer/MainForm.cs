// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Tool.Explorer.Stages;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using static Mosa.Compiler.Framework.CompilerHooks;

namespace Mosa.Tool.Explorer
{
	public partial class MainForm : Form
	{
		#region Classes

		protected class CounterEntry
		{
			//[Browsable(false)]
			public string Name { get; set; }

			public int Value { get; set; }

			public CounterEntry(string name, int counter)
			{
				Name = name;
				Value = counter;
			}
		}

		#endregion Classes

		private readonly Settings Settings = new Settings();

		private MosaCompiler Compiler = null;

		private readonly CompilerData CompilerData = new CompilerData();
		private readonly MethodStore MethodStore = new MethodStore();

		private TypeSystemTree TypeSystemTree;

		private int TotalMethods = 0;
		private int CompletedMethods = 0;
		private string Status = null;

		private MosaMethod CurrentMethodSelected = null;
		private string CurrentLogSection = string.Empty;

		private readonly BindingList<CounterEntry> MethodCounters = new BindingList<CounterEntry>();
		private readonly BindingList<CounterEntry> CompilerCounters = new BindingList<CounterEntry>();

		private int TransformStep = 0;

		private readonly object _statusLock = new object();

		public MainForm()
		{
			InitializeComponent();

			cbPlatform.SelectedIndex = 0;

			gridMethodCounters.DataSource = MethodCounters;
			gridMethodCounters.Columns[0].Width = 370;
			gridMethodCounters.Columns[1].Width = 80;
			gridMethodCounters.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			gridCompilerCounters.DataSource = CompilerCounters;
			gridCompilerCounters.Columns[0].Width = 370;
			gridCompilerCounters.Columns[1].Width = 80;
			gridCompilerCounters.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			ClearAll();

			RegisterPlatforms();
		}

		private static void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
		}

		public void ClearAllLogs()
		{
			CompilerData.ClearAllLogs();

			ClearSectionDropDown();

			cbCompilerSections.SelectedIndex = 0;
		}

		private void ClearSectionDropDown()
		{
			cbCompilerSections.Items.Clear();

			CompilerData.DirtyLogSections = true;
			CompilerData.DirtyLog = true;

			RefreshCompilerSelectionDropDown();
			RefreshCompilerLog();
		}

		private void RefreshCompilerSelectionDropDown()
		{
			if (!CompilerData.DirtyLogSections)
				return;

			CompilerData.DirtyLogSections = false;

			lock (CompilerData.Logs)
			{
				for (int i = cbCompilerSections.Items.Count; i < CompilerData.LogSections.Count; i++)
				{
					var formatted = $"[{i}] {CompilerData.LogSections[i]}";

					cbCompilerSections.Items.Add(formatted);
				}
			}
		}

		private void RefreshCompilerLog()
		{
			if (!CompilerData.DirtyLog)
				return;

			if (tabControl.SelectedTab == tabLogs)
			{
				tbLogs.Text = CreateText(CompilerData.GetLog(CurrentLogSection));
			}

			if (tabControl.SelectedTab == tabCompilerCounters)
			{
				UpdateCompilerCounters();
			}

			CompilerData.DirtyLog = false;
		}

		private void SetStatus(string status)
		{
			toolStripStatusLabel.Text = status;
		}

		private void Main_Load(object sender, EventArgs e)
		{
			Text = "MOSA Explorer v" + CompilerVersion.VersionString;

			SetStatus("Ready!");

			var sourcefiles = Settings.GetValueList("Compiler.SourceFiles");

			if (sourcefiles != null && sourcefiles.Count >= 1)
			{
				var filename = Path.GetFullPath(sourcefiles[0]);

				openFileDialog.FileName = filename;

				UpdateSettings(filename);

				LoadAssembly();
			}
		}

		private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void ToolStripButton1_Click(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void OpenFile()
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				UpdateSettings();

				UpdateSettings(openFileDialog.FileName);

				LoadAssembly();
			}
		}

		protected void CreateTree()
		{
			if (Compiler == null)
				return;

			if (Compiler.TypeSystem == null || Compiler.TypeLayout == null)
			{
				TypeSystemTree = null;
				treeView.Nodes.Clear();
				return;
			}

			var include = GetIncluded(tbFilter.Text, out MosaUnit selected);

			TypeSystemTree = new TypeSystemTree(treeView, Compiler.TypeSystem, Compiler.TypeLayout, showSizes.Checked, include);

			Select(selected);
		}

		protected void Select(MosaUnit selected)
		{
			if (selected == null)
				return;

			foreach (TreeNode node in treeView.Nodes)
			{
				if (Select(node, selected))
					return;
			}
		}

		protected bool Select(TreeNode node, MosaUnit selected)
		{
			if (node == null)
				return false;

			if (node.Tag != null)
			{
				if (node.Tag == selected)
				{
					treeView.SelectedNode = node;
					node.EnsureVisible();
					return true;
				}
			}

			foreach (TreeNode children in node.Nodes)
			{
				if (Select(children, selected))
					return true;
			}

			return false;
		}

		protected HashSet<MosaUnit> GetIncluded(string value, out MosaUnit selected)
		{
			selected = null;

			value = value.Trim();

			if (string.IsNullOrWhiteSpace(value))
				return null;

			if (value.Length < 1)
				return null;

			var include = new HashSet<MosaUnit>();

			MosaUnit typeSelected = null;
			MosaUnit methodSelected = null;

			foreach (var type in Compiler.TypeSystem.AllTypes)
			{
				bool typeIncluded = false;

				var typeMatch = type.FullName.Contains(value);

				if (typeMatch)
				{
					include.Add(type);
					include.AddIfNew(type.Module);

					if (typeSelected == null)
						typeSelected = type;
				}

				foreach (var method in type.Methods)
				{
					bool methodMatch = method.FullName.Contains(value);

					if (typeMatch || methodMatch)
					{
						include.Add(method);
						include.AddIfNew(type);
						include.AddIfNew(type.Module);

						if (methodMatch && methodSelected == null)
							methodSelected = method;
					}
				}

				foreach (var property in type.Properties)
				{
					if (typeIncluded || property.FullName.Contains(value))
					{
						include.Add(property);
						include.AddIfNew(type);
						include.AddIfNew(type.Module);
					}
				}
			}

			selected = methodSelected ?? typeSelected;

			return include;
		}

		protected void UpdateTree()
		{
			if (TypeSystemTree == null)
			{
				CreateTree();
			}
			else
			{
				TypeSystemTree.Update();
			}
		}

		private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void ShowSizes_Click(object sender, EventArgs e)
		{
			CreateTree();
		}

		private void SubmitTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			CompilerData.AddTraceEvent(compilerEvent, message, threadID);
		}

		private void CompileAll()
		{
			if (Compiler == null)
				return;

			CompilerData.CompileStartTime = DateTime.Now;

			Compiler.ScheduleAll();

			toolStrip1.Enabled = false;

			ThreadPool.QueueUserWorkItem(state =>
			{
				try
				{
					Compiler.Compile();
				}
				finally
				{
					OnCompileCompleted();
				}
			});
		}

		private void OnCompileCompleted() => Invoke((MethodInvoker)(() => CompileCompleted()));

		private void CompileCompleted()
		{
			toolStrip1.Enabled = true;

			SetStatus("Compiled!");

			CompilerData.SortLog("Counters");

			UpdateTree();
		}

		private void NowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CompileAll();
		}

		private MosaMethod GetCurrentMethod()
		{
			var node = treeView.SelectedNode;

			return node == null ? null : node.Tag as MosaMethod;
		}

		private string GetCurrentInstructionStage()
		{
			return cbInstructionStages.SelectedItem.ToString();
		}

		private string GetCurrentInstructionLabel()
		{
			return cbInstructionLabels.SelectedItem as string;
		}

		private string GetCurrentDebugStage()
		{
			return cbDebugStages.SelectedItem.ToString();
		}

		private string GetCurrentTransformStage()
		{
			return cbTransformStages.SelectedItem.ToString();
		}

		private string GetCurrentTransformLabel()
		{
			return cbTransformLabels.SelectedItem as string;
		}

		private MethodData GetCurrentMethodData()
		{
			var method = CurrentMethodSelected;

			if (method == null)
				return null;

			var methodData = MethodStore.GetMethodData(method, false);

			return methodData;
		}

		private List<string> GetCurrentInstructionLines()
		{
			var methodData = GetCurrentMethodData();

			if (methodData == null)
				return null;

			var stage = GetCurrentInstructionStage();

			return methodData.InstructionLogs[stage];
		}

		private List<string> GetCurrentTransformLines()
		{
			var methodData = GetCurrentMethodData();

			if (methodData == null)
				return null;

			var stage = GetCurrentTransformStage();

			var logs = methodData.TransformLogs[stage];

			var log = logs[TransformStep];

			return log;
		}

		private List<string> GetCurrentDebugLines()
		{
			var methodData = GetCurrentMethodData();

			if (methodData == null)
				return null;

			string stage = GetCurrentDebugStage();

			return methodData.DebugLogs[stage];
		}

		private void UpdateInstructionStages()
		{
			cbInstructionStages.Items.Clear();

			var methodData = GetCurrentMethodData();

			if (methodData == null)
				return;

			foreach (var stage in methodData.OrderedStageNames)
			{
				cbInstructionStages.Items.Add(stage);
			}

			cbInstructionStages.SelectedIndex = cbInstructionStages.Items.Count == 0 ? -1 : 0;
		}

		private void UpdateDebugStages()
		{
			cbDebugStages.Items.Clear();

			var methodData = GetCurrentMethodData();

			if (methodData == null)
				return;

			foreach (string stage in methodData.OrderedDebugStageNames)
			{
				cbDebugStages.Items.Add(stage);
			}

			if (cbDebugStages.Items.Count > 0)
			{
				cbDebugStages.SelectedIndex = 0;
			}
		}

		private void UpdateTransformStages()
		{
			cbTransformStages.Items.Clear();

			var methodData = GetCurrentMethodData();

			if (methodData == null)
				return;

			foreach (var stage in methodData.OrderedTransformStageNames)
			{
				cbTransformStages.Items.Add(stage);
			}

			if (cbTransformStages.Items.Count > 0)
			{
				cbTransformStages.SelectedIndex = 0;
			}
		}

		private void UpdateMethodCounters()
		{
			tbMethodCounters.Text = string.Empty;

			var methodData = GetCurrentMethodData();

			if (methodData == null)
				return;

			tbMethodCounters.Text = CreateText(methodData.MethodCounters);

			MethodCounters.Clear();

			var filter = tbCounterFilter.Text;

			foreach (var line in methodData.MethodCounters)
			{
				if (!line.Contains(filter))
					continue;

				var entry = ExtractCounterData(line);

				MethodCounters.Add(entry);
			}
		}

		private void UpdateCompilerCounters()
		{
			CompilerCounters.Clear();

			var lines = CompilerData.GetLog("Counters");

			if (lines == null)
				return;

			var filter = tbCompilerCounterFilter.Text;

			foreach (var line in lines)
			{
				if (!line.Contains(filter))
					continue;

				var entry = ExtractCounterData(line);

				CompilerCounters.Add(entry);
			}

			tbCompilerCounters.Text = CreateText(lines);
		}

		private static CounterEntry ExtractCounterData(string line)
		{
			var index = line.IndexOf(':');
			var name = line.Substring(0, index).Trim();
			var value = line.Substring(index + 1).Trim().ToInt32();
			var entry = new CounterEntry(name, value);
			return entry;
		}

		private static List<string> ExtractLabels(List<string> lines)
		{
			if (lines == null)
				return null;

			var labels = new List<string>();

			foreach (var line in lines)
			{
				if (!line.StartsWith("Block #"))
					continue;

				labels.Add(line.Substring(line.IndexOf("L_")));
			}

			return labels;
		}

		private void UpdateInstructionLabels()
		{
			var lines = GetCurrentInstructionLines();

			cbInstructionLabels.Items.Clear();
			cbInstructionLabels.Items.Add("All");

			var labels = ExtractLabels(lines);

			if (labels != null)
			{
				cbInstructionLabels.Items.AddRange(labels.ToArray());
			}
		}

		private void UpdateTransformLabels()
		{
			var lines = GetCurrentTransformLines();

			cbTransformLabels.Items.Clear();
			cbTransformLabels.Items.Add("All");

			var labels = ExtractLabels(lines);

			if (labels != null)
			{
				cbTransformLabels.Items.AddRange(labels.ToArray());
			}
		}

		private void UpdateInstructions()
		{
			tbInstructions.Text = string.Empty;

			if (CurrentMethodSelected == null)
				return;

			var lines = GetCurrentInstructionLines();
			var label = GetCurrentInstructionLabel();

			SetStatus(CurrentMethodSelected.FullName);

			if (lines == null)
				return;

			if (string.IsNullOrWhiteSpace(label) || label == "All")
				label = string.Empty;

			tbInstructions.Text = MethodStore.GetStageInstructions(lines, label, !showOperandTypes.Checked, padInstructions.Checked);
		}

		private void UpdateTransforms()
		{
			tbTransforms.Text = string.Empty;

			if (CurrentMethodSelected == null)
				return;

			var lines = GetCurrentTransformLines();
			var stage = GetCurrentTransformStage();
			var label = GetCurrentTransformLabel();

			if (lines == null)
				return;

			if (string.IsNullOrWhiteSpace(label) || label == "All")
				label = string.Empty;

			tbTransforms.Text = MethodStore.GetStageInstructions(lines, label, !showOperandTypes.Checked, padInstructions.Checked);
		}

		private void UpdateDebugResults()
		{
			tbDebugResult.Text = string.Empty;

			var lines = GetCurrentDebugLines();

			if (lines == null)
				return;

			tbDebugResult.Text = CreateText(lines);
		}

		private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			CurrentMethodSelected = GetCurrentMethod();
			NodeSelected();
		}

		private void NodeSelected()
		{
			if (Compiler == null)
				return;

			tbInstructions.Text = string.Empty;

			var method = CurrentMethodSelected;

			if (method == null)
				return;

			CompilerData.CompileStartTime = DateTime.Now;

			Compiler.CompileSingleMethod(method);
		}

		private void CbInstructionStages_SelectedIndexChanged(object sender, EventArgs e)
		{
			var previousItemLabel = cbInstructionLabels.SelectedItem;

			UpdateInstructionLabels();

			if (previousItemLabel != null && cbInstructionLabels.Items.Contains(previousItemLabel))
				cbInstructionLabels.SelectedItem = previousItemLabel;
			else
				cbInstructionLabels.SelectedIndex = 0;

			cbInstructionLabels_SelectedIndexChanged(null, null);
		}

		private void cbTransformStages_SelectedIndexChanged(object sender, EventArgs e)
		{
			var previousItemLabel = cbTransformLabels.SelectedItem;

			UpdateTransformLabels();

			if (previousItemLabel != null && cbTransformLabels.Items.Contains(previousItemLabel))
				cbTransformLabels.SelectedItem = previousItemLabel;
			else
				cbTransformLabels.SelectedIndex = 0;

			cbTransformLabels_SelectedIndexChanged(null, null);
		}

		private void cbDebugStages_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateDebugResults();
		}

		private void cbInstructionLabels_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateInstructions();
		}

		private void cbTransformLabels_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateTransforms();
		}

		private void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline)
		{
			pipeline.InsertAfterFirst<TypeInitializerStage>(
					new ExplorerMethodCompileTimeStage()
			);
		}

		private void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline)
		{
			pipeline.Add(new DisassemblyStage());
			pipeline.Add(new DebugInfoStage());

			//pipeline.InsertBefore<GreedyRegisterAllocatorStage>(new StopStage());
			//pipeline.InsertBefore<EnterSSAStage>(new DominanceOutputStage());
			//pipeline.InsertBefore<EnterSSAStage>(new GraphVizStage());

			pipeline.Add(new GraphVizStage());

			//pipeline.InsertAfterFirst<ExceptionStage>(new StopStage());
		}

		private CompilerHooks CreateCompilerHook()
		{
			var compilerHooks = new CompilerHooks
			{
				ExtendCompilerPipeline = ExtendCompilerPipeline,
				ExtendMethodCompilerPipeline = ExtendMethodCompilerPipeline,

				NotifyProgress = NotifyProgress,
				NotifyEvent = NotifyEvent,
				NotifyTraceLog = NotifyTraceLog,
				NotifyMethodCompiled = NotifyMethodCompiled,
				NotifyMethodInstructionTrace = NotifyMethodInstructionTrace,
				NotifyMethodTranformTrace = NotifyMethodTranformTrace,
				GetMethodTraceLevel = GetMethodTraceLevel,
			};

			return compilerHooks;
		}

		public int? GetMethodTraceLevel(MosaMethod method)
		{
			return method == CurrentMethodSelected ? 10 : -1;
		}

		public NotifyTraceLogHandler NotifyMethodInstructionTrace(MosaMethod method)
		{
			if (method != CurrentMethodSelected)
				return null;

			return NotifyMethodInstructionTraceResponse;
		}

		private NotifyTraceLogHandler NotifyMethodTranformTrace(MosaMethod method)
		{
			if (method != CurrentMethodSelected)
				return null;

			return NotifyMethodTransformTraceResponse;
		}

		private void NotifyMethodInstructionTraceResponse(TraceLog traceLog)
		{
			MethodStore.SetInstructionTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines, traceLog.Version);
		}

		private void NotifyMethodTransformTraceResponse(TraceLog traceLog)
		{
			MethodStore.SetTransformTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines, traceLog.Version, traceLog.Step);
		}

		private void NotifyMethodCompiled(MosaMethod method)
		{
			if (method == CurrentMethodSelected)
			{
				Invoke((MethodInvoker)(() => UpdateMethodInformation(method)));
			}
		}

		public void LoadAssembly()
		{
			ClearAll();

			UpdateSettings();

			var compilerHooks = CreateCompilerHook();

			Compiler = new MosaCompiler(Settings, compilerHooks);

			Compiler.Load();

			CreateTree();

			SetStatus("Assemblies Loaded!");
		}

		private static string CreateText(List<string> list)
		{
			if (list == null)
				return string.Empty;

			var result = new StringBuilder();

			foreach (var l in list)
			{
				result.AppendLine(l);
			}

			return result.ToString();
		}

		private void ToolStripButton4_Click(object sender, EventArgs e)
		{
			CompileAll();
		}

		private void UpdateProgressBar()
		{
			toolStripProgressBar1.Maximum = TotalMethods;
			toolStripProgressBar1.Value = CompletedMethods;
		}

		private void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			if (compilerEvent != CompilerEvent.Counter)
			{
				var status = $"{compilerEvent.ToText()}{(string.IsNullOrWhiteSpace(message) ? string.Empty : $": {message}")}";

				lock (_statusLock)
				{
					Status = status;
				}
			}

			SubmitTraceEvent(compilerEvent, message, threadID);
		}

		private void NotifyProgress(int totalMethods, int completedMethods)
		{
			TotalMethods = totalMethods;
			CompletedMethods = completedMethods;
		}

		private void NotifyTraceLog(TraceLog traceLog)
		{
			if (traceLog.Type == TraceType.MethodDebug)
			{
				if (traceLog.Lines.Count == 0)
					return;

				var stagesection = traceLog.Stage;

				if (traceLog.Section != null)
					stagesection = $"{stagesection}-{traceLog.Section}";

				MethodStore.SetDebugStageInformation(traceLog.Method, stagesection, traceLog.Lines, traceLog.Version);
			}
			else if (traceLog.Type == TraceType.MethodCounters)
			{
				MethodStore.SetMethodCounterInformation(traceLog.Method, traceLog.Lines, traceLog.Version);
			}
			else if (traceLog.Type == TraceType.MethodInstructions)
			{
				NotifyMethodInstructionTraceResponse(traceLog);
			}
			else if (traceLog.Type == TraceType.GlobalDebug)
			{
				CompilerData.UpdateLog(traceLog.Section, traceLog.Lines, traceLog.Section == CurrentLogSection);
			}
		}

		private void UpdateMethodInformation(MosaMethod method)
		{
			UpdateInstructionStages();
			UpdateDebugStages();
			UpdateMethodCounters();
			UpdateTransformStages();
		}

		private void DumpAllMethodStagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var method = CurrentMethodSelected;

			if (method == null)
				return;

			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				var path = folderBrowserDialog1.SelectedPath;

				cbInstructionStages.SelectedIndex = 0;

				while (true)
				{
					CbInstructionStages_SelectedIndexChanged(null, null);

					var stage = GetCurrentInstructionStage().Replace("\\", " - ").Replace("/", " - ");
					var result = tbInstructions.Text.Replace("\n", "\r\n");

					File.WriteAllText(Path.Combine(path, stage + "-stage.txt"), result);

					if (cbInstructionStages.Items.Count == cbInstructionStages.SelectedIndex + 1)
						break;

					cbInstructionStages.SelectedIndex++;
				}

				cbDebugStages.SelectedIndex = 0;

				while (true)
				{
					cbDebugStages_SelectedIndexChanged(null, null);

					var stage = GetCurrentDebugStage().Replace("\\", " - ").Replace("/", " - ");
					var result = tbDebugResult.Text.Replace("\n", "\r\n");

					File.WriteAllText(Path.Combine(path, stage + "-debug.txt"), result);

					if (cbDebugStages.Items.Count == cbDebugStages.SelectedIndex + 1)
						break;

					cbDebugStages.SelectedIndex++;
				}
			}
		}

		private void showSizesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateTree();
		}

		private void ClearAll()
		{
			Compiler = null;
			TypeSystemTree = null;

			treeView.Nodes.Clear();
			tbInstructions.Text = string.Empty;
			tbDebugResult.Text = string.Empty;
			MethodStore.Clear();
			MethodCounters.Clear();
			CompilerCounters.Clear();

			ClearAllLogs();
		}

		private void padInstructions_CheckStateChanged(object sender, EventArgs e)
		{
			UpdateInstructions();
			UpdateTransforms();
		}

		private void showOperandTypes_CheckStateChanged(object sender, EventArgs e)
		{
			UpdateInstructions();
			UpdateTransforms();
		}

		private void tbFilter_TextChanged(object sender, EventArgs e)
		{
			CreateTree();
		}

		private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			NodeSelected();
		}

		private void cbCompilerSections_SelectedIndexChanged(object sender, EventArgs e)
		{
			var formatted = cbCompilerSections.SelectedItem as string;

			CurrentLogSection = formatted.Substring(formatted.IndexOf(' ') + 1);

			CompilerData.DirtyLog = true;

			RefreshCompilerLog();
		}

		private void RefreshStatus()
		{
			lock (_statusLock)
			{
				if (Status == null)
					return;

				SetStatus(Status);
				Status = null;
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateProgressBar();
			RefreshCompilerSelectionDropDown();
			RefreshCompilerLog();
			RefreshStatus();
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			CompilerData.DirtyLog = true;

			RefreshCompilerLog();
		}

		private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			CurrentMethodSelected = GetCurrentMethod();
		}

		private void cbEnableAllOptimizations_Click(object sender, EventArgs e)
		{
			ToggleOptimization(true);
		}

		private void cbDisableAllOptimizations_Click(object sender, EventArgs e)
		{
			ToggleOptimization(false);
		}

		private void ToggleOptimization(bool state)
		{
			cbEnableSSA.Checked = state;
			cbEnableBasicOptimizations.Checked = state;
			cbEnableValueNumbering.Checked = state;
			cbEnableSparseConditionalConstantPropagation.Checked = state;
			cbEnableBinaryCodeGeneration.Checked = state;
			cbEnableInline.Checked = state;
			cbInlineExplicit.Checked = state;
			cbEnableLongExpansion.Checked = state;
			cbEnableTwoPassOptimizations.Checked = state;
			cbEnableBitTracker.Checked = state;
			cbLoopInvariantCodeMotion.Checked = state;
			cbPlatformOptimizations.Checked = state;
			cbEnableDevirtualization.Checked = state;
		}

		public void LoadArguments(string[] args)
		{
			SetDefaultSettings();

			var arguments = SettingsLoader.RecursiveReader(args);

			Settings.Merge(arguments);

			UpdateDisplay();
		}

		private void SetDefaultSettings()
		{
			Settings.SetValue("Compiler.BaseAddress", 0x00400000);
			Settings.SetValue("Compiler.Binary", true);
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.TraceLevel", 10);
			Settings.SetValue("Launcher.PlugKorlib", true);
			Settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			Settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			Settings.SetValue("CompilerDebug.MapFile", string.Empty);
			Settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			Settings.SetValue("Optimizations.Basic", true);
			Settings.SetValue("Optimizations.BitTracker", true);
			Settings.SetValue("Optimizations.Inline", true);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Optimizations.Inline.Explicit", true);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.Basic.Window", 5);
			Settings.SetValue("Optimizations.LongExpansion", true);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			Settings.SetValue("Optimizations.Platform", true);
			Settings.SetValue("Optimizations.SCCP", true);
			Settings.SetValue("Optimizations.Devirtualization", true);
			Settings.SetValue("Optimizations.SSA", true);
			Settings.SetValue("Optimizations.TwoPass", true);
			Settings.SetValue("Optimizations.ValueNumbering", true);
			Settings.SetValue("Image.BootLoader", "syslinux3.72");
			Settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA"));
			Settings.SetValue("Image.Format", "IMG");
			Settings.SetValue("Image.FileSystem", "FAT16");
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Multiboot.Video", false);
			Settings.SetValue("Multiboot.Video.Width", 640);
			Settings.SetValue("Multiboot.Video.Height", 480);
			Settings.SetValue("Multiboot.Video.Depth", 32);
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Serial", "none");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", 9999);
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			Settings.SetValue("Launcher.Start", false);
			Settings.SetValue("Launcher.Launch", false);
			Settings.SetValue("Launcher.Exit", false);
			Settings.SetValue("Launcher.HuntForCorLib", true);
			Settings.SetValue("OS.Name", "MOSA");

			Settings.SetValue("CompilerDebug.CILDecodingStageV2", false);
		}

		private void UpdateSettings()
		{
			Settings.SetValue("Compiler.MethodScanner", cbEnableMethodScanner.Checked);
			Settings.SetValue("Compiler.Binary", cbEnableBinaryCodeGeneration.Checked);
			Settings.SetValue("Compiler.TraceLevel", 10);
			Settings.SetValue("Compiler.Platform", cbPlatform.Text);
			Settings.SetValue("Compiler.Multithreading", CBEnableMultithreading.Checked);
			Settings.SetValue("Optimizations.SSA", cbEnableSSA.Checked);
			Settings.SetValue("Optimizations.Basic", cbEnableBasicOptimizations.Checked);
			Settings.SetValue("Optimizations.ValueNumbering", cbEnableValueNumbering.Checked);
			Settings.SetValue("Optimizations.SCCP", cbEnableSparseConditionalConstantPropagation.Checked);
			Settings.SetValue("Optimizations.Devirtualization", cbEnableDevirtualization.Checked);
			Settings.SetValue("Optimizations.BitTracker", cbEnableBitTracker.Checked);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", cbLoopInvariantCodeMotion.Checked);
			Settings.SetValue("Optimizations.LongExpansion", cbEnableLongExpansion.Checked);
			Settings.SetValue("Optimizations.TwoPass", cbEnableTwoPassOptimizations.Checked);
			Settings.SetValue("Optimizations.Platform", cbPlatformOptimizations.Checked);
			Settings.SetValue("Optimizations.Inline", cbEnableInline.Checked);
			Settings.SetValue("Optimizations.Inline.Explicit", cbInlineExplicit.Checked);

			Settings.SetValue("CompilerDebug.CILDecodingStageV2", cbCILDecoderStageV2Testing.Checked);

			//Settings.SetValue("Optimizations.Inline.Maximum", 12);
			//Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Multiboot.Version", "v1");
		}

		private void UpdateSettings(string filename)
		{
			var sourceDirectory = Path.GetDirectoryName(filename);
			var fileHunter = new FileHunter(sourceDirectory);

			var platform = Settings.GetValue("Compiler.Platform");

			// Source Files
			Settings.ClearProperty("Compiler.SourceFiles");
			Settings.AddPropertyListValue("Compiler.SourceFiles", filename);
			Settings.AddPropertyListValue("Compiler.SourceFiles", fileHunter.HuntFile("Mosa.Plug.Korlib.dll")?.FullName);
			Settings.AddPropertyListValue("Compiler.SourceFiles", fileHunter.HuntFile("Mosa.Plug.Korlib." + platform + ".dll")?.FullName);
			Settings.AddPropertyListValue("Compiler.SourceFiles", fileHunter.HuntFile("Mosa.Runtime." + platform + ".dll")?.FullName);

			// Search Paths
			Settings.ClearProperty("SearchPaths");
			Settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(filename));
		}

		private void UpdateDisplay()
		{
			cbEnableInline.Checked = Settings.GetValue("Optimizations.Inline", cbEnableInline.Checked);
			cbEnableSSA.Checked = Settings.GetValue("Optimizations.SSA", cbEnableSSA.Checked);
			cbEnableBasicOptimizations.Checked = Settings.GetValue("Optimizations.Basic", cbEnableBasicOptimizations.Checked);
			cbEnableSparseConditionalConstantPropagation.Checked = Settings.GetValue("Optimizations.SCCP", cbEnableSparseConditionalConstantPropagation.Checked);
			cbEnableDevirtualization.Checked = Settings.GetValue("Optimizations.Devirtualization", cbEnableDevirtualization.Checked);
			cbInlineExplicit.Checked = Settings.GetValue("Optimizations.Inline.Explicit", cbInlineExplicit.Checked);
			cbPlatformOptimizations.Checked = Settings.GetValue("Optimizations.Platform", cbPlatformOptimizations.Checked);
			cbEnableLongExpansion.Checked = Settings.GetValue("Optimizations.LongExpansion", cbEnableLongExpansion.Checked);
			cbEnableTwoPassOptimizations.Checked = Settings.GetValue("Optimizations.TwoPass", cbEnableTwoPassOptimizations.Checked);
			cbLoopInvariantCodeMotion.Checked = Settings.GetValue("Optimizations.LoopInvariantCodeMotion", cbLoopInvariantCodeMotion.Checked);
			cbEnableValueNumbering.Checked = Settings.GetValue("Optimizations.ValueNumbering", cbEnableValueNumbering.Checked);
			cbEnableBitTracker.Checked = Settings.GetValue("Optimizations.BitTracker", cbEnableBitTracker.Checked);
			cbEnableBinaryCodeGeneration.Checked = Settings.GetValue("Compiler.Binary", cbEnableBinaryCodeGeneration.Checked);
			cbEnableMethodScanner.Checked = Settings.GetValue("Compiler.MethodScanner", cbEnableMethodScanner.Checked);
			CBEnableMultithreading.Checked = Settings.GetValue("Compiler.Multithreading", CBEnableMultithreading.Checked);

			tbFilter.Text = Settings.GetValue("Explorer.Filter", tbFilter.Text);

			var platform = Settings.GetValue("Compiler.Platform") ?? "x86";

			switch (platform.ToLowerInvariant())
			{
				case "x86": cbPlatform.SelectedIndex = 0; break;
				case "x64": cbPlatform.SelectedIndex = 1; break;
				case "armv8a32": cbPlatform.SelectedIndex = 2; break;
			}

			cbCILDecoderStageV2Testing.Checked = Settings.GetValue("CompilerDebug.CILDecodingStageV2", cbEnableInline.Checked);
		}

		private void cbPlatform_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearAll();
		}

		private void tbMethodCounterFilter_TextChanged(object sender, EventArgs e)
		{
			UpdateMethodCounters();
		}

		private void tbCompilerCounterFilter_TextChanged(object sender, EventArgs e)
		{
			UpdateCompilerCounters();
		}

		private void UpdateTransform()
		{
			//lbSteps.Text = $"{TransformStep} / {TransformTotalSteps}";
		}

		private void btnFirst_Click(object sender, EventArgs e)
		{
		}
	}
}
