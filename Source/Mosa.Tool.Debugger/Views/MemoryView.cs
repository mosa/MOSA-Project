// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
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
			lbMemory.Clear();
		}

		public override void OnPause()
		{
			Query();
		}

		private void Query()
		{
			if (!IsConnected || !IsPaused)
				return;

			Columns = 6 * 4; // (lbMemory.Width - 100) / ((int)lbMemory.Font.Size * 3);
			Rows = lbMemory.Height / (lbMemory.Font.Height + 2);

			var address = MainForm.ParseHexAddress(tbAddress.Text);
			uint bytes = (uint)(Rows * Columns);

			//if (bytes > 0x800)
			//{
			//	Rows = 0x800 / Columns;
			//	bytes = (uint)(Rows * Columns);
			//}

			MemoryCache.ReadMemory(address, bytes, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			ulong at = address;

			var newlines = new string[Rows];

			for (int line = 0; line < Rows; line++)
			{
				string l = at.ToString("X").PadLeft(8, '0') + ':';
				string d = string.Empty;

				for (int x = 0; x < Columns; x++)
				{
					int index = (line * Columns) + x;

					if (index >= memory.Length)
						break;

					byte mem = memory[index];

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
			Query();
		}

		private void MemoryView_Load(object sender, EventArgs e)
		{
			Query();
		}

		private void tbMemory_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				Query();
			}
		}
	}
}
