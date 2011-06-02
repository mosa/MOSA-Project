using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mosa.Tools.TypeExplorer
{
	public partial class CodeForm : Form
	{
		public CodeForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
		}

		public string SourceCode { get { return tbText.Text; } }
	}
}
