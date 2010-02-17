/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mosa.Emulator
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MemoryForm : Form
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryForm"/> class.
		/// </summary>
		public MemoryForm()
		{
			InitializeComponent();
			UpdateForm();
		}

		private void UpdateForm()
		{
			string nbr = tbMemory.Text.ToUpper().Trim();
			int digits = 10;
			int where = nbr.IndexOf('X');

			if (where >= 0) {
				digits = 16;
				nbr = nbr.Substring(where + 1);
			}

			try {
				uint at = Convert.ToUInt32(nbr, digits);
				Dump(at, lbMemory.Height / (lbMemory.Font.Height + 2));
			}
			catch {
			}
		}

		private void Dump(uint start, int lines)
		{
			uint at = start;
			int line = 0;

			lbMemory.Clear();

			while (line < lines) {
				string l = at.ToString("X").PadLeft(8, '0') + ':';
				string d = string.Empty;
				for (int x = 0; x < 16; x++) {
					byte mem = Mosa.EmulatedKernel.MemoryDispatch.Read8(at);
					if (x % 4 == 0) l = l + ' ';
					l = l + mem.ToString("X").PadLeft(2, '0');
					char b = (char)mem;
					d = d + (char.IsLetterOrDigit(b) ? b : '.');
					at++;
				}
				line++;
				lbMemory.AppendText(l + ' ' + d + '\n');
			}

		}

		private void tbMemory_TextChanged(object sender, EventArgs e)
		{
			UpdateForm();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			UpdateForm();
		}

		private void MemoryForm_Resize(object sender, EventArgs e)
		{
			UpdateForm();
		}

		private void cbSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbSelect.SelectedIndex) {
				case 0: tbMemory.Text = "0xB8000"; break;
				case 1: tbMemory.Text = "0x" + Mosa.EmulatedKernel.MemoryDispatch.Read32(0x200004).ToString("X"); break;
				case 2: tbMemory.Text = "0x" + Mosa.EmulatedKernel.MemoryDispatch.CR3.ToString("X"); break;
				case 3: tbMemory.Text = "0x" + Mosa.EmulatedKernel.MemoryDispatch.Read32(1024 * 1024 * 28).ToString("X"); break;
				case 4: tbMemory.Text = "0x" + (1024 * 1024 * 21).ToString("X"); break;
				default: break;
			}
			UpdateForm();
		}


	}
}
