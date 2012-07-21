/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Windows.Forms;
using System.Threading;
using Mosa.Utility.DebugEngine;

namespace Mosa.Tool.Debugger
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MemoryForm : Form
	{
		private DebugServerEngine debugEngine;
		private bool updating = false;

		private uint multibootStructure = 0;
		private uint physicalPageFreeList = 0;
		private uint cr3;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryForm"/> class.
		/// </summary>
		public MemoryForm(DebugServerEngine debugEngine)
		{
			this.debugEngine = debugEngine;
			InitializeComponent();

			debugEngine.SendCommand(new DebugMessage(Codes.ReadMemory, new int[] { (int)0x200004, 4 }, this, UpdateMultibootStructurePointer));
			debugEngine.SendCommand(new DebugMessage(Codes.ReadMemory, new int[] { (int)(1024 * 1024 * 28), 4 }, this, UpdatePhysicalPageFreeListPointer));
			debugEngine.SendCommand(new DebugMessage(Codes.ReadCR3, (byte[])null, this, ReadCR3));

			UpdateForm();
		}

		private void UpdateForm()
		{
			if (updating)
				return;

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
				int lines = lbMemory.Height / (lbMemory.Font.Height + 2);

				toolStripStatusLabel1.Text = "Updating...";
				debugEngine.SendCommand(new DebugMessage(Codes.ReadMemory, new int[] { (int)at, 16 * lines }, this, DisplayMemory));
				updating = true;
			}
			catch
			{
				toolStripStatusLabel1.Text = "Invalid memory location";
			}

		}

		private void UpdateMultibootStructurePointer(DebugMessage message)
		{
			multibootStructure = (uint)message.GetUInt32(8);
		}

		private void UpdatePhysicalPageFreeListPointer(DebugMessage message)
		{
			physicalPageFreeList = (uint)message.GetUInt32(8);
		}

		private void ReadCR3(DebugMessage message)
		{
			cr3 = (uint)message.GetUInt32(0);
		}

		private void DisplayMemory(DebugMessage message)
		{
			updating = false;

			int start = message.GetInt32(0);
			int lines = message.GetInt32(4) / 16;
			uint at = (uint)start;

			try
			{
				string[] newlines = new string[lines];

				for (int line = 0; line < lines; line++)
				{
					string l = at.ToString("X").PadLeft(8, '0') + ':';
					string d = string.Empty;
					for (int x = 0; x < 16; x++)
					{
						byte mem = message.ResponseData[at - start + 8];
						if (x % 4 == 0) l = l + ' ';
						l = l + mem.ToString("X").PadLeft(2, '0');
						char b = (char)mem;
						d = d + (char.IsLetterOrDigit(b) ? b : '.');
						at++;
					}

					newlines[line] = l + ' ' + d;
				}
				lbMemory.Lines = newlines;
				toolStripStatusLabel1.Text = string.Empty;
			}
			catch (Exception e)
			{
				toolStripStatusLabel1.Text = "Error: " + e.ToString();
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

			UpdateForm();
		}

		private void MemoryForm_ResizeEnd(object sender, EventArgs e)
		{
			UpdateForm();
		}

		private void cbSelect_Click(object sender, EventArgs e)
		{

		}


	}
}
