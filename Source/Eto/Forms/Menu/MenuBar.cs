using System;
using System.Collections;

namespace Eto.Forms
{
	public interface IMenuBar : IMenu, ISubMenu
	{

	}
	
	public class MenuBar : Menu, ISubMenuWidget
	{
		IMenuBar inner;
		MenuItemCollection menuItems;
		
		public MenuBar()
			: this(Generator.Current)
		{
			
		}

		public MenuBar(Generator g) : base(g, typeof(IMenuBar))
		{
			//BindingContext = new BindingContext();
			inner = (IMenuBar)base.Handler;
			menuItems = new MenuItemCollection(this, inner);
		}

		public MenuBar(Generator g, ActionItemCollection actionItems) : this(g)
		{
			GenerateActions(actionItems);
		}
		
		public void GenerateActions (ActionItemCollection actionItems)
		{
			foreach (IActionItem ai in actionItems)
			{
				ai.Generate(this);
			}
		}

		#region IParentMenuWidget implementation
		
		public MenuItemCollection MenuItems {
			get { return menuItems; }
		}
		#endregion
	}
}
