// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.DebugData;
using Mosa.Tool.Debugger.GDB;
using Mosa.Utility.Launcher;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.Debugger
{
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

		public string Status { set { MainForm.Status = value; } }

		public DebugSource DebugSource { get { return MainForm.DebugSource; } }

		public Connector GDBConnector { get { return MainForm.GDBConnector; } }
		public BasePlatform Platform { get { return MainForm.Platform; } }

		public uint NativeIntegerSize { get { return Platform.NativeIntegerSize; } }

		public MemoryCache MemoryCache { get { return MainForm.MemoryCache; } }

		public bool IsConnected { get { return GDBConnector?.IsConnected ?? false; } }
		public bool IsRunning { get { return GDBConnector.IsRunning; } }
		public bool IsPaused { get { return GDBConnector.IsPaused; } }

		public bool IsDockUpdatable { get; set; } = true;

		public ulong InstructionPointer { get; set; }
		public ulong StackFrame { get; set; }
		public ulong StackPointer { get; set; }
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
}
