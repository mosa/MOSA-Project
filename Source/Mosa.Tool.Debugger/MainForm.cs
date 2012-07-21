/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using Mosa.Utility.DebugEngine;

namespace Mosa.Tool.Debugger
{
	public partial class MainForm : Form
	{
		private DebugEngine debugEngine = new DebugEngine();

		private string[] events;
		private int eventIndex;

		public MainForm(DebugEngine debugEngine)
		{
			this.debugEngine = debugEngine;
			InitializeComponent();

			events = new string[50];
			eventIndex = 0;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			toolStripStatusLabel1.Text = string.Empty;
		}

		private string FormatResponseMessage(Mosa.Utility.DebugEngine.DebugMessage response)
		{
			switch (response.Code)
			{
				case Codes.Connected: return "Connected";
				case Codes.Connecting: return "Connecting";
				case Codes.Disconnected: return "Disconnected";
				case Codes.UnknownData: return "Unknown Data: " + System.Text.Encoding.UTF8.GetString(response.ResponseData);
				case Codes.InformationalMessage: return "Informational Message: " + System.Text.Encoding.UTF8.GetString(response.ResponseData);
				case Codes.ErrorMessage: return "Error Message: " + System.Text.Encoding.UTF8.GetString(response.ResponseData);
				case Codes.WarningMessage: return "Warning Message: " + System.Text.Encoding.UTF8.GetString(response.ResponseData);
				case Codes.Ping: return "PONG";
				case Codes.Alive: return "Alive";
				case Codes.SendNumber: return "#: " + ((response.ResponseData[0] << 24) | (response.ResponseData[1] << 16) | (response.ResponseData[2] << 8) | response.ResponseData[3]).ToString();
				default: return "Code: " + response.Code.ToString();
			}
		}

		public void ProcessResponses(DebugMessage response)
		{
			string formatted = "RECEIVED: #" + response.ID.ToString() + " -> " + FormatResponseMessage(response);

			toolStripStatusLabel1.Text = formatted;

			events[eventIndex++] = formatted;

			if (eventIndex == events.Length)
				eventIndex = 0;

			listBox1.Items.Clear();

			int start = eventIndex;
			for (int i = 0; i < events.Length; i++)
			{
				start--;

				if (start < 0)
					start = events.Length - 1;

				if (events[start] == null)
					return;

				listBox1.Items.Add(events[start]);

			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			//ProcessResponses();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			toolStripStatusLabel1.Text = "Ping";
			debugEngine.SendCommand(new DebugMessage(Codes.Ping, (byte[])null, this, ProcessResponses));
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var memoryForm = new MemoryForm(debugEngine);
			memoryForm.Show();
		}
	}
}
