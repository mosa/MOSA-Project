using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Mosa.Tools.StageVisualizer
{
	public partial class frmMain : Form
	{
		Output output;

		public frmMain()
		{
			InitializeComponent();
		}

		private void loadButton_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				output = new Output(openFileDialog1.FileName);
				tbSource.Lines = output.Lines;
				UpdateText(sender, e);
				lbStatus.Text = openFileDialog1.FileName;
			}
		}

		private void UpdateText(object sender, EventArgs e)
		{
			cbMethods.Items.Clear();

			foreach (string item in output.GetMethods())
				cbMethods.Items.Add(item);

			cbMethods.SelectedIndex = 0;
			cbMethods_SelectionChangeCommitted(sender, e);
		}

		private void cbMethods_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (output != null)
			{
				cbStages.Items.Clear();

				foreach (string item in output.GetStages(cbMethods.SelectedItem.ToString()))
					cbStages.Items.Add(item);

				cbStages.SelectedIndex = 0;

				cbStages_SelectionChangeCommitted(sender, e);
			}
		}

		private void cbStages_SelectionChangeCommitted(object sender, EventArgs e)
		{
			cbStage.Checked = true;

			string method = cbMethods.SelectedItem.ToString();
			string stage = string.Empty;

			if (cbStages.SelectedItem != null)
				stage = cbStages.SelectedItem.ToString();

			string label = string.Empty;

			if (cbLabels.SelectedItem != null)
				label = cbLabels.SelectedItem.ToString();

			cbLabels.Items.Clear();

			foreach (string item in output.GetLabels(method, stage))
				cbLabels.Items.Add(item);

			if (!string.IsNullOrEmpty(label))
				if (cbLabels.Items.Contains(label))
					cbLabels.SelectedItem = label;

			refreshButton_Click(sender, e);
		}

		private void refreshButton_Click(object sender, EventArgs e)
		{
			if (cbMethods.SelectedItem == null)
			{
				tbResult.Lines = new string[0];
				return;
			}

			string method = cbMethods.SelectedItem.ToString();
			string stage = string.Empty;

			if (cbStages.SelectedItem != null)
				stage = cbStages.SelectedItem.ToString();

			string label = string.Empty;

			if (cbLabels.SelectedItem != null)
				label = cbLabels.SelectedItem.ToString();

			if (!cbLabel.Checked)
				label = string.Empty;

			if (!cbStage.Checked)
				stage = string.Empty;

			List<string> lines = output.GetText(method, stage, label, cbRemoveNextPrev.Checked, cbSpace.Checked);

			string[] final = new string[lines.Count];

			for (int i = 0; i < lines.Count; i++)
				final[i] = lines[i];

			tbResult.Lines = final;
		}

		private void cbLabels_SelectionChangeCommitted(object sender, EventArgs e)
		{
			cbLabel.Checked = true;
			refreshButton_Click(sender, e);
		}

		private void tbSource_TextChanged(object sender, EventArgs e)
		{
			output = new Output(tbSource.Lines);
			UpdateText(sender, e);
			lbStatus.Text = DateTime.Now.ToString();
		}

		private void saveButton_Click(object sender, EventArgs e)
		{

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Stream stream = saveFileDialog1.OpenFile();
				TextWriter writer = new StreamWriter(stream);

				string label = string.Empty;

				List<string> methods = output.GetMethods();
				methods.Sort();

				foreach (string method in methods)
				{
					foreach (string stage in output.GetStages(method))
					{
						string heading = "METHOD: " + method + " STAGE: " + stage;
						writer.WriteLine(string.Empty.PadLeft(heading.Length, '='));
						writer.WriteLine(string.Empty.PadLeft(heading.Length, '='));
						writer.WriteLine(heading);
						writer.WriteLine(string.Empty.PadLeft(heading.Length, '='));
						writer.WriteLine(string.Empty.PadLeft(heading.Length, '='));
						writer.WriteLine();

						List<string> lines = output.GetText(method, stage, label, cbRemoveNextPrev.Checked, cbSpace.Checked);

						foreach (string line in lines)
							writer.WriteLine(line);

						writer.WriteLine();
					}
				}

				stream.Close();
			}

		}


	}
}
