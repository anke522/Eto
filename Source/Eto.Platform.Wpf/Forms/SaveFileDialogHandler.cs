﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using mw = Microsoft.Win32;
using sw = System.Windows;

namespace Eto.Platform.Wpf.Forms
{
	public class SaveFileDialogHandler : WpfFileDialog<mw.SaveFileDialog, SaveFileDialog>, ISaveFileDialog
	{
		public SaveFileDialogHandler ()
		{
			Control = new mw.SaveFileDialog ();
		}
	}
}
