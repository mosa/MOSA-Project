using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mosa.Tool.Bootstrap
{
	public partial class StatusForm : Form
	{
		public StatusForm(string status)
		{
			InitializeComponent();
			this.lbStatus.Text = status;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
