using System;
using Eto.Forms;
using Eto.Drawing;

namespace Eto.Test.Sections.Behaviors
{
	public class AllControlsBase : Panel
	{
		public AllControlsBase ()
		{
			
			var layout = new DynamicLayout (this);
			
			layout.AddRow (null, LabelControl (), ButtonControl (), null);
			layout.AddRow (null, TextBoxControl (), TextAreaControl (), null);
			layout.AddRow (null, CheckBoxControl (), RadioButtonControl (), null);
			layout.AddRow (null, DateTimeControl (), NumericUpDownControl (), null);
			layout.AddRow (null, ComboBoxControl (), DrawableControl (), null);
			layout.Add (null);
		}
		
		Control LabelControl ()
		{
			var control = new Label{ Text = "Label Control" };
			LogEvents (control);
			return control;
		}
		
		Control ButtonControl ()
		{
			var control = new Button { Text = "Button Control" };
			LogEvents (control);
			return control;
		}

		Control TextBoxControl ()
		{
			var control = new TextBox { Text = "TextBox Control" };
			LogEvents (control);
			return control;
		}
		
		Control TextAreaControl ()
		{
			var control = new TextArea { Text = "TextArea Control" };
			LogEvents (control);
			return control;
		}

		Control CheckBoxControl ()
		{
			var control = new CheckBox { Text = "CheckBox Control" };
			LogEvents (control);
			return control;
		}
		
		Control RadioButtonControl ()
		{
			var control = new RadioButton { Text = "RadioButton Control" };
			LogEvents (control);
			return control;
		}

		Control DateTimeControl ()
		{
			var control = new DateTimePicker { };
			LogEvents (control);
			return control;
		}
		
		Control NumericUpDownControl ()
		{
			var control = new NumericUpDown { };
			LogEvents (control);
			return control;
		}

		Control ComboBoxControl ()
		{
			var control = new ComboBox { };
			control.Items.Add (new ListItem{ Text = "Item 1" });
			control.Items.Add (new ListItem{ Text = "Item 2" });
			control.Items.Add (new ListItem{ Text = "Item 3" });
			LogEvents (control);
			return control;
		}
		
		Control DrawableControl ()
		{
			var control = new Drawable { Size = new Size (100, 30), CanFocus = true };
			control.Paint += delegate(object sender, PaintEventArgs pe) {
				pe.Graphics.FillRectangle (Color.Blue, pe.ClipRectangle);
			};
			LogEvents (control);
			return control;
		}

		protected virtual void LogEvents (Button control)
		{
			control.Click += delegate {
				Log.Write (control, "Click");
			};
			
			LogEvents ((Control)control);	
		}
		
		protected virtual void LogEvents (RadioButton control)
		{
			control.CheckedChanged += delegate {
				Log.Write (control, "CheckedChanged");
			};
			
			LogEvents ((Control)control);	
		}
		
		protected virtual void LogEvents (CheckBox control)
		{
			control.CheckedChanged += delegate {
				Log.Write (control, "CheckedChanged");
			};
			
			LogEvents ((Control)control);	
		}
		
		protected virtual void LogEvents (Control control)
		{
		}
	}
}

