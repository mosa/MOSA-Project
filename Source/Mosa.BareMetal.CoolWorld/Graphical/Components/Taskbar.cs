// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Drawing;
using Mosa.DeviceSystem.Mouse;

namespace Mosa.BareMetal.CoolWorld.Graphical.Components;

public class TaskbarButton : Button
{
	public readonly Window AttachedWindow;

	public readonly Taskbar Taskbar;

	public TaskbarButton(Taskbar taskbar, string text, Color backColor, Color foreColor, Color hoverColor,
		Func<object> action, Window attachedWindow = null)
		: base(text, 0, 0, 0, backColor, foreColor, hoverColor, action)
	{
		Taskbar = taskbar;

		if (taskbar.Buttons.Count == 0)
			X = taskbar.DefaultPadding;
		else
		{
			uint totalWidth = 0, count = (uint)taskbar.Buttons.Count;
			for (var i = 0; i < count; i++)
				totalWidth += taskbar.Buttons[i].Width;
			X = totalWidth + taskbar.DefaultPadding * (count + 1);
		}

		Y = taskbar.Y;
		Height = taskbar.Height;

		if (attachedWindow != null)
			AttachedWindow = attachedWindow;
	}
}

public class Taskbar
{
	public uint X, Y, Width = Display.Width, Height = 30, DefaultPadding = 5;
	public Color Color = Color.White;

	public readonly List<TaskbarButton> Buttons;
	public readonly List<TaskbarWindowMenu> Menus;

	public Taskbar()
	{
		Buttons = new List<TaskbarButton>();
		Menus = new List<TaskbarWindowMenu>();

		Y = Display.Height - Height;
	}

	public void Draw()
	{
		Display.DrawRectangle(0, Y, Width, Height, Color, true);
	}

	public void Update()
	{
		foreach (var b in Buttons)
		{
			b.Draw();
			b.Update();
		}

		foreach (var m in Menus)
		{
			m.Draw();
			m.Update();
		}

		// Check if taskbar buttons holding a window (each) are clicked
		for (var i = 0; i < Desktop.Taskbar.Buttons.Count; i++)
		{
			var button = Desktop.Taskbar.Buttons[i];

			if (button.AttachedWindow == null)
				continue;

			if (WindowManager.ActiveWindow != null)
				if (button.AttachedWindow.Id == WindowManager.ActiveWindow.Id && button.AttachedWindow.Opened)
				{
					button.HoverColor = button.AttachedWindow.InactiveTitlebarColor;
					button.BackColor = button.AttachedWindow.ActiveTitlebarColor;
				}
				else
				{
					button.HoverColor = button.AttachedWindow.ActiveTitlebarColor;
					button.BackColor = button.AttachedWindow.InactiveTitlebarColor;
				}

			if (button.IsClicked(MouseState.Left))
			{
				button.AttachedWindow.Opened = !button.AttachedWindow.Opened;

				if (button.AttachedWindow.Opened)
				{
					WindowManager.ActiveWindow = button.AttachedWindow;

					button.HoverColor = button.AttachedWindow.InactiveTitlebarColor;
					button.BackColor = button.AttachedWindow.ActiveTitlebarColor;
				}
				else
				{
					button.HoverColor = button.AttachedWindow.ActiveTitlebarColor;
					button.BackColor = button.AttachedWindow.InactiveTitlebarColor;
				}
			}
			else if (button.IsClicked(MouseState.Right))
			{
				var menu = new TaskbarWindowMenu(button, Color.White, Color.Black, Color.Gray);
				var removed = false;

				for (var j = 0; j < Menus.Count; j++)
				{
					var m = Menus[j];

					if (m.AttachedButton != menu.AttachedButton)
						continue;

					Menus.RemoveAt(j);
					removed = true;

					break;
				}

				if (!removed)
					Menus.Add(menu);
			}
		}
	}
}
