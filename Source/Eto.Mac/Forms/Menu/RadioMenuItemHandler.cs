using System;
using Eto.Forms;
using System.Collections.Generic;
using System.Linq;

#if XAMMAC2
using AppKit;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
using CoreAnimation;

#else
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.ObjCRuntime;
using MonoMac.CoreAnimation;
#endif

namespace Eto.Mac.Forms.Menu
{
	public class RadioMenuItemHandler : MenuHandler<NSMenuItem, RadioMenuItem, RadioMenuItem.ICallback>, RadioMenuItem.IHandler, IMenuActionHandler
	{
		List<RadioMenuItem> radioGroup;

		public RadioMenuItemHandler()
		{
			Control = new NSMenuItem();
			Enabled = true;
			//control.SetButtonType(NSButtonType.Radio);
			Control.Target = new MenuActionHandler { Handler = this };
			Control.Action = MenuActionHandler.selActivate;
		}

		public override void Activate()
		{
			if (radioGroup != null)
			{
				var checkedItem = radioGroup.FirstOrDefault(r => r.Checked);
				if (checkedItem != null)
					checkedItem.Checked = false;
			}
			Checked = true;
			Callback.OnClick(Widget, EventArgs.Empty);
		}

		public void Create(RadioMenuItem controller)
		{
			if (controller != null)
			{
				var controllerInner = (RadioMenuItemHandler)controller.Handler;
				if (controllerInner.radioGroup == null)
				{
					controllerInner.radioGroup = new List<RadioMenuItem>();
					controllerInner.radioGroup.Add(controller);
				}
				controllerInner.radioGroup.Add(Widget);
				radioGroup = controllerInner.radioGroup;
			}
		}

		public bool Enabled
		{
			get { return Control.Enabled; }
			set { Control.Enabled = value; }
		}

		public string Text
		{
			get	{ return Control.Title; }
			set { Control.SetTitleWithMnemonic(value); }
		}

		public string ToolTip
		{
			get { return Control.ToolTip; }
			set { Control.ToolTip = value ?? string.Empty; }
		}

		public Keys Shortcut
		{
			get { return KeyMap.Convert(Control.KeyEquivalent, Control.KeyEquivalentModifierMask); }
			set
			{ 
				Control.KeyEquivalent = KeyMap.KeyEquivalent(value);
				Control.KeyEquivalentModifierMask = KeyMap.KeyEquivalentModifierMask(value);
			}
		}

		public bool Checked
		{
			get { return Control.State == NSCellStateValue.On; }
			set
			{ 
				if (Checked != value)
				{
					Control.State = value ? NSCellStateValue.On : NSCellStateValue.Off; 
					Callback.OnCheckedChanged(Widget, EventArgs.Empty);
				}
			}
		}

		MenuItem IMenuActionHandler.Widget
		{
			get { return Widget; }
		}

		MenuItem.ICallback IMenuActionHandler.Callback
		{
			get { return Callback; }
		}

		public override void AttachEvent(string id)
		{
			switch (id)
			{
				case RadioMenuItem.CheckedChangedEvent:
					break;
				case MenuItem.ValidateEvent:
					// handled in MenuActionHandler
					break;
				default:
					base.AttachEvent(id);
					break;
			}
		}
	}
}
