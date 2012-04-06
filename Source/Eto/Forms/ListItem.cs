using System;
using Eto.Drawing;
#if DESKTOP
using System.Windows.Markup;
using System.ComponentModel;
#endif

namespace Eto.Forms
{
	public interface IListItem
	{
		string Text { get; }

		string Key { get; }
	}
	
#if DESKTOP
	[ContentProperty("Text")]
#endif
	public class ListItem : IListItem, INotifyPropertyChanged
	{
		string key, text;

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				OnPropertyChanged("Text");
				if (key == null)
					OnPropertyChanged("Key");
			}
		}

		public string Key {
			get { return key ?? Text; }
			set { key = value; OnPropertyChanged("Key"); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string Name)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(Name));
		}
	}
	
	public class ImageListItem : ListItem, IImageListItem
	{
		Image image;
		public Image Image
		{
			get { return image; }
			set
			{
				image = value;
				OnPropertyChanged("Image");
			}
		}
	}
	
#if DESKTOP
	[ContentProperty("Item")]
#endif
	public class ObjectListItem : IListItem
	{
		public object Item { get; set; }
		
		public virtual string Text {
			get { return Convert.ToString (Item); }
		}
		
		public virtual string Key {
			get { return Text; }
		}
		
		public ObjectListItem ()
		{
		}
		
		public ObjectListItem (object item)
		{
			this.Item = item;
		}
	}
}

