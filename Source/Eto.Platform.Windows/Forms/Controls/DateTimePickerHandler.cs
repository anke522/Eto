using System;
using Eto.Forms;
using System.Globalization;

namespace Eto.Platform.Windows.Forms.Controls
{
	public class DateTimePickerHandler : WindowsControl<System.Windows.Forms.DateTimePicker, DateTimePicker>, IDateTimePicker
	{
		public DateTimePickerHandler ()
		{
			Control = new System.Windows.Forms.DateTimePicker ();
			Control.ShowCheckBox = true;
			Mode = DateTimePicker.DefaultMode;
			Value = null;
			Control.ValueChanged += delegate {
				Widget.OnValueChanged (EventArgs.Empty);
			};
		}

		public DateTimePickerMode Mode
		{
			get
			{
				switch (Control.Format) {
					case System.Windows.Forms.DateTimePickerFormat.Long:
						return DateTimePickerMode.DateTime;
					case System.Windows.Forms.DateTimePickerFormat.Short:
						return DateTimePickerMode.Date;
					case System.Windows.Forms.DateTimePickerFormat.Time:
						return DateTimePickerMode.Time;
					default:
						throw new NotImplementedException ();
				}
			}
			set
			{
				switch (value) {
					case DateTimePickerMode.DateTime:
						Control.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
						var format = CultureInfo.CurrentUICulture.DateTimeFormat;
						Control.CustomFormat = format.ShortDatePattern + " " + format.LongTimePattern;
						break;
					case DateTimePickerMode.Date:
						Control.Format = System.Windows.Forms.DateTimePickerFormat.Short;
						break;
					case DateTimePickerMode.Time:
						Control.Format = System.Windows.Forms.DateTimePickerFormat.Time;
						break;
					default:
						throw new NotImplementedException ();
				}
			}
		}

		public DateTime MinDate
		{
			get
			{
				return Control.MinDate;
			}
			set
			{
				Control.MinDate = value;
			}
		}

		public DateTime MaxDate
		{
			get
			{
				return Control.MaxDate;
			}
			set
			{
				Control.MaxDate = value;
			}
		}

		public DateTime? Value
		{
			get
			{
				if (!Control.Checked) return null;
				return Control.Value;
			}
			set
			{
				if (value != null) {
					Control.Value = value.Value;
					Control.Checked = true;
				}
				else
					Control.Checked = false;
			}
		}
	}
}

