// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mosa.Tool.Explorer
{
	public partial class MainForm : Form, ITraceListener
	{
		private readonly CodeForm form = new CodeForm();

		private DateTime compileStartTime;

		public readonly MosaCompiler Compiler = new MosaCompiler(new List<BaseCompilerExtension>() { new ExplorerCompilerExtension() });

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

		private string LastAssemblyFilename = null;
		private string LastIncludeDirectory = null;

		public MainForm()
		{
			InitializeComponent();

			tbInstructions.Width = tabControl.Width - 4;
			tbInstructions.Height = tabControl.Height - 52;
			tbDebugResult.Width = tabControl.Width - 4;
			tbDebugResult.Height = tabControl.Height - 52;
			tbMethodCounters.Width = tabControl.Width - 4;
			tbMethodCounters.Height = tabControl.Height - 22;

			tbLogs.Width = tabControl.Width - 4;
			tbLogs.Height = tabControl.Height - (22 + 32 + 8);

			ClearAllLogs();
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
				LoadAssembly(openFileDialog.FileName);
			}
		}

		private Options ParseOptions(string[] args)
		{
			var result = new Parser(config => config.HelpWriter = Console.Out).ParseArguments<Options>(args);

			if (result.Tag == ParserResultType.NotParsed)
			{
				return null;
			}

			return ((Parsed<Options>)result).Value;
		}

		public void LoadArguments(string[] args)
		{
			var options = ParseOptions(args);

			if (options == null)
				return;

			cbEnableInlinedMethods.Checked = !options.InlineOff;
			cbEnableBinaryCodeGeneration.Checked = !options.NoCode;
			cbEnableSSA.Checked = !options.NoSSA;
			cbEnableIROptimizations.Checked = !options.NoIROptimizations;
			cbEnableSparseConditionalConstantPropagation.Checked = !options.NoSparse;
			cbEnableMethodScanner.Checked = options.EnableMethodScanner;

			tbFilter.Text = options.Filter;

			if (options.X86)
				cbPlatform.SelectedIndex = 0;
			else if (options.X64)
				cbPlatform.SelectedIndex = 1;
			else if (options.ARMv8A32)
				cbPlatform.SelectedIndex = 2;
			else
				cbPlatform.SelectedIndex = 0;

			var files = (IList<string>)options.Files;

			if (files.Count == 1)
			{
				string file = files[0];

				if (file.IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					LoadAssembly(file);
				}
				else
				{
					LoadAssembly(Path.Combine(Directory.GetCurrentDirectory(), file));
				}
			}
		}

		public void LoadAssembly(string filename, string includeDirectory = null)
		{
			LastAssemblyFilename = filename;
			LastIncludeDirectory = includeDirectory;

			ClearAllLogs();
			methodStore.Clear();

			if (filename == null)
				return;

			LoadAssembly(filename, cbPlatform.Text, includeDirectory);

			CreateTree();

			SetStatus("Assemblies Loaded!");
		}

		protected void CreateTree()
		{
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

		protected void UpdateTree(MosaMethod method)
		{
			typeSystemTree.Update(method);
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
			return $"{(DateTime.Now - compileStartTime).TotalSeconds:0.00} [{threadID.ToString()}] {compilerEvent.ToText()}: {message}";
		}

		private void SubmitTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			if (compilerEvent == CompilerEvent.StatusUpdate)
			{
				return;
			}

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

		private void SetCompilerOptions()
		{
			Compiler.CompilerOptions.EnableSSA = cbEnableSSA.Checked;
			Compiler.CompilerOptions.EnableIROptimizations = cbEnableIROptimizations.Checked;
			Compiler.CompilerOptions.EnableValueNumbering = cbEnableValueNumbering.Checked;
			Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = cbEnableSparseConditionalConstantPropagation.Checked;
			Compiler.CompilerOptions.EmitBinary = cbEnableBinaryCodeGeneration.Checked;
			Compiler.CompilerOptions.EnableInlinedMethods = cbEnableInlinedMethods.Checked;
			Compiler.CompilerOptions.EnableLongExpansion = cbEnableLongExpansion.Checked;
			Compiler.CompilerOptions.InlinedIRMaximum = 12;
			Compiler.CompilerOptions.TwoPassOptimizations = cbEnableTwoPassOptimizations.Checked;
			Compiler.CompilerOptions.EnableMethodScanner = cbEnableMethodScanner.Checked;
			Compiler.CompilerOptions.TraceLevel = 8;
			Compiler.CompilerOptions.LinkerFormatType = LinkerFormatType.Elf32;
			Compiler.CompilerOptions.EnableBitTracker = cbEnableBitTracker.Checked;

			Compiler.CompilerTrace.SetTraceListener(this);
		}

		private void CompileAll()
		{
			compileStartTime = DateTime.Now;
			SetCompilerOptions();

			Compiler.ScheduleAll();

			toolStrip1.Enabled = false;

			ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
			{
				try
				{
					//Compiler.Execute();

					Compiler.ThreadedCompile();
				}
				finally
				{
					OnCompileCompleted();
				}
			}));
		}

		private void OnCompileCompleted()
		{
			MethodInvoker call = CompileCompleted;

			Invoke(call);
		}

		private void CompileCompleted()
		{
			toolStrip1.Enabled = true;

			SetStatus("Compiled!");

			UpdateTree();
		}

		private static BaseArchitecture GetArchitecture(string platform)
		{
			switch (platform.ToLower())
			{
				case "x86": return Platform.x86.Architecture.CreateArchitecture(Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				case "x64": return Platform.x64.Architecture.CreateArchitecture(Platform.x64.ArchitectureFeatureFlags.AutoDetect);
				case "armv8a32": return Platform.ARMv8A32.Architecture.CreateArchitecture(Platform.ARMv8A32.ArchitectureFeatureFlags.AutoDetect);
				default: return Platform.x86.Architecture.CreateArchitecture(Platform.x86.ArchitectureFeatureFlags.AutoDetect);
			}
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
			tbInstructions.Text = string.Empty;

			var method = CurrentMethodSelected;

			if (method == null)
				return;

			compileStartTime = DateTime.Now;

			SetCompilerOptions();

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

		private void SnippetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeForm();
		}

		private void ToolStripButton2_Click(object sender, EventArgs e)
		{
			ShowCodeForm();
		}

		protected void LoadAssembly(string filename, string platform, string includeDirectory = null)
		{
			Compiler.CompilerOptions.Architecture = GetArchitecture(platform);
			Compiler.CompilerOptions.MultibootSpecification = MultibootSpecification.V1;

			Compiler.CompilerOptions.SearchPaths.Clear();
			Compiler.CompilerOptions.SourceFiles.Clear();

			Compiler.CompilerOptions.AddSearchPath(includeDirectory);
			Compiler.CompilerOptions.AddSearchPath(Path.GetDirectoryName(filename));

			Compiler.CompilerOptions.AddSourceFile(filename);
			Compiler.CompilerOptions.AddSourceFile("Mosa.Plug.Korlib.dll");
			Compiler.CompilerOptions.AddSourceFile("Mosa.Plug.Korlib." + platform + ".dll");
			Compiler.CompilerOptions.AddSourceFile("Mosa.Runtime." + platform + ".dll");

			Compiler.Load();
		}

		private void ShowCodeForm()
		{
			form.ShowDialog();

			if (form.DialogResult == DialogResult.OK && !string.IsNullOrEmpty(form.Assembly))
			{
				LoadAssembly(form.Assembly, AppDomain.CurrentDomain.BaseDirectory);
			}
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

		void ITraceListener.OnCompilerEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			lock (_statusLock)
			{
				Status = compilerEvent.ToText() + ": " + message;
			}

			SubmitTraceEvent(compilerEvent, message, threadID);
		}

		void ITraceListener.OnProgress(int totalMethods, int completedMethods)
		{
			TotalMethods = totalMethods;
			CompletedMethods = completedMethods;
		}

		void ITraceListener.OnTraceLog(TraceLog traceLog)
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
				methodStore.SetInstructionTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines, traceLog.Version);
			}
			else if (traceLog.Type == TraceType.GlobalDebug)
			{
				UpdateLog(traceLog.Section, traceLog.Lines);
			}
		}

		void ITraceListener.OnMethodCompiled(MosaMethod method)
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

					string stage = GetCurrentStage();
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

					string stage = GetCurrentDebugStage();
					var result = tbDebugResult.Text.Replace("\n", "\r\n");

					File.WriteAllText(Path.Combine(path, stage + "-debug.txt"), result);

					if (cbDebugStages.Items.Count == cbDebugStages.SelectedIndex + 1)
						break;

					cbDebugStages.SelectedIndex++;
				}
			}
		}

		private void CbPlatform_SelectedIndexChanged(object sender, EventArgs e)
		{
			// reload assembly
			LoadAssembly(LastAssemblyFilename, LastIncludeDirectory);
		}

		private void showSizesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateTree();
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
	}
}
