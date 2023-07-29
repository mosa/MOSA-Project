// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.MosaTypeSystem.CLR;
using Mosa.Tool.Explorer.Stages;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using static Mosa.Compiler.Framework.CompilerHooks;

namespace Mosa.Tool.Explorer;

public partial class MainForm : Form
{
	#region Classes

	protected class CounterEntry
	{
		public CounterEntry(string name, int counter)
		{
			Name = name;
			Value = counter;
		}

		//[Browsable(false)]
		public string Name { get; set; }

		public int Value { get; set; }
	}

	#endregion Classes

	private readonly object _statusLock = new object();
	private readonly BindingList<CounterEntry> CompilerCounters = new BindingList<CounterEntry>();
	private readonly CompilerData CompilerData = new CompilerData();
	private readonly BindingList<CounterEntry> MethodCounters = new BindingList<CounterEntry>();
	private readonly MethodStore MethodStore = new MethodStore();

	private MosaSettings MosaSettings = new MosaSettings();

	private MosaCompiler Compiler = null;
	private int CompletedMethods = 0;
	private string CurrentLogSection = string.Empty;

	private MosaMethod CurrentMethod = null;
	private MethodData CurrentMethodData = null;

	private string Status = null;
	private int TotalMethods = 0;
	private int TransformStep = 0;
	private TypeSystemTree TypeSystemTree;

	private Stopwatch Stopwatch = new Stopwatch();

	private bool GraphwizFound = false;

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


		Stopwatch.Restart();
	}

	public void ClearAllLogs()
	{
		CompilerData.ClearAllLogs();

		ClearSectionDropDown();

		cbCompilerSections.SelectedIndex = 0;
	}

	public int? GetMethodTraceLevel(MosaMethod method)
	{
		return method == CurrentMethod ? 10 : -1;
	}

	public void LoadArguments(string[] args)
	{
		MosaSettings.SetDetfaultSettings();
		MosaSettings.LoadArguments(args);
		MosaSettings.LoadAppLocations();

		SetRequiredSettings();

		GraphwizFound = File.Exists(MosaSettings.GraphwizApp);

		UpdateDisplay();
	}

	public void LoadAssembly()
	{
		ClearAll();

		UpdateSettings();

		var compilerHooks = CreateCompilerHook();

		Compiler = new MosaCompiler(MosaSettings, compilerHooks, new ClrModuleLoader(), new ClrTypeResolver());

		Compiler.Load();

		CreateTree();

		SetStatus("Assemblies Loaded!");
	}

	public NotifyTraceLogHandler NotifyMethodInstructionTrace(MosaMethod method)
	{
		if (method != CurrentMethod)
			return null;

		return NotifyMethodInstructionTraceResponse;
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

	private static string CreateText(List<string> list)
	{
		if (list == null)
			return string.Empty;

		var result = new StringBuilder();

		lock (list)
		{
			foreach (var l in list)
			{
				result.AppendLine(l);
			}
		}

		return result.ToString();
	}

	private static CounterEntry ExtractCounterData(string line)
	{
		var index = line.IndexOf(':');
		var name = line.Substring(0, index).Trim();
		var value = int.Parse(line.Substring(index + 1).Trim());
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

	private static void RegisterPlatforms()
	{
		PlatformRegistry.Add(new Platform.x86.Architecture());
		PlatformRegistry.Add(new Platform.x64.Architecture());
		PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
	}

	private void btnFirst_Click(object sender, EventArgs e)
	{
		SetTranformationStep(0);
	}

	private void btnLast_Click(object sender, EventArgs e)
	{
		SetTranformationStep(int.MaxValue);
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		SetTranformationStep(TransformStep + 1);
	}

	private void btnPrevious_Click(object sender, EventArgs e)
	{
		SetTranformationStep(TransformStep - 1);
	}

	private void cbCompilerSections_SelectedIndexChanged(object sender, EventArgs e)
	{
		var formatted = cbCompilerSections.SelectedItem as string;

		CurrentLogSection = formatted.Substring(formatted.IndexOf(' ') + 1);

		CompilerData.DirtyLog = true;

		RefreshCompilerLog();
	}

	private void cbDebugStages_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateDebugResults();
	}

	private void cbDisableAllOptimizations_Click(object sender, EventArgs e)
	{
		ToggleOptimization(false);
	}

	private void cbEnableAllOptimizations_Click(object sender, EventArgs e)
	{
		ToggleOptimization(true);
	}

	private void cbInstructionLabels_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateInstructions();
	}

	private void cbInstructionStages_SelectedIndexChanged(object sender, EventArgs e)
	{
		var previousItemLabel = cbInstructionLabels.SelectedItem;

		UpdateInstructionLabels();

		if (previousItemLabel != null && cbInstructionLabels.Items.Contains(previousItemLabel))
			cbInstructionLabels.SelectedItem = previousItemLabel;
		else
			cbInstructionLabels.SelectedIndex = 0;

		cbInstructionLabels_SelectedIndexChanged(null, null);
	}

	private void cbPlatform_SelectedIndexChanged(object sender, EventArgs e)
	{
		ClearAll();
	}

	private void cbTransformLabels_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateTransforms();
	}

	private void cbTransformStages_SelectedIndexChanged(object sender, EventArgs e)
	{
		var previousItemLabel = cbTransformLabels.SelectedItem;

		UpdateTransformLabels();

		if (previousItemLabel != null && cbTransformLabels.Items.Contains(previousItemLabel))
			cbTransformLabels.SelectedItem = previousItemLabel;
		else
			cbTransformLabels.SelectedIndex = 0;

		SetTranformationStep(0);

		cbTransformLabels_SelectedIndexChanged(null, null);

		PopulateTransformList();
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

	private void ClearSectionDropDown()
	{
		cbCompilerSections.Items.Clear();

		CompilerData.DirtyLogSections = true;
		CompilerData.DirtyLog = true;

		RefreshCompilerSelectionDropDown();
		RefreshCompilerLog();
	}

	private void CompileAll()
	{
		if (Compiler == null)
			return;

		CompilerData.Stopwatch.Restart();

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

	private void CompileCompleted()
	{
		toolStrip1.Enabled = true;

		SetStatus("Compiled!");

		CompilerData.SortLog("Counters");

		UpdateTree();
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

	private void DumpAllMethodStagesToolStripMenuItem_Click(object sender, EventArgs e)
	{
		var method = CurrentMethod;

		if (method == null)
			return;

		if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
		{
			var path = folderBrowserDialog1.SelectedPath;

			cbInstructionStages.SelectedIndex = 0;

			while (true)
			{
				cbInstructionStages_SelectedIndexChanged(null, null);

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
		//pipeline.InsertAfterLast<IRTransformsStage>(new GraphVizStage());

		//pipeline.InsertAfterFirst<ExceptionStage>(new StopStage());

		if (cbEnableDebugDiagnostic.Checked)
		{
			for (int i = 1; i < pipeline.Count; i += 2)
			{
				pipeline.Insert(i, new GraphVizStage());
			}
		}
		else
		{
			pipeline.Add(new GraphVizStage());
		}
	}

	private List<string> GetCurrentDebugLines()
	{
		if (CurrentMethodData == null)
			return null;

		var stage = GetCurrentDebugStage();

		return CurrentMethodData.DebugLogs[stage];
	}

	private string GetCurrentDebugStage()
	{
		return cbDebugStages.SelectedItem.ToString();
	}

	private string GetCurrentInstructionLabel()
	{
		return cbInstructionLabels.SelectedItem as string;
	}

	private List<string> GetCurrentInstructionLines()
	{
		if (CurrentMethodData == null)
			return null;

		var stage = GetCurrentInstructionStage();

		return CurrentMethodData.InstructionLogs[stage];
	}

	private string GetCurrentInstructionStage()
	{
		return cbInstructionStages.SelectedItem.ToString();
	}

	private MosaMethod GetCurrentMethod()
	{
		var node = treeView.SelectedNode;

		return node == null ? null : node.Tag as MosaMethod;
	}

	private MethodData GetCurrentMethodData()
	{
		var method = CurrentMethod;

		if (method == null)
			return null;

		var methodData = MethodStore.GetMethodData(method, false);

		return methodData;
	}

	private string GetCurrentTransformLabel()
	{
		return cbTransformLabels.SelectedItem as string;
	}

	private List<string> GetCurrentTransformLines()
	{
		if (CurrentMethodData == null)
			return null;

		var stage = GetCurrentTransformStage();

		var logs = CurrentMethodData.TransformLogs[stage];

		logs.TryGetValue(TransformStep, out var log);

		return log;
	}

	private int GetTransformMaxSteps()
	{
		if (CurrentMethodData == null)
			return 0;

		var stage = GetCurrentTransformStage();

		var logs = CurrentMethodData.TransformLogs[stage];

		return logs.Count;
	}

	private string GetCurrentTransformStage()
	{
		return cbTransformStages.SelectedItem.ToString();
	}

	private void Main_Load(object sender, EventArgs e)
	{
		Text = "MOSA Explorer v" + CompilerVersion.VersionString;

		SetStatus("Ready!");

		if (MosaSettings.SourceFiles != null && MosaSettings.SourceFiles.Count >= 1)
		{
			var filename = Path.GetFullPath(MosaSettings.SourceFiles[0]);

			openFileDialog.FileName = filename;

			UpdateSettings(filename);

			LoadAssembly();

			if (MosaSettings.ExplorerStart)
			{
				CompileAll();
			}
		}
	}

	private void NodeSelected()
	{
		if (Compiler == null)
			return;

		tbInstructions.Text = string.Empty;

		var method = CurrentMethod;

		if (method == null)
			return;

		CompilerData.Stopwatch.Reset();

		Compiler.CompileSingleMethod(method);
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

		if (CurrentMethodData == null)
			CurrentMethodData = GetCurrentMethodData();
	}

	private void NotifyMethodCompiled(MosaMethod method)
	{
		if (method == CurrentMethod)
		{
			Invoke((MethodInvoker)(() => UpdateMethodInformation(method)));
		}
	}

	private void NotifyMethodInstructionTraceResponse(TraceLog traceLog)
	{
		MethodStore.SetInstructionTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines, traceLog.Version);
	}

	private NotifyTraceLogHandler NotifyMethodTranformTrace(MosaMethod method)
	{
		if (method != CurrentMethod)
			return null;

		return NotifyMethodTransformTraceResponse;
	}

	private void NotifyMethodTransformTraceResponse(TraceLog traceLog)
	{
		MethodStore.SetTransformTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines, traceLog.Version, traceLog.Step);
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

	private void NowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		CompileAll();
	}

	private void OnCompileCompleted() => Invoke((MethodInvoker)(() => CompileCompleted()));

	private void OpenFileWithDialog()
	{
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			OpenFile();
		}
	}

	private void OpenFile()
	{
		UpdateSettings();

		UpdateSettings(openFileDialog.FileName);

		LoadAssembly();
	}

	private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
	{
		OpenFileWithDialog();
	}

	private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Application.Exit();
	}

	private void RefreshCompilerLog()
	{
		if (!CompilerData.DirtyLog)
			return;

		if (tabControl.SelectedTab == tabLogs)
		{
			tbCompilerLogs.Text = CreateText(CompilerData.GetLog(CurrentLogSection));
		}

		if (tabControl.SelectedTab == tabCompilerCounters)
		{
			UpdateCompilerCounters();
		}

		CompilerData.DirtyLog = false;
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

	private void SetRequiredSettings()
	{
		MosaSettings.TraceLevel = 10;
		MosaSettings.EmulatorSerial = "none";
		MosaSettings.LauncherExit = false;
	}

	private void SetStatus(string status)
	{
		toolStripStatusLabel.Text = $"{CompilerData.Stopwatch.Elapsed.TotalSeconds:00.00} | {status}"; ;
	}

	private void SetTranformationStep(int step)
	{
		if (step <= 0)
			step = 0;

		int max = GetTransformMaxSteps();

		if (step > max - 1)
			step = max - 1;

		TransformStep = step;

		lbSteps.Text = $"{TransformStep} / {max - 1}";

		UpdateTransforms();
	}

	private void ShowSizes_Click(object sender, EventArgs e)
	{
		CreateTree();
	}

	private void showSizesToolStripMenuItem_Click(object sender, EventArgs e)
	{
		CreateTree();
	}

	private void SubmitTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
	{
		CompilerData.AddTraceEvent(compilerEvent, message, threadID);
	}

	private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		CompilerData.DirtyLog = true;

		RefreshCompilerLog();
	}

	private void tbCompilerCounterFilter_TextChanged(object sender, EventArgs e)
	{
		UpdateCompilerCounters();
	}

	private void tbFilter_TextChanged(object sender, EventArgs e)
	{
		CreateTree();
	}

	private void tbMethodCounterFilter_TextChanged(object sender, EventArgs e)
	{
		UpdateMethodCounters();
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		UpdateProgressBar();
		RefreshCompilerSelectionDropDown();
		RefreshCompilerLog();
		RefreshStatus();
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

	private void ToolStripButton1_Click(object sender, EventArgs e)
	{
		OpenFileWithDialog();
	}

	private void ToolStripButton4_Click(object sender, EventArgs e)
	{
		CompileAll();
	}

	private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		CurrentMethod = GetCurrentMethod();
		CurrentMethodData = GetCurrentMethodData();
		NodeSelected();
	}

	private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
	{
		CurrentMethod = GetCurrentMethod();
		CurrentMethodData = GetCurrentMethodData();
	}

	private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		NodeSelected();
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

	private void UpdateDebugResults()
	{
		tbDebugResult.Text = string.Empty;

		var lines = GetCurrentDebugLines();

		if (lines == null)
			return;

		tbDebugResult.Text = CreateText(lines);

		UpdateGraphviz();
	}

	private void UpdateDebugStages()
	{
		cbDebugStages.Items.Clear();

		if (CurrentMethodData == null)
			return;

		foreach (string stage in CurrentMethodData.OrderedDebugStageNames)
		{
			cbDebugStages.Items.Add(stage);
		}

		if (cbDebugStages.Items.Count > 0)
		{
			cbDebugStages.SelectedIndex = 0;
		}
	}

	private void UpdateDisplay()
	{
		cbEnableInline.Checked = MosaSettings.InlineMethods;
		cbEnableSSA.Checked = MosaSettings.SSA;
		cbEnableBasicOptimizations.Checked = MosaSettings.BasicOptimizations;
		cbEnableSparseConditionalConstantPropagation.Checked = MosaSettings.SparseConditionalConstantPropagation;
		cbEnableDevirtualization.Checked = MosaSettings.Devirtualization;
		cbInlineExplicit.Checked = MosaSettings.InlineExplicit;
		cbPlatformOptimizations.Checked = MosaSettings.PlatformOptimizations;
		cbEnableLongExpansion.Checked = MosaSettings.LongExpansion;
		cbEnableTwoPassOptimizations.Checked = MosaSettings.TwoPassOptimization;
		cbLoopInvariantCodeMotion.Checked = MosaSettings.LoopInvariantCodeMotion;
		cbEnableValueNumbering.Checked = MosaSettings.ValueNumbering;
		cbEnableBitTracker.Checked = MosaSettings.BitTracker;
		cbEnableBinaryCodeGeneration.Checked = MosaSettings.EmitBinary;
		cbEnableMethodScanner.Checked = MosaSettings.MethodScanner;
		cbEnableMultithreading.Checked = MosaSettings.Multithreading;
		tbFilter.Text = MosaSettings.ExplorerFilter; ;
		cbEnableDebugDiagnostic.Checked = MosaSettings.DebugDiagnostic;

		cbPlatform.SelectedIndex = MosaSettings.Platform.ToLowerInvariant() switch
		{
			"x86" => 0,
			"x64" => 1,
			"armv8a32" => 2,
			_ => cbPlatform.SelectedIndex
		};

		cbGraphviz.Checked = GraphwizFound;
		cbGraphviz.Enabled = GraphwizFound;
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

	private void UpdateInstructions()
	{
		tbInstructions.Text = string.Empty;

		if (CurrentMethod == null)
			return;

		var lines = GetCurrentInstructionLines();
		var label = GetCurrentInstructionLabel();

		SetStatus(CurrentMethod.FullName);

		if (lines == null)
			return;

		if (string.IsNullOrWhiteSpace(label) || label == "All")
			label = string.Empty;

		tbInstructions.Text = MethodStore.GetStageInstructions(lines, label, !showOperandTypes.Checked, padInstructions.Checked, removeIRNop.Checked);
	}

	private void UpdateInstructionStages()
	{
		cbInstructionStages.Items.Clear();

		if (CurrentMethodData == null)
			return;

		foreach (var stage in CurrentMethodData.OrderedStageNames)
		{
			cbInstructionStages.Items.Add(stage);
		}

		cbInstructionStages.SelectedIndex = cbInstructionStages.Items.Count == 0 ? -1 : 0;
	}

	private void UpdateMethodCounters()
	{
		tbMethodCounters.Text = string.Empty;

		if (CurrentMethodData == null)
			return;

		tbMethodCounters.Text = CreateText(CurrentMethodData.MethodCounters);

		MethodCounters.Clear();

		var filter = tbCounterFilter.Text;

		foreach (var line in CurrentMethodData.MethodCounters)
		{
			if (!line.Contains(filter))
				continue;

			var entry = ExtractCounterData(line);

			MethodCounters.Add(entry);
		}
	}

	private void UpdateMethodInformation(MosaMethod method)
	{
		UpdateInstructionStages();
		UpdateDebugStages();
		UpdateMethodCounters();
		UpdateTransformStages();
	}

	private void UpdateProgressBar()
	{
		toolStripProgressBar1.Maximum = TotalMethods;
		toolStripProgressBar1.Value = CompletedMethods;
	}

	private void UpdateSettings()
	{
		MosaSettings.MethodScanner = cbEnableMethodScanner.Checked;
		MosaSettings.EmitBinary = cbEnableBinaryCodeGeneration.Checked;
		MosaSettings.Platform = cbPlatform.Text;
		MosaSettings.Multithreading = cbEnableMultithreading.Checked;
		MosaSettings.SSA = cbEnableSSA.Checked;
		MosaSettings.BasicOptimizations = cbEnableBasicOptimizations.Checked;
		MosaSettings.ValueNumbering = cbEnableValueNumbering.Checked;
		MosaSettings.SparseConditionalConstantPropagation = cbEnableSparseConditionalConstantPropagation.Checked;
		MosaSettings.Devirtualization = cbEnableDevirtualization.Checked;
		MosaSettings.BitTracker = cbEnableBitTracker.Checked;
		MosaSettings.LoopInvariantCodeMotion = cbLoopInvariantCodeMotion.Checked;
		MosaSettings.LongExpansion = cbEnableLongExpansion.Checked;
		MosaSettings.TwoPassOptimization = cbEnableTwoPassOptimizations.Checked;
		MosaSettings.PlatformOptimizations = cbPlatformOptimizations.Checked;
		MosaSettings.InlineMethods = cbEnableInline.Checked;
		MosaSettings.InlineExplicit = cbInlineExplicit.Checked;

		MosaSettings.TraceLevel = 10;
		MosaSettings.InlineMaximum = 12;
		MosaSettings.InlineAggressiveMaximum = 24;
		MosaSettings.MultibootVersion = "v1";
	}

	private void UpdateSettings(string filename)
	{
		var sourceDirectory = Path.GetDirectoryName(filename);
		var fileHunter = new FileHunter(sourceDirectory);

		// Source Files
		MosaSettings.ClearSourceFiles();
		MosaSettings.AddSourceFile(filename);
		MosaSettings.AddSourceFile(fileHunter.HuntFile("Mosa.Plug.Korlib.dll")?.FullName);
		MosaSettings.AddSourceFile(fileHunter.HuntFile($"Mosa.Plug.Korlib.{MosaSettings.Platform}.dll")?.FullName);
		MosaSettings.AddSourceFile(fileHunter.HuntFile(filename: "Mosa.Runtime." + MosaSettings.Platform + ".dll")?.FullName);

		// Search Paths
		MosaSettings.ClearSearchPaths();
		MosaSettings.AddSearchPath(Path.GetDirectoryName(filename));
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

	private void UpdateTransforms()
	{
		tbTransforms.Text = string.Empty;

		if (CurrentMethod == null)
			return;

		var lines = GetCurrentTransformLines();
		var label = GetCurrentTransformLabel();

		if (lines == null)
			return;

		if (string.IsNullOrWhiteSpace(label) || label == "All")
			label = string.Empty;

		tbTransforms.Text = MethodStore.GetStageInstructions(lines, label, !showOperandTypes.Checked, padInstructions.Checked, removeIRNop.Checked);
	}

	private void UpdateTransformStages()
	{
		cbTransformStages.Items.Clear();

		if (CurrentMethodData == null)
			return;

		foreach (var stage in CurrentMethodData.OrderedTransformStageNames)
		{
			cbTransformStages.Items.Add(stage);
		}

		if (cbTransformStages.Items.Count > 0)
		{
			cbTransformStages.SelectedIndex = 0;
		}
	}

	private void btnSaveA_Click(object sender, EventArgs e)
	{
		File.WriteAllText(Path.Combine(Path.GetTempPath(), "MOSA", "A.txt"), tbInstructions.Text);
	}

	private void btnSaveB_Click(object sender, EventArgs e)
	{
		File.WriteAllText(Path.Combine(Path.GetTempPath(), "MOSA", "B.txt"), tbInstructions.Text);
	}

	private void DisplayCheckStateChanged(object sender, EventArgs e)
	{
		UpdateInstructions();
		UpdateTransforms();
	}

	private void tsbRefresh_Click(object sender, EventArgs e)
	{
		if (File.Exists(openFileDialog.FileName))
		{
			OpenFile();
		}
	}

	private bool DisplayGraphviz()
	{
		panel1.Controls.Clear();

		if (!GraphwizFound)
			return false;

		if (!cbGraphviz.Checked)
			return false;

		if (!tbDebugResult.Text.Contains("digraph blocks"))
			return false;

		var dot = Path.GetTempFileName();
		var bmp = Path.GetTempFileName();

		try
		{
			File.WriteAllText(dot, tbDebugResult.Text);

			var process = new Process();

			process.StartInfo.FileName = MosaSettings.GraphwizApp;
			process.StartInfo.Arguments = $"dot -Tbmp -o \"{bmp}\" \"{dot}\"";
			process.StartInfo.CreateNoWindow = true;

			process.Start();
			process.WaitForExit();

			var file = File.ReadAllBytes(bmp);

			using var stream = new MemoryStream(file);
			var bitmap = new Bitmap(stream);

			var picture = new PictureBox();
			panel1.Controls.Add(picture);

			picture.Size = bitmap.Size;
			picture.SizeMode = PictureBoxSizeMode.AutoSize;
			picture.Image = bitmap;
			picture.BorderStyle = BorderStyle.None;
			picture.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;

			panel1.AutoScrollMinSize = bitmap.Size;
		}
		finally
		{
			File.Delete(dot);
			File.Delete(bmp);
		}

		return true;
	}

	private void UpdateGraphviz()
	{
		var graphviz = DisplayGraphviz();

		panel1.Visible = graphviz;
		tbDebugResult.Visible = !graphviz;
	}

	private void cbGraphviz_CheckedChanged(object sender, EventArgs e)
	{
		UpdateGraphviz();
	}

	private class TranformEntry
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string Before { get; set; }

		public string After { get; set; }

		public string Block { get; set; }

		public int Pass { get; set; }
	}

	private void PopulateTransformList()
	{
		dataGridView1.DataSource = null;

		if (CurrentMethodData == null)
			return;

		var stage = GetCurrentTransformStage();
		var debug = CurrentMethodData.DebugLogs[stage];

		if (debug.Contains("*** Pass"))
			return;

		var list = new List<TranformEntry>();
		//{
		//	new TranformEntry() { ID = -1, Name = "***Start***" }
		//};

		var pass = 0;
		TranformEntry entry = null;

		foreach (var line in debug)
		{
			if (string.IsNullOrEmpty(line))
				continue;

			if (line.StartsWith("*** Pass"))
			{
				pass = Convert.ToInt32(line[10..]);
				continue;
			}

			if (line.StartsWith("Merge Blocking: ") || line.StartsWith("Removed Unreachable Block:"))
				continue;

			var parts = line.Split('\t');

			if (parts.Length != 2)
				continue;

			var part1 = parts[1].Substring(1).Trim();

			if (parts[0].StartsWith("L_"))
			{
				entry.Block = parts[0].TrimEnd();
				entry.Before = part1;
				continue;
			}

			if (parts[0].StartsWith(" "))
			{
				entry.After = part1;
				continue;
			}

			entry = new TranformEntry();

			entry.ID = Convert.ToInt32(parts[0].Trim());
			entry.Name = part1;
			entry.Pass = pass;

			list.Add(entry);
		}

		dataGridView1.DataSource = list;
		dataGridView1.AutoResizeColumns();
	}

	private void dataGridView1_SelectionChanged(object sender, EventArgs e)
	{
		if (dataGridView1.CurrentCell == null)
			return;

		var entry = dataGridView1.CurrentCell.OwningRow.DataBoundItem as TranformEntry;

		if (cbSetBlock.Checked && !string.IsNullOrEmpty(entry.Block))
		{
			cbTransformLabels.SelectedItem = entry.Block;
		}

		SetTranformationStep(entry.ID);
	}

	private void cbSetBlock_CheckedChanged(object sender, EventArgs e)
	{
		dataGridView1_SelectionChanged(sender, e);
	}
}
