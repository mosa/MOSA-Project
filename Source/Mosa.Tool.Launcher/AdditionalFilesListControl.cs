using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	//Some code is from: https://www.codeproject.com/Articles/6646/In-place-editing-of-ListView-subitems
	class AdditionalFilesListControl : MetroListView
	{
		private struct NMHDR
		{
			public IntPtr hwndFrom;
			public Int32 idFrom;
			public Int32 code;
		}

		private ListViewItem _selectedItem;
		private ListViewItem.ListViewSubItem _selectedSubItem;
		private int _selectedColumn;

		private TextBox _editingControl;

		private const int WM_HSCROLL = 0x114;
		private const int WM_VSCROLL = 0x115;
		private const int WM_SIZE = 0x05;
		private const int WM_NOTIFY = 0x4E;

		private const int HDN_FIRST = -300;
		private const int HDN_BEGINDRAG = (HDN_FIRST - 10);
		private const int HDN_ITEMCHANGINGA = (HDN_FIRST - 0);
		private const int HDN_ITEMCHANGINGW = (HDN_FIRST - 20);

		public AdditionalFilesListControl()
		{
			base.FullRowSelect = true;
			base.View = View.Details;
			base.AllowColumnReorder = false;

			_editingControl = new TextBox();
			base.Controls.Add(_editingControl);

			_editingControl.Hide();
			_editingControl.Leave += _editingControl_Leave;
			_editingControl.KeyPress += _editingControl_KeyPress;
			_editingControl.LostFocus += _editingControl_Leave;
		}

		private void _editingControl_Leave(object sender, EventArgs e)
		{
			HideItemEditor(true);
		}

		private void _editingControl_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
				case (char)Keys.Escape:
					HideItemEditor(false);
					e.Handled = true;
					break;

				case (char)Keys.Enter:
					HideItemEditor(true);
					e.Handled = true;
					break;
			}
		}

		protected override void WndProc(ref Message msg)
		{
			switch (msg.Msg)
			{
				case WM_VSCROLL:
				case WM_HSCROLL:
				case WM_SIZE:
					HideItemEditor(false);
					break;
				case WM_NOTIFY:
					NMHDR h = (NMHDR)Marshal.PtrToStructure(msg.LParam, typeof(NMHDR));
					if (h.code == HDN_BEGINDRAG ||
						h.code == HDN_ITEMCHANGINGA ||
						h.code == HDN_ITEMCHANGINGW)
						HideItemEditor(false);
					break;
			}

			base.WndProc(ref msg);
		}

		public Rectangle GetSubItemBounds(ListViewItem Item, int SubItem)
		{
			Rectangle itemBounds = Item.GetBounds(ItemBoundsPortion.Entire);

			int testX = itemBounds.Left;
			for (int i = 0; i < Columns.Count; i++)
			{
				ColumnHeader col = Columns[i];
				if (col.Index == SubItem)
					return new Rectangle(testX, itemBounds.Top, col.Width, itemBounds.Height);

				testX += col.Width;
			}

			return Rectangle.Empty;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (SelectedItems.Count != 1)
			{
				HideItemEditor(true);
				return;
			}

			ListViewHitTestInfo hitTest = HitTest(PointToClient(Cursor.Position));

			if (hitTest.Location == ListViewHitTestLocations.None)
			{
				_selectedItem = null;
				_selectedSubItem = null;

				HideItemEditor(true);
			}
			else
			{
				_selectedItem = hitTest.Item;
				_selectedSubItem = hitTest.SubItem;
				_selectedColumn = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
			}
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);

			if (_selectedItem == null || _selectedSubItem == null || SelectedItems.Count != 1)
			{
				HideItemEditor(true);
				return;
			}

			if (_selectedColumn != 0)
				return;

			ShowItemEditor();
		}

		private void ShowItemEditor()
		{
			Rectangle rect = GetSubItemBounds(_selectedItem, _selectedColumn);

			_editingControl.Text = _selectedSubItem.Text;
			_editingControl.Bounds = rect;
			_editingControl.Show();
			_editingControl.BringToFront();
			_editingControl.Select();
			_editingControl.SelectAll();
		}

		private void HideItemEditor(bool updateValue)
		{
			_editingControl.Hide();

			if (updateValue && _selectedSubItem != null && !string.IsNullOrEmpty(_editingControl.Text))
			{
				_selectedSubItem.Text = _editingControl.Text;
			}
		}
	}
}
