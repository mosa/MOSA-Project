// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Pdb;
using Mosa.Compiler.Trace;
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

		public readonly MosaCompiler Compiler = new MosaCompiler();

		private enum CompileStage { Nothing, Loaded, PreCompiled, Compiled }

		private CompileStage Stage = CompileStage.Nothing;

		private readonly StringBuilder compileLog = new StringBuilder();
		private readonly StringBuilder counterLog = new StringBuilder();
		private readonly StringBuilder errorLog = new StringBuilder();
		private readonly StringBuilder exceptionLog = new StringBuilder();

		private readonly MethodStore methodStore = new MethodStore();

		public MainForm()
		{
			InitializeComponent();

			Compiler.CompilerTrace.TraceListener = this;
			Compiler.CompilerTrace.TraceFilter.Active = true;
			Compiler.CompilerTrace.TraceFilter.ExcludeInternalMethods = false;
			Compiler.CompilerTrace.TraceFilter.MethodMatch = MatchType.Any;
			Compiler.CompilerTrace.TraceFilter.StageMatch = MatchType.Any;

			Compiler.CompilerFactory = delegate { return new ExplorerCompiler(); };
			Compiler.CompilerOptions.LinkerFormatType = LinkerFormatType.Elf32;
		}

		private void SetStatus(string status)
		{
			toolStripStatusLabel.Text = status;
		}

		private void Main_Load(object sender, EventArgs e)
		{
			cbPlatform.SelectedIndex = 0;
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
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				LoadAssembly(openFileDialog.FileName);
			}
		}

		public void LoadArguments(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				var arg = args[i];

				switch (arg.ToLower())
				{
					case "-inline": cbEnableInlinedMethods.Checked = true; continue;
					case "-inline-off": cbEnableInlinedMethods.Checked = false; continue;
					case "-threading-off": cbEnableInlinedMethods.Checked = false; continue;
					case "-no-code": cbEnableBinaryCodeGeneration.Checked = false; continue;
					case "-no-ssa": cbEnableSSA.Checked = false; continue;
					case "-no-ir-optimizations": cbEnableOptimizations.Checked = false; continue;
					case "-no-sparse": cbEnableSparseConditionalConstantPropagation.Checked = false; continue;
					case "-ir-long-operand": cbEnableLongOperand.Checked = true; continue;
					case "-two-pass-optimization": cbEnableTwoPassOptimizationToolStripMenuItem.Checked = true; continue;
					default: break;
				}

				if (arg.IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					LoadAssembly(arg);
				}
				else
				{
					LoadAssembly(Path.Combine(Directory.GetCurrentDirectory(), arg));
				}
			}
		}

		public void LoadAssembly(string filename, string includeDirectory = null)
		{
			LoadAssembly(filename, cbPlatform.Text, includeDirectory);

			UpdateTree();

			Stage = CompileStage.Loaded;

			methodStore.Clear();

			SetStatus("Assemblies Loaded!");
		}

		protected void UpdateTree()
		{
			TypeSystemTree.UpdateTree(treeView, Compiler.TypeSystem, Compiler.TypeLayout, showSizes.Checked);
		}

		private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void ShowTokenValues_Click(object sender, EventArgs e)
		{
			UpdateTree();
		}

		private void ShowSizes_Click(object sender, EventArgs e)
		{
			UpdateTree();
		}

		private void SubmitTraceEventGUI(CompilerEvent compilerStage, string info)
		{
			if (compilerStage != CompilerEvent.DebugInfo)
			{
				SetStatus(compilerStage.ToText() + ": " + info);
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
			Compiler.CompilerOptions.EnableIROptimizations = cbEnableOptimizations.Checked;
			Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = cbEnableSparseConditionalConstantPropagation.Checked;
			Compiler.CompilerOptions.EmitBinary = cbEnableBinaryCodeGeneration.Checked;
			Compiler.CompilerOptions.EnableInlinedMethods = cbEnableInlinedMethods.Checked;
			Compiler.CompilerOptions.EnableIRLongOperand = cbEnableLongOperand.Checked;
			Compiler.CompilerOptions.InlinedIRMaximum = 20;
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

			if (Stage == CompileStage.PreCompiled)
			{
				Compiler.ScheduleAll();
				Compiler.Compile();
				Compiler.PostCompile();
			}
			else
			{
				CleanGUI();

				methodStore.Clear();

				toolStrip1.Enabled = false;

				ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
					{
						try
						{
							Compiler.Execute(Environment.ProcessorCount);
						}
						finally
						{
							OnCompileCompleted();
						}
					}
				));
			}
		}

		private void OnCompileCompleted()
		{
			MethodInvoker call = CompileCompleted;

			Invoke(call);
		}

		private void CompileCompleted()
		{
			toolStrip1.Enabled = true;

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
				case "x86": return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				case "armv6": return Mosa.Platform.ARMv6.Architecture.CreateArchitecture(Mosa.Platform.ARMv6.ArchitectureFeatureFlags.AutoDetect);
				default: return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
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

		private T GetCurrentNode<T>() where T : class
		{
			if (treeView.SelectedNode == null)
				return null;

			return treeView.SelectedNode as T;
		}

		private ViewNode<MosaMethod> GetCurrentNode()
		{
			return GetCurrentNode<ViewNode<MosaMethod>>();
		}

		private MosaMethod GetCurrentType()
		{
			var node = GetCurrentNode<ViewNode<MosaMethod>>();

			if (node == null)
				return null;
			else
				return node.Type;
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
			var type = GetCurrentType();

			if (type == null)
				return null;

			var methodData = methodStore.GetMethodData(type, false);

			if (methodData == null)
				return null;

			string stage = GetCurrentStage();

			return methodData.InstructionLogs[stage];
		}

		private List<string> GetCurrentDebugLines()
		{
			var type = GetCurrentType();

			if (type == null)
				return null;

			var methodData = methodStore.GetMethodData(type, false);

			if (methodData == null)
				return null;

			string stage = GetCurrentDebugStage();

			return methodData.DebugLogs[stage];
		}

		private void UpdateStages()
		{
			var type = GetCurrentType();

			if (type == null)
				return;

			cbStages.Items.Clear();

			var methodData = methodStore.GetMethodData(type, false);

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
			var type = GetCurrentType();

			if (type == null)
				return;

			cbDebugStages.Items.Clear();

			var methodData = methodStore.GetMethodData(type, false);

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
			var type = GetCurrentType();

			if (type == null)
				return;

			rbMethodCounters.Text = string.Empty;

			var methodData = methodStore.GetMethodData(type, false);

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

			var type = GetCurrentType();
			var lines = GetCurrentLines();
			var label = GetCurrentLabel();

			if (type == null)
				return;

			SetStatus(type.FullName);

			if (lines == null)
				return;

			if (string.IsNullOrWhiteSpace(label) || label == "All")
				tbResult.Text = methodStore.GetStageInstructions(lines, string.Empty);
			else
				tbResult.Text = methodStore.GetStageInstructions(lines, label);
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
			tbResult.Text = string.Empty;

			var type = GetCurrentType();

			if (type == null)
				return;

			PreCompile();

			if (!Compiler.CompilationScheduler.IsScheduled(type))
			{
				Compiler.Schedule(type);
				Compiler.Compile();
			}

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

			var moduleLoader = new MosaModuleLoader();

			if (includeDirectory != null)
				moduleLoader.AddPrivatePath(includeDirectory);

			moduleLoader.AddPrivatePath(Path.GetDirectoryName(filename));
			moduleLoader.LoadModuleFromFile(filename);

			var metadata = moduleLoader.CreateMetadata();

			var typeSystem = TypeSystem.Load(metadata);

			Compiler.Load(typeSystem);
		}

		private void ShowCodeForm()
		{
			form.ShowDialog();

			if (form.DialogResult == DialogResult.OK)
			{
				if (!string.IsNullOrEmpty(form.Assembly))
				{
					LoadAssembly(form.Assembly, AppDomain.CurrentDomain.BaseDirectory);
				}
			}
		}

		private void ToolStripButton3_Click(object sender, EventArgs e)
		{
			Compile();
		}

		private void CbLabels_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateResults();
		}

		private string CreateText(List<string> list)
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

			MethodInvoker call = () => SubmitTraceEventGUI(compilerEvent, message);

			Invoke(call);
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

		protected void LoadAssemblyDebugInfo(string assemblyFileName)
		{
			string dbgFile = Path.ChangeExtension(assemblyFileName, "pdb");

			if (File.Exists(dbgFile))
			{
				tbResult.AppendText("File: " + dbgFile + "\n");
				tbResult.AppendText("======================\n");
				using (FileStream fileStream = new FileStream(dbgFile, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (PdbReader reader = new PdbReader(fileStream))
					{
						tbResult.AppendText("Global targetSymbols: \n");
						tbResult.AppendText("======================\n");
						foreach (CvSymbol symbol in reader.GlobalSymbols)
						{
							tbResult.AppendText(symbol + "\n");
						}

						tbResult.AppendText("Types:\n");
						foreach (PdbType type in reader.Types)
						{
							tbResult.AppendText(type.Name + "\n");
							tbResult.AppendText("======================\n");
							tbResult.AppendText("Symbols:\n");
							foreach (CvSymbol symbol in type.Symbols)
							{
								tbResult.AppendText("\t" + symbol + "\n");
							}

							tbResult.AppendText("Lines:\n");
							foreach (CvLine line in type.LineNumbers)
							{
								tbResult.AppendText("\t" + line.ToString() + "\n");
							}
						}
					}
				}
			}
		}

		private void DumpAllMethodStagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var type = GetCurrentType();

			if (type == null)
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
	}
}
