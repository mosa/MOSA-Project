// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.GUI.Common;
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

		private enum CompileStage { Nothing, Loaded, PreCompiled, Compiled }

		private CompileStage Stage = CompileStage.Nothing;

		private readonly StringBuilder compileLog = new StringBuilder();
		private readonly StringBuilder counterLog = new StringBuilder();
		private readonly StringBuilder errorLog = new StringBuilder();
		private readonly StringBuilder exceptionLog = new StringBuilder();

		private readonly MethodStore methodStore = new MethodStore();

		private TypeSystemTree typeSystemTree;

		public MainForm()
		{
			InitializeComponent();

			Compiler.CompilerTrace.TraceListener = this;
			Compiler.CompilerTrace.TraceFilter.Active = true;
			Compiler.CompilerTrace.TraceFilter.ExcludeInternalMethods = false;
			Compiler.CompilerTrace.TraceFilter.MethodMatch = MatchType.Any;
			Compiler.CompilerTrace.TraceFilter.StageMatch = MatchType.Any;

			Compiler.CompilerOptions.LinkerFormatType = LinkerFormatType.Elf32;
		}

		private void SetStatus(string status)
		{
			toolStripStatusLabel.Text = status;
		}

		private void Main_Load(object sender, EventArgs e)
		{
			Text = "MOSA Explorer v" + CompilerVersion.Version;

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

			cbEnableInlinedMethods.Checked = options.Inline;
			cbEnableBinaryCodeGeneration.Checked = !options.NoCode;
			cbEnableSSA.Checked = !options.NoSSA;
			cbEnableIROptimizations.Checked = !options.NoIROptimizations;
			cbEnableSparseConditionalConstantPropagation.Checked = !options.NoSparse;
			cbEnableMethodScanner.Checked = options.EnableMethodScanner;

			tbFilter.Text = options.Filter;

			if (options.X86)
				cbPlatform.SelectedIndex = 0;

			if (options.X64)
				cbPlatform.SelectedIndex = 1;

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
			LoadAssembly(filename, cbPlatform.Text, includeDirectory);

			CreateTree();

			Stage = CompileStage.Loaded;

			methodStore.Clear();

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

		private void SubmitTraceEventGUI(CompilerEvent compilerEvent, string info)
		{
			if (compilerEvent != CompilerEvent.DebugInfo)
			{
				SetStatus(compilerEvent.ToText() + ": " + info);
				toolStripStatusLabel1.GetCurrentParent().Refresh();
			}
		}

		private readonly object compilerStageLock = new object();

		private void SubmitTraceEvent(CompilerEvent compilerStage, string message, int threadID)
		{
			lock (compilerStageLock)
			{
				if (compilerStage == CompilerEvent.Error)
				{
					errorLog.Append(compilerStage.ToText()).Append(": ").AppendLine(message);
					compileLog.AppendFormat("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds).Append(" [").Append(threadID.ToString()).Append("] ").Append(compilerStage.ToText()).Append(": ").AppendLine(message);
				}
				if (compilerStage == CompilerEvent.Exception)
				{
					var stringBuilder = exceptionLog.Append(compilerStage.ToText()).Append(": ").AppendLine(message);
					var stringBuilder2 = compileLog.AppendFormat("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds).Append(" [").Append(threadID.ToString()).Append("] ").Append(compilerStage.ToText()).Append(": ").AppendLine(message);
				}
				else if (compilerStage == CompilerEvent.Counter)
				{
					counterLog.Append(compilerStage.ToText()).Append(": ").AppendLine(message);
				}
				else
				{
					compileLog.AppendFormat("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds).Append(" [").Append(threadID.ToString()).Append("] ").Append(compilerStage.ToText()).Append(": ").AppendLine(message);
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
			Compiler.CompilerOptions.IRLongExpansion = cbEnableIRLongExpansion.Checked;
			Compiler.CompilerOptions.InlinedIRMaximum = 12;
			Compiler.CompilerOptions.TwoPassOptimizations = cbEnableTwoPassOptimizations.Checked;
			Compiler.CompilerOptions.TraceLevel = 100;
			Compiler.CompilerOptions.EnableMethodScanner = cbEnableMethodScanner.Checked;
		}

		private void CleanGUI()
		{
			compileLog.Clear();
			errorLog.Clear();
			counterLog.Clear();
			exceptionLog.Clear();

			rbLog.Text = string.Empty;
			rbErrors.Text = string.Empty;
			rbGlobalCounters.Text = string.Empty;
			rbException.Text = string.Empty;
		}

		private void Compile()
		{
			compileStartTime = DateTime.Now;
			SetCompilerOptions();

			CleanGUI();

			Compiler.ScheduleAll();

			toolStrip1.Enabled = false;

			ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
			{
				try
				{
					Compiler.Execute();

					//Compiler.ExecuteThreaded();
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

			//Compiler.PostCompile();

			Stage = CompileStage.Compiled;

			SetStatus("Compiled!");

			tabControl1.SelectedTab = tbStages;

			rbLog.Text = compileLog.ToString();
			rbErrors.Text = errorLog.ToString();
			rbGlobalCounters.Text = counterLog.ToString();
			rbException.Text = exceptionLog.ToString();

			UpdateTree();
		}

		private static BaseArchitecture GetArchitecture(string platform)
		{
			switch (platform.ToLower())
			{
				case "x86": return Platform.x86.Architecture.CreateArchitecture(Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				case "x64": return Platform.x64.Architecture.CreateArchitecture(Platform.x64.ArchitectureFeatureFlags.AutoDetect);
				case "armv6": return Platform.ARMv6.Architecture.CreateArchitecture(Platform.ARMv6.ArchitectureFeatureFlags.AutoDetect);
				case "esp32": return Platform.ESP32.Architecture.CreateArchitecture(Platform.ESP32.ArchitectureFeatureFlags.AutoDetect);
				default: return Platform.x86.Architecture.CreateArchitecture(Platform.x86.ArchitectureFeatureFlags.AutoDetect);
			}
		}

		private void NowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Compile();
		}

		private void PreCompile()
		{
			if (Stage == CompileStage.Loaded)
			{
				SetCompilerOptions();
				Compiler.Initialize();
				Compiler.PreCompile();
				Stage = CompileStage.PreCompiled;
			}
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
			var method = GetCurrentMethod();

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
			var method = GetCurrentMethod();

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
			var method = GetCurrentMethod();

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

			cbStages.SelectedIndex = 0;
		}

		private void UpdateDebugStages()
		{
			var method = GetCurrentMethod();

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
			var method = GetCurrentMethod();

			if (method == null)
				return;

			rbMethodCounters.Text = string.Empty;

			var methodData = methodStore.GetMethodData(method, false);

			if (methodData == null)
				return;

			rbMethodCounters.Text = CreateText(methodData.CounterData);
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
			tbResult.Text = string.Empty;

			var method = GetCurrentMethod();
			var lines = GetCurrentLines();
			var label = GetCurrentLabel();

			if (method == null)
				return;

			SetStatus(method.FullName);

			if (lines == null)
				return;

			if (string.IsNullOrWhiteSpace(label) || label == "All")
				tbResult.Text = methodStore.GetStageInstructions(lines, string.Empty, !showOperandTypes.Checked, padInstructions.Checked);
			else
				tbResult.Text = methodStore.GetStageInstructions(lines, label, !showOperandTypes.Checked, padInstructions.Checked);
		}

		private void UpdateDebugResults()
		{
			rbDebugResult.Text = string.Empty;

			var lines = GetCurrentDebugLines();

			if (lines == null)
				return;

			rbDebugResult.Text = CreateText(lines);
		}

		private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			NodeSelected();
		}

		private void NodeSelected()
		{
			tbResult.Text = string.Empty;

			var method = GetCurrentMethod();

			if (method == null)
				return;

			PreCompile();

			if (Compiler.Linker != null && !Compiler.CompilationScheduler.IsScheduled(method))
			{
				Compiler.Schedule(method);
				Compiler.Compile();
			}

			UpdateTree(method);

			UpdateStages();
			UpdateDebugStages();
			UpdateCounters();
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

			Compiler.CompilerOptions.SearchPaths.Clear();
			Compiler.CompilerOptions.SourceFiles.Clear();

			Compiler.CompilerOptions.AddSearchPath(includeDirectory);
			Compiler.CompilerOptions.AddSearchPath(Path.GetDirectoryName(filename));

			Compiler.CompilerOptions.AddSourceFile(filename);
			Compiler.CompilerOptions.AddSourceFile("Mosa.Plug.Korlib.dll");
			Compiler.CompilerOptions.AddSourceFile("Mosa.Plug.Korlib." + platform + ".dll");

			var moduleLoader = new MosaModuleLoader();

			moduleLoader.AddSearchPaths(Compiler.CompilerOptions.SearchPaths);
			moduleLoader.LoadModuleFromFiles(Compiler.CompilerOptions.SourceFiles);

			var metadata = moduleLoader.CreateMetadata();

			var typeSystem = TypeSystem.Load(metadata);

			Compiler.Load(typeSystem);
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
			Compile();
		}

		private void SubmitMethodStatus(int totalMethods, int completedMethods)
		{
			toolStripProgressBar1.Maximum = totalMethods;
			toolStripProgressBar1.Value = completedMethods;
		}

		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			SubmitTraceEvent(compilerEvent, message, threadID);

			if (compilerEvent != CompilerEvent.DebugInfo)
			{
				MethodInvoker call = () => SubmitTraceEventGUI(compilerEvent, message);
				Invoke(call);
			}
		}

		void ITraceListener.OnUpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
			MethodInvoker call = () => SubmitMethodStatus(totalMethods, completedMethods);

			Invoke(call);
		}

		void ITraceListener.OnNewTraceLog(TraceLog traceLog)
		{
			if (traceLog.Type == TraceType.DebugTrace)
			{
				if (traceLog.Lines.Count == 0)
					return;

				var stagesection = traceLog.Stage;

				if (traceLog.Section != null)
					stagesection = stagesection + "-" + traceLog.Section;

				methodStore.SetDebugStageInformation(traceLog.Method, stagesection, traceLog.Lines);
			}
			else if (traceLog.Type == TraceType.Counters)
			{
				methodStore.SetMethodCounterInformation(traceLog.Method, traceLog.Lines);
			}
			else if (traceLog.Type == TraceType.InstructionList)
			{
				methodStore.SetInstructionTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines);
			}
		}

		void ITraceListener.OnMethodcompiled(MosaMethod method)
		{
			//MethodInvoker call = () => UpdateTree(method);

			//Invoke(call);
		}

		private void DumpAllMethodStagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var method = GetCurrentMethod();

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
					var result = tbResult.Text.Replace("\n", "\r\n");

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
					var result = rbDebugResult.Text.Replace("\n", "\r\n");

					File.WriteAllText(Path.Combine(path, stage + "-debug.txt"), result);

					if (cbDebugStages.Items.Count == cbDebugStages.SelectedIndex + 1)
						break;

					cbDebugStages.SelectedIndex++;
				}
			}
		}

		private void CbPlatform_SelectedIndexChanged(object sender, EventArgs e)
		{
			//
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
	}
}
