// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views;

public partial class MemoryView : DebugDockContent
{
	private int Rows = 0;
	private int Columns = 16;

	public MemoryView(MainForm mainForm)
		: base(mainForm)
	{
		InitializeComponent();
		tbAddress.Text = "0xB8000";
	}

	public override void OnRunning()
	{
		ClearDisplay();
	}

	protected override void ClearDisplay()
	{
		lbMemory.Clear();
	}

	protected override void UpdateDisplay()
	{
		Columns = 6 * 4; // (lbMemory.Width - 100) / ((int)lbMemory.Font.Size * 3);
		Rows = lbMemory.Height / (lbMemory.Font.Height + 2);

		var address = MainForm.ParseHexAddress(tbAddress.Text);
		var bytes = (uint)(Rows * Columns);

		MemoryCache.ReadMemory(address, bytes, OnMemoryRead);
	}

	private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

	private void UpdateDisplay(ulong address, byte[] memory)
	{
		var at = address;

		var newlines = new string[Rows];

		for (var line = 0; line < Rows; line++)
		{
			var l = at.ToString("X").PadLeft(8, '0') + ':';
			var d = string.Empty;

			for (var x = 0; x < Columns; x++)
			{
				var index = (line * Columns) + x;

				if (index >= memory.Length)
					break;

				var mem = memory[index];

				if (x % 4 == 0)
					l += ' ';

				l += mem.ToString("X").PadLeft(2, '0');

				var b = (char)mem;

				d += (char.IsLetterOrDigit(b) ? b : '.');

				at++;
			}

			newlines[line] = l + ' ' + d;
		}

		lbMemory.Lines = newlines;
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		if (IsReady)
			UpdateDisplay();
	}

	private void MemoryView_Load(object sender, EventArgs e)
	{
		if (IsReady)
			UpdateDisplay();
	}

	private void tbMemory_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Enter)
		{
			if (IsReady)
				UpdateDisplay();
		}
	}
}
