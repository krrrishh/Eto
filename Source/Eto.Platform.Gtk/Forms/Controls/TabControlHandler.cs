using System;
using System.Collections;
using Eto.Forms;

namespace Eto.Platform.GtkSharp
{
	public class TabControlHandler : GtkControl<Gtk.Notebook, TabControl>, ITabControl
	{
		public TabControlHandler()
		{
			Control = new Gtk.Notebook();
			Control.ChangeCurrentPage += new Gtk.ChangeCurrentPageHandler(control_ChangeCurrentPage);
			//control.SelectedIndexChanged += control_SelectedIndexChanged;
		}

		public int SelectedIndex
		{
			get { return Control.CurrentPage; }
			set { Control.CurrentPage = value; }
		}

		public void AddTab(TabPage page)
		{
			Control.AppendPage((Gtk.Widget)((TabPageHandler)page.Handler).ContainerObject, (Gtk.Widget)page.ControlObject);
		}

		public void RemoveTab(TabPage page)
		{
			Control.RemovePage(Control.PageNum((Gtk.Widget)((TabPageHandler)page.Handler).ContainerObject));
		}

		private void control_ChangeCurrentPage(object o, Gtk.ChangeCurrentPageArgs args)
		{
			Widget.OnSelectedIndexChanged(EventArgs.Empty);
		}
	}
}