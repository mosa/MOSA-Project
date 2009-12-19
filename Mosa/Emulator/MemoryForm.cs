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
using System.Data;
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
			Update();
		}

		private void Update()
		{
			string nbr = tbMemory.Text.ToUpper().Trim();
			int digits = 10;
			int where = nbr.IndexOf('X');

			if (where >= 0) {
				digits = 16;
				nbr = nbr.Substring(where + 1);
			}

			uint at = Convert.ToUInt32(nbr, digits);
			Dump(at, lbMemory.Height / (lbMemory.Font.Height + 1));
		}

		private void Dump(uint start, int lines)
		{
			uint at = start;
			int line = 0;

			lbMemory.Items.Clear();

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
				lbMemory.Items.Add(l + ' ' + d + '\n');
			}

		}

		private void tbMemory_TextChanged(object sender, EventArgs e)
		{
			Update();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			Update();
		}

		private void MemoryForm_Resize(object sender, EventArgs e)
		{
			Update();
		}

	}
}
