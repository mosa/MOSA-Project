using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Pdb;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.TypeSystem.Generic;

namespace Mosa.Tool.TypeExplorer
{
	public partial class Main : Form, ICompilerEventListener, IInstructionTraceListener
	{
		private CodeForm form = new CodeForm();
		private IInternalTrace internalTrace = new BasicInternalTrace();
		private ITypeSystem typeSystem = new TypeSystem();
		private ConfigurableInstructionTraceFilter filter = new ConfigurableInstructionTraceFilter();
		private ITypeLayout typeLayout;
		private DateTime compileStartTime;
		private string currentStageLog;
		private string[] currentStageLogLines;

		private Dictionary<RuntimeMethod, MethodStages> methodStages = new Dictionary<RuntimeMethod, MethodStages>();

		private class MethodStages
		{
			public List<string> OrderedStageNames = new List<string>();
			public Dictionary<string, string> Logs = new Dictionary<string, string>();
		}

		public Main()
		{
			InitializeComponent();
			internalTrace.CompilerEventListener = this;
			internalTrace.InstructionTraceListener = this;
			internalTrace.InstructionTraceFilter = filter;
		}

		private void Main_Load(object sender, EventArgs e)
		{
			cbPlatform.SelectedIndex = 0;
			statusStrip1.Text = "Ready!";
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
				if (Path.GetFileName(openFileDialog.FileName) == "Mosa.Test.Collection.dll")
				{
					includeTestKorlibToolStripMenuItem.Checked = true;
				}

				LoadAssembly(openFileDialog.FileName);
			}
		}

		protected string TokenToString(Token token)
		{
			return token.ToInt32().ToString("X8");
		}

		protected string FormatToString(Token token)
		{
			if (!showTokenValues.Checked)
				return string.Empty;

			return "[" + TokenToString(token) + "] ";
		}

		protected string FormatRuntimeMember(RuntimeMember member)
		{
			if (!showTokenValues.Checked)
				return member.Name;

			return "[" + TokenToString(member.Token) + "] " + member.Name;
		}

		protected string FormatRuntimeMember(RuntimeMethod method)
		{
			if (!showTokenValues.Checked)
				return method.Name;

			return "[" + TokenToString(method.Token) + "] " + method.MethodName;
		}

		protected string FormatRuntimeType(RuntimeType type)
		{
			if (!showTokenValues.Checked)
				return type.Namespace + Type.Delimiter + type.Name;

			return "[" + TokenToString(type.Token) + "] " + type.Namespace + Type.Delimiter + type.Name;
		}

		protected void LoadAssembly(string filename)
		{
			LoadAssembly(filename, includeTestKorlibToolStripMenuItem.Checked, cbPlatform.Text);
		}

		protected void UpdateTree()
		{
			treeView.BeginUpdate();
			treeView.Nodes.Clear();

			foreach (ITypeModule module in typeSystem.TypeModules)
			{
				TreeNode moduleNode = new TreeNode(module.Name);
				treeView.Nodes.Add(moduleNode);

				foreach (RuntimeType type in module.GetAllTypes())
				{
					TreeNode typeNode = new TreeNode(FormatRuntimeType(type));
					moduleNode.Nodes.Add(typeNode);

					if (type.BaseType != null)
					{
						TreeNode baseTypeNode = new TreeNode("Base Type: " + FormatRuntimeType(type.BaseType));
						typeNode.Nodes.Add(baseTypeNode);
					}

					CilGenericType genericType = type as CilGenericType;
					if (genericType != null)
					{
						if (genericType.BaseGenericType != null)
						{
							TreeNode genericBaseTypeNode = new TreeNode("Generic Base Type: " + FormatRuntimeType(genericType.BaseGenericType));
							typeNode.Nodes.Add(genericBaseTypeNode);
						}
					}

					CilGenericType genericOpenType = typeSystem.GetOpenGeneric(type);
					if (genericOpenType != null)
					{
						TreeNode genericOpenTypeNode = new TreeNode("Open Generic Type: " + FormatRuntimeType(genericOpenType));
						typeNode.Nodes.Add(genericOpenTypeNode);
					}

					if (type.GenericParameters.Count != 0)
					{
						TreeNode genericParameterNodes = new TreeNode("Generic Parameters");
						typeNode.Nodes.Add(genericParameterNodes);

						foreach (GenericParameter genericParameter in type.GenericParameters)
						{
							TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
							genericParameterNodes.Nodes.Add(GenericParameterNode);
						}
					}

					if (type.Interfaces.Count != 0)
					{
						TreeNode interfacesNodes = new TreeNode("Interfaces");
						typeNode.Nodes.Add(interfacesNodes);

						foreach (RuntimeType interfaceType in type.Interfaces)
						{
							TreeNode interfaceNode = new TreeNode(FormatRuntimeType(interfaceType));
							interfacesNodes.Nodes.Add(interfaceNode);
						}
					}

					if (type.Fields.Count != 0)
					{
						TreeNode fieldsNode = new TreeNode("Fields");
						if (showSizes.Checked)
							fieldsNode.Text = fieldsNode.Text + " (Count: " + type.Fields.Count.ToString() + " - Size: " + typeLayout.GetTypeSize(type).ToString() + ")";

						typeNode.Nodes.Add(fieldsNode);

						foreach (RuntimeField field in type.Fields)
						{
							TreeNode fieldNode = new TreeNode(FormatRuntimeMember(field));
							fieldsNode.Nodes.Add(fieldNode);

							if (field.IsStaticField)
								fieldNode.Text = fieldNode.Text + " [Static]";

							if (showSizes.Checked)
							{
								fieldNode.Text = fieldNode.Text + " (Size: " + typeLayout.GetFieldSize(field).ToString();

								if (!field.IsStaticField)
									fieldNode.Text = fieldNode.Text + " - Offset: " + typeLayout.GetFieldOffset(field).ToString();

								fieldNode.Text = fieldNode.Text + ")";
							}
						}
					}

					if (type.Methods.Count != 0)
					{
						TreeNode methodsNode = new TreeNode("Methods");
						typeNode.Nodes.Add(methodsNode);

						foreach (RuntimeMethod method in type.Methods)
						{
							TreeNode methodNode = new ViewNode<RuntimeMethod>(method, FormatRuntimeMember(method));
							methodsNode.Nodes.Add(methodNode);

							if ((method.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
								methodNode.Text = methodNode.Text + " [Static]";

							if (method.IsAbstract)
								methodNode.Text = methodNode.Text + " [Abstract]";
						}
					}

					if (typeLayout.GetMethodTable(type) != null)
					{
						TreeNode methodTableNode = new TreeNode("Method Table");
						typeNode.Nodes.Add(methodTableNode);

						foreach (RuntimeMethod method in typeLayout.GetMethodTable(type))
						{
							TreeNode methodNode = new ViewNode<RuntimeMethod>(method, FormatRuntimeMember(method));
							methodTableNode.Nodes.Add(methodNode);
						}
					}
				}
			}

			treeView.EndUpdate();
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
				toolStripStatusLabel1.Text = compilerStage.ToText() + ": " + info;
				toolStripStatusLabel1.GetCurrentParent().Refresh();
			}

			tbResult.AppendText(String.Format("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds) + " secs: " + compilerStage.ToText() + ": " + info + "\n");
		}

		void IInstructionTraceListener.NotifyNewInstructionTrace(RuntimeMethod method, string stage, string log)
		{
			MethodStages methodStage;

			if (!methodStages.TryGetValue(method, out methodStage))
			{
				methodStage = new MethodStages();
				methodStages.Add(method, methodStage);
			}

			methodStage.OrderedStageNames.Add(stage);
			methodStage.Logs.Add(stage, log);
		}

		void Compile()
		{
			compileStartTime = DateTime.Now;
			methodStages.Clear();

			filter.IsLogging = true;
			filter.MethodMatch = MatchType.Any;

			ExplorerAssemblyCompiler.Compile(typeSystem, typeLayout, internalTrace, cbPlatform.Text, enableSSAToolStripMenuItem.Checked);
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

			var node = GetCurrentNode<ViewNode<RuntimeMethod>>();

			if (node == null)
				return;

			MethodStages methodStage;

			if (!methodStages.TryGetValue(node.Type, out methodStage))
				return;

			cbStages.Items.Clear();

			foreach (string stage in methodStage.OrderedStageNames)
				cbStages.Items.Add(stage);

			cbStages.SelectedIndex = 0;
		}

		private void cbStages_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentStageLog = string.Empty;

			var node = GetCurrentNode<ViewNode<RuntimeMethod>>();

			if (node == null)
				return;

			MethodStages methodStage;

			if (methodStages.TryGetValue(node.Type, out methodStage))
			{
				string stage = cbStages.SelectedItem.ToString();

				if (currentStageLog == methodStage.Logs[stage])
					return;

				currentStageLog = methodStage.Logs[stage];

				currentStageLogLines = currentStageLog.Split('\n');

				cbLabels.Items.Clear();
				cbLabels.Items.Add("All");

				foreach (string line in currentStageLogLines)
				{
					if (line.StartsWith("Block #"))
					{
						cbLabels.Items.Add(line.Substring(line.IndexOf("L_")));
					}
				}

				cbLabels.SelectedIndex = 0;
				cbLabels_SelectedIndexChanged(null, null);
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
			IAssemblyLoader assemblyLoader = new AssemblyLoader();

			if (includeTestComponents)
			{
				assemblyLoader.AddPrivatePath(System.IO.Directory.GetCurrentDirectory());
				assemblyLoader.LoadModule("mscorlib.dll");
				assemblyLoader.LoadModule("Mosa.Platform." + platform + ".Intrinsic.dll");
				assemblyLoader.LoadModule("Mosa.Test.Runtime.dll");
			}

			assemblyLoader.AddPrivatePath(System.IO.Path.GetDirectoryName(filename));
			assemblyLoader.LoadModule(filename);

			typeSystem = new TypeSystem();
			typeSystem.LoadModules(assemblyLoader.Modules);

			typeLayout = new TypeLayout(typeSystem, 4, 4);

			UpdateTree();
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

						tbResult.AppendText("Global symbols: \n");
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
			UpdateTree();
		}

		private void cbLabels_SelectedIndexChanged(object sender, EventArgs e)
		{
			tbResult.Text = string.Empty;
			toolStripStatusLabel1.Text = string.Empty;

			if (string.IsNullOrEmpty(currentStageLog))
				return;

			var node = GetCurrentNode<ViewNode<RuntimeMethod>>();

			if (node == null)
				return;

			toolStripStatusLabel1.Text = node.Type.FullName;

			if (cbLabels.SelectedIndex == 0)
			{
				tbResult.Text = currentStageLog;
				return;
			}

			string blockLabel = cbLabels.SelectedItem as string;

			bool inBlock = false;

			foreach (string line in currentStageLogLines)
			{
				if ((!inBlock) && line.StartsWith("Block #") && line.EndsWith(blockLabel))
				{
					inBlock = true;
				}

				if (inBlock)
				{
					tbResult.AppendText(line);

					if (line.StartsWith("  Next:"))
						return;
				}
			}
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			Compile();
			UpdateTree();
		}

	}

	public class ViewNode<T> : TreeNode
	{
		public T Type;

		public ViewNode(T type, string name)
			: base(name)
		{
			this.Type = type;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
