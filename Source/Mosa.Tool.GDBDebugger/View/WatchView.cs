// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.GDB;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
{
	public partial class WatchView : DebugDockContent
	{
		private BindingList<WatchEntry> watches = new BindingList<WatchEntry>();

		private class WatchEntry
		{
			public ulong Address { get { return Watch.Address; } }

			public string Name { get { return Watch.Name; } }

			public int Size { get { return Watch.Size; } }

			public bool Signed { get { return Watch.Signed; } }

			public string HexValue { get; set; }

			public ulong Value { get; set; }

			[Browsable(false)]
			public Watch Watch;

			public WatchEntry(Watch watch)
			{
				Watch = watch;
			}
		}

		public WatchView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = watches;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[1].Width = 250;
		}

		public override void OnPause()
		{
			foreach (var watch in watches)
			{
				watch.Value = 0;
				watch.HexValue = string.Empty;

				GDBConnector.ReadMemory(watch.Address, watch.Size, OnMemoryRead);
			}

			Refresh();
		}

		public override void OnWatchChange()
		{
			watches.Clear();
			foreach (var watch in MainForm.Watchs)
			{
				watches.Add(new WatchEntry(watch));
			}

			OnPause();
		}

		private void OnMemoryRead(ulong address, byte[] bytes)
		{
			MethodInvoker method = delegate ()
			{
				UpdateDisplay(address, bytes);
			};

			BeginInvoke(method);
		}

		private void UpdateDisplay(ulong address, byte[] bytes)
		{
			foreach (var watch in watches)
			{
				if (watch.Address != address || watch.Size != bytes.Length)
					continue;

				watch.Value = ToLong(bytes, bytes.Length);
				watch.HexValue = BasePlatform.ToHex(watch.Value, watch.Size);
			}
		}

		private static ulong ToLong(byte[] bytes, int size)
		{
			ulong value = 0;

			for (int i = 0; i < bytes.Length; i++)
			{
				ulong shifted = (ulong)(bytes[i] << (i * 8));
				value = value | shifted;
			}

			return value;
		}

		public void AddWatch(string name, ulong address, int size, bool signed)
		{
			var watch = new Watch(name, address, size, signed);
			var watchEntry = new WatchEntry(watch);

			watches.Add(watchEntry);
		}
	}
}
