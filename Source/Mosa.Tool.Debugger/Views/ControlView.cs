// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Threading;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views;

public partial class ControlView : DebugDockContent
{
	public ulong ReturnAddress { get; private set; } = 0;

	public ControlView(MainForm mainForm)
		: base(mainForm)
	{
		InitializeComponent();
	}

	public override void OnRunning()
	{
		btnPause.Enabled = true;
		btnContinue.Enabled = false;
	}

	public override void OnPause()
	{
		btnContinue.Enabled = true;
		btnPause.Enabled = false;

		ReturnAddress = 0;

		if (StackFrame == 0 || InstructionPointer == 0 || StackPointer == 0)
			return;

		// FIXME: x86 specific implementation
		var symbol = DebugSource.GetFirstSymbolsStartingAt(InstructionPointer);

		if (symbol != null)
		{
			// new stack frame has not been setup
			MemoryCache.ReadMemory(StackPointer, NativeIntegerSize * 2, OnMemoryReadPrologue);
			return;
		}

		symbol = DebugSource.GetFirstSymbolsStartingAt(InstructionPointer - 2);

		if (symbol != null)
		{
			// new stack frame has not been setup
			MemoryCache.ReadMemory(StackPointer + NativeIntegerSize, NativeIntegerSize * 2, OnMemoryReadPrologue);
			return;
		}

		MemoryCache.ReadMemory(StackFrame, NativeIntegerSize * 2, OnMemoryRead);
	}

	private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

	private void OnMemoryReadPrologue(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplayPrologue(address, bytes)));

	private void UpdateDisplay(ulong address, byte[] memory)
	{
		if (memory.Length < 8)
			return; // something went wrong!

		var ebp = MainForm.ToLong(memory, 0, 4);
		var ip = MainForm.ToLong(memory, 4, 4);

		if (ip == 0)
			return;

		ReturnAddress = ip;

		//btnPause.Enabled = true;
	}

	private void UpdateDisplayPrologue(ulong address, byte[] memory)
	{
		if (memory.Length < 8)
			return; // something went wrong!

		// FIXME: x86 specific implementation
		var ip = MainForm.ToLong(memory, 0, 4);

		if (ip == 0)
			return;

		ReturnAddress = ip;

		//btnPause.Enabled = true;
	}

	private void btnStep_Click(object sender, EventArgs e)
	{
		if (GDBConnector == null)
			return;

		if (GDBConnector.IsRunning)
			return;

		MemoryCache.Clear();
		GDBConnector.ClearAllBreakPoints();
		GDBConnector.Step();
		MainForm.ResendBreakPoints();
	}

	private void btnStepN_Click(object sender, EventArgs e)
	{
		if (GDBConnector == null)
			return;

		if (GDBConnector.IsRunning)
			return;

		uint steps;

		try
		{
			steps = Convert.ToUInt32(tbSteps.Text);
		}
		catch
		{
			MessageBox.Show($"Invalid input, '{tbSteps.Text}' is not a valid number.");
			return;
		}

		if (steps == 0)
			return;

		MemoryCache.Clear();

		if (MainForm.BreakPoints.Count != 0)
		{
			GDBConnector.ClearAllBreakPoints();
			GDBConnector.Step(true);

			while (GDBConnector.IsRunning)
			{
				Thread.Yield();
			}

			MainForm.ResendBreakPoints();

			steps--;
		}

		if (steps == 0)
			return;

		GDBConnector.StepN(steps);
	}

	private void btnRestart_Click(object sender, EventArgs e)
	{
		MainForm.LaunchImage(true);
	}

	private void btnStart_Click(object sender, EventArgs e)
	{
		Continue();
	}

	private void Continue()
	{
		if (GDBConnector == null)
			return;

		if (GDBConnector.IsRunning)
			return;

		MemoryCache.Clear();

		if (MainForm.BreakPoints.Count != 0)
		{
			GDBConnector.ClearAllBreakPoints();
			GDBConnector.Step(true);

			while (GDBConnector.IsRunning)
			{
				Thread.Yield();
			}

			MainForm.ResendBreakPoints();
		}

		GDBConnector.Continue();
	}

	private void btnStop_Click(object sender, EventArgs e)
	{
		if (GDBConnector == null)
			return;

		GDBConnector.Break();
		GDBConnector.GetRegisters();
	}

	private void btnStepOut_Click(object sender, EventArgs e)
	{
		if (Platform == null)
			return;

		MainForm.AddBreakPoint(ReturnAddress, true);

		Continue();
	}
}
