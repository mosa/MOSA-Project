// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Demo.SVGAWorld.x86.Components;

namespace Mosa.Demo.SVGAWorld.x86;

public class WindowManager
{
	public static List<Window> Windows;

	public static Window ActiveWindow;
	public static bool IsWindowMoving = false;

	public static void Initialize()
	{
		Windows = new List<Window>();
	}

	public static void Open(Window w)
	{
		Windows.Add(w);

		w.Opened = true;
		ActiveWindow = w;

		// TODO : Make this update if the backcolor or stuff of the window updates,
		// also maybe make the hover color darker than the original color to make it look nicer?
		Boot.Taskbar.Buttons.Add(new TaskbarButton(Boot.Taskbar, w.Title, w.ActiveTitlebarColor, w.BodyColor, w.InactiveTitlebarColor, null, w));
	}

	public static void Close(Window w)
	{
		w.Opened = false;

		var index = -1;
		TaskbarButton b = null;

		for (var i = 0; i < Boot.Taskbar.Buttons.Count; i++)
		{
			var button = Boot.Taskbar.Buttons[i];

			if (button.AttachedWindow != null && button.AttachedWindow.Id == w.Id)
			{
				index = i;
				b = button;

				Boot.Taskbar.Buttons.RemoveAt(i);
				break;
			}
		}

		// Necessary to prevent bugs on the taskbar
		for (var i = index; i < Boot.Taskbar.Buttons.Count; i++)
		{
			var btn = Boot.Taskbar.Buttons[i];
			btn.X -= b.Width + Boot.Taskbar.DefaultPadding;
			Boot.Taskbar.Buttons[i] = btn;
		}

		// List.IndexOf() doesn't work yet (which is used by Remove()), so we have to do it manually
		for (var i = 0; i < Windows.Count; i++)
		{
			var win = Windows[i];

			if (win.Id != w.Id)
				continue;

			Windows.RemoveAt(i);
			break;
		}

		if (Windows.Count >= 1)
			ActiveWindow = Windows[Windows.Count - 1];
	}

	public static void Update()
	{
		// Logic to make the active window draw on top of all the other windows
		foreach (var w in Windows)
			if (ActiveWindow != null && w != ActiveWindow)
			{
				w.Draw();
				w.Update();
			}

		if (ActiveWindow == null)
			return;

		ActiveWindow.Draw();
		ActiveWindow.Update();
	}
}
