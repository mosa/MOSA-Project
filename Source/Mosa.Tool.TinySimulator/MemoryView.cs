/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator.x86.Emulate;
using System;

namespace Mosa.Tool.TinySimulator
{
	/// <summary>
	///
	/// </summary>
	public partial class MemoryView : SimulatorDockContent
	{
		private uint multibootStructure = Multiboot.MultibootStructure;
		private uint physicalPageFreeList = MosaKernel.PageTable;
		private uint cr3 = MosaKernel.PageDirectory;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryView"/> class.
		/// </summary>
		public MemoryView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		private void UpdateDisplay()
		{
			int lines = lbMemory.Height / (lbMemory.Font.Height + 2);

			try
			{
				string nbr = tbMemory.Text.ToUpper().Trim();
				int digits = 10;
				int where = nbr.IndexOf('X');

				if (where >= 0)
				{
					digits = 16;
					nbr = nbr.Substring(where + 1);
				}

				uint at = Convert.ToUInt32(nbr, digits);

				string[] newlines = new string[lines];

				for (int line = 0; line < lines; line++)
				{
					string l = at.ToString("X").PadLeft(8, '0') + ':';
					string d = string.Empty;
					for (int x = 0; x < 16; x++)
					{
						byte mem = SimCPU.DirectRead8(at);

						if (x % 4 == 0) l = l + ' ';
						l = l + mem.ToString("X").PadLeft(2, '0');
						char b = (char)mem;
						d = d + (char.IsLetterOrDigit(b) ? b : '.');
						at++;
					}

					newlines[line] = l + ' ' + d;
				}
				lbMemory.Lines = newlines;
				Status = string.Empty;
			}
			catch (Exception e)
			{
				Status = "Error: " + e.ToString();
			}
		}

		private void tbMemory_TextChanged(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		private void cbSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbSelect.SelectedIndex)
			{
				case 0: tbMemory.Text = "0xB8000"; break;
				case 1: tbMemory.Text = "0x" + multibootStructure.ToString("X"); break;
				case 2: tbMemory.Text = "0x" + cr3.ToString("X"); break;
				case 3: tbMemory.Text = "0x" + physicalPageFreeList.ToString("X"); break;
				case 4: tbMemory.Text = "0x" + (1024 * 1024 * 21).ToString("X"); break;
				default: break;
			}

			UpdateDisplay();
		}

		private void MemoryForm_ResizeEnd(object sender, EventArgs e)
		{
			UpdateDisplay();
		}
	}
}