using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Tool.MetadataExplorer.Tables;

namespace Mosa.Tool.MetadataExplorer
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
			
		}

		protected void LoadAssembly(string filename)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.AddPrivatePath(System.IO.Path.GetDirectoryName(filename));

			metadataModule = assemblyLoader.LoadModule(filename);

			UpdateTree();

			this.toolStripStatusLabel1.Text = filename;
		}

		protected void UpdateTree()
		{
			treeView.BeginUpdate();
			treeView.Nodes.Clear();

			//Cycle through all metadata tables
			foreach (TableType table in Enum.GetValues(typeof(TableType)))
			{
				if (table == TableType.Module)
					continue;

				int count = metadataModule.Metadata.GetRowCount(table);

				if (count == 0)
					continue;

				TreeNode tableNode = new TreeNode("[" + table.FormatToString() + "] " + table.ToString() + " (" + count.ToString() + ")");
				treeView.Nodes.Add(tableNode);

				//Cycle through all metadata rows
				for (int rowid = 1; rowid <= count; rowid++)
				{
					Token token = new Token(table, rowid);

					TableRow row = Resolver.GetTableRow(metadataModule, token);

					if (row == null)
						continue;

					TreeNode rowNode = new TreeNode(token.FormatToString() + " - " + row.Name);
					tableNode.Nodes.Add(rowNode);

					foreach (KeyValuePair<string, string> data in row.GetValues())
					{
						TreeNode rowValueNode = new TreeNode(data.Key + ": " + data.Value);
						rowNode.Nodes.Add(rowValueNode);
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

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				LoadAssembly(openFileDialog.FileName);
			}
		}

	}
}