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
			openToolStripMenuItem_Click(null, null);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] files = { 
				@"X:\MOSA-Project-tgiphil\bin\mscorlib.dll", 
				@"X:\MOSA-Project-tgiphil\bin\Mosa.Platform.X86.Intrinsic.dll", 
				@"X:\MOSA-Project-tgiphil\bin\Mosa.Kernel.dll",
				@"X:\MOSA-Project-tgiphil\bin\Mosa.HelloWorld.exe"
			};

			//string[] files = { @"X:\MOSA-Project-tgiphil\bin\Mosa.Test.Quick.exe" };

			ITypeSystem typeSystem = new TypeSystem();

			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.InitializePrivatePaths(files);

			foreach (string file in files)
			{
				IMetadataModule metadataModule = assemblyLoader.LoadModule(file);

				typeSystem.LoadModule(metadataModule);
			}

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
