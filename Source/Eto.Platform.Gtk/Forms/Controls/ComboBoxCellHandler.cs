using System;
using Eto.Forms;

namespace Eto.Platform.GtkSharp.Forms.Controls
{
	public class ComboBoxCellHandler : CellHandler<Gtk.CellRendererCombo, ComboBoxCell>, IComboBoxCell
	{
		CollectionHandler collection;
		Gtk.ListStore listStore;
		
		public ComboBoxCellHandler ()
		{
			listStore = new Gtk.ListStore (typeof(string), typeof(string));
			Control = new Gtk.CellRendererCombo {
				Model = listStore,
				TextColumn = 0,
				HasEntry = false
			};
			this.Control.Edited += delegate(object o, Gtk.EditedArgs args) {
				SetValue (args.Path, args.NewText);
			};
		}
		
		protected override void BindCell ()
		{
			Column.ClearAttributes (Control);
			Column.AddAttribute (Control, "text", ColumnIndex);
		}
		
		public override void SetEditable (Gtk.TreeViewColumn column, bool editable)
		{
			this.Control.Editable = editable;
		}
		
		public override GLib.Value GetValue (IGridItem item, int column)
		{
			if (item != null) {
				var ret = item.GetValue (column);
				if (ret != null)
					return new GLib.Value (ret);
			}
			
			return new GLib.Value ((string)null);
		}
		
		public override void AttachEvent (string eventHandler)
		{
			switch (eventHandler) {
			case GridView.EndCellEditEvent:
				Control.Edited += (sender, e) => {
					Source.EndCellEditing (e.Path, this.ColumnIndex);
				};
				break;
			default:
				base.AttachEvent (eventHandler);
				break;
			}
		}

		public class CollectionHandler : CollectionChangedHandler<IListItem, IListStore>
		{
			public ComboBoxCellHandler Handler { get; set; }

			public override void AddItem (IListItem item)
			{
				Handler.listStore.AppendValues (item.Text, item.Key);
			}

			public override void InsertItem (int index, IListItem item)
			{
				Handler.listStore.InsertWithValues (index, item.Text, item.Key);
			}

			public override void RemoveItem (int index)
			{
				Gtk.TreeIter iter;
				if (Handler.listStore.IterNthChild (out iter, index))
					Handler.listStore.Remove (ref iter);
			}

			public override void RemoveAllItems ()
			{
				Handler.listStore.Clear ();
			}
		}
		
		public IListStore DataStore {
			get { return collection != null ? collection.DataStore : null; }
			set {
				if (collection != null)
					collection.Unregister ();
				collection = new CollectionHandler{ Handler = this };
				collection.Register (value);
			}
		}

	}
}

