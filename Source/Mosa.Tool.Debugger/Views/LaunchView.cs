// Copyright (c) MOSA Project. Licensed under the New BSD License.

using SharpDisasm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class LaunchView : DebugDockContent
	{
		private BindingList<LaunchEntry> launchEntries = new BindingList<LaunchEntry>();

		private List<string> ImageExtensions = new List<string>() { ".img", ".iso" };

		private class LaunchEntry
		{
			[Browsable(false)]
			public string ImageFile { get; set; }

			public string Image { get { return Path.GetFileName(ImageFile); } }

			public string Directory { get { return Path.GetDirectoryName(ImageFile); } }

			public string DebugFile { get; set; }

			public string BreakpointFile { get; set; }

			public string WatchFile { get; set; }
		}

		public LaunchView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();

			AddImages();

			dataGridView1.DataSource = launchEntries;
			dataGridView1.AutoResizeColumns();

			dataGridView1.Columns[0].Width = 300;
			dataGridView1.Columns[1].Width = 300;
			dataGridView1.Columns[2].Width = 200;
			dataGridView1.Columns[3].Width = 200;
			dataGridView1.Columns[4].Width = 200;
		}

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			dataGridView1.ClearSelection();
			dataGridView1.Rows[e.RowIndex].Selected = true;

			if (e.Button != MouseButtons.Right)
				return;

			var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

			var clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as LaunchEntry;

			var menu = new MenuItem(clickedEntry.Image);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.Image });

			m.Show(dataGridView1, relativeMousePosition);
		}

		private void AddImages()
		{
			SearchImages(Path.GetTempPath(), ImageExtensions);
			SearchImages(Path.Combine(Path.GetTempPath(), "MOSA"), ImageExtensions);
			SearchImages(Directory.GetCurrentDirectory(), ImageExtensions);
		}

		private void SearchImages(string directory, List<string> patterns)
		{
			if (!Directory.Exists(directory))
				return;

			foreach (var pattern in patterns)
			{
				SearchImages(directory, pattern);
			}
		}

		private void SearchImages(string directory, string pattern)
		{
			if (!Directory.Exists(directory))
				return;

			foreach (var file in Directory.GetFiles(directory, "*" + pattern))
			{
				AddEntry(file);
			}
		}

		private void AddEntry(string imagefile)
		{
			var entry = CreateEntry(imagefile);

			RemoveEntry(imagefile);

			launchEntries.Add(entry);
		}

		private LaunchEntry CreateEntry(string imagefile)
		{
			if (!File.Exists(imagefile))
				return null;

			string directory = Path.GetDirectoryName(imagefile);
			string imagefileWithException = Path.GetFileNameWithoutExtension(imagefile);

			var debugFile = Path.Combine(directory, imagefileWithException + ".debug");
			var breakpointFile = Path.Combine(directory, imagefileWithException + ".breakpoints");
			var watchFile = Path.Combine(directory, imagefileWithException + ".watches");

			if (!File.Exists(debugFile))
			{
				debugFile = null;
			}

			if (!File.Exists(breakpointFile))
			{
				breakpointFile = null;
			}

			if (!File.Exists(watchFile))
			{
				watchFile = null;
			}

			var entry = new LaunchEntry()
			{
				ImageFile = imagefile,
				DebugFile = debugFile,
				WatchFile = watchFile,
				BreakpointFile = breakpointFile
			};

			return entry;
		}

		private void RemoveEntry(string filename)
		{
			for (int i = 0; i < launchEntries.Count; i++)
			{
				if (launchEntries[i].ImageFile == filename)
				{
					launchEntries.RemoveAt(i);
				}
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (odfVMImage.ShowDialog() == DialogResult.OK)
			{
				AddEntry(odfVMImage.FileName);

				dataGridView1.ClearSelection();
				dataGridView1.Rows[launchEntries.Count - 1].Selected = true;
			}
		}

		private void btnLaunch_Click(object sender, EventArgs e)
		{
			if (dataGridView1.SelectedRows.Count == 0)
				return;

			var entry = dataGridView1.SelectedRows[0].DataBoundItem as LaunchEntry;

			Launch(entry);
		}

		private void Launch(LaunchEntry entry)
		{
			MainForm.LaunchImage(entry.ImageFile, entry.DebugFile, entry.BreakpointFile, entry.WatchFile);
		}

		private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			var entry = dataGridView1.Rows[e.RowIndex].DataBoundItem as LaunchEntry;

			Launch(entry);
		}
	}
}
