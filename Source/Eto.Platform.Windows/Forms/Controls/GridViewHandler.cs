using System;
using swf = System.Windows.Forms;
using Eto.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Eto.Platform.Windows.Forms.Controls
{
	public class GridViewHandler : WindowsControl<swf.DataGridView, GridView>, IGridView
	{
		CollectionHandler collection;
		ContextMenu contextMenu;
		
		public GridViewHandler ()
		{
			Control = new swf.DataGridView {
				VirtualMode = true,
				MultiSelect = false,
				SelectionMode = swf.DataGridViewSelectionMode.FullRowSelect,
				RowHeadersVisible = false,
				AllowUserToAddRows = false,
				AllowUserToResizeRows = false,
				ColumnHeadersHeightSizeMode = swf.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
			};
			Control.CellValueNeeded += (sender, e) => {
				var item = collection.DataStore [e.RowIndex];
				var col = Widget.Columns [e.ColumnIndex].Handler as GridColumnHandler;
				if (item != null && col != null)
					e.Value = col.GetCellValue (item.GetValue (e.ColumnIndex));
			};

			Control.CellValuePushed += (sender, e) => {
				var item = collection.DataStore [e.RowIndex];
				var col = Widget.Columns [e.ColumnIndex].Handler as GridColumnHandler;
				if (item != null && col != null)
					item.SetValue (e.ColumnIndex, col.GetItemValue (e.Value));
			};
		}

		public override void OnLoadComplete (EventArgs e)
		{
			base.OnLoadComplete (e);

			// user can resize auto-sizing columns
			foreach (swf.DataGridViewColumn col in Control.Columns) {
				var width = col.Width;
				col.AutoSizeMode = swf.DataGridViewAutoSizeColumnMode.None;
				col.Width = width;
			}
		}

		public override void AttachEvent (string handler)
		{
			switch (handler) {
			case GridView.BeginCellEditEvent:
				Control.CellBeginEdit += (sender, e) => {
					var item = collection.DataStore [e.RowIndex];
					var column = Widget.Columns [e.ColumnIndex];
					Widget.OnBeginCellEdit (new GridViewCellArgs (column, e.RowIndex, e.ColumnIndex, item));
				};
				break;
			case GridView.EndCellEditEvent:
				Control.CellEndEdit += (sender, e) => {
					var item = collection.DataStore [e.RowIndex];
					var column = Widget.Columns [e.ColumnIndex];
					Widget.OnEndCellEdit (new GridViewCellArgs (column, e.RowIndex, e.ColumnIndex, item));
				};
				break;
			case GridView.SelectionChangedEvent:
				Control.SelectionChanged += delegate {
					Widget.OnSelectionChanged (EventArgs.Empty);
				};
				break;
			default:
				base.AttachEvent (handler);
				break;
			}
		}

		public void InsertColumn (int index, GridColumn column)
		{
			var colHandler = ((GridColumnHandler)column.Handler);
			if (index >= 0 && this.Control.Columns.Count != 0)
				this.Control.Columns.Insert (index, colHandler.Control);
			else
				this.Control.Columns.Add (colHandler.Control);
		}

		public void RemoveColumn (int index, GridColumn column)
		{
			var colHandler = ((GridColumnHandler)column.Handler);
			if (index >= 0)
				this.Control.Columns.RemoveAt (index);
			else
				this.Control.Columns.Remove (colHandler.Control);
		}

		public void ClearColumns ()
		{
			this.Control.Columns.Clear ();
		}

		public bool ShowHeader {
			get { return this.Control.ColumnHeadersVisible; }
			set { this.Control.ColumnHeadersVisible = value; }
		}

		public bool AllowColumnReordering {
			get { return this.Control.AllowUserToOrderColumns; }
			set { this.Control.AllowUserToOrderColumns = value; }
		}
		
		class CollectionHandler : CollectionChangedHandler<IGridItem, IGridStore>
		{
			public GridViewHandler Handler { get; set; }
			
			public override void AddRange (IEnumerable<IGridItem> items)
			{
				Handler.Control.Refresh ();
				Handler.Control.RowCount = DataStore.Count;
			}
			
			public override void AddItem (IGridItem item)
			{
				Handler.Control.RowCount ++;
				Handler.Control.Refresh ();
			}

			public override void InsertItem (int index, IGridItem item)
			{
				Handler.Control.RowCount ++;
				Handler.Control.Refresh ();
			}

			public override void RemoveItem (int index)
			{
				Handler.Control.RowCount --;
				Handler.Control.Refresh ();
			}

			public override void RemoveAllItems ()
			{
				Handler.Control.RowCount = 0;
				Handler.Control.Refresh ();
			}
		}

		public IGridStore DataStore {
			get { return collection != null ? collection.DataStore : null; }
			set {
				if (collection != null)
					collection.Unregister ();
				collection = new CollectionHandler { Handler = this };
				collection.Register (value);
			}
		}
		
		public ContextMenu ContextMenu {
			get { return contextMenu; }
			set {
				contextMenu = value;
				if (contextMenu != null)
					this.Control.ContextMenuStrip = ((ContextMenuHandler)contextMenu.Handler).Control;
				else
					this.Control.ContextMenuStrip = null;
			}
		}

		public bool AllowMultipleSelection {
			get { return Control.MultiSelect; }
			set { Control.MultiSelect = value; }
		}

		public IEnumerable<int> SelectedRows {
			get { return Control.SelectedRows.OfType<swf.DataGridViewRow> ().Select (r => r.Index); }
		}

		public void SelectAll ()
		{
			Control.SelectAll ();
		}

		public void SelectRow (int row)
		{
			Control.Rows [row].Selected = true;
		}

		public void UnselectRow (int row)
		{
			Control.Rows [row].Selected = false;
		}

		public void UnselectAll ()
		{
			Control.ClearSelection ();
		}
	}
}

