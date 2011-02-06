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

		protected void LoadAssembly(string filename)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.LoadModule(filename);

			ITypeSystem typeSystem = new TypeSystem();
			typeSystem.LoadModules(assemblyLoader.Modules);

			treeView.BeginUpdate();
			treeView.Nodes.Clear();

			foreach (ITypeModule module in typeSystem.TypeModules)
			{
				TreeNode moduleNode = new TreeNode(module.MetadataModule.Name);
				treeView.Nodes.Add(moduleNode);

				foreach (RuntimeType type in module.GetAllTypes())
				{
					TreeNode typeNode = new TreeNode(type.ToString());
					moduleNode.Nodes.Add(typeNode);

					foreach (RuntimeMethod method in type.Methods)
					{
						TreeNode methodNode = new TreeNode(method.ToString());
						typeNode.Nodes.Add(methodNode);
					}

					foreach (RuntimeField field in type.Fields)
					{
						TreeNode fieldNode = new TreeNode(field.ToString());
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

	}
}
