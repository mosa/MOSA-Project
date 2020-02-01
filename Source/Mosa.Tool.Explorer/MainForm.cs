// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Tool.Explorer.Stages;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static Mosa.Compiler.Framework.CompilerHooks;

namespace Mosa.Tool.Explorer
{
	public partial class MainForm : Form
	{
		private readonly Settings Settings = new Settings();

		private DateTime compileStartTime;

		private MosaCompiler Compiler = null;

		private readonly MethodStore methodStore = new MethodStore();

		private TypeSystemTree typeSystemTree;

		private int TotalMethods = 0;
		private int CompletedMethods = 0;
		private string Status = null;

		private MosaMethod CurrentMethodSelected = null;

		private readonly Dictionary<string, List<string>> Logs = new Dictionary<string, List<string>>();
		private readonly List<string> LogSections = new List<string>();

		private string CurrentLogSection = string.Empty;
		private bool DirtyLogDropDown = true;
		private bool DirtyLog = true;

		public MainForm()
		{
			InitializeComponent();

			tbInstructions.Width = tabControl.Width - 12;
			tbInstructions.Height = tabControl.Height - 52;
			tbDebugResult.Width = tabControl.Width - 12;
			tbDebugResult.Height = tabControl.Height - 52;
			tbMethodCounters.Width = tabControl.Width - 12;
			tbMethodCounters.Height = tabControl.Height - 22;

			tbLogs.Width = tabControl.Width - 4;
			tbLogs.Height = tabControl.Height - (22 + 32 + 8);

			cbPlatform.SelectedIndex = 0;

			ClearAll();

			RegisterPlatforms();
		}

		private static void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
		}

		private void ClearAllLogs()
		{
			lock (Logs)
			{
				Logs.Clear();
				LogSections.Clear();
			}

			UpdateLog("Compiler", (string)null);

			ClearSectionDropDown();

			cbSectionLogs.SelectedIndex = 0;
		}

		private void UpdateLog(string section, List<string> lines)
		{
			lock (Logs)
			{
				if (!Logs.TryGetValue(section, out List<string> log))
				{
					log = new List<string>(100);
					Logs.Add(section, log);
					LogSections.Add(section);
					DirtyLogDropDown = true;
				}

				if (log != null)
				{
					log.AddRange(lines);
					DirtyLog = CurrentLogSection == section;
				}
			}
		}

		private void UpdateLog(string section, string line)
		{
			lock (Logs)
			{
				if (!Logs.TryGetValue(section, out List<string> log))
				{
					log = new List<string>(100);
					Logs.Add(section, log);
					LogSections.Add(section);
					DirtyLogDropDown = true;
				}

				if (line != null)
				{
					log.Add(line);
					DirtyLog = CurrentLogSection == section;
				}
			}
		}

		private void ClearSectionDropDown()
		{
			cbSectionLogs.Items.Clear();

			DirtyLogDropDown = true;
			DirtyLog = true;

			RefreshLogDropDown();
			RefreshLog();
		}

		private void RefreshLogDropDown()
		{
			if (!DirtyLogDropDown)
				return;

			DirtyLogDropDown = false;

			lock (Logs)
			{
				for (int i = cbSectionLogs.Items.Count; i < LogSections.Count; i++)
				{
					var formatted = "[" + i.ToString() + "] " + LogSections[i];

					cbSectionLogs.Items.Add(formatted);
				}
			}
		}

		private void RefreshLog()
		{
			if (tabControl.SelectedTab != tabLogs)
				return;

			if (!DirtyLog)
				return;

			DirtyLog = false;

			lock (Logs)
			{
				if (!Logs.ContainsKey(CurrentLogSection))
				{
					tbLogs.Text = string.Empty;
					return;
				}

				var lines = Logs[CurrentLogSection];

				if (lines == null)
				{
					tbLogs.Text = string.Empty;
				}
				else
				{
					tbLogs.Text = CreateText(lines);
				}
			}
		}

		private void SetStatus(string status)
		{
			toolStripStatusLabel.Text = status;
		}

		private void Main_Load(object sender, EventArgs e)
		{
			Text = "MOSA Explorer v" + CompilerVersion.VersionString;

			SetStatus("Ready!");
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
				typeSystemTree = null;
				treeView.Nodes.Clear();
				return;
			}

			var include = GetIncluded(tbFilter.Text, out MosaUnit selected);

			typeSystemTree = new TypeSystemTree(treeView, Compiler.TypeSystem, Compiler.TypeLayout, showSizes.Checked, include);

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
			if (typeSystemTree == null)
			{
				CreateTree();
			}
			else
			{
				typeSystemTree.Update();
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

		private readonly object compilerStageLock = new object();

		private string CreateTimeStampedLog(CompilerEvent compilerEvent, string message, int threadID = 0)
		{
			message = string.IsNullOrWhiteSpace(message) ? string.Empty : $": {message}";

			return $"{(DateTime.Now - compileStartTime).TotalSeconds:0.00} [{threadID.ToString()}] {compilerEvent.ToText()}{message}";
		}

		private void SubmitTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			var log = CreateTimeStampedLog(compilerEvent, message, threadID);

			lock (compilerStageLock)
			{
				if (compilerEvent == CompilerEvent.Error)
				{
					UpdateLog("Error", message);
					UpdateLog("Compiler", log);
				}
				if (compilerEvent == CompilerEvent.Exception)
				{
					UpdateLog("Exception", message);
					UpdateLog("Compiler", log);
				}
				else if (compilerEvent == CompilerEvent.Counter)
				{
					UpdateLog("Counters", message);
				}
				else
				{
					UpdateLog("Compiler", log);
				}
			}
		}

		private void CompileAll()
		{
			if (Compiler == null)
				return;

			compileStartTime = DateTime.Now;

			Compiler.ScheduleAll();

			toolStrip1.Enabled = false;

			var multithread = CBEnableMultithreading.Checked;

			ThreadPool.QueueUserWorkItem(state =>
			{
				try
				{
					if (multithread)
						Compiler.ThreadedCompile();
					else
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

			UpdateTree();
		}

		private void NowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CompileAll();
		}

		private MosaMethod GetCurrentMethod()
		{
			var node = treeView.SelectedNode;

			if (node == null)
				return null;
			else
				return node.Tag as MosaMethod;
		}

		private string GetCurrentStage()
		{
			return cbStages.SelectedItem.ToString();
		}

		private string GetCurrentDebugStage()
		{
			return cbDebugStages.SelectedItem.ToString();
		}

		private string GetCurrentLabel()
		{
			return cbLabels.SelectedItem as string;
		}

		private List<string> GetCurrentLines()
		{
			var method = CurrentMethodSelected;

			if (method == null)
				return null;

			var methodData = methodStore.GetMethodData(method, false);

			if (methodData == null)
				return null;

			string stage = GetCurrentStage();

			return methodData.InstructionLogs[stage];
		}

		private List<string> GetCurrentDebugLines()
		{
			var method = CurrentMethodSelected;

			if (method == null)
				return null;

			var methodData = methodStore.GetMethodData(method, false);

			if (methodData == null)
				return null;

			string stage = GetCurrentDebugStage();

			return methodData.DebugLogs[stage];
		}

		private void UpdateStages()
		{
			var method = CurrentMethodSelected;

			if (method == null)
				return;

			cbStages.Items.Clear();

			var methodData = methodStore.GetMethodData(method, false);

			if (methodData == null)
				return;

			foreach (string stage in methodData.OrderedStageNames)
			{
				cbStages.Items.Add(stage);
			}

			cbStages.SelectedIndex = cbStages.Items.Count == 0 ? -1 : 0;
		}

		private void UpdateDebugStages()
		{
			var method = CurrentMethodSelected;

			if (method == null)
				return;

			cbDebugStages.Items.Clear();

			var methodData = methodStore.GetMethodData(method, false);

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

		private void UpdateCounters()
		{
			var method = CurrentMethodSelected;

			if (method == null)
				return;

			tbMethodCounters.Text = string.Empty;

			var methodData = methodStore.GetMethodData(method, false);

			if (methodData == null)
				return;

			tbMethodCounters.Text = CreateText(methodData.CounterData);
		}

		private void UpdateLabels()
		{
			var lines = GetCurrentLines();

			cbLabels.Items.Clear();
			cbLabels.Items.Add("All");

			foreach (var line in lines)
			{
				if (line.StartsWith("Block #"))
				{
					cbLabels.Items.Add(line.Substring(line.IndexOf("L_")));
				}
			}
		}

		private void UpdateResults()
		{
			tbInstructions.Text = string.Empty;

			var method = CurrentMethodSelected;
			var lines = GetCurrentLines();
			var label = GetCurrentLabel();

			if (method == null)
				return;

			SetStatus(method.FullName);

			if (lines == null)
				return;

			if (string.IsNullOrWhiteSpace(label) || label == "All")
				tbInstructions.Text = methodStore.GetStageInstructions(lines, string.Empty, !showOperandTypes.Checked, padInstructions.Checked);
			else
				tbInstructions.Text = methodStore.GetStageInstructions(lines, label, !showOperandTypes.Checked, padInstructions.Checked);
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

			compileStartTime = DateTime.Now;

			Compiler.CompileSingleMethod(method);
		}

		private void CbStages_SelectedIndexChanged(object sender, EventArgs e)
		{
			var previousItemLabel = cbLabels.SelectedItem;

			UpdateLabels();

			if (previousItemLabel != null && cbLabels.Items.Contains(previousItemLabel))
				cbLabels.SelectedItem = previousItemLabel;
			else
				cbLabels.SelectedIndex = 0;

			CbLabels_SelectedIndexChanged(null, null);
		}

		private void CbDebugStages_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateDebugResults();
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
				NotifyMethodInstructionTrace = NotifyMethodInstructionTrace
			};

			return compilerHooks;
		}

		public NotifyTraceLogHandler NotifyMethodInstructionTrace(MosaMethod method)
		{
			if (method != CurrentMethodSelected)
				return null;

			return NotifyMethodInstructionTraceResponse;
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

		private void CbLabels_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateResults();
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
			message = string.IsNullOrWhiteSpace(message) ? string.Empty : $": {message}";

			var status = $"{compilerEvent.ToText()}{message}";

			lock (_statusLock)
			{
				Status = status;
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
					stagesection = stagesection + "-" + traceLog.Section;

				methodStore.SetDebugStageInformation(traceLog.Method, stagesection, traceLog.Lines, traceLog.Version);
			}
			else if (traceLog.Type == TraceType.MethodCounters)
			{
				methodStore.SetMethodCounterInformation(traceLog.Method, traceLog.Lines, traceLog.Version);
			}
			else if (traceLog.Type == TraceType.MethodInstructions)
			{
				NotifyMethodInstructionTraceResponse(traceLog);
			}
			else if (traceLog.Type == TraceType.GlobalDebug)
			{
				UpdateLog(traceLog.Section, traceLog.Lines);
			}
		}

		private void NotifyMethodInstructionTraceResponse(TraceLog traceLog)
		{
			//if (traceLog.Method != CurrentMethodSelected)
			//	return;

			methodStore.SetInstructionTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines, traceLog.Version);
		}

		private void NotifyMethodCompiled(MosaMethod method)
		{
			if (method == CurrentMethodSelected)
			{
				Invoke((MethodInvoker)(() => UpdateMethodInformation(method)));
			}
		}

		private void UpdateMethodInformation(MosaMethod method)
		{
			UpdateStages();
			UpdateDebugStages();
			UpdateCounters();
		}

		private void DumpAllMethodStagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var method = CurrentMethodSelected;

			if (method == null)
				return;

			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				var path = folderBrowserDialog1.SelectedPath;

				cbStages.SelectedIndex = 0;

				while (true)
				{
					CbStages_SelectedIndexChanged(null, null);

					string stage = GetCurrentStage().Replace("\\", " - ").Replace("/", " - ");
					var result = tbInstructions.Text.Replace("\n", "\r\n");

					File.WriteAllText(Path.Combine(path, stage + "-stage.txt"), result);

					if (cbStages.Items.Count == cbStages.SelectedIndex + 1)
						break;

					cbStages.SelectedIndex++;
				}

				cbDebugStages.SelectedIndex = 0;

				while (true)
				{
					CbDebugStages_SelectedIndexChanged(null, null);

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
			typeSystemTree = null;

			treeView.Nodes.Clear();
			tbInstructions.Text = string.Empty;
			tbDebugResult.Text = string.Empty;
			methodStore.Clear();

			ClearAllLogs();
		}

		private void padInstructions_CheckStateChanged(object sender, EventArgs e)
		{
			UpdateResults();
		}

		private void showOperandTypes_CheckStateChanged(object sender, EventArgs e)
		{
			UpdateResults();
		}

		private void tbFilter_TextChanged(object sender, EventArgs e)
		{
			CreateTree();
		}

		private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			NodeSelected();
		}

		private void cbSections_SelectedIndexChanged(object sender, EventArgs e)
		{
			var formatted = cbSectionLogs.SelectedItem as string;
			CurrentLogSection = formatted.Substring(formatted.IndexOf(' ') + 1);

			DirtyLog = true;
			RefreshLog();
		}

		private readonly object _statusLock = new object();

		private void RefreshStatus()
		{
			lock (_statusLock)
			{
				if (Status != null)
				{
					SetStatus(Status);
					Status = null;
				}
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateProgressBar();
			RefreshLogDropDown();
			RefreshLog();
			RefreshStatus();
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshLog();
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
			cbEnableLongExpansion.Checked = state;
			cbEnableTwoPassOptimizations.Checked = state;
			cbEnableBitTracker.Checked = state;
			cbLoopInvariantCodeMotion.Checked = state;
			cbPlatformOptimizations.Checked = state;
		}

		public void LoadArguments(string[] args)
		{
			SetDefaultSettings();

			var arguments = SettingsLoader.RecursiveReader(args);

			Settings.Merge(arguments);

			UpdateDisplay();

			var sourcefiles = Settings.GetValueList("Compiler.SourceFiles");

			if (sourcefiles != null && sourcefiles.Count >= 1)
			{
				UpdateSettings(Path.GetFullPath(sourcefiles[0]));

				LoadAssembly();
			}
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
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
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
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", cbInlineExplicitOnly.Checked);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
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
			cbInlineExplicitOnly.Checked = Settings.GetValue("Optimizations.Inline.ExplicitOnly", cbInlineExplicitOnly.Checked);
			cbPlatformOptimizations.Checked = Settings.GetValue("Optimizations.Platform", cbPlatformOptimizations.Checked);
			cbEnableLongExpansion.Checked = Settings.GetValue("Optimizations.LongExpansion", cbEnableLongExpansion.Checked);
			cbEnableTwoPassOptimizations.Checked = Settings.GetValue("Optimizations.TwoPass", cbEnableTwoPassOptimizations.Checked);
			cbLoopInvariantCodeMotion.Checked = Settings.GetValue("Optimizations.LoopInvariantCodeMotion", cbLoopInvariantCodeMotion.Checked);
			cbEnableValueNumbering.Checked = Settings.GetValue("Optimizations.ValueNumbering", cbEnableValueNumbering.Checked);
			cbEnableBitTracker.Checked = Settings.GetValue("Optimizations.BitTracker", cbEnableBitTracker.Checked);
			cbEnableBinaryCodeGeneration.Checked = Settings.GetValue("Compiler.Binary", cbEnableBinaryCodeGeneration.Checked);
			cbEnableMethodScanner.Checked = Settings.GetValue("Compiler.MethodScanner", cbEnableMethodScanner.Checked);

			tbFilter.Text = Settings.GetValue("Explorer.Filter", tbFilter.Text);

			var platform = Settings.GetValue("Compiler.Platform");

			if (platform != null)
			{
				if (platform.ToLower() == "x86")
					cbPlatform.SelectedIndex = 0;
				else if (platform.ToLower() == "x64")
					cbPlatform.SelectedIndex = 1;
				else if (platform.ToLower() == "armv8a32")
					cbPlatform.SelectedIndex = 2;
			}
		}

		private void cbPlatform_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearAll();
		}
	}
}
