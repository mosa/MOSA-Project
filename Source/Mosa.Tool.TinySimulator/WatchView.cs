/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mosa.Tool.TinySimulator
{
	public partial class WatchView : SimulatorDockContent
	{
		private BindingList<WatchEntry> watches = new BindingList<WatchEntry>();

		private class WatchEntry
		{
			public string Address { get; set; }

			public ulong address;

			public string Name { get; set; }

			public string Value { get; set; }

			public string Hex { get; set; }

			public int size;

			public bool signed;

			public bool display32 = true;

			public WatchEntry(string name, ulong address, int size, bool signed, bool display32)
			{
				Name = name;
				this.Address = "0x" + address.ToString("X8");
				this.address = address;
				this.size = size;
				this.signed = signed;
				this.Value = "[N/A]";
				this.Hex = "[N/A]";
				this.display32 = display32;
			}

			public void Update(object value)
			{
				if (size == 32 && !signed)
				{
					uint val = (uint)value;
					Value = val.ToString();
					Hex = "0x" + val.ToString("X8");
					return;
				}
				if (size == 32 && signed)
				{
					int val = (int)value;
					Value = val.ToString();
					Hex = "0x" + val.ToString("X8");
					return;
				}
				if (size == 16 && signed)
				{
					short val = (short)value;
					Value = val.ToString();
					Hex = "0x" + val.ToString("X4");
					return;
				}
				if (size == 16 && signed)
				{
					ushort val = (ushort)value;
					Value = val.ToString();
					Hex = "0x" + val.ToString("X4");
					return;
				}
				if (size == 8 && signed)
				{
					byte val = (byte)value;
					Value = val.ToString();
					Hex = "0x" + val.ToString("X2");
					return;
				}
				if (size == 8 && signed)
				{
					sbyte val = (sbyte)value;
					Value = val.ToString();
					Hex = "0x" + val.ToString("X2");
					return;
				}
			}
		}

		public WatchView()
		{
			InitializeComponent();
			dataGridView1.DataSource = watches;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[1].Width = 250;
		}

		public override void UpdateDock(BaseSimState simState)
		{
			if (simState == null)
				return;

			var toplist = simState.Values["WatchValues"] as Dictionary<int, Dictionary<ulong, object>>;

			if (toplist == null)
				return;

			foreach (var entry in watches)
			{
				var bysize = toplist[entry.size] as Dictionary<ulong, object>;

				if (bysize == null)
					continue;

				var data = bysize[entry.address];

				if (data == null)
					continue;

				entry.Update(data);
			}

			this.Refresh();
		}

		public void AddWatch(string name, ulong address, int size, bool signed)
		{
			var watch = new WatchEntry(name, address, size, signed, MainForm.Display32);

			watches.Add(watch);
		}
	}
}