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
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.Debugger
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MemoryView : DebuggerDockContent
	{
		private uint multibootStructure = 0;
		private uint physicalPageFreeList = 0;
		private uint cr3;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryView"/> class.
		/// </summary>
		public MemoryView()
		{
			InitializeComponent();
		}

		private void MemoryView_Load(object sender, EventArgs e)
		{
			cbSelect.Enabled = false;
			Connect();
		}

		public override void Connect()
		{
			Status = "Querying...";
			SendCommand(new DebugMessage(Codes.Scattered32BitReadMemory, new int[] { (int)0x200004, (int)(1024 * 1024 * 28) }, this, UpdatePointers));
			SendCommand(new DebugMessage(Codes.ReadCR3, (byte[])null, this, ReadCR3));
		}

		public override void Disconnect()
		{
			cbSelect.Enabled = false;
		}

		private void UpdatePointers(DebugMessage message)
		{
			multibootStructure = (uint)message.GetUInt32(4);
			physicalPageFreeList = (uint)message.GetUInt32(12);
		}

		private void ReadCR3(DebugMessage message)
		{
			cr3 = (uint)message.GetUInt32(0);

			cbSelect.Enabled = Enabled;
			if (cbSelect.SelectedIndex == -1)
				cbSelect.SelectedIndex = 0;
		}

		private void DisplayMemory(DebugMessage message)
		{
			cbSelect.Enabled = Enabled;

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
				Status = string.Empty;
			}
			catch (Exception e)
			{
				Status = "Error: " + e.ToString();
			}
		}

		private void UpdateForm()
		{
			if (!cbSelect.Enabled)
				return;

			cbSelect.Enabled = false;

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

				Status = "Updating...";
				SendCommand(new DebugMessage(Codes.ReadMemory, new int[] { (int)at, 16 * lines }, this, DisplayMemory));
			}
			catch
			{
				Status = "ERROR: Invalid memory location";
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


	}
}
