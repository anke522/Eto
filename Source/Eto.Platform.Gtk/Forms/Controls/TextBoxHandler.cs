using System;
using Eto.Forms;
using System.Runtime.InteropServices;
using GLib;

namespace Eto.Platform.GtkSharp
{
	public class TextBoxHandler : GtkControl<Gtk.Entry, TextBox>, ITextBox
	{
		public TextBoxHandler ()
		{
			Control = new Gtk.Entry ();
			Control.SetSizeRequest (20, -1);
			Control.ActivatesDefault = true;
		}

		public override void AttachEvent (string handler)
		{
			switch (handler) {
				case TextBox.TextChangedEvent:
					Control.Changed += delegate {
						Widget.OnTextChanged (EventArgs.Empty);
					};
					break;
				default:
					base.AttachEvent (handler);
					break;
			}
		}

		public override string Text {
			get { return Control.Text; }
			set { Control.Text = value; }
		}

		public bool ReadOnly {
			get { return !Control.IsEditable; }
			set { Control.IsEditable = !value; }
		}
		
		public int MaxLength {
			get { return Control.MaxLength; }
			set { Control.MaxLength = value; }
		}

	}
}
