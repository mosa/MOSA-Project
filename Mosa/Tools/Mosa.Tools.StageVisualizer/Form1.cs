using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                output = new Output(openFileDialog1.FileName);

                tbSource.Lines = output.Lines;

                cbMethods.Items.Clear();

                foreach (string item in output.GetMethods())
                    cbMethods.Items.Add(item);

                cbMethods.SelectedIndex = 0;
                cbMethods_SelectionChangeCommitted(sender, e);
            }
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

            btnUpdate_Click(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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

            List<string> lines = output.GetText(method, stage, label);

            string[] final = new string[lines.Count];

            for (int i = 0; i < lines.Count; i++)
                final[i] = lines[i];

            tbResult.Lines = final;
        }

        private void cbLabels_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbLabel.Checked = true;
            btnUpdate_Click(sender, e);
        }
    }
}
