#define SWF_1_1
using System;
using SD = System.Drawing;
using SWF = System.Windows.Forms;
using Eto.Forms;
using Eto.Drawing;
using System.Linq;

namespace Eto.Platform.Windows
{
	public class TableLayoutHandler : WindowsLayout<System.Windows.Forms.TableLayoutPanel, TableLayout>, ITableLayout
	{
		Size spacing;
		Control[,] views;

		public override object LayoutObject
		{
			get	{ return Control; }
		}
		
		/*
		class MyTableLayoutPanel : System.Windows.Forms.TableLayoutPanel
		{
			public MyTableLayoutPanel()
			{
				base.DoubleBuffered = true;
			}
		}*/

		public TableLayoutHandler()
		{
			Control = new SWF.TableLayoutPanel();
			this.Control.SuspendLayout ();
			this.Control.Margin = SWF.Padding.Empty;
			this.Control.Dock = SWF.DockStyle.Fill;
			this.Control.Size = SD.Size.Empty;
			this.Control.MinimumSize = SD.Size.Empty;
			this.Control.AutoSize = true;
			this.Control.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Spacing = TableLayout.DefaultSpacing;
			this.Padding = TableLayout.DefaultPadding;
		}

		public override void OnLoad ()
		{
			base.OnLoad ();
			this.Control.ResumeLayout ();
		}
		
		public override void Update ()
		{
			this.Control.Update();
		}
		
		public Size Spacing {
			get {
				return spacing;
			}
			set {
				spacing = value;
				var newpadding = new SWF.Padding(0, 0, spacing.Width, spacing.Height);
				//this.Control.Padding = newpadding;
				foreach (SWF.Control control in this.Control.Controls)
				{
					control.Margin = newpadding;
				}
			}
		}
		
		SWF.Padding GetPadding(int x, int y)
		{
			return new SWF.Padding(
				x == 0 ? 0 : spacing.Width / 2, 
				y == 0 ? 0 : spacing.Height / 2, 
				x == this.views.GetLength(0)-1 ? 0 : (spacing.Width + 1) / 2, 
				y == this.views.GetLength(1)-1 ? 0 : (spacing.Height + 1) / 2);
		}
		
		public Padding Padding {
			get {
				return Generator.Convert(Control.Padding);
			}
			set
			{
				Control.Padding = Generator.Convert(value);
				//Control.Padding = new SWF.Padding(0);
			}
		}
		
		public void Add(Control child, int x, int y)
		{
			Control.SuspendLayout ();
			var old = views[x, y];
			if (old != null) Control.Controls.Remove((SWF.Control)old.ControlObject);
			views[x, y] = child;
			if (child != null) {
				SWF.Control childControl = (SWF.Control)child.ControlObject;
				if (childControl.Parent != null) childControl.Parent.Controls.Remove (childControl);
				childControl.Dock = ((IWindowsControl)child.Handler).DockStyle;
				childControl.Margin = GetPadding (x, y);
				Control.Controls.Add (childControl, x, y);
			}
			SetMinSize ();
			Control.ResumeLayout ();
		}
		
		void SetMinSize()
		{
			//return;
			/* What was this for?  doesn't seem to be needed anymore..*/
			var widths = this.Control.GetColumnWidths();
			var colstyles = this.Control.ColumnStyles;
			int minwidth = 0;//(widths.Length-1) * spacing.Width;
			for (int i = 0; i < widths.Length; i++) if (colstyles[i].SizeType != SWF.SizeType.Percent) minwidth += widths[i];
			
			var heights = this.Control.GetRowHeights();
			var rowstyles = this.Control.RowStyles;
			int minheight = 0; //(heights.Length-1) * spacing.Height;
			for (int i = 0; i < heights.Length; i++) if (rowstyles[i].SizeType != SWF.SizeType.Percent) minheight += heights[i];
			
			this.Control.MinimumSize = new System.Drawing.Size(minwidth, minheight);
			/**/
		}
		
		public void Move(Control child, int x, int y)
		{
			SWF.Control childControl = (SWF.Control)child.ControlObject;
			//IEnhancedControl ec = childControl as IEnhancedControl;
			//if (ec != null) ec.Margin = new Margin(4, 4, 4, 4);
			Control.SetCellPosition(childControl, new SWF.TableLayoutPanelCellPosition(x, y));
		}
		
		public void Remove (Control child)
		{
			SWF.Control childControl = (SWF.Control)child.ControlObject;
			if (childControl.Parent != null) childControl.Parent.Controls.Remove(childControl);
			for (int y=0; y<views.GetLength(0); y++)
			for (int x=0; x<views.GetLength(1); x++)
			{
				if (views[y,x] == child) views[y,x] = null;
			}
		}

		public void CreateControl(int cols, int rows)
		{
			views = new Control[cols, rows];
			Control.RowCount = rows;
			Control.ColumnCount = cols;
			Control.ColumnStyles.Clear();
			Control.RowStyles.Clear();
			for (int i = 0; i < cols; i++)
			{
				Control.ColumnStyles.Add(new SWF.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
			}
			for (int i = 0; i < rows; i++)
			{
				Control.RowStyles.Add(new SWF.RowStyle(System.Windows.Forms.SizeType.AutoSize));
			}
			if (cols == 1) Control.ColumnStyles[0] = new SWF.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F); 
			if (rows == 1) Control.RowStyles[0] = new SWF.RowStyle(System.Windows.Forms.SizeType.Percent, 1F); 
		}

		public void SetColumnScale(int column, bool scale)
		{
			if (scale || this.Control.ColumnCount == 1)
			{
				Control.ColumnStyles[column].SizeType = SWF.SizeType.Percent;
				Control.ColumnStyles[column].Width = 1F;
			}
			else
			{
				Control.ColumnStyles[column].SizeType = SWF.SizeType.AutoSize;
			}
			SetMinSize();
		}
		
		public void SetRowScale(int row, bool scale)
		{
			if (scale || this.Control.RowCount == 1)
			{
				Control.RowStyles[row].SizeType = SWF.SizeType.Percent;
				Control.RowStyles[row].Height = 1F;
			}
			else
			{
				Control.RowStyles[row].SizeType = SWF.SizeType.AutoSize;
			}
			SetMinSize();
		}
	}
}
