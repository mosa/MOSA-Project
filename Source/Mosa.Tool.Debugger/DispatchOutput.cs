// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.DebugEngine;
using System.Collections.Generic;

namespace Mosa.Tool.Debugger
{
	public partial class DispatchOutput : DebuggerDockContent
	{
		private List<string> events;

		public DispatchOutput()
		{
			InitializeComponent();

			events = new List<string>();

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
				case Codes.Ping: return "Ping ACK";
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
			string formatted = events.Count.ToString() + ": " + FormatResponseMessage(response) + " (" + response.ID.ToString() + ")";

			events.Add(formatted);

			if (listBox1.Items.Count == 25)
			{
				listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
			}

			listBox1.Items.Insert(0, formatted);
		}
	}
}
