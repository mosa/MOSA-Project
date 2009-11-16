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
    public partial class Form1 : Form
    {
        Output output;

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                output = new Output(openFileDialog1.FileName);

                tbSource.Lines = output.Lines;

                foreach (string item in output.GetMethods())
                    cbMethods.Items.Add(item);
            }
        }
    }
}
