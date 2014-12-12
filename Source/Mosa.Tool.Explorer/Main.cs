/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
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
	public partial class Main : Form, ITraceListener
	{
		private CodeForm form = new CodeForm();

		private DateTime compileStartTime;
		private List<string> currentInstructionLines;

		private MosaCompiler Compiler = new MosaCompiler();

		private Dictionary<MosaMethod, MethodStages> methodStages = new Dictionary<MosaMethod, MethodStages>();

		private enum CompileStage { Nothing, Loaded, PreCompiled, Compiled };

		private CompileStage Stage = CompileStage.Nothing;

		private class MethodStages
		{
			public List<string> OrderedStageNames = new List<string>();
			public List<string> OrderedDebugStageNames = new List<string>();
			public Dictionary<string, List<string>> InstructionLogs = new Dictionary<string, List<string>>();
			public Dictionary<string, string> DebugLogs = new Dictionary<string, string>();
		}

		private StringBuilder compileLog = new StringBuilder();
		private StringBuilder counterLog = new StringBuilder();
		private StringBuilder errorLog = new StringBuilder();
		private StringBuilder exceptionLog = new StringBuilder();

		public Main()
		{
			InitializeComponent();

			Compiler.CompilerTrace.TraceListener = this;
			Compiler.CompilerTrace.TraceFilter.Active = true;
			Compiler.CompilerTrace.TraceFilter.ExcludeInternalMethods = false;
			Compiler.CompilerTrace.TraceFilter.MethodMatch = MatchType.Any;
			Compiler.CompilerTrace.TraceFilter.StageMatch = MatchType.Exclude;
			Compiler.CompilerTrace.TraceFilter.Stage = "PlatformStubStage|ProtectedRegionLayoutStage|DominanceCalculationStage|CodeGenerationStage";

			Compiler.CompilerFactory = delegate { return new ExplorerCompiler(); };
			Compiler.CompilerOptions.LinkerFactory = delegate { return new ExplorerLinker(); };
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

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
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

		public void LoadAssembly(string filename)
		{
			if (Path.GetFileName(filename) == "Mosa.Test.Collection.dll" || Path.GetFileName(filename) == "Mosa.Kernel.x86Test.dll")
			{
				includeTestKorlibToolStripMenuItem.Checked = true;
			}

			LoadAssembly(filename, includeTestKorlibToolStripMenuItem.Checked, cbPlatform.Text);

			methodStages.Clear();

			SetStatus("Assemblies Loaded!");
		}

		protected void UpdateTree()
		{
			TypeSystemTree.UpdateTree(treeView, Compiler.TypeSystem, Compiler.TypeLayout, showSizes.Checked);
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void showTokenValues_Click(object sender, EventArgs e)
		{
			UpdateTree();
		}

		private void showSizes_Click(object sender, EventArgs e)
		{
			UpdateTree();
		}

		private void SubmitTraceEventGUI(CompilerEvent compilerStage, string info, int threadID)
		{
			if (compilerStage != CompilerEvent.DebugInfo)
			{
				SetStatus(compilerStage.ToText() + ": " + info);
				toolStripStatusLabel1.GetCurrentParent().Refresh();
			}
		}

		private object compilerStageLock = new object();

		private void SubmitTraceEvent(CompilerEvent compilerStage, string message, int threadID)
		{
			lock (compilerStageLock)
			{
				if (compilerStage == CompilerEvent.Error)
				{
					errorLog.AppendLine(compilerStage.ToText() + ": " + message);
					compileLog.AppendLine(String.Format("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds) + " [" + threadID.ToString() + "] " + compilerStage.ToText() + ": " + message);
				}
				if (compilerStage == CompilerEvent.Exception)
				{
					exceptionLog.AppendLine(compilerStage.ToText() + ": " + message);
					compileLog.AppendLine(String.Format("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds) + " [" + threadID.ToString() + "] " + compilerStage.ToText() + ": " + message);
				}
				else if (compilerStage == CompilerEvent.Counter)
				{
					counterLog.AppendLine(compilerStage.ToText() + ": " + message);
				}
				else
				{
					compileLog.AppendLine(String.Format("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds) + " [" + threadID.ToString() + "] " + compilerStage.ToText() + ": " + message);
				}
			}
		}

		private object methodStagelock = new object();

		private void SubmitInstructionTraceInformation(MosaMethod method, string stage, List<string> lines)
		{
			lock (methodStagelock)
			{
				MethodStages methodStage;

				if (!methodStages.TryGetValue(method, out methodStage))
				{
					methodStage = new MethodStages();
					methodStages.Add(method, methodStage);
				}

				methodStage.OrderedStageNames.AddIfNew(stage);

				methodStage.InstructionLogs.Remove(stage);

				methodStage.InstructionLogs.Add(stage, lines);
			}
		}

		private void SubmitDebugStageInformation(MosaMethod method, string stage, string lines)
		{
			lock (methodStagelock)
			{
				MethodStages methodStage;

				if (!methodStages.TryGetValue(method, out methodStage))
				{
					methodStage = new MethodStages();
					methodStages.Add(method, methodStage);
				}

				methodStage.OrderedDebugStageNames.AddIfNew(stage);

				methodStage.DebugLogs.Remove(stage);

				methodStage.DebugLogs.Add(stage, lines);
			}
		}

		private void SetCompilerOptions()
		{
			Compiler.CompilerOptions.EnableSSA = enableSSA.Checked;
			Compiler.CompilerOptions.EnableOptimizations = enableOptimizations.Checked;
			Compiler.CompilerOptions.EnablePromoteTemporaryVariablesOptimization = enableOptimizations.Checked;
			Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = enableSparseConditionalConstantPropagation.Checked;
			Compiler.CompilerOptions.EmitBinary = enableBinaryCodeGeneration.Checked;
		}

		private void CleanGUI()
		{
			compileLog.Clear();
			errorLog.Clear();
			counterLog.Clear();
			exceptionLog.Clear();

			rbLog.Text = string.Empty;
			rbErrors.Text = string.Empty;
			rbCounters.Text = string.Empty;
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

				methodStages.Clear();

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
			MethodInvoker call = delegate()
			{
				CompileCompleted();
			};

			Invoke(call);
		}

		private void CompileCompleted()
		{
			toolStrip1.Enabled = true;

			Stage = CompileStage.Compiled;

			SetStatus("Compiled!");

			tabControl1.SelectedTab = tabPage1;

			rbLog.Text = compileLog.ToString();
			rbErrors.Text = errorLog.ToString();
			rbCounters.Text = counterLog.ToString();
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

		private void nowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Compile();
		}

		private T GetCurrentNode<T>() where T : class
		{
			if (treeView.SelectedNode == null)
				return null;

			T node = treeView.SelectedNode as T;

			return node;
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

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			tbResult.Text = string.Empty;

			var node = GetCurrentNode<ViewNode<MosaMethod>>();

			if (node == null)
				return;

			PreCompile();

			if (!Compiler.CompilationScheduler.IsScheduled(node.Type))
			{
				Compiler.Schedule(node.Type);

				Compiler.Compile();
			}

			MethodStages methodStage;

			if (!methodStages.TryGetValue(node.Type, out methodStage))
				return;

			cbStages.Items.Clear();

			foreach (string stage in methodStage.OrderedStageNames)
				cbStages.Items.Add(stage);

			cbStages.SelectedIndex = 0;

			cbDebugStages.Items.Clear();

			foreach (string stage in methodStage.OrderedDebugStageNames)
				cbDebugStages.Items.Add(stage);

			if (cbDebugStages.Items.Count > 0)
				cbDebugStages.SelectedIndex = 0;
		}

		private void cbStages_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentInstructionLines = null;

			var node = GetCurrentNode<ViewNode<MosaMethod>>();

			if (node == null)
				return;

			MethodStages methodStage;

			if (!methodStages.TryGetValue(node.Type, out methodStage))
				return;

			string stage = cbStages.SelectedItem.ToString();

			currentInstructionLines = methodStage.InstructionLogs[stage];
			var previousItemLabel = cbLabels.SelectedItem;

			cbLabels.Items.Clear();
			cbLabels.Items.Add("All");

			foreach (string line in currentInstructionLines)
			{
				if (line.StartsWith("Block #"))
				{
					cbLabels.Items.Add(line.Substring(line.IndexOf("L_")));
				}
			}

			if (previousItemLabel != null && cbLabels.Items.Contains(previousItemLabel))
				cbLabels.SelectedItem = previousItemLabel;
			else
				cbLabels.SelectedIndex = 0;

			cbLabels_SelectedIndexChanged(null, null);
		}

		private string ReplaceTypeName(string value)
		{
			if (value.Contains("-") || value.Contains("+"))
				return value;

			switch (value)
			{
				case "System.Object": return "O";
				case "System.Char": return "C";
				case "System.Void": return "V";
				case "System.String": return "String";
				case "System.Byte": return "U1";
				case "System.SByte": return "I1";
				case "System.Boolean": return "B";
				case "System.Int8": return "I1";
				case "System.UInt8": return "U1";
				case "System.Int16": return "I2";
				case "System.UInt16": return "U2";
				case "System.Int32": return "I4";
				case "System.UInt32": return "U4";
				case "System.Int64": return "I8";
				case "System.UInt64": return "U8";
				case "System.Single": return "R4";
				case "System.Double": return "R8";
				//default: return "O";
			}

			return value;
		}

		private string ReplaceType(string value)
		{
			if (value.Length < 2)
				return value;

			string type = value;
			string end = string.Empty;

			if (value.EndsWith("*"))
			{
				type = value.Substring(0, value.Length - 1);
				end = "*";
			}
			if (value.EndsWith("&"))
			{
				type = value.Substring(0, value.Length - 1);
				end = "&";
			}
			if (value.EndsWith("[]"))
			{
				type = value.Substring(0, value.Length - 1);
				end = "[]";
			}

			return ReplaceTypeName(type) + end;
		}

		private string UpdateParameter(string p)
		{
			if (!(p.StartsWith("[") && p.EndsWith("]")))
				return p;

			string value = p.Substring(1, p.Length - 2);

			return "[" + ReplaceType(value) + "]";
		}

		private string UpdateLine(string line)
		{
			if (!line.StartsWith("L_"))
				return line;

			string l = line;
			int at = 0;

			while (true)
			{
				int s = l.IndexOf('[', at);

				if (s < 0)
					break;

				int e = l.IndexOf(']', s);

				if (e < 0)
					break;

				string oldvalue = l.Substring(s, e - s + 1);

				string newvalue = UpdateParameter(oldvalue);

				at = s + 1;

				l = l.Substring(0, s) + newvalue + l.Substring(e + 1);
			}

			return l;
		}

		private void cbDebugStages_SelectedIndexChanged(object sender, EventArgs e)
		{
			var node = GetCurrentNode<ViewNode<MosaMethod>>();

			if (node == null)
				return;

			MethodStages methodStage;

			if (methodStages.TryGetValue(node.Type, out methodStage))
			{
				string stage = cbDebugStages.SelectedItem.ToString();

				if (methodStage.DebugLogs.ContainsKey(stage))
					rbOtherResult.Text = methodStage.DebugLogs[stage].ToString();
				else
					rbOtherResult.Text = string.Empty;
			}
		}

		private void snippetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeForm();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			ShowCodeForm();
		}

		protected void LoadAssembly(string filename, bool includeTestComponents, string platform)
		{
			Compiler.CompilerOptions.Architecture = GetArchitecture(cbPlatform.Text);

			var moduleLoader = new MosaModuleLoader();

			if (includeTestComponents)
			{
				moduleLoader.AddPrivatePath(System.IO.Directory.GetCurrentDirectory());
				moduleLoader.LoadModuleFromFile("mscorlib.dll");
				moduleLoader.LoadModuleFromFile("Mosa.Platform.Internal." + platform + ".dll");
				moduleLoader.LoadModuleFromFile("Mosa.Kernel.x86Test.dll");
			}

			moduleLoader.AddPrivatePath(Path.GetDirectoryName(filename));
			moduleLoader.LoadModuleFromFile(filename);

			var metadata = moduleLoader.CreateMetadata();

			Compiler.Load(TypeSystem.Load(metadata));

			UpdateTree();

			Stage = CompileStage.Loaded;
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
							tbResult.AppendText(symbol.ToString() + "\n");
						}

						tbResult.AppendText("Types:\n");
						foreach (PdbType type in reader.Types)
						{
							tbResult.AppendText(type.Name + "\n");
							tbResult.AppendText("======================\n");
							tbResult.AppendText("Symbols:\n");
							foreach (CvSymbol symbol in type.Symbols)
							{
								tbResult.AppendText("\t" + symbol.ToString() + "\n");
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

		private void ShowCodeForm()
		{
			form.ShowDialog();

			if (form.DialogResult == DialogResult.OK)
			{
				if (!string.IsNullOrEmpty(form.Assembly))
				{
					LoadAssembly(form.Assembly, true, cbPlatform.Text);
				}
			}
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			Compile();
		}

		private void cbLabels_SelectedIndexChanged(object sender, EventArgs e)
		{
			tbResult.Text = string.Empty;
			SetStatus(string.Empty);

			if (currentInstructionLines == null)
				return;

			var node = GetCurrentNode<ViewNode<MosaMethod>>();

			if (node == null)
				return;

			SetStatus(node.Type.FullName);

			if (cbLabels.SelectedIndex == 0)
			{
				foreach (string l in currentInstructionLines)
				{
					string line = l;

					if (displayShortName.Checked)
						line = UpdateLine(line);

					if (line.Contains("IR.BlockStart") || line.Contains("IR.BlockEnd"))
						continue;

					tbResult.AppendText(line);
					tbResult.AppendText("\n");
				}

				return;
			}

			string blockLabel = cbLabels.SelectedItem as string;

			bool inBlock = false;

			foreach (string l in currentInstructionLines)
			{
				string line = l;

				if ((!inBlock) && line.StartsWith("Block #") && line.EndsWith(blockLabel))
				{
					inBlock = true;
				}

				if (inBlock)
				{
					if (displayShortName.Checked)
						line = UpdateLine(line);

					if (line.Contains("IR.BlockStart") || line.Contains("IR.BlockEnd"))
						continue;

					tbResult.AppendText(line);
					tbResult.AppendText("\n");

					if (line.StartsWith("  Next:"))
						return;
				}
			}
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			Compile();
		}

		private void SubmitMethodStatus(int totalMethods, int completedMethods)
		{
			toolStripProgressBar1.Maximum = totalMethods;
			toolStripProgressBar1.Value = completedMethods;
		}

		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerStage, string message, int threadID)
		{
			SubmitTraceEvent(compilerStage, message, threadID);

			MethodInvoker call = delegate()
			{
				SubmitTraceEventGUI(compilerStage, message, threadID);
			};

			Invoke(call);
		}

		void ITraceListener.OnUpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
			MethodInvoker call = delegate()
			{
				SubmitMethodStatus(totalMethods, completedMethods);
			};

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

				SubmitDebugStageInformation(traceLog.Method, stagesection, traceLog.ToString());
			}
			else if (traceLog.Type == TraceType.InstructionList)
			{
				SubmitInstructionTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines);
			}
		}
	}
}