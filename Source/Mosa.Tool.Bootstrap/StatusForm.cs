// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

namespace Mosa.Tool.Bootstrap;

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
