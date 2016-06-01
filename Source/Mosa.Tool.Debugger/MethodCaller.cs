// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Utility.DebugEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger
{
	/// <summary>
	///
	/// </summary>
	public partial class MethodCaller : DebuggerDockContent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodCaller"/> class.
		/// </summary>
		public MethodCaller()
		{
			InitializeComponent();
			cbParameter1.SelectedIndex = 0;
			cbParameter2.SelectedIndex = 0;
			cbParameter3.SelectedIndex = 0;
			cbResultType.SelectedIndex = 0;
		}

		public override void Connect()
		{
		}

		public override void Disconnect()
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{
			SetUnitTestMethodAddress();
		}

		private bool AddData(List<uint> data, ComboBox box, TextBox tb)
		{
			if (box.SelectedIndex == 0)
				return false;

			if (box.SelectedIndex == 1)
			{
				data.Add((uint)tb.Text.ParseHexOrDecimal());
				return true;
			}
			else if (box.SelectedIndex == 2)
			{
				ulong val = tb.Text.ParseHexOrDecimal();
				data.Add((uint)(uint)(val & 0xFFFFFFFF));
				data.Add((uint)(uint)((val >> 32) & 0xFFFFFFFF));
				return true;
			}
			else
			{
				throw new Exception();
			}
		}

		private void SetUnitTestMethodAddress()
		{
			uint at = 0;
			List<uint> data = new List<uint>();

			try
			{
				at = (uint)tbMethodAddress.Text.ParseHexOrDecimal();
			}
			catch { }

			if (at == 0)
			{
				Status = "ERROR: Invalid address parameter";
				return;
			}

			Status = "Preparing (1/4)...";

			SendCommand(new DebugMessage(DebugCode.SetUnitTestMethodAddress, new uint[] { at }));

			try
			{
				if (AddData(data, cbParameter1, tbParameter1))
					if (AddData(data, cbParameter2, tbParameter2))
						AddData(data, cbParameter3, tbParameter3);
			}
			catch
			{
				Status = "Invalid Parameter";
				return;
			}

			Status = "Preparing (2/4)...";

			uint index = 0;
			foreach (var value in data)
			{
				SendCommand(new DebugMessage(DebugCode.SetUnitTestMethodParameter, new uint[] { index, value }));
				index++;
			}

			Status = "Preparing (3/4)...";

			SendCommand(new DebugMessage(DebugCode.SetUnitTestMethodParameterCount, new uint[] { index }));

			Status = "Preparing (4/4)...";

			SendCommand(new DebugMessage(DebugCode.SetUnitTestResultType, new uint[] { (uint)cbResultType.SelectedIndex }));

			Status = "Executing...";

			SendCommand(new DebugMessage(DebugCode.StartUnitTest, (byte[])null));
		}
	}
}
