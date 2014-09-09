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
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Pdb;
using Mosa.Utility.GUI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Mosa.Tool.Explorer
{
	public partial class Main : Form, ICompilerEventListener, ITraceListener
	{
		private CodeForm form = new CodeForm();
		private CompilerTrace compilerTrace = new CompilerTrace();
		private MosaModuleLoader assemblyLoader;
		private TypeSystem typeSystem;
		private ConfigurableTraceFilter filter = new ConfigurableTraceFilter();
		private MosaTypeLayout typeLayout;
		private DateTime compileStartTime;
		private StringBuilder currentInstructionLog;
		private string[] currentInstructionLogLines;

		private Dictionary<MosaMethod, MethodStages> methodStages = new Dictionary<MosaMethod, MethodStages>();

		private class MethodStages
		{
			public List<string> OrderedStageNames = new List<string>();
			public List<string> OrderedDebugStageNames = new List<string>();
			public Dictionary<string, StringBuilder> InstructionLogs = new Dictionary<string, StringBuilder>();
			public Dictionary<string, StringBuilder> DebugLogs = new Dictionary<string, StringBuilder>();
		}

		private StringBuilder compileLog = new StringBuilder();

		public Main()
		{
			InitializeComponent();
			compilerTrace.CompilerEventListener = this;
			compilerTrace.TraceListener = this;
			compilerTrace.TraceFilter = filter;

			filter.Active = true;
			filter.ExcludeInternalMethods = false;
			filter.MethodMatch = MatchType.Any;
			filter.StageMatch = MatchType.Exclude;
			filter.Stage = "PlatformStubStage|ExceptionLayoutStage|DominanceCalculationStage|CodeGenerationStage";
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

			SetStatus("Assemblies Loaded!");
		}

		protected void UpdateTree()
		{
			TypeSystemTree.UpdateTree(treeView, typeSystem, typeLayout, showSizes.Checked);
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

		void ICompilerEventListener.SubmitTraceEvent(CompilerEvent compilerStage, string info)
		{
			if (compilerStage != CompilerEvent.DebugInfo)
			{
				SetStatus(compilerStage.ToText() + ": " + info);
				toolStripStatusLabel1.GetCurrentParent().Refresh();
			}

			compileLog.Append(String.Format("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds) + " secs: " + compilerStage.ToText() + ": " + info + "\n");
		}

		void ITraceListener.SubmitInstructionTraceInformation(MosaMethod method, string stage, string log)
		{
			MethodStages methodStage;

			if (!methodStages.TryGetValue(method, out methodStage))
			{
				methodStage = new MethodStages();
				methodStages.Add(method, methodStage);
			}

			methodStage.OrderedStageNames.AddIfNew(stage);

			StringBuilder stringbuilder;

			if (methodStage.InstructionLogs.TryGetValue(stage, out stringbuilder))
			{
				stringbuilder.Append(log);
			}
			else
			{
				stringbuilder = new StringBuilder(log);
				methodStage.InstructionLogs.Add(stage, stringbuilder);
			}
		}

		void ITraceListener.SubmitDebugStageInformation(MosaMethod method, string stage, string line)
		{
			MethodStages methodStage;

			if (!methodStages.TryGetValue(method, out methodStage))
			{
				methodStage = new MethodStages();
				methodStages.Add(method, methodStage);
			}

			methodStage.OrderedDebugStageNames.AddIfNew(stage);

			StringBuilder stringbuilder;

			if (!methodStage.DebugLogs.TryGetValue(stage, out stringbuilder))
			{
				stringbuilder = new StringBuilder(line.Length + 2);
				methodStage.DebugLogs.Add(stage, stringbuilder);
			}
			stringbuilder.AppendLine(line);
		}

		private void Compile()
		{
			compileStartTime = DateTime.Now;
			methodStages.Clear();

			filter.MethodMatch = MatchType.Any;

			CreateTypeSystemAndLayout();

			//try
			//{
				ExplorerCompiler.Compile(typeSystem, typeLayout, compilerTrace, cbPlatform.Text, enableSSAToolStripMenuItem.Checked, enableSSAOptimizations.Checked, enableBinaryCodeGenerationToolStripMenuItem.Checked);
				SetStatus("Compiled!");
			//}
			//catch (Exception e)
			//{
			//	SetStatus("ERROR: " + toolStripStatusLabel.Text + " " + e.ToString());
			//}

			tabControl1.SelectedTab = tabPage1;
			rbOtherResult.Text = compileLog.ToString();
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

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			tbResult.Text = string.Empty;

			var node = GetCurrentNode<ViewNode<MosaMethod>>();

			if (node == null)
				return;

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
			currentInstructionLog = null;

			var node = GetCurrentNode<ViewNode<MosaMethod>>();

			if (node == null)
				return;

			MethodStages methodStage;

			if (!methodStages.TryGetValue(node.Type, out methodStage))
				return;

			string stage = cbStages.SelectedItem.ToString();

			if (currentInstructionLog != null && currentInstructionLog.Equals(methodStage.InstructionLogs[stage]))
				return;

			currentInstructionLog = methodStage.InstructionLogs[stage];

			currentInstructionLogLines = currentInstructionLog.ToString().Split('\n');

			var previousItemLabel = cbLabels.SelectedItem;

			cbLabels.Items.Clear();
			cbLabels.Items.Add("All");

			foreach (string line in currentInstructionLogLines)
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
			assemblyLoader = new MosaModuleLoader();

			if (includeTestComponents)
			{
				assemblyLoader.AddPrivatePath(System.IO.Directory.GetCurrentDirectory());
				assemblyLoader.LoadModuleFromFile("mscorlib.dll");
				assemblyLoader.LoadModuleFromFile("Mosa.Platform.Internal." + platform + ".dll");
				assemblyLoader.LoadModuleFromFile("Mosa.Kernel.x86Test.dll");
			}

			assemblyLoader.AddPrivatePath(Path.GetDirectoryName(filename));
			assemblyLoader.LoadModuleFromFile(filename);

			CreateTypeSystemAndLayout();
			UpdateTree();
		}

		protected void CreateTypeSystemAndLayout()
		{
			if (assemblyLoader == null)
				return;

			typeSystem = TypeSystem.Load(assemblyLoader.CreateMetadata());
			typeLayout = new MosaTypeLayout(typeSystem, 4, 4); // FIXME
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

			if (currentInstructionLog == null)
				return;

			var node = GetCurrentNode<ViewNode<MosaMethod>>();

			if (node == null)
				return;

			SetStatus(node.Type.FullName);

			if (cbLabels.SelectedIndex == 0)
			{
				foreach (string l in currentInstructionLogLines)
				{
					string line = l;

					if (displayShortName.Checked)
						line = UpdateLine(line);

					tbResult.AppendText(line);
				}

				return;
			}

			string blockLabel = cbLabels.SelectedItem as string;

			bool inBlock = false;

			foreach (string l in currentInstructionLogLines)
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

					tbResult.AppendText(line);

					if (line.StartsWith("  Next:"))
						return;
				}
			}
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			Compile();
		}

		void ICompilerEventListener.SubmitMethodStatus(int totalMethods, int completedMethods)
		{
			toolStripProgressBar1.Maximum = totalMethods;
			toolStripProgressBar1.Value = completedMethods;
		}
	}
}