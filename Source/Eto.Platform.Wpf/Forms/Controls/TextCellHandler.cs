﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using swc = System.Windows.Controls;
using swd = System.Windows.Data;
using sw = System.Windows;

namespace Eto.Platform.Wpf.Forms.Controls
{
	public class TextCellHandler : CellHandler<swc.DataGridTextColumn, TextCell>, ITextCell
	{
		class Column : swc.DataGridTextColumn
		{
			public TextCellHandler Handler { get; set; }

			protected override sw.FrameworkElement GenerateElement (swc.DataGridCell cell, object dataItem)
			{
				var element = base.GenerateElement (cell, dataItem);
				element.DataContextChanged += (sender, e) => {
					var text = sender as swc.TextBlock;
					var item = text.DataContext as IGridItem;
					text.Text = item != null ? Convert.ToString (item.GetValue (this.Handler.DataColumn)) : null;
				};
				return element;
			}

			protected override sw.FrameworkElement GenerateEditingElement (swc.DataGridCell cell, object dataItem)
			{
				var element = base.GenerateEditingElement (cell, dataItem) as swc.TextBox;
				element.DataContextChanged += (sender, e) => {
					var text = sender as swc.TextBox;
					var item = text.DataContext as IGridItem;
					text.Text = item != null ? Convert.ToString (item.GetValue (this.Handler.DataColumn)) : null;
				};
				return element;
			}

			protected override bool CommitCellEdit (sw.FrameworkElement editingElement)
			{
				var text = editingElement as swc.TextBox;
				var item = text.DataContext as IGridItem;
				if (item != null)
					item.SetValue (Handler.DataColumn, text.Text);
				return true;
			}

		}

		public TextCellHandler ()
		{
			Control = new Column { Handler = this };
		}
	}
}
