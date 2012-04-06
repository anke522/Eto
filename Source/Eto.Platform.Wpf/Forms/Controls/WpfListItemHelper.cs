﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sw = System.Windows;
using swc = System.Windows.Controls;
using swd = System.Windows.Data;
using Eto.Forms;
using Eto.Platform.Wpf.Drawing;

namespace Eto.Platform.Wpf.Forms.Controls
{
	public static class WpfListItemHelper
	{
		public static sw.FrameworkElementFactory ItemTemplate ()
		{
			var factory = new sw.FrameworkElementFactory (typeof (swc.StackPanel));
			factory.SetValue (swc.StackPanel.OrientationProperty, swc.Orientation.Horizontal);
			factory.AppendChild (ImageBlock ());
			factory.AppendChild (TextBlock ());
			return factory;
		}

		class ColorConverter : swd.IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				sw.Media.Brush lCol = sw.SystemColors.WindowTextBrush;
				if (value != null)
				{
					var col = (Eto.Drawing.Color)value;
					if (col !=  Eto.Drawing.Color.Transparent)
						lCol = new sw.Media.SolidColorBrush(Generator.Convert(col));
				}
				return lCol;
			}

			public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}

		public static sw.FrameworkElementFactory TextBlock ()
		{
			var factory = new sw.FrameworkElementFactory (typeof (swc.TextBlock));
			factory.SetBinding (swc.TextBlock.TextProperty, new sw.Data.Binding { Path = new sw.PropertyPath("Text"), Mode = swd.BindingMode.TwoWay });
			factory.SetBinding(swc.TextBlock.ForegroundProperty, new sw.Data.Binding { Path = new sw.PropertyPath("TextColor"), Mode = swd.BindingMode.OneWay, Converter = new ColorConverter() });
			return factory;
		}

		class ImageConverter : swd.IValueConverter
		{
			public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				var item = value as IImageListItem;
				if (item == null || item.Image == null) return null;
				return ((IWpfImage)item.Image.Handler).GetIconClosestToSize (16);
			}

			public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				throw new NotImplementedException ();
			}
		}

		public static sw.FrameworkElementFactory ImageBlock ()
		{
			var factory = new sw.FrameworkElementFactory (typeof (swc.Image));
			factory.SetValue (swc.Image.MaxHeightProperty, 16.0);
			factory.SetValue (swc.Image.MaxWidthProperty, 16.0);
			factory.SetValue (swc.Image.StretchDirectionProperty, swc.StretchDirection.DownOnly);
			factory.SetValue (swc.Image.MarginProperty, new sw.Thickness (0, 2, 2, 2));
			factory.SetBinding (swc.Image.SourceProperty, new sw.Data.Binding { Converter = new ImageConverter () });
			return factory;
		}

	}
}
