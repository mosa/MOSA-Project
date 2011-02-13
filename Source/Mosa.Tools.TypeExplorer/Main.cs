using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem.Generic;

namespace Mosa.Tools.TypeExplorer
{
	public partial class Main : Form
	{
		//ITypeSystem typeSystem = new TypeSystem();

		public Main()
		{
			InitializeComponent();
		}

		private void Main_Load(object sender, EventArgs e)
		{
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				LoadAssembly(openFileDialog.FileName);
			}
		}

		protected string TokenToString(TokenTypes token)
		{
			return ((int)token).ToString("X8");
		}

		protected string FormatToString(TokenTypes token, bool show)
		{
			if (!show)
				return string.Empty;

			return "[" + TokenToString(token) + "] ";
		}

		protected string FormatRuntimeMember(RuntimeMember member, bool show)
		{
			if (!show)
				return member.Name;

			return "[" + TokenToString(member.Token) + "] " + member.Name;
		}

		ITypeSystem typeSystem = new TypeSystem();

		protected void LoadAssembly(string filename)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.LoadModule(filename);

			typeSystem = new TypeSystem();
			typeSystem.LoadModules(assemblyLoader.Modules);

			Update();
		}

		protected void Update()
		{
			treeView.BeginUpdate();
			treeView.Nodes.Clear();

			bool show = showTokenValues.Checked;

			foreach (ITypeModule module in typeSystem.TypeModules)
			{
				TreeNode moduleNode = new TreeNode(module.MetadataModule.Name);
				treeView.Nodes.Add(moduleNode);

				foreach (RuntimeType type in module.GetAllTypes())
				{
					TreeNode typeNode = new TreeNode(FormatRuntimeMember(type, show));
					moduleNode.Nodes.Add(typeNode);

					if (type.BaseType != null)
					{
						TreeNode baseTypeNode = new TreeNode("Base Type: " + FormatRuntimeMember(type.BaseType, show));
						typeNode.Nodes.Add(baseTypeNode);
					}

					CilGenericType genericType = type as CilGenericType;
					if (genericType != null)
					{
						if (genericType.BaseGenericType != null)
						{
							TreeNode genericBaseTypeNode = new TreeNode("Generic Base Type: " + FormatRuntimeMember(genericType.BaseGenericType, show));
							typeNode.Nodes.Add(genericBaseTypeNode);
						}
					}

					if (type.Interfaces.Count != 0)
					{
						TreeNode interfacesNodes = new TreeNode("Interfaces");
						typeNode.Nodes.Add(interfacesNodes);

						foreach (RuntimeType interfaceType in type.Interfaces)
						{
							TreeNode interfaceNode = new TreeNode(FormatRuntimeMember(interfaceType, show));
							interfacesNodes.Nodes.Add(interfaceNode);
						}
					}

					foreach (RuntimeMethod method in type.Methods)
					{
						TreeNode methodNode = new TreeNode(FormatRuntimeMember(method, show));
						typeNode.Nodes.Add(methodNode);
					}

					foreach (RuntimeField field in type.Fields)
					{
						TreeNode fieldNode = new TreeNode(FormatRuntimeMember(field, show));
						typeNode.Nodes.Add(fieldNode);
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
			Update();
		}

	}
}
