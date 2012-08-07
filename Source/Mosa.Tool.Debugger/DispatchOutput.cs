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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Mosa.Utility.DebugEngine;

namespace Mosa.Tool.Debugger
{
	public partial class DispatchOutput : DebuggerDockContent
	{

		private string[] events;
		private int eventIndex;

		public DispatchOutput()
		{
			InitializeComponent();

			events = new string[50];
			eventIndex = 0;

			//DebugEngine.SetDispatchMethod(this, Dispatch);
		}

		public override void Connect()
		{
			Status = "Connected!";
		}

		public override void Disconnect()
		{
			Status = "Disconnected!";
		}

		private string FormatResponseMessage(DebugMessage response)
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
				case Codes.Ping: return "Pong";
				case Codes.Alive: return "Alive";
				case Codes.ReadCR3: return "ReadCR3";
				case Codes.ReadMemory: return "ReadMemory";
				case Codes.Scattered32BitReadMemory: return "Scattered32BitReadMemory";
				case Codes.SendNumber: return "#: " + ((response.ResponseData[0] << 24) | (response.ResponseData[1] << 16) | (response.ResponseData[2] << 8) | response.ResponseData[3]).ToString();
				default: return "Code: " + response.Code.ToString();
			}
		}

		public void ProcessResponses(DebugMessage response)
		{
			string formatted = "RECEIVED: #" + response.ID.ToString() + " -> " + FormatResponseMessage(response);

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

	}
}