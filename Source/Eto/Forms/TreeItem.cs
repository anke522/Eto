using System;
using System.Collections.Generic;
using Eto.Collections;
#if DESKTOP
using System.Windows.Markup;
#endif
using System.Collections.Specialized;
using Eto.Drawing;

namespace Eto.Forms
{
	public interface ITreeItem : IImageListItem, ITreeStore
	{
		bool Expanded { get; set; }
		
		bool Expandable { get; }
		
		ITreeItem Parent { get; set; }

		Color TextColor { get; set; }
	}

	public class TreeItemCollection : DataStoreCollection<ITreeItem>
	{
	}
	
#if DESKTOP
	[ContentProperty("Children")]
#endif
	public class TreeItem : ImageListItem, ITreeItem
	{
		TreeItemCollection children;

		public TreeItemCollection Children
		{
			get { 
				if (children != null)
					return children;
				children = new TreeItemCollection ();
				children.CollectionChanged += (sender, e) => {
					if (e.Action == NotifyCollectionChangedAction.Add) {
						foreach (ITreeItem item in e.NewItems) {
							item.Parent = this;
						}
					}
				};
				return children; 
			}
		}
		
		public ITreeItem Parent { get; set; }
		
		public virtual bool Expandable { get { return this.Count > 0; } }
		
		private bool expanded;
		public virtual bool Expanded
		{
			get { return expanded; }
			set
			{
				expanded = value;
				OnPropertyChanged("Expanded");
			}
		}

		private Color textcolor;

		public virtual Color TextColor
		{
			get
			{
				return textcolor;
			}
			set
			{
				textcolor = value;
				OnPropertyChanged("TextColor");
			}
		}
		
		public virtual ITreeItem this[int index]
		{
			get { return children [index]; }
		}

		public virtual int Count {
			get { return (children != null) ? children.Count : 0; }
		}
		
		public TreeItem ()
		{
			TextColor = Color.Transparent;
		}
		
		public TreeItem (IEnumerable<ITreeItem> children)
		{
			this.Children.AddRange (children);
		}
	}
}

