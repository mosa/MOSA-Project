// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.DebugData;
using Mosa.Tool.Debugger.GDB;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;

namespace Mosa.Tool.Debugger;

public partial class DebugDockContent : DockContent
{
	protected MainForm MainForm;

	public DebugDockContent()
	{
		InitializeComponent();
	}

	public DebugDockContent(MainForm mainForm) : this()
	{
		MainForm = mainForm;
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public string Status
	{ set => MainForm.Status = value; }

	public DebugDockContent(string status)
	{
		Status = status;
	}

	public DebugSource DebugSource => MainForm.DebugSource;

	public Connector GDBConnector => MainForm.GDBConnector;

	public BasePlatform Platform => MainForm.Platform;

	public uint NativeIntegerSize => Platform.NativeIntegerSize;

	public MemoryCache MemoryCache => MainForm.MemoryCache;

	public bool IsConnected => GDBConnector?.IsConnected ?? false;

	public bool IsRunning => GDBConnector.IsRunning;

	public bool IsPaused => GDBConnector.IsPaused;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public bool IsDockUpdatable { get; set; } = true;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public ulong InstructionPointer { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public ulong StackFrame { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public ulong StackPointer { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public ulong StatusFlag { get; set; }

	protected bool IsReady
	{
		get
		{
			if (!IsConnected)
				return false;

			if (Platform == null)
				return false;

			if (Platform.Registers == null)
				return false;

			return true;
		}
	}

	public void UpdateDockFocus()
	{
		if (!IsDockUpdatable)
			return;

		InstructionPointer = MainForm.InstructionPointer;
		StackFrame = MainForm.StackFrame;
		StackPointer = MainForm.StackPointer;
		StatusFlag = MainForm.StatusFlag;
	}

	public virtual void OnPause()
	{
		if (IsReady)
			UpdateDisplay();
		else
			ClearDisplay();
	}

	public virtual void OnRunning()
	{
	}

	public virtual void OnBreakpointChange()
	{
	}

	public virtual void OnWatchChange()
	{
	}

	protected virtual void UpdateDisplay()
	{
	}

	protected virtual void ClearDisplay()
	{
	}

	public static string ToHex(ulong address)
	{
		return $"0x{address.ToString((address <= uint.MaxValue) ? "X4" : "X8")}"; ;
	}
}
