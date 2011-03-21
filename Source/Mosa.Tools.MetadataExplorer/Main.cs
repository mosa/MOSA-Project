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

namespace Mosa.Tools.MetadataExplorer
{
	public partial class Main : Form
	{
		protected IMetadataModule metadataModule = null;

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

		protected string FormatToString(TokenTypes token)
		{
			if (!showTokenValues.Checked)
				return string.Empty;

			return "[" + TokenToString(token) + "] ";
		}

		protected void LoadAssembly(string filename)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.AddPrivatePath(System.IO.Path.GetDirectoryName(filename));

			metadataModule = assemblyLoader.LoadModule(filename);
			
			UpdateTree();
		}

		protected void UpdateTree()
		{
			treeView.BeginUpdate();
			treeView.Nodes.Clear();

			//TODO: xxxx

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

	}
}
