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

		public ulong InstructionPointer { get { return MainForm.InstructionPointer; } }
		public ulong StackFrame { get { return MainForm.StackFrame; } }
		public ulong StackPointer { get { return MainForm.StackPointer; } }
		public ulong StatusFlag { get { return MainForm.StatusFlag; } }

		public virtual void OnPause()
		{
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
	}
}
